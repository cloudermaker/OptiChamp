using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptiChamp
{
    class Program
    {
        static void Main(string[] args)
        {
            int N = int.Parse(Console.ReadLine());
            string input = Console.ReadLine();
            List<double> choice = new List<double>();
            List<int> allNB = new List<int>();
            string inp = input.Replace(" ", "");
            bool negativ = false;

            if (inp.Contains('-'))
            {
                negativ = true;
                inp = inp.Replace("-", "");
                N--;
            }
            if (inp.Contains('.'))
                N--;

            foreach (string c in input.Split(' '))
            {
                if (c == "." || c == "-")
                    continue;
                allNB.Add(int.Parse(c.ToString()));
            }

            double bornSup = (negativ ? allNB.Min() : allNB.Max()) * Math.Pow(10, N);
            double bornInf = (negativ ? allNB.Min() : allNB.Max()) * Math.Pow(10, N - 1);
            double pas = 1;

            if (inp.Contains('.'))
            {
                if (negativ)
                {
                    bornInf = allNB.Min() + Math.Pow(10, -(N - 1));
                    bornSup = (allNB.Min() + 1) + Math.Pow(10, -(N - 1));
                    pas = (int)Math.Pow(10, -(N - 1));
                }
                else
                {
                    bornInf = allNB.Max() * Math.Pow(10, N - 1);
                    bornSup = (allNB.Max() + 1) * Math.Pow(10, N - 1);
                    pas = 0.1;
                }
            }

            for (int i = bornInf; i < bornSup; i += pas )
            {
                string s = i.ToString().Replace('.',',');
                string tmp = inp;
                bool isOK = true;
                foreach (char c in s)
                {
                    isOK &= tmp.Contains(c);
                    if (isOK)
                    {
                        int index = tmp.IndexOf(c);
                        tmp = tmp.Remove( index, 1 );
                    }
                }

                if (isOK)
                    choice.Add(i);
            }

            if (negativ)
                Console.WriteLine("-" + choice.Min() );
            else
                Console.WriteLine(choice.Max());

            Console.In.ReadLine();
        }
        

        /*
        static void Main(string[] args)
        {
            // Créé la partie
            Game g = new Game();

            // Construit tous les champs dispos
            g.CreateAllChamp();

            // Lance le draft (param true si on commance à pick)
            g.LaunchGame(true);

            Console.Out.Write("\nPress one touch to leave...");
            Console.In.ReadLine();
        }*/
    }
}
