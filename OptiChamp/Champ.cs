using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptiChamp
{
    public enum Position
    {
        TOP,
        MID,
        JUNGLE,
        ADC,
        SUPPORT
    }

    public class Champ
    {
        public string Nom { get; set; }
        public string position { get; set; } // 1

        // caracs
        public string degat { get; set; } // 2
        public List<string> faibleContre { get; set; } // 3
        public List<string> fortContre { get; set; } // 4
        public List<string> CombinateChamp { get; set; } // 5
        public int nbCC { get; set; } // 6
        public int noteBurst { get; set; } // 7
        public bool peutEtreFirstPick { get; set; } // 8
        public int nbEngage { get; set; } // 9
        public int nbDisengage { get; set; } // 10
        public int nbPoke { get; set; } // 11
        public bool ultyGlobal { get; set; } // 12
        public int nbEscape { get; set; } // 13
        public int noteDegat { get; set; } // 14
        public int noteTank { get; set; } // 15
        public int healShield { get; set; } // 16

        /*
         * rm
         * sort de zone
         * armure
         * peel
         * */

        /// <summary>
        /// Créé un nouveau champion en utilisant un nom
        /// </summary>
        public Champ(string nom)
        {
            this.Nom = nom;
            this.faibleContre = new List<string>();
            this.fortContre = new List<string>();
            this.CombinateChamp = new List<string>();
        }

        /// <summary>
        /// Calcule le gain d'un champion dans une situation précise et en utilisant le numéro du tour [1-10]
        /// Retourne 0 si la position est déja prise
        /// </summary>
        public int CalculeCout(List<Champ> ennemi, List<Champ> allie, int nbTour)
        {
            int cout = 0;

            // 1. regarde si la position est pas déja utilisé
            if (allie.Count(a => a.position == this.position) > 0)
                return 0;

            // 2. Ajout du type de dégat: 10 à 50 PTS
            int AP = allie.Count(a => a.degat == "AP");
            int AD = allie.Count(a => a.degat == "AD");
            cout += this.degat == "AD" ? AP * 10 : AD * 10; // on privilegie un mix de degat

            // 3. Faible contre: -10 à -50 PTS
            foreach (string s in this.faibleContre)
                cout -= ennemi.Count(a => a.Nom == s) * 50;

            // 4. Fort contre: 10 à 50 PTS
            foreach (string s in this.fortContre)
                cout += ennemi.Count(a => a.Nom == s) * 50;

            // 5. Combinaison: 10 à 50 PTS
            foreach (string s in this.CombinateChamp)
                cout += allie.Count(a => a.Nom == s) * 50;

            // 6. Ajout des CC: 10 à 50 PTS
            cout += this.nbCC * 10 + 10;

            // 7. Ajout de la notation du burst: 10 à 50 PTS
            cout += this.noteBurst * 10;

            // 8. Eviter de sortir un champ qui peut être counter facilement
            cout += (this.peutEtreFirstPick ? 1 : -1) * 10 / nbTour * 5;

            // 9. Ajout du nb d'engage
            cout += this.nbEngage * 10;

            // 10. Ajout du nb de disengage: plus intéressant si ya de l'engage en face
            cout += this.nbDisengage * 10 * ennemi.Count(a => a.nbEngage > 0) * nbTour / 10;

            // 11. Ajout du nb de poke
            cout += this.nbPoke * 10;

            // 12. Ajout d'ulty globale ou pas
            cout += (this.ultyGlobal ? 50 : 0);

            // 13. Ajout du nb d'escape: plus intéressant si ya de l'engage en face
            cout += this.nbEscape * 10 * ennemi.Count(a => a.nbEngage > 0) * nbTour / 10;

            // 14. Ajout d'une note sur 5 des dégats du champion: privilégie les dégats si on en a peu
            cout += this.noteDegat * 10 * (25 / (allie.Sum(a => a.noteDegat) + 1)) * nbTour / 10;

            // 15. Ajout d'une note sur 5 de la capacité à tanker: privilégie les tanks si on en a peu
            cout += this.noteTank * 10 * (25 / (allie.Sum(a => a.noteTank) + 1)) * nbTour / 10;

            // 16. Ajout du nombre de heal ou shield (passif ou actif): poussé encore plus si on a une team faible (peu de tanks)
            cout += this.healShield * 10 * (25 / (allie.Sum(a => a.noteTank) + 1)) * nbTour / 10;

            return cout;
        }

        public Champ(Champ ch)
        {
            this.Nom = ch.Nom;
            this.nbCC = ch.nbCC;
            this.degat = ch.degat;
            this.position = ch.position;
            this.noteBurst = ch.noteBurst;
            this.noteDegat = ch.noteDegat;
            this.noteTank = ch.noteTank;
            this.healShield = ch.healShield;
            this.peutEtreFirstPick = ch.peutEtreFirstPick;
            this.faibleContre = new List<string>(ch.faibleContre);
            this.fortContre = new List<string>(ch.fortContre);
            this.CombinateChamp = new List<string>(ch.CombinateChamp);            
        }
    }
}
