using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackHole.Persistance
{
    public interface IStore
    {
        Task<IEnumerable<string>> GetFilesAsync();
        Task<DateTime> GetModifiedTimeAsync(string name);
    }
}
