using TreeViewTest.Models.Instruments;

namespace TreeViewTest.Models.EventArgs
{
    public enum RequestState
    {
        RequestSent,
        RequestReceived
    }

    public class RequestEventArgs : System.EventArgs
    {
        public InstrumentNode? TreeView { get; set; }
        public bool IsChanged { get; set; }
        public RequestState State { get; set; }

        public RequestEventArgs(InstrumentNode? root, RequestState currentState, bool isChanged = false)
        {
            TreeView = root;
            State = currentState;
            IsChanged = isChanged;
        }
    }
}
