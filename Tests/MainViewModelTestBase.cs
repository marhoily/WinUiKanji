using Shared;

namespace Tests
{
    public abstract class MainViewModelTestBase
    {
        protected static readonly Card C1 = C(1);
        protected static readonly Card C2 = C(2);
        protected static readonly Card C3 = C(3);
        protected readonly FakePlayer _player = new ();

        private static Card C(int number) => new Card { Meaning = "m" + number, ToPronounce = "p" + number };
    }
}