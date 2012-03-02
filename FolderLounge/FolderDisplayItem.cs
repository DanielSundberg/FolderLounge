using System.ComponentModel;

namespace FolderLounge
{
    public class FolderDisplayItem : INotifyPropertyChanged
    {
        private string _folder;
        private bool _visible = true;
        private bool _pinned;
        private string _category;

        public string Category
        {
            get { return _category; }
            set { _category = value; }
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
        public FolderDisplayItem(string folder, bool pinned, string category)
        {
            _folder = folder;
            _pinned = pinned;
            _category = category;
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
