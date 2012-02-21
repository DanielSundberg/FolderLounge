using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FolderLounge
{
    class FolderReader
    {
        public static string GetLnkTarget(string lnkPath)
        {
            var shl = new Shell32.Shell();         // Move this to class scope
            lnkPath = System.IO.Path.GetFullPath(lnkPath);
            var dir = shl.NameSpace(System.IO.Path.GetDirectoryName(lnkPath));
            var itm = dir.Items().Item(System.IO.Path.GetFileName(lnkPath));
            try
            {
                // Remove exception
                var lnk = (Shell32.ShellLinkObject)itm.GetLink;
                return lnk.Target.Path;
            }
            catch (NotImplementedException)
            {
                return string.Empty;
            }
        }

        public List<string> GetFolders()
        {
            List<string> folders = new List<string>();
            //foreach (var dir in d.GetFiles())
            //{
            //    using (TextReader tr = File.OpenText(dir.FullName))
            //    {
            //        string line;
            //        while ((line = tr.ReadLine()) != null)
            //        {
            //            Console.WriteLine(line);
                        
            //        }


            //    }
            //}
            using (TextReader textReader = File.OpenText("folders.cfg"))
            {
                string line;
                while ((line = textReader.ReadLine()) != null)
                {
                    folders.Add(line);

                }
            }

            //var links = new List<string>();
            DirectoryInfo d = new DirectoryInfo(System.Environment.GetFolderPath(Environment.SpecialFolder.Recent));
            foreach (FileInfo lnk in d.GetFiles())
            {
                string path = GetLnkTarget(lnk.FullName);
                if (Directory.Exists(path))
                {
                    folders.Add(path);
                }
            }

            // Setup file monitor for recent folders

            return folders;
        }
    }
}
