using System.Windows.Controls;
using TreeViewTest.ViewModels;

namespace TreeViewTest.Views;

public partial class InstrumentsMenuView : Page
{
    public InstrumentsMenuView()
    {
        InitializeComponent();
    }

    private async void InitializedHandler(object sender, System.EventArgs e)
    {
        if (DataContext is InstrumentsMenuViewModel viewModel)
        {
            await viewModel.BeginInitializeRootNode();
        }
    }
}
