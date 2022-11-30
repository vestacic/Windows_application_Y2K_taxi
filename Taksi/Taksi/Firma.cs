using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taksi
{
    class Firma
    {
        const int cn = 100;
        Automobilis[] a;
        public int n { get; private set; }
        public Firma()
        {
            n = 0;
            a = new Automobilis[cn];
        }
        public void deti(Automobilis aa) { a[n++] = aa; }
        public Automobilis imti (int i) { return a[i]; }
    }
}
