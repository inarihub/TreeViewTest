using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TreeViewTest.Models.Instruments;

namespace TreeViewTest.Controls;

public partial class InstrumentsTreeControl : UserControl
{
    public List<InstrumentNode> Instruments
    {
        get { return (List<InstrumentNode>)GetValue(InstrumentsProperty); }
        set { SetValue(InstrumentsProperty, value); }
    }

    public static readonly DependencyProperty InstrumentsProperty =
        DependencyProperty.Register("Instruments", typeof(List<InstrumentNode>), typeof(InstrumentsTreeControl), new PropertyMetadata(null));

    public InstrumentsTreeControl()
    {
        InitializeComponent();
    }

   
}
