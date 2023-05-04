using System.Windows.Controls;
using System.Windows;

namespace TreeViewTest.Controls
{
    public class InstrumentsCheckBox : CheckBox
    {


        public bool PartialToChecked
        {
            get { return (bool)GetValue(PartialToCheckedProperty); }
            set { SetValue(PartialToCheckedProperty, value); }
        }

        public static readonly DependencyProperty PartialToCheckedProperty =
            DependencyProperty.Register("PartialToChecked", typeof(bool), typeof(InstrumentsCheckBox), new UIPropertyMetadata(false));

        protected override void OnToggle()
        {
            if (PartialToChecked && !IsChecked.HasValue)
                IsChecked = true;
            else
                base.OnToggle();
        }
    }
}
