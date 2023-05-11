using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TreeViewTest.Models.Instruments;

public class InstrumentNode : IInstrument, ITreeSearchable, INotifyPropertyChanged
{
    #region interfaces implementation
    public string Name { get; init; }
    public int Hits { get; set; }

    private bool _isSearchMatch;
    public bool IsSearchMatch
    {
        get => _isSearchMatch;
        set
        {
            _isSearchMatch = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion

    public string Key { get; init; }
    public int Level { get; init; }
    public List<InstrumentNode> Items { get; init; }
    public InstrumentNode? Parent { get; set; }
    public string Path { get; init; }
    public bool HasChildren { get; set; }

    public bool? _isSelected;
    public bool? IsSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;
            OnPropertyChanged();
        }
    }

    public InstrumentNode()
    {
        Key = string.Empty;
        Level = 0;
        Name = string.Empty;
        Parent = null;
        Path = string.Empty;
        HasChildren = false;
        Items = new List<InstrumentNode>();
        IsSelected = false;
        IsSearchMatch = true;
    }
}
