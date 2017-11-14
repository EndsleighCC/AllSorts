using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace TestXMLReader
{
    class Program
    {

        public class EarnixAttributeReader
        {
            public EarnixAttributeReader(string filename)
            {
                _filename = filename;
                Read();
            }

            public SortedDictionary<string, string> EarnixAttributesDictionary
            {
                get { return _earnixAttributesDictionary; }
            }

            public string ErrorText
            {
                get { return _errorText; }
            }
            public bool Error
            {
                get { return _errorText != null; }
            }

            private class StringPair
            {
                public StringPair()
                {
                }

                public StringPair(string first, string second)
                {
                    _first = first;
                    _second = second;
                }

                public string First
                {
                    get { return _first; }
                    set { _first = value; }
                }

                public string Second
                {
                    get { return _second; }
                    set { _second = value; }
                }

                private string _first = null;
                private string _second = null;

            }

            private void Read()
            {
                List<StringPair> earnixAttributesList = new List<StringPair>();

                try
                {
                    System.Xml.XmlTextReader reader = new System.Xml.XmlTextReader(_filename);

                    reader.WhitespaceHandling = WhitespaceHandling.None;

                    bool readingValues = false;

                    int pairIndex = 0;

                    while (reader.Read())
                    {
                        // reader.MoveToContent();

                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                if (_debug)
                                {
                                    Console.WriteLine("Element = \"" + reader.Name + "\"");
                                    // Console.WriteLine("    " + reader.ReadElementString() ;
                                }
                                if (String.Compare(reader.Name, "attributeNames") == 0)
                                {
                                    if (_debug)
                                    {
                                        Console.WriteLine("Reading Earnix attribute names");
                                    }
                                    readingValues = false;
                                    pairIndex = 0;
                                }
                                else if (String.Compare(reader.Name, "ProfileValues") == 0)
                                {
                                    if (_debug)
                                    {
                                        Console.WriteLine("Reading Earnix attribute values");
                                    }
                                    readingValues = true;
                                    pairIndex = 0;
                                }
                                else if (String.Compare(reader.Name, "values") == 0)
                                {
                                    if (_debug)
                                    {
                                        Console.WriteLine("Reading Earnix attribute values");
                                    }
                                    readingValues = true;
                                    pairIndex = 0;
                                }
                                break;
                            case XmlNodeType.Text:
                                // Reading a Response
                                if (!readingValues)
                                {
                                    StringPair stringPair = new StringPair();
                                    stringPair.First = reader.Value;
                                    earnixAttributesList.Add(stringPair);
                                    pairIndex += 1;
                                }
                                else
                                {
                                    if (pairIndex >= earnixAttributesList.Count)
                                    {
                                        Console.WriteLine("Attribute Count {0} out bounds of {1} List Members", pairIndex,
                                            earnixAttributesList.Count);
                                    }
                                    else
                                    {
                                        earnixAttributesList[pairIndex].Second = reader.Value;
                                        pairIndex += 1;
                                    }
                                }
                                if (_debug)
                                {
                                    Console.WriteLine("Text = \"" + reader.Value + "\"");
                                }
                                break;
                            case XmlNodeType.CDATA:
                                // Reading a Request
                                if (!readingValues)
                                {
                                    StringPair stringPair = new StringPair();
                                    stringPair.First = reader.Value;
                                    earnixAttributesList.Add(stringPair);
                                    pairIndex += 1;
                                }
                                else
                                {
                                    if (pairIndex >= earnixAttributesList.Count)
                                    {
                                        Console.WriteLine("Attribute Count {0} out bounds of {1} List Members", pairIndex,
                                            earnixAttributesList.Count);
                                    }
                                    else
                                    {
                                        earnixAttributesList[pairIndex].Second = reader.Value;
                                        pairIndex += 1;
                                    }
                                }
                                if (_debug)
                                {
                                    Console.WriteLine("CDATA = \"" + reader.Value + "\"");
                                }
                                break;
                            case XmlNodeType.ProcessingInstruction:
                                if (_debug)
                                {
                                    Console.WriteLine("Processing = \"" + reader.Name + "\"=\"" + reader.Value + "\"");
                                }
                                break;
                            case XmlNodeType.Comment:
                                if (_debug)
                                {
                                    Console.WriteLine("Comment = \"" + reader.Value + "\"");
                                }
                                break;
                            case XmlNodeType.XmlDeclaration:
                                if (_debug)
                                {
                                    Console.WriteLine("Declaration \"<?xml version='1.0'?>\"");
                                }
                                break;
                            case XmlNodeType.Document:
                            case XmlNodeType.DocumentType:
                                if (_debug)
                                {
                                    Console.WriteLine("DOC Type =\"" + reader.Name + "\" Value = \"" + reader.Value +
                                                      "\"");
                                }
                                break;
                            case XmlNodeType.EntityReference:
                                if (_debug)
                                {
                                    Console.WriteLine("Entity Reference = \"" + reader.Name + "\"");
                                }
                                break;
                            case XmlNodeType.EndElement:
                                if (_debug)
                                {
                                    Console.WriteLine("End Element \"" + reader.Name + "\"");
                                }
                                break;
                            default:
                                Console.WriteLine("Unknown Node Type " + reader.NodeType.ToString());
                                break;
                        } // switch ( reader.NodeType )

                    } // while

                } // try
                catch (System.ArgumentException)
                {
                    _errorText = String.Format("Invalid filename \"{0}\"", _filename);
                }
                catch (System.IO.FileNotFoundException)
                {
                    _errorText = String.Format("File not found \"{0}\"", _filename);
                }
                catch (System.IO.IOException eek)
                {
                    _errorText = String.Format("File \"{0}\" : System.IO.IOException \"{1}\"", _filename, eek.ToString());
                }

                _earnixAttributesDictionary.Clear();

                foreach (StringPair stringPair in earnixAttributesList)
                {
                    _earnixAttributesDictionary.Add(stringPair.First, stringPair.Second);
                }

            } // Read

            private bool _debug = true;
            private string _filename = null;

            private string _errorText = null;
            private SortedDictionary<string, string> _earnixAttributesDictionary = new SortedDictionary<string, string>();

        } // EarnixAttributeReader

        static void Main(string[] args)
        {
            if (args.Count() > 0)
            {
                string filename = args[0];
                EarnixAttributeReader earnixAttributeReader = new EarnixAttributeReader(filename);

                SortedDictionary<string, string> earnixAttributesDictionary =
                    earnixAttributeReader.EarnixAttributesDictionary;

                if (earnixAttributeReader.Error)
                {
                    Console.WriteLine("**** Error = \"{0}\"",earnixAttributeReader.ErrorText);
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("\"{0}\" contained {1}", filename, earnixAttributesDictionary.Count);
                    Console.WriteLine();
                    foreach (KeyValuePair<string, string> keyValuePair in earnixAttributesDictionary)
                    {
                        Console.WriteLine("\"{0}\" = \"{1}\"", keyValuePair.Key, keyValuePair.Value);
                    }
                }

            }

        }
    }
}
