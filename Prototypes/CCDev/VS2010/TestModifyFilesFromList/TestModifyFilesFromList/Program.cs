using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TestModifyFilesFromList
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length >= 2 )
            {
                bool makeChanges = String.Compare(args[0], "change", true /*ignore case*/) == 0;
                string filenameList = args[1];

                string fullPathAndFilenameList = Path.GetFullPath(filenameList);

                try
                {
                    if (makeChanges)
                    {
                        Console.WriteLine("Selected to make file changes");
                    }
                    else
                    {
                        Console.WriteLine("Selected NOT to make file changes");
                    }

                    StreamReader fileListStream = new StreamReader(fullPathAndFilenameList);

                    int lineNumber = 0;
                    int lineModifiedCount = 0;
                    while (!fileListStream.EndOfStream)
                    {
                        lineNumber += 1;
                        string fileline = fileListStream.ReadLine();
                        if ( fileline.IndexOf("D:") != -1)
                        {
                            int indexStartMinusOne = fileline.IndexOf(' ');
                            int indexEnd = fileline.IndexOf(" - ");
                            string filename = fileline.Substring(indexStartMinusOne + 1, indexEnd-indexStartMinusOne-1);

                            filename = filename.Replace("D:\\SysST01", "\\\\adebs02\\SysUT00");

                            FileInfo fileinfo = new FileInfo(filename);
                            if (! fileinfo.Exists)
                            {
                                Console.WriteLine("{0} : {1} : \"{2}\" does not exist", lineNumber,
                                                  lineModifiedCount, filename);
                            }
                            else
                            {
                                string filenameLowerCase = filename.ToLower();
                                string AjaxControlToolkitLowerCase = "\\AjaxControlToolkit\\".ToLower();
                                string javaScriptExtensionLowerCase = ".js".ToLower();

                                if ((filenameLowerCase.IndexOf(javaScriptExtensionLowerCase) >= 0 )
                                    && (filenameLowerCase.IndexOf(AjaxControlToolkitLowerCase) >= 0 )
                                    )
                                {
                                    Console.WriteLine("{0} : {1} : \"{2}\" Not modifying because it's JAVA script and AjaxControlToolkit",
                                                        lineNumber,
                                                        lineModifiedCount,
                                                        filename);
                                }
                                else
                                {
                                    // Not JAVA script and not AjaxControlToolkit

                                    // Don't modify the Ajax Control Toolkit because the build will fail
                                    // because these files are modified during the build

                                    if (fileinfo.IsReadOnly)
                                    {
                                        Console.WriteLine("{0} : {1} : \"{2}\" is already read-only", lineNumber,
                                                          lineModifiedCount, filename);
                                    }
                                    else
                                    {
                                        lineModifiedCount += 1;

                                        if (makeChanges)
                                        {
                                            Console.WriteLine("{0} : {1} : Setting \"{2}\" to read-only", lineNumber,
                                                              lineModifiedCount, filename);
                                            fileinfo.IsReadOnly = true;
                                        }
                                        else
                                        {
                                            Console.WriteLine("{0} : {1} : Would set \"{2}\" to read-only", lineNumber,
                                                              lineModifiedCount, filename);
                                        }
                                    }

                                } // Not JAVA script and not AjaxControlToolkit
                            }
                        }
                    }

                    Console.WriteLine();
                    Console.WriteLine("File \"{0}\" modified file count is {1}", fullPathAndFilenameList, lineModifiedCount);

                }
                catch (Exception)
                {
                    Console.WriteLine("File \"{0}\" does not exist", fullPathAndFilenameList);
                }

            }
        }
    }
}
