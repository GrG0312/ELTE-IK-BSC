using Bomberman_Prototype1.Model;
using Bomberman_Prototype1.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman_Prototype1_Test
{
    public class MockedMapLoader : IMapLoader
    {
        public Dictionary<int, string> Maps
        {
            get
            {
                return new()
                    {
                        { 1, "../../../Maps/map1.txt" },
                        { 2, "../../../Maps/map2.txt" },
                        { 3, "../../../Maps/map3.txt" }
                    };
            }
        }

        public Field[,] Map
        {
            get
            {
                Field[,] map = new Field[3, 3];
                for (int i = 0; i < 3; ++i)
                {
                    for (int j = 0; j < 3; ++j)
                    {
                        map[i, j] = Field.EMPTY;
                    }
                }
                map[0, 0] = Field.WALL;
                map[0, 1] = Field.WEAK_WALL;
                map[1, 1] = Field.BOX;

                return map;
            }
        }

        public event EventHandler<EventArgs>? MapLoaded;

        public void LoadMap(int mapID)
        {
            MapLoaded?.Invoke(this, EventArgs.Empty);
        }
    }
}
