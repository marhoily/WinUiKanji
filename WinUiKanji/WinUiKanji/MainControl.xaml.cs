namespace WinUiKanji
{
    public sealed partial class MainControl
    {
        public MainViewModel ViewModel { get; } = new();

        public MainControl() => InitializeComponent();
    }
}
