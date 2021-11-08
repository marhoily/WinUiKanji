using System.Threading.Tasks;

namespace Shared
{
    public interface IPlayer
    {
        Task Blimp();
        Task Say(string language, string text);
    }
}