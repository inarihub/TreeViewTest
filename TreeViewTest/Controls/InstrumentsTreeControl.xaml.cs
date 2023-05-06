using System.Windows;
using System.Windows.Controls;
using TreeViewTest.Models.Instruments;

namespace TreeViewTest.Controls;

public partial class InstrumentsTreeControl : UserControl
{
    public InstrumentNode InstrumentsRoot
    {
        get { return (InstrumentNode)GetValue(InstrumentsRootProperty); }
        set { SetValue(InstrumentsRootProperty, value); }
    }

    public static readonly DependencyProperty InstrumentsRootProperty =
        DependencyProperty.Register("InstrumentsRoot", typeof(InstrumentNode), typeof(InstrumentsTreeControl), new PropertyMetadata(null));

    public InstrumentsTreeControl()
    {
        InitializeComponent();
    }
}
