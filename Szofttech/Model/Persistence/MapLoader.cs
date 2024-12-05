using Bomberman_Prototype1.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman_Prototype1.Persistence
{
    public class MapLoader : IMapLoader
    {
        private int boardType; 
        private int rows;
        private int cols;
        private Field[,]? map;

        public static readonly Dictionary<int, string> maps = new()
        {
            { 1, "../../../Maps/map1.txt" },
            { 2, "../../../Maps/map2.txt" },
            { 3, "../../../Maps/map3.txt" }
        };

        public Dictionary<int, string> Maps // this is needed for the IMapLoader interface only
        {
            get { return maps; }
        }

        public Field[,] Map { 
            get {
                if (map != null)
                {
                    Field[,] returnedMap = new Field[cols, rows];
                    for (int i = 0; i < rows; ++i)
                    {
                        for (int j = 0; j < cols; ++j)
                        {
                            returnedMap[j, i] = map[j, i]; // this is so bad
                        }
                    }

                    return returnedMap;
                }
                else { return null!; }
            }
        }

        // This function loads the map from the file system into the MapLoader's inner buffer stage
        public void LoadMap(int mapID)
        {
            string mapuri = maps.GetValueOrDefault(mapID, "../../../Maps/map" + mapID + ".txt");
            using (StreamReader reader = new StreamReader(mapuri))
            {
                string? current = reader.ReadLine();
                if (current != null) {
                    switch (current)
                    {
                        case "Cyber": boardType = 1; break;
                        case "Desert": boardType = 2; break;
                        case "Swamp": boardType = 3; break;
                        default: boardType = 1; break;
                    }}

                current = reader.ReadLine();
                if (current != null)
                {
                    string[] line = current.Split(' ');
                    int.TryParse(line[1], out int rows);
                    int.TryParse(line[0], out int cols);
                    this.rows = rows;
                    this.cols = cols;

                    //cols - oszlopszám - 19
                    //rows - sorszám - 11
                    map = new Field[cols, rows];

                    for (int i = 0; i < rows; ++i)
                    {
                        current = reader.ReadLine();
                        if (current != null)
                        {
                            line = current.Split(' ');

                            for (int j = 0; j < cols; ++j)
                            {
                                map[j, i] = ParseFieldToken(line[j]);
                            }
                        }
                    }
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }

            MapLoaded?.Invoke(this, EventArgs.Empty);
        }

        public Field ParseFieldToken(string c)
        {
            // n - empty field
            // b - unbreakable wall
            // r - breakable wall
            // x - box
            switch (c)
            {
                case "b":
                    return Field.WALL;
                case "n":
                    return Field.EMPTY;
                case "r":
                    return Field.WEAK_WALL;
                case "x":
                    return Field.BOX;
                default:
                    return Field.EMPTY;
            }
        }

        public event EventHandler<EventArgs>? MapLoaded;
    }
}
