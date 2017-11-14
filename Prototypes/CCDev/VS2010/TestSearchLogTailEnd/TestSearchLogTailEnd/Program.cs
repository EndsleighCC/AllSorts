using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace TestSearchLogTailEnd
{
    class Program
    {
        public class BuildLogDetail
        {
            #region Constructor(s)

            public BuildLogDetail(string filename)
            {
                BuildLogFilename = filename;
            }

            #endregion Constructor(s)

            #region Class Specific Exceptions

            public class BuildLogIsBusyException : ApplicationException
            {
                public BuildLogIsBusyException(string description)
                    : base(description)
                {
                }
            }

            #endregion

            #region Public Members

            public bool Succeeded()
            {
                bool success = false;

                LogErrorData lastLogErrorData = LastLogErrorData();
                if ( lastLogErrorData != null )
                {
                    string logErrorText = lastLogErrorData.ErrorDataLine.Substring(lastLogErrorData.TokenStartIndex,
                                                                                  lastLogErrorData.TokenLength);
                    const string errorWordIdentifier = "Error(s)";
                    const string failedWordIdentifier = "failed";
                    if (String.Compare(logErrorText, errorWordIdentifier,/*ignore case*/ true) == 0)
                    {
                        // VS6 or MSBUILD
                        success = Succeeded(lastLogErrorData, errorWordIdentifier);
                    }
                    else
                    {
                        // VS2005, VS2008 or VS2010
                        success = Succeeded(lastLogErrorData, failedWordIdentifier);
                    }
                }

                return success;
            }

            public LogErrorData LastLogErrorData()
            {
                LogErrorData lastLogErrorData = null;

                if (File.Exists(BuildLogFilename))
                {
                    // Build log file exists

                    FileInfo fileInfo = new FileInfo(BuildLogFilename);

                    try
                    {
                        using (StreamReader streamReader = new StreamReader(BuildLogFilename))
                        {

                            // Regex regex = new Regex("(.*error\\(s\\).*|.*succeeded.*failed.*|.*Error\\(s\\))");
                            Regex regex = new Regex("(error\\(s\\)|.*failed.*|Error\\(s\\))");

                            lastLogErrorData = IdentifyLastErrorDataLine(streamReader, fileInfo.Length, /*4096*/ 300,
                                                                         regex);
                        }

                        if (lastLogErrorData != null)
                        {
                            // Store the last Error Data Line
                            _lastLogErrorDataLine = lastLogErrorData.ErrorDataLine;
                        }
                    }
                    catch (Exception)
                    {
                        throw new BuildLogIsBusyException( String.Format("Build Log \"{0}\" is busy",BuildLogFilename));
                    }
                }

                return lastLogErrorData;
            }

            public string LastErrorDataLine()
            {
                // Return the last Error Data Line if it has already been determined
                string lastErrorDataLine = _lastLogErrorDataLine;

                if (lastErrorDataLine == null)
                {
                    LogErrorData lastLogErrorData = LastLogErrorData();
                    if (lastLogErrorData != null)
                    {
                        lastErrorDataLine = lastLogErrorData.ErrorDataLine;
                        // Store the line
                        _lastLogErrorDataLine = lastErrorDataLine;
                    }
                }

                return lastErrorDataLine;
            }

            public class LogErrorData
            {
                public LogErrorData( string errorDataLine , int tokenStartIndex, int tokenLength )
                {
                    ErrorDataLine = errorDataLine;
                    TokenStartIndex = tokenStartIndex;
                    TokenLength = tokenLength;
                }

                public string ErrorDataLine { get; private set; }
                public int TokenStartIndex { get; private set; }
                public int TokenLength { get; private set; }
            }

            public string BuildLogFilename { get; private set; }

            #endregion Public Members

            #region Private Members

            private LogErrorData IdentifyLastErrorDataLine(StreamReader streamReader, long fileSize, long fileEndWindowSize, Regex regexErrorIndicator)
            {
                LogErrorData logErrorData = null;

                // Ensure that the seek position is within the file
                long setPosition = fileSize - fileEndWindowSize;
                if (setPosition < 0)
                    setPosition = 0;
                // Seek relative to the beginning of the file
                streamReader.BaseStream.Seek(setPosition, SeekOrigin.Begin);

                while (!streamReader.EndOfStream)
                {
                    string fileline = streamReader.ReadLine();
                    if (!((String.IsNullOrEmpty(fileline)) || (String.IsNullOrWhiteSpace(fileline))))
                    {
                        // Line is not empty
                        Match match = regexErrorIndicator.Match(fileline);
                        if (match.Success)
                        {
                            logErrorData = new LogErrorData(fileline,match.Index,match.Length);
                        }
                    } // Line is not empty
                } // while

                return logErrorData;
            }

            private bool Succeeded(LogErrorData lastLogErrorData, string errorWordIdentifier)
            {
                bool success = false;

                // Look for the number that precedes the text
                string[] errorToken = lastLogErrorData.ErrorDataLine.Split(new char[] { ' ', ',' },
                                                                           StringSplitOptions.RemoveEmptyEntries);
                // Find the matching token
                int errorIdentifierIndex = -1;
                for (int tokenIndex = 0; (errorIdentifierIndex == -1) && tokenIndex < errorToken.Length; ++tokenIndex)
                {
                    if (String.Compare(errorToken[tokenIndex], errorWordIdentifier,/*ignore case*/true) == 0)
                        errorIdentifierIndex = tokenIndex;
                }

                if ((errorIdentifierIndex != -1) && (errorIdentifierIndex >= 1))
                {
                    int errorCount = 0;
                    if (Int32.TryParse(errorToken[errorIdentifierIndex - 1], out errorCount))
                        success = errorCount == 0;
                }

                return success;
            }

            private string _lastLogErrorDataLine = null;

            #endregion Private Members

        } // class BuildLogDetail

        static void Main(string[] args)
        {
            foreach ( string arg in args )
            {
                BuildLogDetail buildLogDetail = new BuildLogDetail(arg);
                Console.WriteLine("{0}",arg);
                try
                {
                    Console.WriteLine("    \"{0}\"", buildLogDetail.LastErrorDataLine());
                    // " ()+-,"
                    string lastErrorDataLine = buildLogDetail.LastErrorDataLine();
                    Regex regex = new Regex("([A-Z, ]+[a-z, ]+[0-9, ]+\\(\\))+");
                    Match match = regex.Match(lastErrorDataLine);
                    if (match.Success)
                    {
                        Console.WriteLine("Match is \"{0}\"", lastErrorDataLine.Substring(match.Index,match.Length));
                    }
                        
                    Console.WriteLine("    {0}", (buildLogDetail.Succeeded() ? "++++ Succeeded" : "---- Failed"));
                }
                catch (BuildLogDetail.BuildLogIsBusyException eek)
                {
                    Console.WriteLine("    \"{0}\" is busy",arg);
                }
                catch (Exception eek)
                {
                    Console.WriteLine("    Exception : {0}",eek.ToString());
                }
            }
        }
    }
}
