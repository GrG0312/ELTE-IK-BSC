using Bomberman_Prototype1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman_Prototype1.Persistence
{
    public interface IMapLoader
    {
        public Dictionary<int, string> Maps { get; } // this is the dictionary that contains the URIs of the maps
        public Field[,] Map { get; } // This is the property for the map that gets loaded into the MapLoader

        public void LoadMap(int mapID); // this loads the map from the file system into the MapLoader

        public event EventHandler<EventArgs>? MapLoaded;
    }
}
