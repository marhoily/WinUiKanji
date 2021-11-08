using Shared;

namespace WinUiKanji
{
    public sealed partial class MainControl
    {
        public MainViewModel ViewModel { get; } = new(new PlayerImpl());

        public MainControl() => InitializeComponent();
    }
}
