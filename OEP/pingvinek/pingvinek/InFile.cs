using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextFile;

namespace pingvinek
{
    public class InFile
    {
        private TextFileReader reader;

        public InFile(string fn)
        {
            reader = new TextFileReader(fn);
        }

        public bool Read(out Megfigyelés m)
        {
            m = null;
            bool okay = reader.ReadLine(out string line);
            if (okay)
            {
                string[] darabol = line.Split(new char[]{' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
                m = new Megfigyelés(darabol[0], int.Parse(darabol[1]));
                for (int i = 2; i < darabol.Length; i+=3)
                {
                    m.Add(new Pingvin(
                        darabol[i], 
                        darabol[i+1], 
                        int.Parse(darabol[i+2]
                        )));
                }
            }
            return okay;
        }
    }
}
