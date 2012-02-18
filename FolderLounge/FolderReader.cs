using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FolderLounge
{
    class FolderReader
    {
        public List<string> GetFolders()
        {
            DirectoryInfo d = new DirectoryInfo(System.Environment.GetFolderPath(Environment.SpecialFolder.Recent));
            List<string> folders = new List<string>();
            foreach (var dir in d.GetFiles())
            {
                using (TextReader tr = File.OpenText(dir.FullName))
                {
                    string line;
                    while ((line = tr.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                        
                    }


                }
            }
            return folders;
        }
    }
}
