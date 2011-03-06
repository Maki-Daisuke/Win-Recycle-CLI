using System;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.Reflection;

namespace Recycle
{
    public class Recycle
    {
        public static void Main(string[] args) {
            List<string> targets = new List<string>();

            bool is_interctive = false;
            bool is_force = false;
            bool is_verbose = false;
            bool is_version = false;
            bool is_help = false;

            int length = args.Length;  // real number of target files
            for (int i = 0; i < length; i++) {
                string arg = args[i];
                if (arg[0] == '-') {  // parse options!
                    if (arg[1] == '-') {  // long name options
                        switch (arg.Substring(2)) {
                            case "force":
                                is_interctive = false;
                                is_force = true;
                                break;
                            case "verbose":
                                is_verbose = true;
                                break;
                            case "version":
                                is_version = true;
                                break;
                            case "help":
                                is_help = true;
                                break;
                            default:
                                System.Console.Error.WriteLine("Unknown option: " + arg);
                                Environment.Exit(1);
                                throw new Exception("never reach here");
                        }
                    } else {  // short name options
                        foreach (var c in arg.Substring(1)) {
                            switch (c) {
                                case 'f':
                                    is_force = false;
                                    is_interctive = false;
                                    break;
                                case 'i':
                                    is_force = false;
                                    is_interctive = true;
                                    break;
                                case 'r':
                                    break;  // ignore for compatibility with rm command
                                case 'R':
                                    break;  // ignore for compatibility with rm command
                                case 'v':
                                    is_verbose = true;
                                    break;
                                default:
                                    System.Console.Error.WriteLine("Unknown option: -" + c);
                                    Environment.Exit(1);
                                    throw new Exception("never reach here");
                            }
                        }
                    }
                } else {
                    targets.Add(arg);
                }
            }

            if (is_help) {
                System.Console.Out.WriteLine(appName);
                System.Console.Out.WriteLine(copyrightString);
                System.Console.Out.WriteLine("\nTODO: help message");
                Environment.Exit(0);
            }
            if (is_version) {
                System.Console.Out.WriteLine(appName);
                System.Console.Out.WriteLine(copyrightString);
                Environment.Exit(0);
            }

            foreach (var i in targets) {
                if (is_interctive) {
                    System.Console.Out.Write("remove `{0}'? ", i);
                    var line = System.Console.In.ReadLine();
                    if (line.Length < 1 || line[0] != 'y' && line[0] != 'Y') continue;
                }
                try {
                    if (FileSystem.DirectoryExists(i)) {
                        FileSystem.DeleteDirectory(i, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin, UICancelOption.ThrowException);
                    } else {
                        FileSystem.DeleteFile(i, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin, UICancelOption.ThrowException);
                    }                
                } catch (OperationCanceledException e) {
                    System.Console.Error.WriteLine(e.Message);
                    Environment.Exit(1);
                } catch (IOException e) {
                    if (is_force) {
                        continue;
                    } else {
                        System.Console.Error.WriteLine(e.Message);
                        Environment.Exit(1);
                    }
                } catch (Exception e) {
                    System.Console.Error.WriteLine(e.Message);
                    Environment.Exit(1);
                }
                if (is_verbose) {
                    System.Console.Out.WriteLine(i);
                }
            }
        }

        static string versionString {
            get {
                Version v =  Assembly.GetExecutingAssembly().GetName().Version;
                return String.Format("{0}.{1}.{2}", v.Major, v.Minor, v.Revision);
            }
        }

        static string appName {
            get {
                return "Recycle version " + versionString;
            }
        }

        static string copyrightString {
            get {
                return ((System.Reflection.AssemblyCopyrightAttribute)
                         Attribute.GetCustomAttribute(System.Reflection.Assembly.GetExecutingAssembly(),
                                                      typeof(System.Reflection.AssemblyCopyrightAttribute))).Copyright;
            }
        }
    }
}
