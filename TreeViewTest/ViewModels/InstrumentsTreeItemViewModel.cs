using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TreeViewTest.Models.Instruments;

namespace TreeViewTest.ViewModels;

public class InstrumentsTreeItemViewModel : DependencyObject
{
    public IRelayCommand<CheckBox> CheckBranchCommand { get; set; }

    public InstrumentsTreeItemViewModel()
    {
        CheckBranchCommand = new RelayCommand<CheckBox>(CheckBranch!);
    }

    public void CheckBranch(CheckBox source)
    {
        var node = (InstrumentNode)source.DataContext;
        bool? isChecked = source.IsChecked;
        if (isChecked is null) return;

        SetChildChecked(node, isChecked.Value);
        SetParentChecked(node, isChecked.Value);
    }

    private void SetChildChecked(InstrumentNode node, bool value)
    {
        if (node.Items.Count > 0)
        {
            foreach (var item in node.Items)
            {
                SetChildChecked(item, value);
                item.IsSelected = value;
            }
        }
    }

    private void SetParentChecked(InstrumentNode node, bool? isChecked)
    {
        if (node.Parent is null) return;

        if (isChecked is null) // short curcuit return to avoid unnessecary checkings
        {
            node.Parent.IsSelected = null;
        }
        else
        {
            if (node.Parent.Items.Count > 1 && HasMixedValues(node.Parent.Items, isChecked.Value))
            {
                isChecked = null;
                node.Parent.IsSelected = isChecked;
            }
            else // last child has been equaled to others
            {
                node.Parent.IsSelected = isChecked;
            }
        }

        SetParentChecked(node.Parent, isChecked);
    }

    private static bool HasMixedValues(List<InstrumentNode> nodes, bool isChecked)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].IsSelected != isChecked)
                return true;
        }

        return false;
    }
}
