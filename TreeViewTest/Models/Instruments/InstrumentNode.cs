using System;
using System.Collections.Generic;
using System.Windows;

namespace TreeViewTest.Models.Instruments;

public class InstrumentNode : DependencyObject, IInstrument
{
    #region interface implementation
    public string Name { get; init; }
    public int Hits { get; set; }
    #endregion

    public string Key { get; init; }
    public int Level { get; init; }
    public List<InstrumentNode> Items { get; init; }
    public InstrumentNode? Parent { get; internal set; }
    public string Path { get; init; }
    public bool HasChildren { get; internal set; }

    public bool? IsSelected
    {
        get { return (bool?)GetValue(IsSelectedProperty); }
        set { SetValue(IsSelectedProperty, value); }
    }

    public static readonly DependencyProperty IsSelectedProperty =
        DependencyProperty.Register("IsSelected", typeof(bool?), typeof(InstrumentNode), new PropertyMetadata(false));

    public InstrumentNode()
    {
        Key = string.Empty;
        Level = 0;
        Name = string.Empty;
        Parent = null;
        Path = string.Empty;
        HasChildren = false;
        Items = new List<InstrumentNode>();
    }
}
