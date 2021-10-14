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
        
        //An observablecollection of the distancemodel which will contain all the information given to the datamodel and then added to the listview.
        private ObservableCollection<DistanceModel> distanceList = new ObservableCollection<DistanceModel>();
        public ObservableCollection<DistanceModel> DistanceList
        {
            get { return distanceList; }
            set { distanceList = value; propertyIsChanged(); }
        }

        //The list containing the dictonary
        List<string> AllWordsDictonary = new List<string>();

        //Value used to read content from texts
        string readContents = "";
        //The dictonary path way
        string dictoryFile = Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory) + "/TextReader/DictoryBag.txt";
        //The desktop folder
        string desktop = Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);
        //All Pathways
        string[] path = { "/TextReader/Eventyr/Fairytale1.txt", "/TextReader/Eventyr/Fairytale2.txt", "/TextReader/Eventyr/Fairytale3.txt", "/TextReader/Eventyr/Fairytale4.txt", "/TextReader/Eventyr/Fairytale5.txt",
                          "/TextReader/Eventyr/Fairytale6.txt", "/TextReader/Eventyr/Fairytale7.txt", "/TextReader/Eventyr/Fairytale8.txt", "/TextReader/Eventyr/Fairytale9.txt", "/TextReader/Eventyr/Fairytale10.txt",
                          "/TextReader/Sport/Sport1.txt", "/TextReader/Sport/Sport2.txt", "/TextReader/Sport/Sport3.txt", "/TextReader/Sport/Sport4.txt", "/TextReader/Sport/Sport5.txt", "/TextReader/Sport/Sport6.txt",
                          "/TextReader/Sport/Sport7.txt", "/TextReader/Sport/Sport8.txt", "/TextReader/Sport/Sport9.txt", "/TextReader/Sport/Sport10.txt",};

        //Deletegate command which is bound to the open file botton
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

        //File chooser, will open the file and put it into the textbox and then make the unknown text ready
        private void FileChooser()
        {
            distanceList.Clear();

            string fixedInput = "";
            string textTest = "";
            List<string> unknownTextWordList = new List<string>();
            List<bool> unknownVectorList = new List<bool>();

            //Takes the text which you open and put it into the textbox
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

            //Making the unknowntext into singlel ine words and add it to the vectorlist
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

        //Making the vectorList off each text
        private void vectorTheTexts()
        {

            string singleTextFixedInput = "";
            string typeOfText = "";

            List<bool> vectorList = new List<bool>();
            List<string> textWordList = new List<string>();

            //All the texts
            string[] pathAllTexts = { "/TextReader/Eventyr/Fairytale1.txt", "/TextReader/Eventyr/Fairytale2.txt", "/TextReader/Eventyr/Fairytale3.txt", "/TextReader/Eventyr/Fairytale4.txt", "/TextReader/Eventyr/Fairytale5.txt",
                          "/TextReader/Eventyr/Fairytale6.txt", "/TextReader/Eventyr/Fairytale7.txt", "/TextReader/Eventyr/Fairytale8.txt", "/TextReader/Eventyr/Fairytale9.txt", "/TextReader/Eventyr/Fairytale10.txt",
                          "/TextReader/Sport/Sport1.txt", "/TextReader/Sport/Sport2.txt", "/TextReader/Sport/Sport3.txt", "/TextReader/Sport/Sport4.txt", "/TextReader/Sport/Sport5.txt", "/TextReader/Sport/Sport6.txt",
                          "/TextReader/Sport/Sport7.txt", "/TextReader/Sport/Sport8.txt", "/TextReader/Sport/Sport9.txt", "/TextReader/Sport/Sport10.txt",};


            //Taking all the texts and makes vector of all the texts and add it to an array
            foreach (string text in pathAllTexts)
            {
                //Clears it everytime so it a fresh one.
                vectorList.Clear();
                textWordList.Clear();

                //Just setting the type of text used later
                if(text.Contains("Fairytale"))
                {
                    typeOfText = "Fairytale";
                }
                else
                {
                    typeOfText = "Sport";
                }

                //Reading the text
                using (StreamReader streamReader2 = new StreamReader(desktop + text))
                {
                    singleTextFixedInput = streamReader2.ReadToEnd();
                }

                //Cleaning the text and trims it into single words on a line
                singleTextFixedInput = Regex.Replace(singleTextFixedInput, "[^a-zA-Z0-9% ._]", string.Empty);
                singleTextFixedInput = singleTextFixedInput.Replace(".", string.Empty);
                var punctuation = singleTextFixedInput.Where(Char.IsPunctuation).ToArray();
                var words = singleTextFixedInput.Split().Distinct().Select(x => x.Trim(punctuation));

            
                //Delete the file if it exist, so its a new list everytime
                if (File.Exists(desktop + "/TextReader/TempList.txt")) 
                {
                    File.Delete(desktop + "/TextReader/TempList.txt");
                }

                //Writing it into a templist which only is used to make it into lines
                using (StreamWriter sw2 = File.CreateText(desktop + "/TextReader/TempList.txt"))
                {
                    sw2.WriteLine(string.Join(Environment.NewLine, words));
                }

                //The lines are readed
                string[] textLines = File.ReadAllLines(desktop + "/TextReader/TempList.txt");

                //Adding all the words to the list
                foreach (string fairyTaleline in textLines)
                {
                    textWordList.Add(fairyTaleline);
                }

                //Checking all the words if it the dictonary and then it will add the vector
                foreach (string line in AllWordsDictonary)
                    {
                    if (textWordList.Contains(line))
                    {
                        vectorList.Add(true);
                    }
                    else
                    {
                        vectorList.Add(false);
                    }
                    }
                vectorArray = vectorList.ToArray();
               
                //Calling the distance calculation on every single text and check it upon the unknown text
                DistanceList.Add(CalculateInfo(vectorArray, unknownVectorArray, typeOfText));
            }
            knnAlgo();
        }

        //Taking 3 parameters, the text to check, the unknown text and the story type.
        private DistanceModel CalculateInfo (bool[] textArray, bool[] uknownTextArray, string StoryType)
        {
            DistanceModel dm = new DistanceModel();

            //Making the calculation
            for (int i = 0; i < AllWordsDictonary.Count; i++)
            {
                dm.Sum += Math.Pow((vectorArray[i] ? 1 : 0) - (unknownVectorArray[i] ? 1 : 0), 2);
            }

            double distance = Math.Sqrt(dm.Sum);
            dm.Distance = distance;
            dm.Type = StoryType;
       
            return dm;
        }

        //Calculating the the knn. Here there is used a 5knn. 
        private void knnAlgo()
        {
            int fairytaleScore = 0;
            int sportScore = 0;
            double chanceForRight = 0;

            List<DistanceModel> distanceSortList = new List<DistanceModel>();
            //Making the observable list to a list and order it from lowest to highest
            distanceSortList = distanceList.ToList();
            distanceSortList = distanceSortList.OrderBy((dm) => dm.Distance).ToList();

            //Taking the first 5 and then it check which type of text it is.
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

            //Making the calculations in %, basic math
            chanceForRight = Math.Max(fairytaleScore, sportScore);
            chanceForRight = (chanceForRight / 5) * 100;

            //Just taking the highest one, and tells if its a fairytale or sport article and tells the % for it too.
            if(fairytaleScore > sportScore)
            {

                MessageBox.Show("Your unknown text is a fairytale! I am " + chanceForRight + "% sure!");
            }
            else
            {
                MessageBox.Show("Your unknown text is a Sport article! I am " + chanceForRight + "% sure!");
            }

            //Resetting the values, so you can do it again without closing the program.
            sportScore = 0;
            fairytaleScore = 0;
            chanceForRight = 0;
        }
    }
}
