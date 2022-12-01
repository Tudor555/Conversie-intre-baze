using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Conversie_baze
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BazeCalcul ();
        }
        private static void BazeCalcul()
        {
            int b1, b2, intreg = 0, frac = 0, frcifre0 = 0;
            bool checkfrac = false;
            string conv, cifrefstring;
            bool perioada = false;
            char[] hex = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
            char[] fchar = new char[] { ',', '.' };
            Console.WriteLine("Introduceti baza din care vreti sa convertiti (b1): ");
            b1 = BaseCheck();
            Console.WriteLine("Introduceti baza in care vreti sa convertiti (b2): ");
            b2 = BaseCheck2();
            Console.WriteLine("Introduceti numarul pe care vreti sa il convertiti: ");
            conv = CONV(ref b1, ref hex);
            Console.Clear();
            Console.WriteLine($"Numarul {conv} din baza {b1} in baza {b2} este: \n");
            if (conv.Contains('-')) // in caz de nr negativ, folosim semnul negativ in fata nr convertit
            {
                conv = conv.Replace("-", "");
                Console.Write("-");
            }
            try
            {
                checked 
                {
                    if (b1 > 10)
                    {
                        string convint, convfrac = "0";
                        convint = conv;
                        if (conv.Contains(',') || conv.Contains('.'))
                        {
                            string[] nrc = conv.Split(fchar);
                            convint = nrc[0];
                            convfrac = nrc[1];
                            if (nrc.Length > 2)
                            {
                                Console.WriteLine("Nr introdus gresit!!!!");
                                return;
                            }
                            checkfrac = true;
                        }
                        char[] cifre = convint.ToCharArray();
                        int[] cifreint = new int[cifre.Length];
                        int countcif = 0;
                        for (int i = 0; i < cifre.Length; i++)
                        {
                            for (int j = 0; j < hex.Length; j++)
                            {
                                if (cifre[i] == hex[j])
                                {
                                    cifreint[i] = j;
                                }
                            }
                        }
                        for (int i = cifre.Length - 1; i >= 0; i--)
                        {
                            intreg = intreg + cifreint[i] * (int)Math.Pow(b1, countcif);
                            countcif++;
                        }
                        //// de aici partea fractiala
                        if (checkfrac)
                        {
                            double fractie;                                            
                            int cifref = 0, countfrac = -1;
                            char[] cifreb10pf = convfrac.ToCharArray();
                            int[] cifrefrac = new int[convfrac.Length];
                            for (int i = 0; i < cifreb10pf.Length; i++)
                            {
                                for (int j = 0; j < hex.Length; j++)
                                {
                                    if (cifreb10pf[i] == hex[j])
                                    {
                                        cifrefrac[i] = j;
                                    }
                                }                               
                            }
                            fractie = 0;
                            for (int i = 0; i < cifrefrac.Length; i++)
                            {
                                fractie = fractie + cifrefrac[i] * Math.Pow(b1, countfrac);
                                countfrac--;
                            }
                            int countperiod = 0;
                            while (fractie != 0 && countperiod < 9)
                            {
                                cifref = 10 * cifref + (int)Math.Floor(fractie * 10);
                                fractie = (fractie * 10) - (int)Math.Floor(fractie * 10);
                                countperiod++;
                            }
                            cifrefstring = Convert.ToString(cifref); 
                            while (countperiod != Convert.ToString(cifref).Length)
                            {
                                frcifre0++;
                                countperiod--;
                            }
                            while (frcifre0 != 0)
                            {
                                cifrefstring = string.Concat("0", Convert.ToString(cifref));
                                frcifre0--;
                            }
                            conv = string.Concat(Convert.ToString(intreg), ',', cifrefstring); 
                        }
                        else
                        {
                            conv = Convert.ToString(intreg); 
                        }
                        b1 = 10; 
                    }
                    if (conv.Contains(',') || conv.Contains('.'))
                    {
                        string[] nrc = conv.Split(fchar);
                        intreg = int.Parse(nrc[0]);
                        frac = int.Parse(nrc[1]);
                        if (nrc.Length > 2)
                        {
                            Console.WriteLine("nr introdus gresit!");
                            return;
                        }
                        checkfrac = true;
                    }
                    else
                    {
                        intreg = int.Parse(conv);
                    }
                    int intb10 = 0;
                    int countb10 = 1;
                    int intreglength = Convert.ToString(intreg).Length;
                    if (b1 <= 10 && b2 <= 10) 
                    {
                        int intregaux = intreg;
                        int countcif = 0;
                        int[] cifre = new int[intreglength];
                        for (int i = 0; i < cifre.Length; i++)
                        {
                            cifre[i] = intregaux % 10;
                            intregaux = intregaux / 10;
                        }
                        for (int i = 0; i < cifre.Length; i++)
                        {
                            intb10 = intb10 + cifre[i] * (int)Math.Pow(b1, countcif);
                            countcif++;
                        }                       
                        intreg = intb10;                        
                        intregaux = intreg;
                        while (intregaux > 0)
                        {                            
                            intregaux = intregaux / b2;
                            countb10++;
                        }
                        int[] r = new int[countb10];
                        int count2 = 0;
                        while (intreg > 0)
                        {
                            r[count2] = intreg % b2;
                            intreg = intreg / b2;
                            count2++;
                        }
                        for (int i = r.Length - 2; i >= 0; i--)
                        {
                            Console.Write(r[i]);
                        }
                        if (r.Length < 2)
                        {
                            Console.Write("0");
                        }

                        if (checkfrac)
                        {
                            Console.Write(",");
                            decimal fractie;          
                            int cifref = 0, countfrac = -1;
                            char[] cifrefrac = conv.Split(fchar)[1].ToCharArray();
                            int[] cifreb1 = new int[cifrefrac.Length];
                            for (int i = 0; i < cifrefrac.Length; i++)
                            {
                                cifreb1[i] = int.Parse(Convert.ToString(cifrefrac[i]));
                            }
                            fractie = 0;
                            for (int i = 0; i < cifreb1.Length; i++)
                            {
                                fractie = fractie + cifreb1[i] * (decimal)Math.Pow(b1, countfrac);
                                countfrac--;
                            }   
                            int countperiod = 0;
                            decimal[] fractierest = new decimal[8];
                            while (fractie != 0 && countperiod < 8)
                            {
                                cifref = 10 * cifref + (int)Math.Floor(fractie * b2);
                                fractie = (fractie * b2) - (int)Math.Floor(fractie * b2);
                                fractierest[countperiod] = fractie;
                                for (int i = 0; i <= countperiod - 1; i++)
                                {
                                    if (Decimal.Compare(fractierest[i], fractie) == 0) 
                                    {
                                        perioada = true;
                                        break;
                                    }
                                }
                                if (perioada) break;
                                countperiod++;
                            }
                            cifrefstring = Convert.ToString(cifref); 
                            while (countperiod != Convert.ToString(cifref).Length)
                            {
                                frcifre0++;
                                countperiod--;
                            }
                            while (frcifre0 != 0)
                            {
                                cifrefstring = string.Concat("0", Convert.ToString(cifref));
                                frcifre0--;
                            }
                            Console.Write(cifrefstring);
                        }
                    }
                    if (b2 > 10)
                    {
                        int intregaux = intreg;
                        if (b1 < 10)
                        {
                            int countcif = 0;
                            int[] cifre = new int[intreglength];

                            for (int i = 0; i < cifre.Length; i++)
                            {
                                cifre[i] = intregaux % 10;
                                intregaux = intregaux / 10;
                            }
                            for (int i = 0; i < cifre.Length; i++)
                            {
                                intb10 = intb10 + cifre[i] * (int)Math.Pow(b1, countcif);
                                countcif++;
                            }
                            intreg = intb10;
                        }
                        intregaux = intreg;

                        while (intregaux > 0)
                        {
                            intregaux = intregaux / b2;
                            countb10++;
                        }

                        string[] r = new string[countb10];
                        int count2 = 0;
                        while (intreg > 0)
                        {
                            r[count2] = Convert.ToString(intreg % b2);
                            intreg = intreg / b2;
                            count2++;
                        }
                        string[] hexb = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };

                        for (int i = 0; i < r.Length; i++)
                        {
                            for (int j = 0; j < hexb.Length; j++)
                            {
                                if (r[i] == Convert.ToString(j))
                                {
                                    r[i] = hexb[j];
                                }
                            }
                        }
                        for (int i = r.Length - 2; i >= 0; i--)
                        {
                            Console.Write(r[i]);
                        }
                        if (r.Length < 2)
                        {
                            Console.Write("0");
                        }
                        if (checkfrac)
                        {
                            Console.Write(",");
                            decimal fractie;                  
                            int countfrac = -1;
                            char[] cifrefrac = conv.Split(fchar)[1].ToCharArray(); 
                            int[] cifreb1 = new int[cifrefrac.Length];
                            for (int i = 0; i < cifrefrac.Length; i++)
                            {
                                cifreb1[i] = int.Parse(Convert.ToString(cifrefrac[i]));                               
                            }
                            fractie = 0;
                            for (int i = 0; i < cifreb1.Length; i++)
                            {
                                fractie = fractie + cifreb1[i] * (decimal)Math.Pow(b1, countfrac);
                                countfrac--;
                            }    
                            int countperiod = 0;
                            string[] cifresuperior = new string[15];
                            decimal[] fractierest = new decimal[15]; 
                            while (fractie != 0 && countperiod < 15)
                            {

                                cifresuperior[countperiod] = Convert.ToString((int)Math.Floor(fractie * b2));
                                fractie = (fractie * b2) - (int)Math.Floor(fractie * b2);
                                fractierest[countperiod] = fractie;
                                for (int i = 0; i <= countperiod - 1; i++)
                                {                                    
                                    if (Decimal.Compare(fractierest[i], fractie) == 0)
                                    {                                        
                                        perioada = true;
                                        break;
                                    }
                                }
                                if (perioada) break;
                                countperiod++;
                            }

                            for (int i = 0; i < cifresuperior.Length; i++)
                            {
                                for (int j = 0; j < hexb.Length; j++)
                                {
                                    if (cifresuperior[i] == Convert.ToString(j))
                                    {
                                        cifresuperior[i] = hexb[j];
                                    }
                                }
                            }
                            for (int i = 0; i < cifresuperior.Length; i++)
                            {
                                Console.Write(cifresuperior[i]);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Convertorul nu functioneaza pt numere introduse mai mari de " + Int32.MaxValue + ".");
            }
            if (perioada)
            {
                Console.Write("... numarul contine o perioada. Perioada incepe cu ultima cifra.");
            }
            Console.WriteLine("\nPentru a iesi din aplicatie apasati orice buton.");
            Console.ReadKey();
        }
        private static string CONV(ref int b1, ref char[] hex)
        {
        tryagain:
            string conv = Console.ReadLine();
            conv = conv.ToLower();
            string convaux = conv;
            char[] sep = new char[] { ',', '.' };
            bool fractie = false;
            if (conv.Contains(',') || conv.Contains('.'))
            {
                convaux = conv.Split(sep)[0];
                fractie = true;
            }
        testbase:
            int basecount = 0;
            char[] cifre = convaux.ToCharArray();
            for (int j = 0; j < cifre.Length; j++)
            {
                for (int i = 0; i < b1; i++)
                {
                    if (cifre[j] == hex[i])
                    {
                        basecount++;
                    }
                }
            }
            if (basecount != cifre.Length || convaux == "")
            {
                Console.WriteLine("Numarul introdus contine cifre necorespunzatoare bazei alese! Incearca din nou!");
                goto tryagain;
            }
            if (fractie)
            {
                convaux = conv.Split(sep)[1];
                fractie = false; 
                goto testbase; 
            }
            return conv;

        }
        private static int BaseCheck2()
        {
            string basstring = Console.ReadLine();
            int b;
        tryagain:
            while (!int.TryParse(basstring, out b))
            {
                Console.WriteLine("Valoare trebuie sa fie intre 2 si 16!");
                Console.Write("b2 = ");
                basstring = Console.ReadLine();
            }
            if (b < 2 || b > 16)
            {
                basstring = "wrong";
                goto tryagain;
            }
            return b;
        }
        private static int BaseCheck()
        {
            string basstring = Console.ReadLine();
            int b;
        tryagain:
            while (!int.TryParse(basstring, out b))
            {
                Console.WriteLine("Valoare trebuie sa fie intre 2 si 16!");
                Console.Write("b1 = ");
                basstring = Console.ReadLine();
            }
            if (b < 2 || b > 16)
            {
                basstring = "wrong";
                goto tryagain; 
            }
            return b;
        }
    }
}