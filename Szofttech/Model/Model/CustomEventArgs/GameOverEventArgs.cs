using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman_Prototype1.Model.CustomEventArgs
{
    public class GameOverEventArgs
    {
        public int WinnerID { get; private set; }
        public int PlayedTime { get; private set; }
        public GameOverEventArgs(int w, int p)
        {
            WinnerID = w;
            PlayedTime = p;
        }
    }
}
