using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace Kriptiranje
{
    public class KriptoServis
    {
        static string ulaz = "ulaz.txt";

        static List<string> lines = File.ReadAllLines(ulaz).ToList();

        List<Tuple<string, string>> mapirano = ListaNtorki(lines, ' ');

        static List<Tuple<string, string>> ListaNtorki(List<string> lines, char delimiter)
        {
            List<Tuple<string, string>> result = new List<Tuple<string, string>>();

            foreach (var line in lines)
            {
                string[] values = line.Split(delimiter);
                if (values.Length >= 2)
                {
                    result.Add(Tuple.Create(values[0], values[1]));
                }
            }
            return result;
        }

        static System.Collections.Generic.IEnumerable<string> DohvatiDvaZnaka(string input)
        {
            for (int i = 0; i < input.Length - 1; i += 2)
            {
                yield return $"{input[i]}{input[i + 1]}";
            }
        }

        static string KljucIzVrijednosti(List<Tuple<string, string>> kljucVrijednostParovi, string targetValue)
        {
            var par = kljucVrijednostParovi.Find(tuple => tuple.Item2.Trim().Equals(targetValue.Trim(), StringComparison.OrdinalIgnoreCase));

            return par != null ? par.Item1 : null;
        }

        static string VrijednostIzKljuca(List<Tuple<string, string>> kljucVrijednostParovi, string targetKey)
        {
            var par = kljucVrijednostParovi.Find(tuple => tuple.Item1.Trim().Equals(targetKey.Trim(), StringComparison.OrdinalIgnoreCase));

            return par != null ? par.Item2 : null;
        }

        public string Enkriptiraj(string a)
        {
            string b = "";
            foreach (char znak in a)
            {
                string targetEnkripcija = VrijednostIzKljuca(mapirano, znak.ToString());
                b += targetEnkripcija;
            }
            return b;

        }

        public string Dekriptiraj(string a)
        {
            string b = "";
            foreach (var par in DohvatiDvaZnaka(a))
            {
                string targetEnkripcija = KljucIzVrijednosti(mapirano, par.ToString());
                b = b + targetEnkripcija;
            }
            return b;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            KriptoServis servis = new KriptoServis();

            string niz = "aplikacija_ispravno_radi.";
            string kriptiraniNiz = servis.Enkriptiraj(niz);
            string dekriptiraniNiz = servis.Dekriptiraj(kriptiraniNiz);

            Console.WriteLine("Originalni niz:\t\t{0}", niz);
            Console.WriteLine("Kriptirani niz:\t\t{0}", kriptiraniNiz);
            Console.WriteLine("Dekriptirani niz:\t{0}", dekriptiraniNiz);

        }
    }
}
