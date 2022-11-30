using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Taksi
{
    public partial class Form1 : Form
    {
        string cfr;
        Firma firma1;
        Firma firma2;
        string pavadinimas1, pavadinimas2;
        List<Auto> masinuamzius;
        List<Auto> iterpimas;
        public Form1()
        {
            InitializeComponent();
            //pirmosfirmospavadinimas.BackColor = System.Drawing.Color.Transparent;
            //antraspvadinimas.BackColor = System.Drawing.Color.Transparent;
            //eks.BackColor = System.Drawing.Color.Transparent;
            isvestitoolStripMenuItem1.Enabled = false;
            eksploatacijatoolStripMenuItem1.Enabled = false;
            spalvostoolStripMenuItem1.Enabled = false;
            sarasoformavimastoolStripMenuItem1.Enabled = false;
            rikiavimastoolStripMenuItem1.Enabled = false;
            iterptitoolStripMenuItem1.Enabled = false;
            pasalintitoolStripMenuItem1.Enabled = false;
        }
        static Firma skaitymas(string cfd, out string pavadinimas)
        {
            Firma f = new Firma();
            using (StreamReader reader=new StreamReader(cfd))
            {
                string line = reader.ReadLine();
                pavadinimas = line;
                while((line=reader.ReadLine())!=null)
                {
                    string[] parts = line.Split(';');
                    string pav = parts[0];
                    string mark = parts[1];
                    string nr = parts[2];
                    int metai = int.Parse(parts[3]);
                    string spalva = parts[4];
                    int rida = int.Parse(parts[5]);
                    Automobilis a = new Automobilis(pav, mark, nr, metai, spalva, rida);
                    f.deti(a);
                }
            }
            return f;
        }
        static List<Auto> sarasoskaitymas(string cfd)
        {
            List<Auto> a = new List<Auto>();
            using(StreamReader reader=new StreamReader(cfd))
            {
                string line;
                while((line=reader.ReadLine())!=null)
                {
                    string[] parts = line.Split(';');
                    string numeris = parts[0];
                    string marke = parts[1];
                    int metai = int.Parse(parts[2]);
                    Auto x = new Auto(numeris, marke, metai);
                    a.Add(x);
                }
            }
            return a;
        }

        private void baigtitoolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        static void isvestis(Firma f, string cfr, string pavadinimas, string dalis, out string a)
        { 
            string etikete = string.Format("|{0,-10}|{1, -10}|{2, -8}|{3, -5}|{4, -10}|{5, -5}|", "Pavardė", "Markė", "Numeris", "Metai", "Spalva", "Rida");
            using(var fr=File.AppendText(cfr))
            {
                fr.WriteLine(dalis);
                fr.WriteLine(pavadinimas);
                fr.WriteLine("-------------------------------------------------------");
                a = "-------------------------------------------------------\r\n";
                fr.WriteLine(etikete);
                a = a + etikete + "\r\n";
                fr.WriteLine("-------------------------------------------------------");
                a = a + "-------------------------------------------------------\r\n";
                for (int i=0; i<f.n; i++)
                {
                    fr.WriteLine(f.imti(i).ToString());
                    a = a + f.imti(i).ToString() + "\r\n";
                }
                fr.WriteLine("-------------------------------------------------------");
                a = a + "-------------------------------------------------------\r\n";
                fr.WriteLine();
            }
        }

        private void isvestitoolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.Title = "Pasirinkite rezultatų failą";
            DialogResult result = saveFileDialog1.ShowDialog();
            cfr = saveFileDialog1.FileName;
            if (File.Exists(cfr)) File.Delete(cfr);
            string a, b;
            isvestis(firma1, cfr, pavadinimas1, "Pirmos firmos duomenys:", out a);
            isvestis(firma2, cfr, pavadinimas2, "Antros firmos duomenys:", out b);
            firma1richTextBox1.Text = a;
            firma2richTextBox1.Text = b;
        }
        static int eksploatacija(Firma f)
        {
            int ats = 0;
            for(int i=1; i<f.n; i++)
            {
                if (f.imti(i).rida > ats) ats = f.imti(i).rida;
            }
            return ats;
        }
        static void eksploatacijastring (Firma f, int x, ref string ats)
        {
            for(int i=0; i<f.n; i++)
            {
                if (f.imti(i).rida == x) ats = ats + f.imti(i).marke + " "; 
            }
        }
        static void eilutesisvestis(string cfr, string eilute)
        {
            using (var fr=File.AppendText(cfr))
            {
                fr.WriteLine(eilute);
            }
        }
        private void eksploatacijatoolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int i1 = eksploatacija(firma1);
            int i2 = eksploatacija(firma2);
            int ats;
            if (i1 > i2) ats = i1;
            else ats = i2;
            string a = "Labiausiai eksploatuojamos markės: ";
            eksploatacijastring(firma1, ats, ref a);
            eksploatacijastring(firma2, ats, ref a);
            eks.Text = a;
            eilutesisvestis(cfr, a);
        }
        static string spalvos(Firma f, Firma ieskoma)
        {
            string ats = "";
            for(int i=0; i<f.n; i++)
            {
                string spalva = f.imti(i).spalva;
                int x = 0;
                for(int j=0; j<ieskoma.n; j++)
                {
                    if(spalva==ieskoma.imti(j).spalva) { x = 100; break; }
                }
                if (x != 100) ats = ats + spalva + " ";
            }
            return ats;
        }
        private void spalvostoolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string a= spalvos(firma1, firma2);
            if (a == "")
                a = "Tokių spalvų nėra";
            string b = spalvos(firma2, firma1);
            if (b == "")
                b = "Tokių spalvų nėra!";
            spalvosetikete.Text = a;
            spalvuetikete2.Text = b;
            eilutesisvestis(cfr, "Spalvos, kurios yra pirmoje firmoje, tačiau nėra antroje:");
            eilutesisvestis(cfr, a);
            eilutesisvestis(cfr, "Spalvos, kurios yra antroje firmoje, tačiau nėra pirmoje:");
            eilutesisvestis(cfr, b);
        }

        private void sarasoformavimastoolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int a = int.Parse(textBox1.Text);
            int b = int.Parse(textBox2.Text);
            masinuamzius = new List<Auto>();
            pridetiauto(firma1, masinuamzius, a, b);
            pridetiauto(firma2, masinuamzius, a, b);
            string x= "";
            if(masinuamzius.Count!=0)
            {
                string c = string.Format("Mašinų, kurių amžius nuo {0} iki {1}, sąrašas:", a, b);
                eilutesisvestis(cfr, c);
                isvestisarasa(masinuamzius, cfr, ref x);
                sarasasrichTextBox1.Text = x;
                rikiavimastoolStripMenuItem1.Enabled = true;
            }
            else
            {
                string c = string.Format("Mašinų, kurių amžius nuo {0} iki {1}, nėra!", a, b);
                eilutesisvestis(cfr, c);
                sarasasrichTextBox1.Text = c;
                iterptitoolStripMenuItem1.Enabled = true;
            }
            pasalintitoolStripMenuItem1.Enabled = true;
        }
        static void pridetiauto(Firma f, List<Auto> aa, int a, int b)
        {
            for(int i=0; i<f.n; i++)
            {
                int x = f.imti(i).metai;
                if(x>=a && x<=b)
                {
                    string numeris = f.imti(i).numeris;
                    string marke = f.imti(i).marke;
                    Auto aaa = new Auto(numeris, marke, x);
                    aa.Add(aaa);
                }
            }
        }
        static void isvestisarasa (List<Auto> a, string cfr, ref string ats)
        {
            string etikete = string.Format("|{0, -8}|{1, -10}|{2, 5}|", "Numeris", "Markė", "Metai");
            using (var fr=File.AppendText(cfr))
            {
                fr.WriteLine("---------------------------");
                ats = ats + "---------------------------\r\n";
                fr.WriteLine(etikete);
                ats = ats + etikete + "\r\n";
                fr.WriteLine("---------------------------");
                ats = ats + ("---------------------------\r\n");
                for (int i=0; i<a.Count; i++)
                {
                    fr.WriteLine(a[i].ToString());
                    ats=ats+a[i].ToString()+"\r\n";
                }
                fr.WriteLine("---------------------------");
                ats = ats + ("---------------------------\r\n");
            }
        }

        private void rikiavimastoolStripMenuItem1_Click(object sender, EventArgs e)
        {
            rikiavimas(masinuamzius);
            string ats = "";
            eilutesisvestis(cfr, "Išrikiuotas mašinų sąrašas:");
            isvestisarasa(masinuamzius, cfr, ref ats);
            sarasasrichTextBox1.Text = "Išrikiuotas mašinų sąrašas:\r\n"+ats;
            iterptitoolStripMenuItem1.Enabled = true;
        }
        static void rikiavimas(List<Auto> a)
        {
            for(int i=0; i<a.Count-1; i++)
            {
                Auto dabar = a[i];
                Auto didziausias = a[i];
                int sukeisti = i;
                for(int j=i+1; j<a.Count; j++)
                {
                    if(didziausias<a[j]) { sukeisti = j; didziausias = a[j]; }
                }
                a[i] = didziausias;
                a[sukeisti] = dabar;
            }    
        }

        private void iterptitoolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog3 = new OpenFileDialog();
            openFileDialog3.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog3.Title = "Pasirinkite duomenų failą";
            DialogResult result = openFileDialog3.ShowDialog();
            iterpimas = new List<Auto>();
            if(result==DialogResult.OK)
            {
                string cfd3 = openFileDialog3.FileName;
                iterpimas = sarasoskaitymas(cfd3);
                string xy = "";
                eilutesisvestis(cfr, "Įterpiamų automobilių sąrašas:");
                isvestisarasa(iterpimas, cfr, ref xy);
                for(int i=0; i<iterpimas.Count; i++)
                {
                    int x=rastiindeksa(iterpimas[i], masinuamzius);
                    masinuamzius.Insert(x, iterpimas[i]);
                }
                string ats = "";
                eilutesisvestis(cfr, "Išrikiuotas sąrašas su naujai įterptais taksi:");
                isvestisarasa(masinuamzius, cfr, ref ats);
                sarasasrichTextBox1.Text = "Išrikiuotas sąrašas su naujai įterptais taksi: \r\n" + ats;
                rikiavimastoolStripMenuItem1.Enabled = true;
            }
        }
        static int rastiindeksa(Auto a, List<Auto>sarasas)
        {
            int ats;
            for (ats = 0; (ats < sarasas.Count) && (a < sarasas[ats]); ats++)
            { }
                return ats;
        }
        private void pasalintitoolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string marke = textBox3.Text;
            ismesti(marke, masinuamzius);
            string ats = "";
            string a = string.Format("Sąrašas, pašalinus markės {0} automobilius:", marke);
            eilutesisvestis(cfr, a);
            if (masinuamzius.Count == 0)
            {
                eilutesisvestis(cfr, "Sąrašas tuščias!");
                sarasasrichTextBox1.Text = "Po pašalinimo sąrašas tapo tuščias!";
            }
            else
            {
                isvestisarasa(masinuamzius, cfr, ref ats);
                string b = a + "\r\n" + ats;
                sarasasrichTextBox1.Text = b;
            }
        }
        static void ismesti(string marke, List<Auto> a)
        {
            int m = 0;
            for(int i=0; i<a.Count; i++)
            {
                if (a[i].marke == marke) { a.RemoveAt(i); i--; }
            }
        }
        private void ivestitoolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.Title = "Pasirinkite duomenų failą";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string cfd1 = openFileDialog1.FileName;
                firma1 = skaitymas(cfd1, out pavadinimas1);
                string a = File.ReadAllText(cfd1);
                firma1richTextBox1.Text = a;
                pirmosfirmospavadinimas.Text = pavadinimas1;
            }
            OpenFileDialog openFileDialog2 = new OpenFileDialog();
            openFileDialog2.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog2.Title = "Pasirinkite duomenų failą";
            DialogResult result2 = openFileDialog2.ShowDialog();
            if(result2==DialogResult.OK)
            {
                string cfd2 = openFileDialog2.FileName;
                firma2 = skaitymas(cfd2, out pavadinimas2);
                antraspvadinimas.Text = pavadinimas2;
                string b = File.ReadAllText(cfd2);
                firma2richTextBox1.Text = b;
            }
            isvestitoolStripMenuItem1.Enabled = true;
            eksploatacijatoolStripMenuItem1.Enabled = true;
            spalvostoolStripMenuItem1.Enabled = true;
            sarasoformavimastoolStripMenuItem1.Enabled = true;

        }
    }
}
