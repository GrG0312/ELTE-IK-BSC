using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BlackHole.Persistance
{
    public class BlackHoleDataHandler : IBlackHoleDataManager
    {

        public async Task SaveGame(string path, int currentParty, int party1Point, int party2Point, int gameSize, BlackHoleTable table)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path)) // fájl megnyitása
                {
                    writer.WriteLine(currentParty); // soron következő játékos
                    await writer.WriteLineAsync(party1Point + " " + party2Point);
                    await writer.WriteLineAsync(gameSize.ToString());

                    for (int i = 0; i < gameSize; i++)
                    {
                        for (int j = 0; j < gameSize; j++)
                        {
                            await writer.WriteAsync(table.GetFieldValue(i,j) + " ");
                        }
                        await writer.WriteLineAsync();
                    }
                }
            }
            catch
            {
                throw new BlackHoleDataException();
            }
        }

        public async Task<(int, int, int, int, BlackHoleTable)> LoadGame(string path)
        {
            (int current, int party1, int party2, int size, BlackHoleTable table) retVal;
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    retVal.current = int.Parse(reader.ReadLine()!);
                    string line = await reader.ReadLineAsync() ?? String.Empty;
                    string[] splitLine = line.Split(' ');
                    retVal.party1 = int.Parse(splitLine[0]);
                    retVal.party2 = int.Parse(splitLine[1]);
                    retVal.size = int.Parse(reader.ReadLine()!);
                    retVal.table = new BlackHoleTable(retVal.size);
                    for (int i = 0; i < retVal.size; i++)
                    {
                        line = reader.ReadLine()!;
                        splitLine = line.Split(' ');
                        /*
                        retVal.table.SetFieldValue(i, 0, int.Parse(splitLine[0]));
                        retVal.table.SetFieldValue(i, 1, int.Parse(splitLine[1]));
                        retVal.table.SetFieldValue(i, 2, int.Parse(splitLine[2]));
                        retVal.table.SetFieldValue(i, 3, int.Parse(splitLine[3]));
                        retVal.table.SetFieldValue(i, 4, int.Parse(splitLine[4]));*/
                        for (int j = 0; j < retVal.size; j++)
                        {
                            retVal.table.SetFieldValue(i, j, int.Parse(splitLine[j]));
                        }
                    }
                }
                return retVal;
            }
            catch
            {
                throw new BlackHoleDataException();
            }
        }
    }
}
