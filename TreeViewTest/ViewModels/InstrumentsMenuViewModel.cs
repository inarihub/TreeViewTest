using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TreeViewTest.Models.EventArgs;
using TreeViewTest.Models.Instruments;
using TreeViewTest.Services;

namespace TreeViewTest.ViewModels;

public partial class InstrumentsMenuViewModel : BaseViewModel
{
    InstrumentsProviderService _instrumentProvider;

    public IRelayCommand GetKeysCommand { get; set; }
    public IRelayCommand SearchCommand { get; set; }
    public IRelayCommand ResetCommand { get; set; }

    public event EventHandler<TreeChangedEventArgs> OnTreeStateChanged;

    private bool _isInitializing;
    public bool IsInitializing
    {
        get => _isInitializing;
        private set
        {
            _isInitializing = value;
            OnPropertyChanged();
            GetKeysCommand.NotifyCanExecuteChanged();
            SearchCommand.NotifyCanExecuteChanged();
            ResetCommand.NotifyCanExecuteChanged();
        }
    }

    private InstrumentNode? _instrumentRootNode;
    public InstrumentNode? InstrumentRootNode
    {
        get => _instrumentRootNode;
        set
        {
            _instrumentRootNode = value;
            OnPropertyChanged();
        }
    }

    private List<string> _selectedKeys;
    public List<string> SelectedKeys
    {
        get => _selectedKeys;
        set
        {
            _selectedKeys = value;
            OnPropertyChanged();
        }
    }

    public InstrumentsMenuViewModel()
    {
        _instrumentProvider = new();
        _selectedKeys = new();
        OnTreeStateChanged += TreeStateChangedHandler;
        GetKeysCommand = new AsyncRelayCommand(GetKeys, CanExecuteButton);
        SearchCommand = new AsyncRelayCommand(SearchBySelected, CanExecuteButton);
        ResetCommand = new AsyncRelayCommand(ResetNodesView, CanExecuteButton);
    }

    public async Task BeginInitializeRootNode()
    {
        OnTreeStateChanged?.Invoke(this, new TreeChangedEventArgs(InstrumentRootNode, TreeState.InitializationStarted));

        using (var instrumentsStream = await _instrumentProvider.GetInstrumentsAsync())
        {
            InstrumentRootNode = await Task.Run(() => InstrumentNodeBuilder.PopulateNodeAsync(instrumentsStream));
        }

        OnTreeStateChanged?.Invoke(this, new TreeChangedEventArgs(InstrumentRootNode, TreeState.InitializationEnded, true));
    }

    #region Reset methods

    public async Task ResetNodesView()
    {
        if (InstrumentRootNode is null) return;

        await Task.Run(() => ResetSearching(InstrumentRootNode.Items));
    }

    private void ResetSearching(List<InstrumentNode> instruments)
    {
        foreach (var instrument in instruments)
        {
            if (!instrument.IsSearchMatch)
            {
                instrument.IsSearchMatch = true;

                if (instrument.Parent?.IsSelected is bool value && value is true)
                {
                    instrument.Parent.IsSelected = null;
                } 
            }

            if (instrument.HasChildren)
                ResetSearching(instrument.Items);
        }
    }
    #endregion

    #region GetKeysCommand methods

    private async Task GetKeys()
    {
        OnTreeStateChanged?.Invoke(InstrumentRootNode, new TreeChangedEventArgs(InstrumentRootNode, TreeState.InitializationStarted));

        if (InstrumentRootNode is not null)
        {
            var newKeys = new List<string>();
            await Task.Delay(3000); // simulation
            await Task.Run(() => FindSelected(InstrumentRootNode.Items, newKeys));
            SelectedKeys = newKeys;
        }
        
        OnTreeStateChanged?.Invoke(InstrumentRootNode, new TreeChangedEventArgs(InstrumentRootNode, TreeState.InitializationEnded));
    }

    private void FindSelected(List<InstrumentNode> instruments, List<string> keys)
    {
        foreach (var instrument in instruments)
        {
            if (instrument.IsSelected is null)
            {
                FindSelected(instrument.Items, keys);
            }
            else if (instrument.IsSelected.Value is true)
            {
                if (instrument.HasChildren)
                {
                    FindSelected(instrument.Items, keys);
                }
                else if (!instrument.HasChildren)
                {
                    keys.Add(instrument.Key);
                }
            }
        }
    }
    #endregion

    #region SearchCommand methods

    private async Task SearchBySelected()
    {
        OnTreeStateChanged?.Invoke(this, new TreeChangedEventArgs(InstrumentRootNode, TreeState.InitializationStarted));
        if (InstrumentRootNode is null) return;
        await Task.Delay(3000);
        await Task.Run(() => FilterItemsView(InstrumentRootNode, IsMatched));
        OnTreeStateChanged?.Invoke(this, new TreeChangedEventArgs(InstrumentRootNode, TreeState.InitializationEnded, true));
    }

    static bool IsMatched(InstrumentNode node)
    {
        if (node.Level <= 1) return true;

        if (node.IsSelected is bool isSelected)
        {
            if (isSelected) return true;

            if (!isSelected && node.Parent is InstrumentNode parent && parent.Items.IndexOf(node) < 3 && parent.IsSelected is not null)
                return true;
            else
                return false;
        }
        else return true;
    }

    /// <summary>
    /// Changes IsSearchMatch
    /// </summary>
    /// <param name="instrument">InstrumentNode root instance</param>
    /// <param name="predicate">Condition of searching</param>
    /// <returns></returns>
    private void FilterItemsView(InstrumentNode instrument, Func<InstrumentNode, bool> predicate)
    {
        bool isBecameSelected = true;

        foreach (var item in instrument.Items)
        {
            if (!predicate(item))
            {
                item.IsSearchMatch = false;
            }
            else if (item.IsSelected is bool value && value is false)
            {
                isBecameSelected = false;
            } // little helper while we has "simulation rules"

            if (item.HasChildren)
                FilterItemsView(item, predicate);
        }

        if (isBecameSelected) instrument.IsSelected = true;
    }
    #endregion

    private void TreeStateChangedHandler(object? sender, TreeChangedEventArgs e)
    {
        IsInitializing = e.State == TreeState.InitializationStarted;
    }

    private bool CanExecuteButton()
    {
        return !IsInitializing;
    }
}
