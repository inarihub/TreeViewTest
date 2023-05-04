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

    public List<InstrumentNode> InstrumentsCollection { get; set; }

    public List<string> SelectedCodes
    {
        get { return (List<string>)GetValue(SelectedCodesProperty); }
        set { SetValue(SelectedCodesProperty, value); }
    }

    public static readonly DependencyProperty SelectedCodesProperty =
        DependencyProperty.Register("SelectedCodes", typeof(List<string>), typeof(InstrumentsMenuViewModel), new PropertyMetadata(null));


    public InstrumentsMenuViewModel()
    {
        InstrumentsCollection = new();
        InitializeTreeCollection();
        GetCodesCommand = new RelayCommand(GetCodes);
        SelectAllCommand = new RelayCommand(SelectAll);
        UnselectAllCommand = new RelayCommand(UnselectAll);
    }

    private async void InitializeTreeCollection()
    {
        Uri uri = new("/filters.txt", UriKind.Relative);
        await InstrumentNodeBuilder.PopulateFromFile(uri, InstrumentsCollection);
    }

    private async void GetCodes()
    {
        SelectedCodes = await GetSelected();
    }

    private Task<List<string>> GetSelected()
    {
        List<string> list = new();
        var res = FindSelected(InstrumentsCollection, list);

        return Task.FromResult(res);
    }

    private List<string> FindSelected(List<InstrumentNode> instruments, List<string> keys)
    {
        foreach (var instrument in instruments)
        {
            if (instrument.IsSelected is null || instrument.IsSelected.Value is true)
            {
                if (instrument.Items.Count > 0)
                {
                    FindSelected(instrument.Items, keys);
                }

                keys.Add(instrument.Key);
            }
        }

        return keys;
    }

    public void SelectAll()
    {
        SetSelectedTo(true, InstrumentsCollection);
    }

    public void UnselectAll()
    {
        SetSelectedTo(false, InstrumentsCollection);
    }

    private void SetSelectedTo(bool selected, List<InstrumentNode> list)
    {
        foreach (var instrument in list)
        {
            if (instrument.Items.Count > 0)
                SetSelectedTo(selected, instrument.Items);

            if (instrument.IsSelected is null || instrument.IsSelected.Value != selected)
                instrument.IsSelected = selected;
        }
    }
}
