namespace TreeViewTest.Models.Instruments;

public interface IInstrument
{
    public string Name { get; init; }
    public int Hits { get; set; }
}
