namespace TreeViewTest.Models.Instruments
{
    public interface IInstrument
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int HitCount { get; set; }
    }
}
