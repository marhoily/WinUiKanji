using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared
{
    public interface IStudySet
    {
        List<Card> GetShuffle();
        void Load(string fileName);
        Task SaveAsync();
    }
}