using Shared;
using Xunit;

namespace Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            new MainViewModel(new FakePlayer());
        }
    }
}