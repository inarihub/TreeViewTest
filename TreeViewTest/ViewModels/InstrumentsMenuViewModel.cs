using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using TreeViewTest.Models.Instruments;

namespace TreeViewTest.ViewModels;

public partial class InstrumentsMenuViewModel : DependencyObject
{
    public IRelayCommand GetCodesCommand { get; set; }
    public IRelayCommand SelectAllCommand { get; set; }
    public IRelayCommand UnselectAllCommand { get; set; }

    public InstrumentNode InstrumentsRootNode
    {
        get { return (InstrumentNode)GetValue(InstrumentsRootNodeProperty); }
        set { SetValue(InstrumentsRootNodeProperty, value); }
    }

    public static readonly DependencyProperty InstrumentsRootNodeProperty =
        DependencyProperty.Register("InstrumentsRootNode", typeof(InstrumentNode), typeof(InstrumentsMenuViewModel), new PropertyMetadata(null));

    public List<string> SelectedCodes
    {
        get { return (List<string>)GetValue(SelectedCodesProperty); }
        set { SetValue(SelectedCodesProperty, value); }
    }

    public static readonly DependencyProperty SelectedCodesProperty =
        DependencyProperty.Register("SelectedCodes", typeof(List<string>), typeof(InstrumentsMenuViewModel), new PropertyMetadata(null));


    public InstrumentsMenuViewModel()
    {
        InitializeTreeCollection();
        GetCodesCommand = new RelayCommand(GetCodes);
        SelectAllCommand = new RelayCommand(SelectAll);
        UnselectAllCommand = new RelayCommand(UnselectAll);
    }

    private async void InitializeTreeCollection()
    {
        Uri uri = new("/filters.txt", UriKind.Relative);
        InstrumentsRootNode = await InstrumentNodeBuilder.PopulateFromFile(uri) ?? throw new Exception();
    }

    private async void GetCodes()
    {
        SelectedCodes = await GetSelected();
    }

    private Task<List<string>> GetSelected()
    {
        List<string> list = new();
        var res = FindSelected(InstrumentsRootNode, list);

        return Task.FromResult(res);
    }

    private List<string> FindSelected(InstrumentNode instrumentsRoot, List<string> keys)
    {
        foreach (var instrument in instrumentsRoot.Items)
        {
            if (instrument.IsSelected is null)
            {
                FindSelected(instrument, keys);
            }
            else if (instrument.IsSelected.Value && instrument.Items.Count == 0)
            {
                keys.Add(instrument.Key);
            }
        }

        return keys;
    }

    public void SelectAll()
    {
        if (InstrumentsRootNode is null) return;
        SetSelectedTo(true, InstrumentsRootNode);
    }

    public void UnselectAll()
    {
        if (InstrumentsRootNode is null) return;
        SetSelectedTo(false, InstrumentsRootNode);
    }

    private static void SetSelectedTo(bool selected, InstrumentNode list)
    {
        foreach (var instrument in list.Items)
        {
            if (instrument.Items.Count > 0)
                SetSelectedTo(selected, instrument);

            if (instrument.IsSelected is null || instrument.IsSelected.Value != selected)
                instrument.IsSelected = selected;
        }
    }
}
