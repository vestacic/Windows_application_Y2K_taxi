using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taksi
{
    class Auto
    {
        public string numeris { private set; get; }
        public string marke { private set; get; }
        public int metai { private set; get; }
        public Auto(string numeris, string marke, int metai)
        {
            this.numeris = numeris;
            this.marke = marke;
            this.metai = metai;
        }
        public override string ToString()
        {
            string eilute = string.Format("|{0, -8}|{1, -10}|{2, 5}|", numeris, marke, metai);
            return eilute;
        }
        public static bool operator > (Auto a, Auto b)
        {
            int x = string.Compare(a.marke, b.marke);
            return (x > 0 || x == 0 && a.metai > b.metai);
        }
        public static bool operator < (Auto a, Auto b)
        {
            int x = string.Compare(a.marke, b.marke);
            return (x < 0 || x == 0 && a.metai < b.metai);
        }
    }
}
