using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshAnimation.Util
{
    class FileProcessing
    {
        public static string[] GetObjFromDir(string path)
        {
            List<string> objFiles = new List<string>();
            string[] fileEntries = Directory.GetFiles(path);
            foreach (string fileName in fileEntries)
            {
                if (fileName.EndsWith(".obj"))
                    objFiles.Add(fileName);
            }

            return objFiles.ToArray();
        }

    }
}
