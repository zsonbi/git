﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace git
{
    internal class FileDetails
    {
        public string state { get; private set; }
        public string fileName { get; private set; }
        public string date { get; private set; }

        public FileDetails(string input, string previous)
        {
            this.fileName = input.Split('\\').Last();
            if (previous == "")
            {
                state = "uj";
                date = File.GetLastWriteTime(input).ToString().Split('\\').Last();
            }
            else if (input == "")
            {
                this.fileName = previous.Split('\\').Last();
                state = "torolt";
                date = File.GetLastWriteTime(previous).ToString();
            }
            else if (File.GetLastWriteTime(input) == File.GetLastWriteTime(previous))
            {
                state = "same";
                date = File.GetLastWriteTime(input).ToString().Split('\\').Last();
            }
            else
            {
                state = "valtozott";
                date = File.GetLastWriteTime(input).ToString().Split('\\').Last();
            }
        }

        public void ChangeState(string newState)
        {
            this.state = newState;
            date = DateTime.Now.ToString();
        }
    }
}