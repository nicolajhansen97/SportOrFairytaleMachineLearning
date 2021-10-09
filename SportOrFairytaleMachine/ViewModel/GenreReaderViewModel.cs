using Microsoft.Win32;
using SportOrFairytaleMachine.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace SportOrFairytaleMachine.ViewModel
{
    class GenreReaderViewModel : Bindable
    {

        public DelegateCommand openFileChooser { get; set; }

        private string _loadFile = "Load file";

        public string LoadFile
        {
            get { return _loadFile; }
            set { _loadFile = value; propertyIsChanged(); }
        }

        public GenreReaderViewModel()
        {
            openFileChooser = new DelegateCommand(o =>
                {
                    FileChooser();
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
    }
}
