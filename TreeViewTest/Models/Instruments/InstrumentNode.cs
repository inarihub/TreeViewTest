using System.Collections.Generic;
using System.Windows;

namespace TreeViewTest.Models.Instruments
{
    public class InstrumentNode : DependencyObject, IInstrument
    {
        #region interface implementation
        public string Name { get; set; }
        public string Code { get; set; }
        public int HitCount { get; set; }
        #endregion

        public List<InstrumentNode> Items { get; set; }
        public InstrumentNode? Parent { get; set; }
        public bool? IsSelected
        {
            get { return (bool?)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool?), typeof(InstrumentNode), new PropertyMetadata(false));

        public InstrumentNode(string name, InstrumentNode? parent = null, int hitCount = -1)
        {
            Name = name;
            Code = name.GetHashCode().ToString(); // temp
            Parent = parent;
            HitCount = hitCount;
            Items = new List<InstrumentNode>();
        }
    }
}
