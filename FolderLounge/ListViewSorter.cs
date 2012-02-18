using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Documents;

namespace FolderLounge
{
    public class ListViewSorter
    {
        private GridViewColumnHeader _currentSortCol = null;
        private SortAdorner _sortAdorner = null;

        public void Sort(ListView listView, GridViewColumnHeader sortColumn)
        {            
            String field = sortColumn.Tag as String;

            if (_currentSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(_currentSortCol).Remove(_sortAdorner);
                listView.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (_currentSortCol == sortColumn && _sortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            _currentSortCol = sortColumn;
            _sortAdorner = new SortAdorner(_currentSortCol, newDir);
            AdornerLayer.GetAdornerLayer(_currentSortCol).Add(_sortAdorner);
            listView.Items.SortDescriptions.Add(new SortDescription(field, newDir));

        }
    }
}
