using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FolderLounge
{
    class FolderReader
    {
        private const string CONFIG_FILE = "folders.cfg";
        private const string CONFIG_FILE_BACKUP = "folders.cfg.bak";

        public static string GetLnkTarget(string lnkPath)
        {
            FileInfo fileInfo = new FileInfo(lnkPath);
            if (fileInfo.Extension.ToUpper() == ".LNK")
            {
                var shl = new Shell32.Shell();         // Move this to class scope
                lnkPath = System.IO.Path.GetFullPath(lnkPath);
                var dir = shl.NameSpace(System.IO.Path.GetDirectoryName(lnkPath));
                var itm = dir.Items().Item(System.IO.Path.GetFileName(lnkPath));

                var lnk = (Shell32.ShellLinkObject)itm.GetLink;
                return lnk.Target.Path;
            }
            else
            {
                return string.Empty;
            }
        }

        public List<FolderDisplayItem> GetFolders()
        {
            Dictionary<string, FolderDisplayItem> folders = new Dictionary<string, FolderDisplayItem>();

            // First get config file from local app data folder
            //string homePath = Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), 

            // Then read default folders.cfg
            using (TextReader textReader = File.OpenText(CONFIG_FILE))
            {
                string line;
                while ((line = textReader.ReadLine()) != null)
                {
                    if (!folders.ContainsKey(line))
                    {
                        folders.Add(line, new FolderDisplayItem(line, true));
                    }
                }
            }

            // Last read recent folders;
            DirectoryInfo d = new DirectoryInfo(System.Environment.GetFolderPath(Environment.SpecialFolder.Recent));
            foreach (FileInfo lnk in d.GetFiles())
            {
                string path = GetLnkTarget(lnk.FullName);
                if (Directory.Exists(path))
                {
                    if (!folders.ContainsKey(path))
                    {
                        folders.Add(path, new FolderDisplayItem(path, false));
                    }
                }
            }

            // Setup file monitor for recent folders

            return folders.Values.ToList();
        }

        internal void Save(System.Collections.ObjectModel.ObservableCollection<FolderDisplayItem> folderDisplayItems)
        {
            File.Copy(CONFIG_FILE, CONFIG_FILE_BACKUP, true);
            File.Delete(CONFIG_FILE);
            using (StreamWriter fileWriter = new StreamWriter(CONFIG_FILE))
            {
                foreach (var f in folderDisplayItems) 
                {
                    if (f.Pinned)
                    {
                        fileWriter.WriteLine(f.Folder);
                    }
                }
            }
        }
    }
}
