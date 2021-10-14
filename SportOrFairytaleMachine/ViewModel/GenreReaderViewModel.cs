using Microsoft.Win32;
using SportOrFairytaleMachine.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        bool[] vectorArray;
        bool[] unknownVectorArray;
        
        private ObservableCollection<DistanceModel> distanceList = new ObservableCollection<DistanceModel>();
        public ObservableCollection<DistanceModel> DistanceList
        {
            get { return distanceList; }
            set { distanceList = value; propertyIsChanged(); }
        }

        List<string> AllWordsDictonary = new List<string>();

        string readContents = "";
        string dictoryFile = Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory) + "/TextReader/DictoryBag.txt";
        string desktop = Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);
        string[] path = { "/TextReader/Eventyr/Fairytale1.txt", "/TextReader/Eventyr/Fairytale2.txt", "/TextReader/Eventyr/Fairytale3.txt", "/TextReader/Eventyr/Fairytale4.txt", "/TextReader/Eventyr/Fairytale5.txt",
                          "/TextReader/Eventyr/Fairytale6.txt", "/TextReader/Eventyr/Fairytale7.txt", "/TextReader/Eventyr/Fairytale8.txt", "/TextReader/Eventyr/Fairytale9.txt", "/TextReader/Eventyr/Fairytale10.txt",
                          "/TextReader/Sport/Sport1.txt", "/TextReader/Sport/Sport2.txt", "/TextReader/Sport/Sport3.txt", "/TextReader/Sport/Sport4.txt", "/TextReader/Sport/Sport5.txt", "/TextReader/Sport/Sport6.txt",
                          "/TextReader/Sport/Sport7.txt", "/TextReader/Sport/Sport8.txt", "/TextReader/Sport/Sport9.txt", "/TextReader/Sport/Sport10.txt",};


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
            readAllTexts();


            openFileChooser = new DelegateCommand(o =>
                {
                    FileChooser();  
                });

        }

        private void FileChooser()
        {
            distanceList.Clear();

            string fixedInput = "";
            string textTest = "";
            List<string> unknownTextWordList = new List<string>();
            List<bool> unknownVectorList = new List<bool>();


            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                textTest = File.ReadAllText(openFileDialog.FileName);
            }
            TextBox = textTest;

            //This will remove all ., etc which we dont want into our list. It will also split the text so only 1 word will be on each line.
            fixedInput = Regex.Replace(textTest, "[^a-zA-Z0-9% ._]", string.Empty);
            fixedInput = fixedInput.Replace(".", string.Empty);
            var punctuation = fixedInput.Where(Char.IsPunctuation).ToArray();
            var words = fixedInput.Split().Distinct().Select(x => x.Trim(punctuation));

            using (StreamWriter sw = File.CreateText(desktop + "/TextReader/TestUnknown.txt"))
            {
                sw.WriteLine(string.Join(Environment.NewLine, words));
            }

            string[] unknownText = File.ReadAllLines(desktop + "/TextReader/TestUnknown.txt");

            foreach (string unknownTextLine in unknownText)
            {
                unknownTextWordList.Add(unknownTextLine);
            }

            foreach (string line in AllWordsDictonary)
            {
                if (unknownTextWordList.Contains(line))
                {
                    unknownVectorList.Add(true);
                }
                else
                {
                    unknownVectorList.Add(false);
                }
            }
            unknownVectorArray = unknownVectorList.ToArray();
           
            vectorTheTexts();

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

            string[] lines = File.ReadAllLines(dictoryFile);

            foreach (string line in lines)
            {
                AllWordsDictonary.Add(line);
            }
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

        private void vectorTheTexts()
        {

            string singleTextFixedInput = "";
            string typeOfText = "";

            List<bool> vectorList = new List<bool>();
           
            List<string> fairyTaleWordList = new List<string>();
            string[] pathFairyTale = { "/TextReader/Eventyr/Fairytale1.txt", "/TextReader/Eventyr/Fairytale2.txt", "/TextReader/Eventyr/Fairytale3.txt", "/TextReader/Eventyr/Fairytale4.txt", "/TextReader/Eventyr/Fairytale5.txt",
                          "/TextReader/Eventyr/Fairytale6.txt", "/TextReader/Eventyr/Fairytale7.txt", "/TextReader/Eventyr/Fairytale8.txt", "/TextReader/Eventyr/Fairytale9.txt", "/TextReader/Eventyr/Fairytale10.txt",
                          "/TextReader/Sport/Sport1.txt", "/TextReader/Sport/Sport2.txt", "/TextReader/Sport/Sport3.txt", "/TextReader/Sport/Sport4.txt", "/TextReader/Sport/Sport5.txt", "/TextReader/Sport/Sport6.txt",
                          "/TextReader/Sport/Sport7.txt", "/TextReader/Sport/Sport8.txt", "/TextReader/Sport/Sport9.txt", "/TextReader/Sport/Sport10.txt",};



            foreach (string fairyTale in pathFairyTale)
            {
                vectorList.Clear();
                fairyTaleWordList.Clear();

                if(fairyTale.Contains("Fairytale"))
                {
                    typeOfText = "Fairytale";
                }
                else
                {
                    typeOfText = "Sport";
                }

                using (StreamReader streamReader2 = new StreamReader(desktop + fairyTale))
                {
                    singleTextFixedInput = streamReader2.ReadToEnd();
                }

                singleTextFixedInput = Regex.Replace(singleTextFixedInput, "[^a-zA-Z0-9% ._]", string.Empty);
                singleTextFixedInput = singleTextFixedInput.Replace(".", string.Empty);
                var punctuation = singleTextFixedInput.Where(Char.IsPunctuation).ToArray();
                var words = singleTextFixedInput.Split().Distinct().Select(x => x.Trim(punctuation));

                if (File.Exists(desktop + fairyTale + "Vector.txt"))
                {
                    File.Delete(desktop + fairyTale + "Vector.txt");
                }
                if (File.Exists(desktop + "/TextReader/TempList.txt")) 
                {
                    File.Delete(desktop + "/TextReader/TempList.txt");
                }

                using (StreamWriter sw2 = File.CreateText(desktop + "/TextReader/TempList.txt"))
                {
                    sw2.WriteLine(string.Join(Environment.NewLine, words));
                }

                string[] fairyTaleLines = File.ReadAllLines(desktop + "/TextReader/TempList.txt");

                foreach (string fairyTaleline in fairyTaleLines)
                {
                    fairyTaleWordList.Add(fairyTaleline);
                }

                foreach (string line in AllWordsDictonary)
                    {
                    if (fairyTaleWordList.Contains(line))
                    {
                        vectorList.Add(true);
                    }
                    else
                    {
                        vectorList.Add(false);
                    }
                    }
                vectorArray = vectorList.ToArray();
               
                DistanceList.Add(CalculateInfo(vectorArray, unknownVectorArray, typeOfText));



                using (var file = new StreamWriter(desktop + fairyTale + "Vector.txt"))
                {
                    vectorList.ForEach(v => file.WriteLine(v));
                }
            }
            knnAlgo();
        }
        private DistanceModel CalculateInfo (bool[] textArray, bool[] uknownTextArray, string StoryType)
        {
            DistanceModel dm = new DistanceModel();

            for (int i = 0; i < AllWordsDictonary.Count; i++)
            {
                dm.Sum += Math.Pow((vectorArray[i] ? 1 : 0) - (unknownVectorArray[i] ? 1 : 0), 2);
            }

            double distance = Math.Sqrt(dm.Sum);
            dm.Distance = distance;
            dm.Type = StoryType;
       
            return dm;
        }

        private void knnAlgo()
        {
            int fairytaleScore = 0;
            int sportScore = 0;
            double chanceForRight = 0;

            List<DistanceModel> distanceSortList = new List<DistanceModel>();

            distanceSortList = distanceList.ToList();
            distanceSortList = distanceSortList.OrderBy((dm) => dm.Distance).ToList();

            for (int i = 0; i < 5; i++)
            {
                if (distanceSortList[i].Type.Equals("Fairytale"))
                    {
                    fairytaleScore++;
                }
                else
                {
                    sportScore++;
                }
            }

           chanceForRight = Math.Max(fairytaleScore, sportScore);
            chanceForRight = (chanceForRight / 5) * 100;

            if(fairytaleScore > sportScore)
            {

                MessageBox.Show("Your unknown text is a fairytale! I am " + chanceForRight + "% sure!");
            }
            else
            {
                MessageBox.Show("Your unknown text is a Sport article! I am " + chanceForRight + "% sure!");
            }
            sportScore = 0;
            fairytaleScore = 0;
            chanceForRight = 0;
        }
    }
}
