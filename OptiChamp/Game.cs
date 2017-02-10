using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace OptiChamp
{
    public class Game
    {
        public List<Champ> allyChamps = new List<Champ>();
        public List<Champ> ennemyChamps = new List<Champ>();
        public List<Champ> champs = new List<Champ>();

        public Game()
        {
            Console.Out.WriteLine("-- Nouveau DRAFT --");
        }

        public void LaunchGame(bool allyStart)
        {
            int tour = 0;

            if (champs.Count() == 0)
            {
                Console.Error.WriteLine("/!\\ Liste de champion vide ! /!\\");
                return;
            }

            // Boucle Principale
            while (tour < 10)
            {
                bool allyTour = allyStart && "03478".Contains(tour.ToString());
                Console.Out.WriteLine(string.Format("# Tour {0}:", tour + 1));                

                // Si c'est notre tour, on affiche une aide
                if (allyTour)
                    this.PrintBestChamp(3, tour + 1);                    

                Console.Out.Write(string.Format("=> {0} champion: ", allyTour ? "Notre equipe choisit le " : "L'équipe adverse a choisi le"));
                string champIn = Console.In.ReadLine();

                while ( !champs.Exists(a => champIn == a.Nom) ) // Si le champion n'existe pas => on demande à rechoisir
                {
                    Console.Out.Write("/!\\ champion invalide, veuillez recommencer: ");
                    champIn = Console.In.ReadLine().ToLower();
                }

                Champ ch = new Champ(champs.Find(a => a.Nom == champIn));
                if (allyTour)
                    allyChamps.Add(ch);
                else
                    ennemyChamps.Add(ch);

                champs.RemoveAll(a => a.Nom == champIn);
                tour++;
                Console.Error.WriteLine();
            }

            Console.Out.WriteLine("-- DRAFT TERMINE --\n");
            this.FinalPrint();
        }

        private void FinalPrint()
        {
            Console.Out.WriteLine("Liste finale:\t<allie\tVS\tennemy>");
            Console.Out.WriteLine(string.Format("TOP:    \t{0}\t\tVS\t{1}", allyChamps.Find(a => a.position == "TOP").Nom, ennemyChamps.Find(a => a.position == "TOP").Nom));
            Console.Out.WriteLine(string.Format("MID:    \t{0}\t\tVS\t{1}", allyChamps.Find(a => a.position == "MID").Nom, ennemyChamps.Find(a => a.position == "MID").Nom));
            Console.Out.WriteLine(string.Format("JUNGLER:\t{0}\t\tVS\t{1}", allyChamps.Find(a => a.position == "JUNGLER").Nom, ennemyChamps.Find(a => a.position == "JUNGLER").Nom));
            Console.Out.WriteLine(string.Format("ADC:    \t{0}\t\tVS\t{1}", allyChamps.Find(a => a.position == "ADC").Nom, ennemyChamps.Find(a => a.position == "ADC").Nom));
            Console.Out.WriteLine(string.Format("SUPPORT:\t{0}\t\tVS\t{1}", allyChamps.Find(a => a.position == "SUPPORT").Nom, ennemyChamps.Find(a => a.position == "SUPPORT").Nom));
        }

        /// <summary>
        ///  Affiche les nbToPrint meilleurs champions et les nbToPrint pires champions
        /// </summary>
        public void PrintBestChamp(int nbToPrint, int nbTour)
        {
            List<KeyValuePair<int, string>> bChamp = new List<KeyValuePair<int, string>>();

            foreach (Champ ch in this.champs)
            {
                int currCount = ch.CalculeCout(this.ennemyChamps, this.allyChamps, nbTour);
                KeyValuePair<int, string> tmpKVP = new KeyValuePair<int, string>(currCount, ch.Nom);

                bChamp.Add(tmpKVP);
            }

            bChamp.Sort((a,b) => -a.Key.CompareTo(b.Key));

            Console.Out.Write("- On vous conseille: ");
            int inc = nbToPrint;
            for (int i = 0; inc > 0 && i < bChamp.Count(); i++)
            {
                if (bChamp[i].Key > 0)
                {
                    Console.Out.Write(string.Format(" {0} [{1} pts] ;", bChamp[i].Value, bChamp[i].Key));
                    inc--;
                }
            }

            Console.Out.WriteLine();

            Console.Out.Write("- On vous déconseille: ");
            inc = nbToPrint;
            for (int i = bChamp.Count() - 1; inc > 0 && i >= 0; i--)
            {
                if (bChamp[i].Key > 0)
                {
                    Console.Out.Write(string.Format(" {0} [{1} pts] ;", bChamp[i].Value, bChamp[i].Key));
                    inc--;
                }
            }

            Console.Out.WriteLine();
        }

        /// <summary>
        /// A partir du fichier champ.csv à la racine de l'exe: init tous les champs
        /// </summary>
        public void CreateAllChamp()
        {
            int incr = 0;

            if ( !File.Exists("champs.csv") )
            {
                Console.Error.WriteLine("Fichier introuvable: champs.csv");
                return;
            }

            foreach (string line in File.ReadAllLines("champs.csv"))
            {
                if (incr > 0 && line.Length > 0)
                {
                    string[] caracs = line.Split(',');
                    Champ ch = new Champ(caracs[0].ToLower());

                    // stock les caracs
                    ch.position = caracs[1]; // split avec les ;
                    ch.degat = caracs[2];
                    ch.faibleContre.Add(caracs[3]); // split avec les ;
                    ch.fortContre.Add(caracs[4]); // split avec les ;
                    ch.CombinateChamp.Add(caracs[5]); // split avec les ;
                    ch.nbCC = int.Parse(caracs[6]);
                    ch.noteBurst = int.Parse(caracs[7]);
                    ch.peutEtreFirstPick = caracs[8] == "OUI";
                    ch.nbEngage = int.Parse(caracs[9]);
                    ch.nbDisengage = int.Parse(caracs[10]);
                    ch.nbPoke = int.Parse(caracs[11]);
                    ch.ultyGlobal = caracs[12] == "OUI";
                    ch.nbEscape = int.Parse(caracs[13]);
                    ch.noteDegat = int.Parse(caracs[14]);
                    ch.noteTank = int.Parse(caracs[15]);

                    this.champs.Add(ch);
                }

                incr++;
            }

            Console.Out.WriteLine(string.Format("* {0} champions créé(s).", this.champs.Count()));
        }
    }
}
