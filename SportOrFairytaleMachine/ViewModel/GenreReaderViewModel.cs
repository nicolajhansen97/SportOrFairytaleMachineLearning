using Microsoft.Win32;
using SportOrFairytaleMachine.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Shapes;

namespace SportOrFairytaleMachine.ViewModel
{
    class GenreReaderViewModel : Bindable
    {

        string readContents = "";
        string dictoryFile = Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory) + "/DictoryBag.txt";
        string desktop = Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);
        string[] path = { "/Eventyr/Fairytale1.txt", "/Eventyr/Fairytale2.txt", "/Eventyr/Fairytale3.txt", "/Eventyr/Fairytale4.txt", "/Eventyr/Fairytale5.txt",
                          "/Eventyr/Fairytale6.txt", "/Eventyr/Fairytale7.txt", "/Eventyr/Fairytale8.txt", "/Eventyr/Fairytale9.txt", "/Eventyr/Fairytale10.txt",
                          "/Sport/Sport1.txt", "/Sport/Sport2.txt", "/Sport/Sport3.txt", "/Sport/Sport4.txt", "/Sport/Sport5.txt", "/Sport/Sport6.txt",
                          "/Sport/Sport7.txt", "/Sport/Sport8.txt", "/Sport/Sport9.txt", "/Sport/Sport10.txt",};


        public DelegateCommand openFileChooser { get; set; }

        private string _loadFile = "Load file";

        public string LoadFile
        {
            get { return _loadFile; }
            set { _loadFile = value; propertyIsChanged(); }
        }

        private string _textBox = "Text";

        public string TextBox
        {
            get { return _textBox; }
            set { _textBox = value; propertyIsChanged(); }
        }

        public GenreReaderViewModel()
        {
            openFileChooser = new DelegateCommand(o =>
                {
                    //FileChooser();
                    //TextSplitter();
                    readAllTexts();
                });

        }

        private void FileChooser()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string textTest = File.ReadAllText(openFileDialog.FileName);
                MessageBox.Show(textTest);
            }
        }

        //This method will split the text, but also clean it and prepare it for vectors.
        private void TextSplitter()
        {
            //Used to save the output after deleting ., etc
            string fixedInput = "";
          
            //Reads the file where all the texts is putted into.
            using (StreamReader streamReader2 = new StreamReader(dictoryFile))
            {
                fixedInput = streamReader2.ReadToEnd();
            }

    
            //This will remove all ., etc which we dont want into our list. It will also split the text so only 1 word will be on each line.
            fixedInput = Regex.Replace(fixedInput, "[^a-zA-Z0-9% ._]", string.Empty);
            fixedInput = fixedInput.Replace(".", string.Empty);
            var punctuation = fixedInput.Where(Char.IsPunctuation).ToArray();
            var words = fixedInput.Split().Distinct().Select(x => x.Trim(punctuation));

          
            //Deletes the file as we dont need it anymore.
            File.Delete(dictoryFile);

            //Creates a new file where it will put everything to a new line.
            using (StreamWriter sw = File.CreateText(dictoryFile))
            {
                sw.WriteLine(string.Join(Environment.NewLine, words));
            }
               TextBox = string.Join(Environment.NewLine, words);
        }

        //This method will read all textes and save it all into one single file which will be used as the dictonary. 
        private void readAllTexts()
        {
            //If the file exists, it delete as we create it again.
            if (File.Exists(dictoryFile))
            {
                File.Delete(dictoryFile);
            }


            //Creates the file, adds everything to it and make it to lower case, then calling the text splitter method.
            using (StreamWriter sw = File.AppendText(dictoryFile))
            {

                foreach (string i in path)
                {
                    using (StreamReader streamReader = new StreamReader(desktop + i))
                    {
                        readContents = streamReader.ReadToEnd();
                    }
                    
                    sw.WriteLine(readContents.ToLower());
                }

            }
                TextSplitter();
        }
    }
}
