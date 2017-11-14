using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Linq.Expressions;
using System.Text;

namespace eisTmsDocumentationGenerator
{
    class Program
    {

        public class TmsDocumentation
        {
            #region Constructors

            public TmsDocumentation(string tmsRiskName, string rootPath)
            {
                TmsRiskType tmsRiskType = TmsRiskType.Unknown;
                // Try to identify the appropriate memeber of the TMS Risk Type enumeration
                try
                {
                    tmsRiskType =
                        _riskEngineName.Where(m => String.Compare(m.Value, tmsRiskName, true /* ignore case */ ) == 0)
                        .First()
                        .Key;
                }
                catch (Exception)
                {
                    Console.WriteLine("TMS Risk Name \"{0}\" is not known",tmsRiskName);
                    throw;
                }
                Initialise(tmsRiskType, rootPath);
            }

            public TmsDocumentation(TmsRiskType tmsRiskType, string rootPath)
            {
                Initialise(tmsRiskType,rootPath);
            }

            #endregion Constructors

            #region Public Types

            public enum TmsRiskType
            {
                Unknown,
                Household,
                Motor,
                Possessions
            }

            #endregion Public Types

            #region Public Methods

            public void ReadData()
            {
                ReadCauseAndEffectData();
                ReadPremCalcDataItemData();
            }

            public void Display()
            {
                if (_causeHeaderCollection.Count != _causeHelperCollection.Count)
                {
                    Console.WriteLine( "The count of Cause Headers ({0}) is not equal to the count of Cause Helpers ({1})",
                        _causeHeaderCollection.Count,_causeHelperCollection.Count);
                }
                else
                {
                    if (_effectHeaderCollection.Count != _effectHelperCollection.Count)
                    {
                        Console.WriteLine(
                            "The count of Effect Headers ({0}) is not equal to the count of Effect Helpers ({1})",
                            _effectHeaderCollection.Count, _effectHelperCollection.Count);
                    }
                    else
                    {
                        DisplayCausesAndEffects();
                    }
                }

                DisplayPremCalcDataItemData();
            }

            #endregion

            #region Private Methods

            private void Initialise(TmsRiskType tmsRiskType, string rootPath)
            {
                _tmsRiskType = tmsRiskType;

                if (rootPath.Substring(rootPath.Length - 1) != "\\")
                {
                    rootPath = rootPath.ToUpper() + "\\";
                }
                _rootPath = rootPath;
                try
                {
                    _riskEngineFullPath = Path.Combine(rootPath, _riskEngineRelativePath[tmsRiskType]);
                    _riskDataFullPath = Path.Combine(rootPath, _riskDataRelativePath[tmsRiskType]);
                }
                catch (Exception)
                {
                    Console.WriteLine("Unexpected Risk Type \"{0}\"", Enum.GetName(typeof(TmsRiskType), tmsRiskType));
                    throw;
                }
                Console.WriteLine("Path to {0} PremCalc is \"{1}\"", _riskEngineName[tmsRiskType],_riskEngineFullPath);
                Console.WriteLine("Path to {0} TMS is \"{1}\"", _riskEngineName[tmsRiskType],_riskDataFullPath);
            }

            #region Cause and Effect Management

            static class CauseEffectUtility 
            {
                public static string Tidy(string str)
                {
                    string strInternal = str.TrimEnd(new char[] { '\0' });
                    strInternal = strInternal.Replace('(', '[');
                    strInternal = strInternal.Replace(')', ']');
                    return strInternal;
                }
            }

            private class CauseEffectHeader
            {
                public CauseEffectHeader(BinaryReader binaryReader)
                {
                    Read(binaryReader);
                }

                public int Number { get; private set; }
                public int Sector { get; private set; }
                public string Type { get; private set; }
                public string Description { get; private set; }
                public int ParameterCount { get; private set; }

                public List<string> Parameter = new List<string>();

                private void Read(BinaryReader binaryReader)
                {
                    Number = binaryReader.ReadInt16();
                    Sector = binaryReader.ReadInt16();
                    Type = new string(binaryReader.ReadChar(), 1);
                    Description = CauseEffectUtility.Tidy(new string(binaryReader.ReadChars(CauseEffectHeaderDescriptionFieldSize)));
                    ParameterCount = binaryReader.ReadInt16();

                    for (int parameterIndex = 0; parameterIndex < ParameterCount; parameterIndex++)
                    {
                        Parameter.Add(CauseEffectUtility.Tidy(new string(binaryReader.ReadChars(CauseEffectHeaderParameterFieldSize))));
                    }
                }

                private const int CauseEffectHeaderDescriptionFieldSize = 45;
                private const int CauseEffectHeaderParameterFieldSize = 23;

            } // CauseEffectHeader

            private class CauseEffectHelper
            {
                public CauseEffectHelper(BinaryReader binaryReader)
                {
                    Read(binaryReader);
                }

                public int Number { get; private set; }
                public string Description { get; private set; }
                public int Sector { get; private set; }

                private void Read(BinaryReader binaryReader)
                {
                    Number = binaryReader.ReadInt16();
                    Description = CauseEffectUtility.Tidy(new string(binaryReader.ReadChars(CauseEffectHelpDescriptionFieldSize)));
                    Sector = binaryReader.ReadInt16();
                    // Skip the carriage return, linefeed pair
                    binaryReader.ReadChars(2);
                }

                private const int CauseEffectHelpDescriptionFieldSize = 80;

            } // CauseEffectHelper

            private void ReadCauseEffectData(string causeOrEffect,
                                             string headerFilename,
                                             string helperFilename,
                                             SortedDictionary<int,CauseEffectHeader> causeEffectHeaderCollection ,
                                             SortedDictionary<int,CauseEffectHelper> causeEffectHelperCollection )
            {
                causeEffectHeaderCollection.Clear();
                if (!File.Exists(headerFilename))
                {
                    Console.WriteLine("TMS {0} File \"{1}\" does not exist", causeOrEffect,headerFilename);
                }
                else
                {
                    using (BinaryReader binaryReader = new BinaryReader(File.OpenRead(headerFilename)))
                    {
                        while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
                        {
                            CauseEffectHeader causeEffectHeader = new CauseEffectHeader(binaryReader);
                            causeEffectHeaderCollection.Add(causeEffectHeader.Number, causeEffectHeader);
                        }

                    }
                }
                causeEffectHelperCollection.Clear();
                if (!File.Exists(helperFilename))
                {
                    Console.WriteLine("TMS {0} File \"{1}\" does not exist", causeOrEffect, helperFilename);
                }
                else
                {
                    using (BinaryReader binaryReader = new BinaryReader(File.OpenRead(helperFilename)))
                    {
                        while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
                        {
                            CauseEffectHelper causeEffectHelper = new CauseEffectHelper(binaryReader);
                            causeEffectHelperCollection.Add(causeEffectHelper.Number, causeEffectHelper);
                        }

                    }
                }
            }

            private void ReadCauseAndEffectData()
            {
                ReadCauseData();
                ReadEffectData();
            }

            private void ReadCauseData()
            {
                string causeHeaderFilename = _riskDataFullPath + "\\" + _causeHeaderFileName;
                string causeHelperFilename = _riskDataFullPath + "\\" + _causeHelperFileName;
                ReadCauseEffectData("Cause", causeHeaderFilename, causeHelperFilename, _causeHeaderCollection, _causeHelperCollection);
            }

            private void ReadEffectData()
            {
                string effectHeaderFilename = _riskDataFullPath + "\\" + _effectHeaderFileName;
                string effectHelperFilename = _riskDataFullPath + "\\" + _effectHelperFileName;
                ReadCauseEffectData("Effect", effectHeaderFilename, effectHelperFilename, _effectHeaderCollection, _effectHelperCollection);
            }

            private void DisplayCausesOrEffects(string causeOrEffectWord,
                                                SortedDictionary<int, CauseEffectHeader> causeEffectHeaderCollection,
                                                SortedDictionary<int, CauseEffectHelper> causeEffectHelperCollection)
            {
                Console.WriteLine();
                Console.WriteLine("Count of {0}s is {1}", causeOrEffectWord, causeEffectHeaderCollection.Count);
                Console.WriteLine();
                foreach (int causeEffectHeaderIndex in causeEffectHeaderCollection.Keys)
                {
                    CauseEffectHeader causeEffectHeader = causeEffectHeaderCollection[causeEffectHeaderIndex];
                    CauseEffectHelper causeEffectHelper = causeEffectHelperCollection[causeEffectHeaderIndex];
                    Console.WriteLine("    {0} = \"{1}\" = \"{2}\"",
                                        causeEffectHeader.Number,
                                        causeEffectHeader.Description,
                                        causeEffectHelper.Description);
                }
            }

            private void DisplayCausesAndEffects()
            {
                DisplayCausesOrEffects("Cause", _causeHeaderCollection, _causeHelperCollection);
                DisplayCausesOrEffects("Effect", _effectHeaderCollection, _effectHelperCollection);
            }

            #endregion Cause and Effect Management

            #region PremCalc Data Item Management

            private class PremCalcDataItemInternalData
            {
                public PremCalcDataItemInternalData(string premCalcDataItemInternalId)
                {
                    PremCalcDataItemInternalId = premCalcDataItemInternalId;
                }
                public string PremCalcDataItemInternalId { get; private set; }
                public List<string> PremCalcDataItemSourceCode { get; set; }
            } // PremCalcDataItemInternalData

            private string ReadSourceLine(StreamReader streamReader, ref int lineNumber)
            {
                string sourceLine = "";

                bool finished = false;
                while ((!streamReader.EndOfStream) && ( !finished ) )
                {
                    lineNumber += 1;
                    string line = streamReader.ReadLine();
                    // Console.WriteLine("ReadSourceLine {0} : {1}", lineNumber, line);
                    if (line == null)
                    {
                        finished = true;
                    }
                    else
                    {
                        sourceLine += line;
                        finished = line.Contains(";");
                    }
                }
                // Console.WriteLine("ReadSourceLine return {0} : {1}", lineNumber, sourceLine);
                return sourceLine;
            }

            private bool IsEmbeddableChar(char ch)
            {
                bool isEmbeddableChar = ((ch >= '0') && (ch <= '9')
                                         || (ch >= 'A') && (ch <= 'Z')
                                         || (ch >= 'a') && (ch <= 'z')
                                         || (ch == '_'));
                return isEmbeddableChar;
            }

            private bool IsEmbedded(string item, string str)
            {
                bool isEmbedded = false;

                int indexItem = str.IndexOf(item);
                // Check for embedding on the left first
                if (indexItem >= 0)
                {
                    // Could be embedded on the left
                    isEmbedded = IsEmbeddableChar(str[indexItem - 1]);
                }
                if ((!isEmbedded) && (indexItem + item.Length < str.Length))
                {
                    // Could be embedded on the right
                    isEmbedded = IsEmbeddableChar(str[indexItem + item.Length ]);
                }
                return isEmbedded;
            }

            private void ReadPremCalcDataItemData()
            {
                string premCalcDataItemFilename = _riskEngineFullPath + "\\" + _riskEngineMnemonic[_tmsRiskType] +
                                                  RiskConstants.PremCalcDataItemSourceFileEnd;
                using (StreamReader streamReader = File.OpenText(premCalcDataItemFilename))
                {
                    try
                    {
                        int lineNumber = 0;
                        int lineNumberFirstDefinition = 0;
                        int lineNumberLastDefinition = 0;

                        while (!streamReader.EndOfStream)
                        {
                            string sourceLine = ReadSourceLine(streamReader,ref lineNumber);

                            // Console.WriteLine("ReadPremCalcDataItemData {0} : {1}", lineNumber, sourceLine);

                            if (((sourceLine.Contains("InsertSimpleMapEntry"))) || ((sourceLine.Contains("InsertMapEntry"))))
                            {
                                // Perhaps found a PremCalc Data Item Definition

                                string[] sourceLinePart = sourceLine.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                                if (sourceLinePart.Length > 0)
                                {
                                    // Looks like a PremCalc Data Item definition of some sort

                                    // InsertSimpleMapEntry( "THING" , epdiThing ) ;
                                    //           0              1    2    3
                                    // InsertMapEntry( "OTHERTHING" , epdiDriverAccidentIsFaultCount , epdiotSubObjectArray , epdiDriverAccidentCount ) ;
                                    //       0              1       2              3

                                    //if (sourceLinePart[0].Contains("InsertSimpleMapEntry"))
                                    //{
                                    //    Console.WriteLine("InsertSimpleMapEntry");
                                    //} else if (sourceLinePart[0].Contains("InsertMapEntry"))
                                    //{
                                    //    Console.WriteLine("InsertMapEntry");
                                    //}

                                    int firstDoubleQuoteIndex = sourceLinePart[1].IndexOf('\"');
                                    if ( firstDoubleQuoteIndex >= 0 )
                                    {
                                        // A string PremCalc Data Item constant name

                                        if (lineNumberFirstDefinition == 0)
                                        {
                                            lineNumberFirstDefinition = lineNumber;
                                        }

                                        lineNumberLastDefinition = lineNumber;

                                        string premCalcDataItemName = null;
                                        string premCalcDataItemInternalId = null;

                                        premCalcDataItemName = sourceLinePart[1].Trim('\"');
                                        premCalcDataItemInternalId = sourceLinePart[3].Trim();

                                        _premCalcDataItemCollection.Add(premCalcDataItemName,
                                            new PremCalcDataItemInternalData(premCalcDataItemInternalId));

                                    } // A string PremCalc Data Item constant name

                                } // Looks like a PremCalc Data Item definition of some sort

                            } // Perhaps found a PremCalc Data Item Definition
                            else
                            {
                                // Guess that there are no more PremCalc Data Item name definitions after this many lines
                                // Console.WriteLine("ReadPremCalcDataItemData {0} : Processing {1}", lineNumber, sourceLine);
                                const int noMoreDefinitionsLineWindow = 5;
                                if ((lineNumberFirstDefinition > 0) && (lineNumber > (lineNumberLastDefinition + noMoreDefinitionsLineWindow)))
                                {
                                    // Search for case labels containing the enumerations to find the code

                                    // case epdiThing : stuff
                                    //  0      1      2 ... 
                                    // case epdiThing: stuff
                                    //  0      1         2 ... 
                                    string[] sourceLinePart = sourceLine.Split(new char[] { ' ' , '\t' }, StringSplitOptions.RemoveEmptyEntries);
                                    try
                                    {
                                        string caseWord = "case";
                                        if ((sourceLinePart.Length > 0) && (String.Compare(sourceLinePart[0], caseWord) == 0))
                                        {
                                            // case label identified

                                            // Console.WriteLine("Case Label identified in {0}",sourceLine);

                                            // Trim trailing switch statement colons from the case label
                                            sourceLinePart[1] = sourceLinePart[1].Trim(':');

                                            if (sourceLinePart[1].StartsWith("epdi"))
                                            {
                                                // Read the code definition for the case
                                                string caseLabel = sourceLinePart[1];

                                                List<string> sourceLineList = new List<string>();

                                                // Tidy up the case item definition by taking everything after the colon

                                                int colonPos = sourceLine.IndexOf(':');
                                                if (colonPos >= 0)
                                                {
                                                    // There is a colon
                                                    string sourceLineDefinition =
                                                        sourceLine.Substring(colonPos + 1).Trim();

                                                    sourceLineList.Add(sourceLineDefinition);

                                                    bool foundCase = false;
                                                    while ((!foundCase) && (!streamReader.EndOfStream))
                                                    {
                                                        long filePositionPriorToCase = streamReader.BaseStream.Position;
                                                        int lineNumberPriorToCase = lineNumber;
                                                        // Remember the file position prior to the next statement
                                                        sourceLineDefinition = ReadSourceLine(streamReader,ref lineNumber);
                                                        if (sourceLineDefinition.Contains(caseWord)
                                                            && (!IsEmbedded(caseWord, sourceLineDefinition)))
                                                        {
                                                            string[] innerSourceLinePart = sourceLine.Split(new char[] { ' ', '\t' },
                                                                                                        StringSplitOptions.RemoveEmptyEntries);
                                                            if (innerSourceLinePart[1].StartsWith("epdi"))
                                                            {
                                                                // Next epdi case
                                                                foundCase = true;

                                                                // Ensure the case can be reprocessed
                                                                streamReader.BaseStream.Position = filePositionPriorToCase;
                                                                lineNumber = lineNumberPriorToCase;

                                                                string nextLine = streamReader.ReadLine();
                                                                streamReader.BaseStream.Position = filePositionPriorToCase;

                                                            } // Next epdi case
                                                            else
                                                            {
                                                                sourceLineList.Add(sourceLineDefinition);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            sourceLineList.Add(sourceLineDefinition);
                                                        }
                                                    } // while

                                                } // There is a colon

                                                // Look for a matching PremCalc Data Item Internal Id
                                                foreach (PremCalcDataItemInternalData premCalcDataItemInternalData in _premCalcDataItemCollection.Values)
                                                {
                                                    if ( String.Compare(caseLabel,premCalcDataItemInternalData.PremCalcDataItemInternalId) == 0 )
                                                    {
                                                        premCalcDataItemInternalData.PremCalcDataItemSourceCode = sourceLineList;
                                                    }
                                                } // foreach

                                            } // Read the code definition for the case

                                        } // case label identified
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("ReadPremCalcDataItemData : Exception \"{0}\"",ex.ToString());
                                        throw;
                                    }

                                } // Search for case labels containing the enumerations to find the code
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ReadPremCalcDataItemData Exception \"{0}\"",ex.ToString());
                        throw;
                    }
                }

            }

            private void DisplayPremCalcDataItemData()
            {
                Console.WriteLine();
                Console.WriteLine("Count of PremCalc Data Items is {0}",_premCalcDataItemCollection.Count);
                Console.WriteLine();
                foreach (string premCalcDataItemName in _premCalcDataItemCollection.Keys)
                {
                    try
                    {
                        PremCalcDataItemInternalData premCalcDataItemInternalData = _premCalcDataItemCollection[premCalcDataItemName];
                        Console.WriteLine("    {0} = {1}", premCalcDataItemName, premCalcDataItemInternalData.PremCalcDataItemInternalId);
                        if (premCalcDataItemInternalData.PremCalcDataItemSourceCode == null)
                        {
                            Console.WriteLine("    0 : No source");
                        }
                        else
                        {
                            int lineNumber = 0;
                            try
                            {
                                foreach (string sourceLine in premCalcDataItemInternalData.PremCalcDataItemSourceCode)
                                {
                                    lineNumber += 1;
                                    Console.WriteLine("    {0} : {1}", lineNumber, sourceLine);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("DisplayPremCalcDataItemData inner exception \"{0}\"", ex.ToString());
                                throw;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("DisplayPremCalcDataItemData exception \"{0}\"", ex.ToString());
                        throw;
                    }
                }
            }

            #endregion PremCalc Data Item Management

            #endregion Private Methods

            #region Private Data Definitions

            private class RiskConstants
            {
                public const string RiskDataRelativePath = "TMS\\";
                public const string PremCalcEnginesRelativePath = "TECH\\Code\\";
                public const string HouseholdRiskName = "Household";
                public const string HouseholdRiskMnemonic = "HLD";
                public const string MotorRiskName = "Motor";
                public const string MotorRiskMnemonic = "MOT";
                public const string PossessionsRiskName = "Possessions";
                public const string PossessionsRiskMnemonic = "POS";
                public const string PremCalcDataItemSourceFileEnd = "pdati.cpp";
            }

            private readonly SortedDictionary<TmsRiskType, string> _riskEngineName =
                new SortedDictionary<TmsRiskType, string>
                {
                    {TmsRiskType.Household,RiskConstants.HouseholdRiskName},
                    {TmsRiskType.Motor,RiskConstants.MotorRiskName},
                    {TmsRiskType.Possessions,RiskConstants.PossessionsRiskName}
                };

            private readonly SortedDictionary<TmsRiskType,string> _riskEngineMnemonic =
                new SortedDictionary<TmsRiskType,string>
                {
                    {TmsRiskType.Household,RiskConstants.HouseholdRiskMnemonic},
                    {TmsRiskType.Motor,RiskConstants.MotorRiskMnemonic},
                    {TmsRiskType.Possessions,RiskConstants.PossessionsRiskMnemonic}
                };

            private readonly SortedDictionary<TmsRiskType, string> _riskEngineRelativePath =
                new SortedDictionary<TmsRiskType, string>
                {
                    { TmsRiskType.Household, RiskConstants.PremCalcEnginesRelativePath+RiskConstants.HouseholdRiskMnemonic} ,
                    { TmsRiskType.Motor, RiskConstants.PremCalcEnginesRelativePath+RiskConstants.MotorRiskMnemonic} ,
                    { TmsRiskType.Possessions, RiskConstants.PremCalcEnginesRelativePath+RiskConstants.MotorRiskMnemonic}
                };

            private readonly SortedDictionary<TmsRiskType, string> _riskDataRelativePath =
                new SortedDictionary<TmsRiskType, string>
                {
                    { TmsRiskType.Household, RiskConstants.RiskDataRelativePath+RiskConstants.HouseholdRiskName} ,
                    { TmsRiskType.Motor, RiskConstants.RiskDataRelativePath+RiskConstants.MotorRiskName} ,
                    { TmsRiskType.Possessions, RiskConstants.RiskDataRelativePath+RiskConstants.MotorRiskName}
                };

            private const string _causeHeaderFileName = "CAUSE.DAT";
            private const string _causeHelperFileName = "CAUSE.HLP";
            private const string _effectHeaderFileName = "EFFECT.DAT";
            private const string _effectHelperFileName = "EFFECT.HLP";

            private string _rootPath = null;

            private string _riskEngineFullPath = null;
            private string _riskDataFullPath = null;

            private TmsRiskType _tmsRiskType = TmsRiskType.Unknown;

            private SortedDictionary<int,CauseEffectHeader> _causeHeaderCollection = new SortedDictionary<int, CauseEffectHeader>();
            private SortedDictionary<int, CauseEffectHelper> _causeHelperCollection = new SortedDictionary<int, CauseEffectHelper>();
            private SortedDictionary<int, CauseEffectHeader> _effectHeaderCollection = new SortedDictionary<int, CauseEffectHeader>();
            private SortedDictionary<int, CauseEffectHelper> _effectHelperCollection = new SortedDictionary<int, CauseEffectHelper>();
            private SortedDictionary<string, PremCalcDataItemInternalData> _premCalcDataItemCollection = new SortedDictionary<string, PremCalcDataItemInternalData>();

            #endregion Private Data Declarations

        } // class TmsDocumentation

        private static void ShowUsage()
        {
            Console.WriteLine("*** EIS TMS Documentation Generator ***");
            Console.WriteLine("C. Cornelius and K. Mooney  08-Apr-2016");
            Console.WriteLine();
            Console.WriteLine("Usage: eisTmsDocumentationGenerator RiskName RootPath");
            Console.WriteLine();
            Console.WriteLine("Where:");
            Console.WriteLine("    RiskName = The name of the type of risk i.e. Household, Motor or Possessions");
            Console.WriteLine("    RootPath = Path to the top of the source directory tree");
        }



        static void Main(string[] args)
        {
            if (args.Count() < 2)
            {
                ShowUsage();
            }
            else
            {
                string tmsRiskName = args[0];
                string rootPath = args[1];

                try
                {
                    TmsDocumentation tmsDocumentation = new TmsDocumentation(tmsRiskName, rootPath);
                    tmsDocumentation.ReadData();
                    tmsDocumentation.Display();
                }
                catch (Exception)
                {
                    Console.WriteLine("Documentation for \"{0}\" failed to be generated",tmsRiskName);
                }
            }
        }
    }
}
