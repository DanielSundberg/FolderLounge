using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Forms;

namespace FolderLounge
{
    
    public class FolderViewModel : INotifyPropertyChanged
    {
        ObservableCollection<FolderDisplayItem> _folderDisplayItems = new ObservableCollection<FolderDisplayItem>();
        
        public FolderViewModel()
        {
            var folders = (new FolderReader()).GetFolders(); 
            folders.ForEach(f => _folderDisplayItems.Add(f));

            // Read from text file

            

            //_folderDisplayItems.Add(new FolderDisplayItem(@"c:\Windows"));
            //_folderDisplayItems.Add(new FolderDisplayItem(@"c:\Windows\System"));
            //_folderDisplayItems.Add(new FolderDisplayItem(@"c:\Documents"));
            //_folderDisplayItems.Add(new FolderDisplayItem(@"c:\Users\dasun"));
            //var folders = (new FolderReader()).GetFolders();
            //folders.ForEach(f => _folderDisplayItems.Add(new FolderDisplayItem(f)));
        }

        public ObservableCollection<FolderDisplayItem> FolderDisplayItems
        {
            get
            {
                return _folderDisplayItems;
            }
        }

        public FolderDisplayItem FirstVisible()
        {
            return _folderDisplayItems.FirstOrDefault(t => t.Visible);
        }

        public string SearchFilter 
        {
            get
            {
                return _searchFilter;
            }
            set
            {
                _searchFilter = value;
                
                var view = CollectionViewSource.GetDefaultView(_folderDisplayItems);
                view.Filter = null;
                view.Filter = i => ((FolderDisplayItem)i).Folder.ToLower().Contains(_searchFilter.ToLower());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private string _searchFilter;

        internal void SelectPrev()
        {
            var dv = CollectionViewSource.GetDefaultView(_folderDisplayItems);
            dv.MoveCurrentToPrevious();
            if (dv.IsCurrentBeforeFirst)
            {
                dv.MoveCurrentToFirst();
            }
        }

        internal void SelectNext()
        {
            var dv = CollectionViewSource.GetDefaultView(_folderDisplayItems);
            dv.MoveCurrentToNext();
            if (dv.IsCurrentAfterLast)
            {
                dv.MoveCurrentToLast();
            }
        }

        internal string GetSelectedFolder()
        {
            var dv = CollectionViewSource.GetDefaultView(_folderDisplayItems);
            if (dv.IsCurrentAfterLast || dv.IsCurrentBeforeFirst)
            {
                dv.MoveCurrentToNext();
            }
            if (dv.CurrentItem != null)
                return ((FolderDisplayItem)dv.CurrentItem).Folder;
            else
                return null;
        }

        internal void Launch()
        {
            Process.Start(GetSelectedFolder());
        }
    }
}
