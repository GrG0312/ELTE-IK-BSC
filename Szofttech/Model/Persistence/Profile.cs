using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman_Prototype1.Persistence
{
    public class Profile
    {
        public string Name { get; private set; }
        public int PlayedGames { get; private set; }
        public int GamesWon { get; private set; }
        public string SpritePath {  get; private set; }
        public TimeSpan PlayTime { get; private set; }

        public Profile(string name, int playedGames, int gamesWon, string spritePath, int hour = 0, int min = 0, int sec = 0)
        {
            PlayTime = new TimeSpan(hour, min, sec);
            Name = name;
            PlayedGames = playedGames;
            GamesWon = gamesWon;
            SpritePath = spritePath;
        }
        public void UpdateStats(bool won, int playtime)
        {
            if (won) { GamesWon++; }
            PlayTime += TimeSpan.FromSeconds(playtime);
            PlayedGames++;
        }
        public override string ToString()
        {
            return $"{Name};{PlayedGames};{GamesWon};{SpritePath}";
        }
    }
}
