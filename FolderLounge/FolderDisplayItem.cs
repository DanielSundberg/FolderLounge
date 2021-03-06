﻿using System.ComponentModel;

namespace FolderLounge
{
    public class FolderDisplayItem : INotifyPropertyChanged
    {
        private string _folder;
        private bool _visible = true;
        private bool _pinned;

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
        public FolderDisplayItem(string folder, bool pinned)
        {
            _folder = folder;
            _pinned = pinned;
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
