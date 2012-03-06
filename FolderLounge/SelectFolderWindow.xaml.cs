using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Globalization;
using System.ComponentModel;
using System.Windows.Forms;

namespace FolderLounge
{
    /// <summary>
    /// Interaction logic for SelectTaskWindow.xaml
    /// </summary>
    public partial class SelectTaskWindow : Window
    {
        private ListViewSorter _listViewSorter = new ListViewSorter();
        private HotKey _hotkey;

        public SelectTaskWindow()
        {
            InitializeComponent();

            var folderViewModel = new FolderViewModel();
            DataContext = folderViewModel;

            // Setup sys tray icon
            System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon();
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            ToolStripItem item = new ToolStripMenuItem();
            item.Text = "Quit";
            item.Click += (s, e) =>
                {
                    System.Windows.Application.Current.Shutdown();
                };
            contextMenu.Items.Add(item);
            notifyIcon.Icon = new System.Drawing.Icon(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("FolderLounge.propertysheets.ico"));
            notifyIcon.ContextMenuStrip = contextMenu;
            notifyIcon.Visible = true;
            notifyIcon.DoubleClick += (s, e) => 
            {
                ShowThisWindow();
            };

            // Setup global hotkey for showing main Window
            Loaded += (s, e) =>
            {
                _hotkey = new HotKey(ModifierKeys.Windows | ModifierKeys.Shift, Keys.E, this);
                _hotkey.HotKeyPressed += (k) => ShowThisWindow();
                Hide();

                // Setup grouping in list view
                var dv = CollectionViewSource.GetDefaultView((DataContext as FolderViewModel).FolderDisplayItems);
                dv.GroupDescriptions.Add(new PropertyGroupDescription("Pinned", new BooleanToPinnedTextConverter()));
            };
        }

        private void folderViewModel_PinnedFoldersChanged()
        {
            // Refresh view to update categories
            var dv = CollectionViewSource.GetDefaultView((DataContext as FolderViewModel).FolderDisplayItems);
            dv.Refresh();
        }

        private void ShowThisWindow()
        {
            this.Show();
            _textBox.Clear();
            
            SelectFirstListViewItem();

            this.WindowState = WindowState.Normal;
        }

        private void SelectFirstListViewItem()
        {
            var dv = CollectionViewSource.GetDefaultView((DataContext as FolderViewModel).FolderDisplayItems);
            dv.MoveCurrentTo((DataContext as FolderViewModel).FirstVisible());
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            WindowState = WindowState.Minimized;
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                this.Hide();
            base.OnStateChanged(e);
        }

        private void _textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            (DataContext as FolderViewModel).SearchFilter = _textBox.Text;
            SelectFirstListViewItem();
        }

        private void _clearButton_Click(object sender, RoutedEventArgs e)
        {
            _textBox.Text = string.Empty;
        }

        private void _sortClicked(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = sender as GridViewColumnHeader;
            _listViewSorter.Sort(_folderListView, column);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _textBox.Focus();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                (DataContext as FolderViewModel).SelectNext();
            }
            else if (e.Key == Key.Up)
            {
                (DataContext as FolderViewModel).SelectPrev();
            }
            else if (e.Key == Key.Escape)
            {
                HandleEscKeyDown();
            }
            else if (e.Key == Key.Enter)
            {
                Submit();
            }
        }

        private void HandleEscKeyDown()
        {
            if (_textBox.Text.Length > 0)
            {
                _textBox.Clear();
            }
            else
            {
                Hide();    
            }
        }

        private void Submit()
        {
            Hide();
            (DataContext as FolderViewModel).Launch();
        }

        private void _okButton_Click(object sender, RoutedEventArgs e)
        {
            Submit();
        }

        private void _cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void _textBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                _textBox.Text = string.Empty;
            }
        }

        private void _folderListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Submit();
        }

    }
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value == true)
            {
                return System.Windows.Visibility.Visible;
            }
            else
            {
                return System.Windows.Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

}
