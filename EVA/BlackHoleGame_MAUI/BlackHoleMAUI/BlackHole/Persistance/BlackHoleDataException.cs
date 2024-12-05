using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackHole.Persistance
{
    public class BlackHoleDataException : Exception
    {
        public BlackHoleDataException() { }

        public BlackHoleDataException(string message) : base(message) { }

        public BlackHoleDataException(string message, Exception innerException) : base(message, innerException) { }
    }
}
