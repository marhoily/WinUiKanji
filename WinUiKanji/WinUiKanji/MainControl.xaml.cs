using Microsoft.UI.Xaml.Controls;

namespace WinUiKanji
{
    public sealed partial class MainControl : UserControl
    {
        public MainControl()
        {
            this.InitializeComponent();
            ViewModel = new MainViewModel();
        }
        public MainViewModel ViewModel { get; }

    }
}
