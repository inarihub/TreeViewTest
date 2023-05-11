using System;
using System.IO;
using System.Threading.Tasks;

namespace TreeViewTest.Models.Instruments;

public class InstrumentNodeBuilder
{
    const int INSTRUMENTNODE_PROP_COUNT = 6;

    public async static Task<InstrumentNode?> PopulateNodeAsync(Stream stream)
    {
        using var reader = new StreamReader(stream);
        string? bufferString = await reader.ReadLineAsync();
        InstrumentNode root = SetRoot(bufferString);

        while ((bufferString = await reader.ReadLineAsync()) is not null)
        {
            if (ParseToInstrument(bufferString, out InstrumentNode? result))
            {
                SetInstrumentNode(root, result!);
            }
        }
        return root;
    }

    private static InstrumentNode SetRoot(string? str)
    {
        ParseToInstrument(str, out InstrumentNode? result);
        if (result is null || result.Level != 0) throw new Exception("Cannot parse to instrument");

        return result;
    }

    private static void SetInstrumentNode(InstrumentNode rootNode, InstrumentNode result)
    {
        var parent = FindParent(rootNode, result.Key);
        if (parent is null) return;
        parent.HasChildren = true;
        result.Parent = parent;
        parent.Items.Add(result);
    }

    private static InstrumentNode? FindParent(InstrumentNode node, string key)
    {
        var parentNode = node;
        int position = key.IndexOf('.', node.Key.Length + 1);

        if (position == -1) return parentNode;

        var nextLevelNode = node.Items.Find(i => i.Key == key[..position]) ?? throw new Exception();
        parentNode = FindParent(nextLevelNode, key);

        return parentNode;
    }

    private static bool ParseToInstrument(string? inputString, out InstrumentNode? node)
    {
        node = null;
        if (inputString is null) return false;

        var parseObj = inputString.Split('|');
        if (parseObj is null || parseObj.Length != INSTRUMENTNODE_PROP_COUNT) return false;

        string key = parseObj[0];

        if (!IsKeyValid(parseObj[0], out int levelCount) ||
            !IsLevelValid(parseObj[1], out int level) || levelCount != level)
            return false;

        string name = parseObj[2];
        int hits = ParseHits(parseObj[3]);
        string path = parseObj[4];

        if (!bool.TryParse(parseObj[5], out bool hasChildren))
            return false;

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

    private static bool IsKeyValid(string keyStr, out int levelCount)
    {
        var levels = keyStr.Split('.');
        levelCount = -1;

        foreach (var level in levels)
        {
            levelCount++;
            if (level != string.Empty) continue;
            else return false;
        }

        return true;
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
