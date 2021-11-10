using Shared;

namespace WinUiKanji
{
    public sealed partial class MainControl
    {
        public MainViewModel ViewModel { get; } = 
            new(new PlayerImpl(), new StudySet());

        public MainControl() => InitializeComponent();
    }
}
