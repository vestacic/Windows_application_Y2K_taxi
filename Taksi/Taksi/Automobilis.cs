using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taksi
{
    class Automobilis
    {
        public string pavarde { get; set; }
        public string marke { get; set; }
        public string numeris { get; set; }
        public int metai { get; set; }
        public string spalva { get; set; }
        public int rida { get; set; }
        public Automobilis(string pavarde, string marke, string numeris, int metai, string spalva, int rida)
        {
            this.pavarde = pavarde;
            this.marke = marke;
            this.numeris = numeris;
            this.metai = metai;
            this.spalva = spalva;
            this.rida = rida;
        }
        public override string ToString()
        {
            string eilute = string.Format("|{0,-10}|{1, -10}|{2, -8}|{3, 5}|{4, -10}|{5, 5}|", pavarde, marke, numeris, metai, spalva, rida);
            return eilute;
        }
    }
}
