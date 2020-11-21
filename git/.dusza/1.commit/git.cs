using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace git
{
    internal class git
    {
        private List<int> CommitNumbers = new List<int>();
        public int currentCommitNumber { get; private set; }
        public bool initialized { get; private set; }

        private string name = Program.name;
        private const string path = "..\\..";
        public string commitName = "";
        private int parentCommitNumber = 0;

        public git()
        {
            if (Directory.Exists(path + "\\.dusza"))
            {
                initialized = true;
                string[] temp = File.ReadAllLines(path + "\\.dusza\\head.txt");
                currentCommitNumber = Convert.ToInt32(temp[0]);
                commitName = currentCommitNumber + ".commit";

                string author = File.ReadAllLines(path + "\\.dusza\\" + commitName + "\\commit.details")[1].Split(':')[1].Trim();
                currentCommitNumber = Convert.ToInt32(temp[0]);
            }
            else
            {
                currentCommitNumber = 0;
                initialized = false;
            }
        }

        //---------------------------------------------
        //private Methods
        //Updates the content of the head
        private void UpdateHead()
        {
            string number = currentCommitNumber.ToString();
            File.WriteAllLines(path + "\\.dusza\\head.txt", new string[] { currentCommitNumber.ToString() });
        }

        //----------------------------------------------------------------
        private FileDetails[] GetChangedFiles()
        {
            List<FileDetails> changedFiles = new List<FileDetails>();
            string[] newFiles = Directory.GetFiles(path);
            string[] previousFiles;
            if (currentCommitNumber != 0)
                previousFiles = Directory.GetFiles(path + "\\.dusza\\" + commitName);
            else
            {
                previousFiles = new string[] { "a\\a", "a\\a" };
            }

            int separateCounter = 0;
            for (int i = 0; i < newFiles.Length; i++)
            {
                if (newFiles[i].Split('\\').Last() == previousFiles[separateCounter].Split('\\').Last())
                {
                    changedFiles.Add(new FileDetails(newFiles[i], previousFiles[separateCounter]));
                    separateCounter++;
                }
                else if (newFiles[i].Split('\\').Last() == previousFiles[separateCounter + 1].Split('\\').Last())
                {
                    changedFiles.Add(new FileDetails("", previousFiles[separateCounter]));
                    separateCounter++;
                }
                else
                {
                    changedFiles.Add(new FileDetails(newFiles[i], ""));
                }
            }
            return changedFiles.ToArray();
        }

        //------------------------------------------------------------
        //Copies the files and the directory to the repository
        private void CopyFiles(string input = "commitPath")
        {
            string commitPath = path + "\\.dusza" + "\\";
            string basicPath = path;
            if (input == "commitPath")
            {
                string fileName = "";
                string destinationFile = Path.Combine(commitPath + commitName, fileName);
                foreach (string sourceF in Directory.GetFiles(path))
                {
                    fileName = Path.GetFileName(sourceF);
                    destinationFile = Path.Combine(commitPath + commitName, fileName);
                    File.Copy(sourceF, destinationFile, true);
                }

                //Create all of the directories
                foreach (string dirPath in Directory.GetDirectories(path, "*",
                    SearchOption.AllDirectories))
                {
                    if (!dirPath.Contains("\\.dusza"))
                        Directory.CreateDirectory(dirPath.Replace(path, commitPath + commitName));
                }

                //Copy all the files & Replaces any files with the same name
                foreach (string newPath in Directory.GetFiles(path, "*.*",
                    SearchOption.AllDirectories))
                {
                    if (!newPath.Contains("\\.dusza"))
                        File.Copy(newPath, newPath.Replace(path, commitPath + commitName), true);
                }
            }
            else
            {
                string fileName = "";
                string destinationFile = Path.Combine(basicPath, fileName);
                foreach (string sourceF in Directory.GetFiles(commitPath))
                {
                    fileName = Path.GetFileName(sourceF);
                    destinationFile = Path.Combine(basicPath, fileName);
                    File.Copy(sourceF, destinationFile, true);
                }

                //Create all of the directories
                foreach (string dirPath in Directory.GetDirectories(path, "*",
                    SearchOption.AllDirectories))
                {
                    if (!dirPath.Contains("\\.dusza"))
                        Directory.CreateDirectory(dirPath.Replace(path, commitPath + commitName));
                }

                //Copy all the files & Replaces any files with the same name
                foreach (string newPath in Directory.GetFiles(path, "*.*",
                    SearchOption.AllDirectories))
                {
                    if (!newPath.Contains("\\.dusza"))
                        File.Copy(newPath, newPath.Replace(path, commitPath + commitName), true);
                }
            }
        }

        //---------------------------------------------------------
        //Creates the content of the commit.details.txt file
        private string[] CreateCommitDetails(FileDetails[] ChangedFiles)
        {
            int parentCommit = currentCommitNumber;
            List<string> output = new List<string>();
            //Parent
            output.Add("Szulo:" + (parentCommit == 0 ? "-" : parentCommit.ToString()));
            //Author
            output.Add("Szerzo:" + name);
            //Date
            output.Add(DateTime.Now.ToString());
            output.Add("Valtozott:");
            foreach (var file in ChangedFiles)
            {
                output.Add(file.state + " " + file.fileName + " " + file.date);
            }
            return output.ToArray();
        }

        //Public Methods
        //Inicializes a new repository
        public void Initialize(string name)
        {
            this.name = name;
            DirectoryInfo repo = Directory.CreateDirectory(path + "\\.dusza");
            repo.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            CreateCommit();
            initialized = true;
        }

        //------------------------------------------------------------
        public void ChangeVersion()
        {
        }

        //---------------------------------------------------------
        //Creates a new commit
        public void CreateCommit()
        {
            commitName = (currentCommitNumber + 1) + ".commit";
            Directory.CreateDirectory(path + "\\.dusza\\" + commitName);
            File.WriteAllLines(path + "\\.dusza\\" + commitName + "\\commit.details", CreateCommitDetails(GetChangedFiles()));
            currentCommitNumber++;

            CopyFiles();
            UpdateHead();
        }
    }
}