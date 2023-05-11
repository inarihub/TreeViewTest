using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TreeViewTest.Models.Instruments;

namespace TreeViewTest.Controls;

public partial class InstrumentsTreeControl : UserControl
{
    public List<InstrumentNode> CollectionNodeView
    {
        get { return (List<InstrumentNode>)GetValue(CollectionNodeViewProperty); }
        set { SetValue(CollectionNodeViewProperty, value); }
    }

    public static readonly DependencyProperty CollectionNodeViewProperty =
        DependencyProperty.Register("CollectionNodeView", typeof(List<InstrumentNode>), typeof(InstrumentsTreeControl), new PropertyMetadata(null));

    public InstrumentsTreeControl() 
    {
        InitializeComponent();
        Binding binding = new("CollectionNodeView")
        {
            Source = this,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        };
        BindingOperations.SetBinding(instrumentsTree, TreeView.ItemsSourceProperty, binding);
    }
}
