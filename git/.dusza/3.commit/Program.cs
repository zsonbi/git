using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace git
{
    internal class Program
    {
        public static string name;
        public static git Git;

        private static void Main(string[] args)
        {
            //Getting the author's name
            Git = new git();
            if (!Git.initialized)
            {
                Console.WriteLine("Adja meg a nevét");
                name = Console.ReadLine();
            }
            //displaying if there is already a .dusza folder
            else
            {
                Console.WriteLine("Van .dusza mappa");
            }
            string userPressedKey = "";
            do
            {
                Console.WriteLine("Válassza ki az adott menüpontot");

                if (!Git.initialized)
                {
                    CreateMenu(new string[] { "Inicializálás" });
                    userPressedKey = Console.ReadLine();
                    switch (userPressedKey)
                    {
                        case "1":
                            Initialize();
                            break;

                        case "*":
                            return;

                        default:
                            Console.WriteLine("Nincs ilyen menüpont");
                            break;
                    }
                }
                else
                {
                    CreateMenu(new string[] { "Inicializálás", "Commit", "Verziótörténet megjelenítése", "Verzió változtatása" });
                    userPressedKey = Console.ReadLine();
                    switch (userPressedKey)
                    {
                        case "1":
                            Initialize();
                            break;

                        case "2":
                            CreateCommit();
                            break;

                        case "3":
                            ShowVersionHistory();
                            break;

                        case "4":
                            ChangeVersion();
                            break;

                        case "*":
                            return;

                        default:
                            break;
                    }
                }
            } while (true);
        }

        private static void CreateMenu(string[] Menu)
        {
            Console.Clear();
            for (int i = 0; i < Menu.Length; i++)
            {
                Console.WriteLine(i + 1 + " " + Menu[i]);
            }
        }

        private static void ShowVersionHistory()
        {
            Console.WriteLine("Commitok listázása:");
            string[] commits = Directory.GetDirectories("..\\..\\.dusza").ToArray();
            //the commits in the .dusza folder
            for (int i = 0; i < commits.Length; i++)
            {
                Console.WriteLine(i + 1 + ". commit" + commits[i]);
            }
            //exiting or getting detalis of the selected commit
            Console.WriteLine("Ha ki szeretne lépni, nyomjon *-ot.");
            string commitID = "";
            do
            {
                Console.WriteLine("Válasszon ki egy commitot, amelynek a részleteit szeretné megjeleníteni:");
                commitID = Console.ReadLine();
                //determining if the commit exists or not
                if (int.Parse(commitID) > commits.Length)
                {
                    Console.WriteLine("Nincs ilyen commit.");
                }
                //if the commit exists, displaying the correct commit.details
                else
                {
                    Console.Write(File.ReadAllLines("..\\..\\.dusza\\" + commitID + ".commit\\commit.details"));
                }
            }
            while (commitID != "*");
        }

        private static void ChangeVersion()
        {
            string[] commits = Directory.GetDirectories("..\\..\\.dusza");

            for (int i = 0; i < commits.Length; i++)
            {
                Console.WriteLine(i + 1 + " " + commits[i].Split('\\').Last());
            }
            Console.WriteLine("Válassza ki a visszaállítandó verziót");
            string userInput = Console.ReadLine();
        }

        private static void CreateCommit()
        {
            //Creating a commit, and displaying its ID
            Git.CreateCommit();
            Console.WriteLine("Új commit létrehozva a" + Git.currentCommitNumber + " számmal");
        }

        private static void Initialize()
        {
            Git.Initialize(name);
        }
    }
}