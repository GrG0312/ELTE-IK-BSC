using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beadandó
{
    public class Telephely
    {
        private class TruckNotFoundException : Exception { }

        public string cím;
        private List<Kamion> járművek;

        public Telephely(string cím)
        {
            this.cím = cím;
            this.járművek = new List<Kamion>();
        }

        public void Jön(Kamion k)
        {
            járművek.Add(k);
        }

        public void Megy(Kamion k)
        {
            if (!járművek.Contains(k))
            {
                throw new TruckNotFoundException();
            }
            járművek.Remove(k);
        }

        public void Megy(Kamion k, Telephely hova)
        {
            if (!járművek.Contains(k))
            {
                throw new TruckNotFoundException();
            }
            k.Vált(hova);
        }

        public int Mennyi()
        {
            return járművek.Count;
        }

        public double MaxTeher()
        {
            double sum = 0;
            foreach (Kamion k in járművek)
            {
                sum += k.terhel;
            }
            return sum;
        }
    }
}
