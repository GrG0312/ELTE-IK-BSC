using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackHole.Persistance
{
    public interface IBlackHoleDataManager
    {
        Task SaveGame(string path, int currentParty, int party1Point, int Party2Point, int gameSize, BlackHoleTable table);

        Task<(int, int, int, int, BlackHoleTable)> LoadGame(string path);
    }
}
