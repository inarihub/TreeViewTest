using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using TreeViewTest.Models.Instruments;

namespace TreeViewTest.Controls
{
    public partial class InstrumentsTreeControl : UserControl
    {
        public InstrumentsTreeControl()
        {
            InitializeComponent();
        }

        public ObservableCollection<IInstrumentItem> Instruments
        {
            get { return (ObservableCollection<IInstrumentItem>)GetValue(InstrumentsProperty); }
            set { SetValue(InstrumentsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Instruments.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InstrumentsProperty =
            DependencyProperty.Register("Instruments", typeof(ObservableCollection<IInstrumentItem>), typeof(InstrumentsTreeControl), new PropertyMetadata(null));


    }
}
