using System.Collections.Generic;
using TreeViewTest.Models.Instruments;

namespace TreeViewTest.Models.EventArgs
{
    public enum TreeState
    {
        InitializationStarted,
        InitializationEnded
    }

    public class TreeChangedEventArgs : System.EventArgs
    {
        public InstrumentNode? TreeView { get; set; }
        public bool IsChanged { get; set; }
        public TreeState State { get; set; }

        public TreeChangedEventArgs(InstrumentNode? root, TreeState currentState, bool isChanged = false)
        {
            TreeView = root;
            State = currentState;
            IsChanged = isChanged;
        }
    }
}
