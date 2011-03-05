using System;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace Recycle
{
    public class Recycle
    {
        public static void Main(string[] args) {
            try {
                foreach (var i in args) {
                    FileSystem.DeleteFile(i, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin, UICancelOption.ThrowException);
                }
            } catch ( Exception e ) {
                System.Console.Error.WriteLine(e.Message);
                Environment.Exit(1);
            }
        }
    }
}
