using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Forms;
using System.Collections.Specialized;

namespace FolderLounge
{
    public class FolderViewModel
    {
        private ObservableCollection<FolderDisplayItem> _folderDisplayItems = new ObservableCollection<FolderDisplayItem>();
        
        public FolderViewModel()
        {
            var folders = (new FolderReader()).GetFolders(); 
            folders.ForEach(f => _folderDisplayItems.Add(f));

            _folderDisplayItems.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(_folderDisplayItems_CollectionChanged);
        }

        private void _folderDisplayItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (FolderDisplayItem item in e.OldItems)
                {
                    //Removed items
                    item.PropertyChanged -= EntityViewModelPropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (FolderDisplayItem item in e.NewItems)
                {
                    //Added items
                    item.PropertyChanged += EntityViewModelPropertyChanged;
                }
            }    
        }

        private void EntityViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // This will get called when the property of an object inside the collection changes
            // Persist items
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
