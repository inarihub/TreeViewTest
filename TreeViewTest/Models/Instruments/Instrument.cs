namespace TreeViewTest.Models.Instruments
{
    public class Instrument : IInstrumentItem
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int HitCount { get; set; }

        public Instrument(string name, int hitCount = 0)
        {
            Name = name;
            Code = name + "#code";
            HitCount = hitCount;
        }
    }
}
