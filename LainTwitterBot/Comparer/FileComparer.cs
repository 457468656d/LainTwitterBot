using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LainTwitterBot.Comparer
{
    /// <summary>
    /// FileComparer to sort the files like the windows explorer.
    /// </summary>
    public class FileComparer : IComparer<string>
    {
        // Source:https://learn.microsoft.com/en-us/windows/win32/api/shlwapi/nf-shlwapi-strcmplogicalw
        /// <summary>
        /// Importing the shlwapi.dll. Windows Explorer sorts the files using this function.
        /// </summary>
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        static extern int StrCmpLogicalW(String x, String y);

        public int Compare(string x, string y)
        {
            // Putting this function into the Compare Methode in a IComparer class to sort arrays.
            return StrCmpLogicalW(x, y);
        }
    }
}
