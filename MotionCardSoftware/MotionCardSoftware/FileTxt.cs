using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;


namespace MotionCardSoftware
{
    class FileTxt
    {
        static public void FileTxtCopy(string OriginalFile,string targetFile)
        {
            using (StreamReader sr = new StreamReader(OriginalFile))
            {
                String txt = sr.ReadToEnd();
                using (StreamWriter sw = new StreamWriter(targetFile))
                {
                    sw.Write(txt);
                    sw.Flush();
                }
            }
        }
    }
}
