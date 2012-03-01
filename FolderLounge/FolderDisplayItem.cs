using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace FolderLounge
{
    public class FolderDisplayItem : INotifyPropertyChanged
    {
        private string _folder;
        private bool _visible = true;
        private bool _pinned;
        private string _state;

        public string State
        {
            get { return _state; }
            set { _state = value; }
        }

        public bool Pinned
        {
            get { return _pinned; }
            set 
            { 
                _pinned = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Pinned"));
                }
            }
        }

        public bool Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Visible"));
                }
            }
        }
        public string Folder
        {
            get { return _folder; }
            set { _folder = value; }
        }
        public FolderDisplayItem(string folder, bool pinned, string state)
        {
            _folder = folder;
            _pinned = pinned;
            _state = state;
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
