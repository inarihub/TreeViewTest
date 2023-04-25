using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TreeViewTest.Models.Instruments
{
    public class InstrumentGroup : IInstrumentItem
    {
        public string Name { get; set; }
        public ObservableCollection<IInstrumentItem> Items { get; set; } = new();

        public InstrumentGroup(string name) => Name = name;
    }
}
