using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace TestInetRegisterCOMObjects
{
    public class Program
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetShortPathName(
            [MarshalAs(UnmanagedType.LPTStr)] string path,
            [MarshalAs(UnmanagedType.LPTStr)] StringBuilder shortPath,
            int shortPathLength
            );

        private static void Main(string[] args)
        {
            foreach (string arg in args)
            {
                StringBuilder shortPath = new StringBuilder(255);
                GetShortPathName(arg, shortPath, shortPath.Capacity);
                Console.WriteLine(shortPath.ToString());
            }
        }
    }
}