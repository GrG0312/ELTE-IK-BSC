using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TextFile;

namespace ZhGyak
{
    public class InFile
    {
        private TextFileReader reader;

        public InFile(string fn) { 
        reader = new TextFileReader(fn);
        }

        public bool Read(out Megfigyelés megf) {
            megf = null;
            if (reader.ReadLine(out string line)) {
                string[] temp = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                megf = new Megfigyelés(temp[0], int.Parse(temp[1]));
                for (int i = 2; i < temp.Length; i+=3)
                {
                    megf.össz(new Megfigyelés.Pingvin(
                        temp[i],
                        temp[i + 1],
                        int.Parse(temp[i + 2])
                        ));
                }
                return true;
            }

            return false;
        }
    }
}
