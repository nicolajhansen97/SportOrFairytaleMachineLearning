using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportOrFairytaleMachine.Model;

namespace SportOrFairytaleMachine.ViewModel
{
    /// <summary>
    /// a baseclass for changing viewmodels, but here it only has one. 
    /// </summary>
    class MainViewModel : Bindable
    {
        /// <summary>
        /// A getter and setter for my viewmodel
        /// </summary>
        public GenreReaderViewModel TDFVM { get; set; }
        /// <summary>
        /// a private instacnce of my current view
        /// </summary>
        private object _currentView;
        /// <summary>
        /// a complete getter and setter with propyischanged to dectect if i change viewmodel.
        /// </summary>
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; propertyIsChanged(); }
        }
        /// <summary>
        /// The constructor that is used to new the viewmodel and make the currentview into the viewmodel.
        /// </summary>
        public MainViewModel()
        {
            TDFVM = new GenreReaderViewModel();
            CurrentView = TDFVM;
        }
    }
}
