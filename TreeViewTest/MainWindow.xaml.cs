using System.Collections.ObjectModel;
using System.Windows;
using TreeViewTest.Models.Instruments;

namespace TreeViewTest
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<IInstrumentItem> InstrumentItems { get; set; }

        public MainWindow()
        {
            InstrumentItems = PopulateItems();
            InitializeComponent();
        }

        private ObservableCollection<IInstrumentItem> PopulateItems()
        {
            var collection = new ObservableCollection<IInstrumentItem>();
            for (int i = 0; i < 10; i++)
            {
                var group = new InstrumentGroup("group #" + i);
                PopulateGroup(group, 1000);
                collection.Add(group);
            }

            var smallGroup = new InstrumentGroup("smallGroup");
            PopulateGroup(smallGroup, 10);
            collection.Add(smallGroup);

            var largeGroup = new InstrumentGroup("largeGroup");
            PopulateGroup(largeGroup, 2500);
            collection.Add(largeGroup);

            return collection;
        }

        private void PopulateGroup(InstrumentGroup group, int amount)
        {
            var subGroup = new InstrumentGroup("subgroup");
            group.Items.Add(subGroup);

            for (int i = 0; i < amount; i++)
            {
                subGroup.Items.Add(new Instrument("item #" + i));
            }

            group.Items.Add(new Instrument("inner visible item"));
            group.Items.Add(new Instrument("inner invisible item", -1));
        }
    }
}
