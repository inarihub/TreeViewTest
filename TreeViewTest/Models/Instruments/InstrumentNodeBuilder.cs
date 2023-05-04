using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TreeViewTest.Models.Instruments
{
    public class InstrumentNodeBuilder
    {
        public async static Task PopulateFromFile(Uri uri, List<InstrumentNode> nodes)
        {
            using var stream = App.GetContentStream(uri).Stream;
            using var reader = new StreamReader(stream);
            string? line;

            while ((line = await reader.ReadLineAsync()) is not null)
            {
                if (ParseToInstrument(line, out InstrumentNode? result))
                {
                    SetInstrumentNode(nodes, result!);
                }
            }
        }

        private static void SetInstrumentNode(List<InstrumentNode> nodes, InstrumentNode result)
        {
            var targetList = FindParent(nodes, result.Key, out InstrumentNode? parent);

            result.Parent = parent;
            targetList.Add(result);
        }

        private static List<InstrumentNode> FindParent(List<InstrumentNode> nodes, string key, out InstrumentNode? lastParentNode, int position = 0)
        {
            List<InstrumentNode> currentList = nodes;
            lastParentNode = null;
            position = key.IndexOf('.', position + 1);

            if (!string.IsNullOrEmpty(key) && position != -1)
            {
                lastParentNode = currentList.Find(x => x.Key == key[..position]) ?? throw new Exception("Invalid key path");
                currentList = FindParent(lastParentNode.Items, key, out InstrumentNode? nextParentNode, position);

                if (nextParentNode is not null)
                    lastParentNode = nextParentNode;
            }

            return currentList;
        }

        private static bool ParseToInstrument(string result, out InstrumentNode? node)
        {
            node = default;
            var regRes = GenerateMatchGroups(result);

            if (regRes.Success)
            {
                string key = regRes.Groups["Key"].Value;
                string levelStr = regRes.Groups["Level"].Value;

                if (!IsKeyValid(key) || !IsLevelValid(levelStr, out int level))
                    return false;

                string name = regRes.Groups["Name"].Value;
                int hits = ParseHits(regRes.Groups["Hits"].Value); // default value is -1

                string path = regRes.Groups["Path"].Value;
                bool hasChildren = bool.Parse(regRes.Groups["HasChildren"].Value.ToLower());

                node = new()
                {
                    Key = key,
                    Level = level,
                    Name = name,
                    Hits = hits,
                    Path = path,
                    HasChildren = hasChildren
                };
                return true;
            }

            return false;
        }

        private static Match GenerateMatchGroups(string nodeString)
        {
            return Regex.Match(nodeString,
                @"^(?<Key>.+(\.\d+)*)\|(?<Level>\d+)\|(?<Name>.+)\|(?<Hits>\d*)\|(?<Path>.*)\|(?<HasChildren>(\bTrue\b)|(\bFalse\b))\z");
        }

        private static bool IsKeyValid(string keyStr)
        {
            return Regex.IsMatch(keyStr, @"^\w+(\.\w+)*\z");
        }

        private static bool IsLevelValid(string levelStr, out int levelValue)
        {
            return int.TryParse(levelStr, out levelValue);
        }

        private static int ParseHits(string strHits)
        {
            if (strHits == string.Empty || !int.TryParse(strHits, out int hits))
                hits = -1;

            return hits;
        }
    }
}
