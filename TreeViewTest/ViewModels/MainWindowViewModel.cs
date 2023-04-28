using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using TreeViewTest.Models.Instruments;

namespace TreeViewTest.ViewModels
{
    public partial class MainWindowViewModel : DependencyObject
    {
        public IRelayCommand GetCodesCommand { get; set; }
        public IRelayCommand SelectAllCommand { get; set; }
        public IRelayCommand UnselectAllCommand { get; set; }
        public List<InstrumentNode> InstrumentsRootNode { get; set; }

        public List<string> SelectedCodes
        {
            get { return (List<string>)GetValue(SelectedCodesProperty); }
            set { SetValue(SelectedCodesProperty, value); }
        }

        public static readonly DependencyProperty SelectedCodesProperty =
            DependencyProperty.Register("SelectedCodes", typeof(List<string>), typeof(MainWindowViewModel), new PropertyMetadata(null));

  
        public MainWindowViewModel()
        {
            InstrumentsRootNode = PopulateRootNode();
            GetCodesCommand = new RelayCommand(GetCodes);
            SelectAllCommand = new RelayCommand(SelectAll);
            UnselectAllCommand = new RelayCommand(UnselectAll);
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

        private List<string> FindSelected(List<InstrumentNode> instruments, List<string> codes)
        {
            foreach (var instrument in instruments)
            {
                if (instrument.IsSelected is null || instrument.IsSelected.Value is true)
                {
                    if (instrument.Items.Count > 0)
                    {
                        FindSelected(instrument.Items, codes);
                    }

                    codes.Add(instrument.Name);
                }
            }

            return codes;
        }

        public void SelectAll()
        {
            SetSelectedTo(true, InstrumentsRootNode);
        }

        public void UnselectAll()
        {
            SetSelectedTo(false, InstrumentsRootNode);
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

        private static List<InstrumentNode> PopulateRootNode()
        {
            var list = new List<InstrumentNode>();

            for (int i = 0; i < 15; i++)
            {
                var node = new InstrumentNode($"Node #{i}", null, i + 1);
                PopulateSubNodes(node);
                list.Add(node);
            }

            //little bit rude, just for tests
            var negativeHitCountNode = new InstrumentNode("NegativeHitCountNode", null, -1);
            for (int i = 0; i < 2; i++)
            {
                var negativeHitCountSubNode = new InstrumentNode($"NegativeHitCountSubNode #{i}", negativeHitCountNode, -1);
                for (int j = 0; j < 2; j++)
                {
                    negativeHitCountSubNode.Items.Add(new InstrumentNode($"NoHitCountItem #{j}", negativeHitCountSubNode, -1));
                }
                negativeHitCountNode.Items.Add(negativeHitCountSubNode);
            }
            list.Add(negativeHitCountNode);

            return list;
        }

        private static void PopulateSubNodes(InstrumentNode node)
        {
            var nextNode = new InstrumentNode("SubNode", node, node.HitCount);
            for (int i = 0; i < 1000; i++)
            {
                nextNode.Items.Add(new InstrumentNode($"ItemNode #{i}", nextNode, new Random().Next(100, 10000)));
            }

            node.Items.Add(nextNode);
        }
    }
}
