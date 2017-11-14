using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Xml;
using System.IO;

namespace TestEarnixAttributes
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

        private enum ReadingState
        {
            None,
            Names,
            Values
        };

        private void Read()
        {
            List<StringPair> earnixAttributesList = new List<StringPair>();

            try
            {
                System.Xml.XmlTextReader reader = new System.Xml.XmlTextReader(_filename);

                reader.WhitespaceHandling = WhitespaceHandling.None;

                ReadingState readingState = ReadingState.None;
                bool reading = false;
                string elementName = null;

                int pairIndex = 0;

                int nodeIndex = -1;

                while (reader.Read())
                {
                    // reader.MoveToContent();

                    nodeIndex += 1;

                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (_debug)
                            {
                                Debug.WriteLine("Element = \"" + reader.Name + "\"");
                                // Debug.WriteLine("    " + reader.ReadElementString() ;
                            }
                            elementName = reader.Name;
                            if (String.Compare(reader.Name, "attributeNames") == 0)
                            {
                                if (_debug)
                                {
                                    Debug.WriteLine("Reading Earnix attribute names");
                                }
                                readingState = ReadingState.Names;
                                pairIndex = 0;
                            }
                            else if (String.Compare(reader.Name, "ProfileValues") == 0)
                            {
                                if (_debug)
                                {
                                    Debug.WriteLine("Reading Request Earnix attribute values");
                                }
                                readingState = ReadingState.Values;
                                pairIndex = 0;
                            }
                            else if (String.Compare(reader.Name, "PricingResultsV6") == 0)
                            {
                                if (_debug)
                                {
                                    Debug.WriteLine("Reading Response Earnix attribute values");
                                }
                                readingState = ReadingState.Values;
                                pairIndex = 0;
                            }
                            else if (String.Compare(reader.Name, "string") == 0)
                            {
                                if (reading)
                                {
                                    Debug.WriteLine("Inconsistent state. Already reading.");
                                }
                                else if (readingState != ReadingState.None)
                                {
                                    reading = true;
                                }
                            }
                            break;
                        case XmlNodeType.Text:
                            // Reading a Response
                            if (_debug)
                            {
                                Debug.WriteLine("Text = \"" + reader.Value + "\"");
                            }
                            switch (readingState)
                            {
                                case ReadingState.None:
                                    if (elementName != null)
                                    {
                                        Debug.WriteLine("Unexpected read of Value \"{0}\"", reader.Value);
                                        StringPair stringPair = new StringPair();
                                        stringPair.First = elementName;
                                        stringPair.Second = reader.Value;
                                        earnixAttributesList.Add(stringPair);
                                        pairIndex += 1;
                                    }
                                    break;
                                case ReadingState.Names:
                                    if (reading)
                                    {
                                        StringPair stringPair = new StringPair();
                                        stringPair.First = reader.Value;
                                        earnixAttributesList.Add(stringPair);
                                        pairIndex += 1;
                                    }
                                    else
                                    {
                                        Debug.WriteLine("Unexpected read of Name \"{0}\"",reader.Value);
                                        StringPair stringPair = new StringPair();
                                        stringPair.First = elementName;
                                        stringPair.Second = reader.Value;
                                        earnixAttributesList.Add(stringPair);
                                        pairIndex += 1;
                                    }
                                    break;
                                case ReadingState.Values:
                                {
                                    if (reading)
                                    {
                                        if (pairIndex >= earnixAttributesList.Count)
                                        {
                                            Debug.WriteLine("Attribute Count {0} out bounds of {1} List Members",
                                                pairIndex,
                                                earnixAttributesList.Count);
                                        }
                                        else
                                        {
                                            earnixAttributesList[pairIndex].Second = reader.Value;
                                            pairIndex += 1;
                                        }
                                    }
                                    else
                                    {
                                        Debug.WriteLine("Unexpected read of Value \"{0}\"", reader.Value);
                                        StringPair stringPair = new StringPair();
                                        stringPair.First = elementName;
                                        stringPair.Second = reader.Value;
                                        earnixAttributesList.Add(stringPair);
                                        pairIndex += 1;
                                    }

                                }
                                break;
                            }
                            break;
                        case XmlNodeType.CDATA:
                            if (_debug)
                            {
                                Debug.WriteLine("CDATA = \"" + reader.Value + "\"");
                            }
                            // Reading a Request
                            switch (readingState)
                            {
                                case ReadingState.None:
                                    if (elementName != null)
                                    {
                                        Debug.WriteLine("Unexpected read of Value \"{0}\"", reader.Value);
                                        StringPair stringPair = new StringPair();
                                        stringPair.First = elementName;
                                        stringPair.Second = reader.Value;
                                        earnixAttributesList.Add(stringPair);
                                        pairIndex += 1;
                                    }
                                    break;
                                case ReadingState.Names:
                                    if (reading)
                                    {
                                        StringPair stringPair = new StringPair();
                                        stringPair.First = reader.Value;
                                        earnixAttributesList.Add(stringPair);
                                        pairIndex += 1;
                                    }
                                    else
                                    {
                                        Debug.WriteLine("Unexpected read of Name \"{0}\"",reader.Value);
                                        StringPair stringPair = new StringPair();
                                        stringPair.First = elementName;
                                        stringPair.Second = reader.Value;
                                        earnixAttributesList.Add(stringPair);
                                        pairIndex += 1;
                                    }
                                    break;
                                case ReadingState.Values:
                                    if (reading)
                                    {
                                        if (pairIndex >= earnixAttributesList.Count)
                                        {
                                            Debug.WriteLine("Attribute Count {0} out bounds of {1} List Members",
                                                pairIndex,
                                                earnixAttributesList.Count);
                                        }
                                        else
                                        {
                                            earnixAttributesList[pairIndex].Second = reader.Value;
                                            pairIndex += 1;
                                        }
                                    }
                                    else
                                    {
                                        Debug.WriteLine("Unexpected read of Value \"{0}\"",reader.Value);
                                        StringPair stringPair = new StringPair();
                                        stringPair.First = elementName;
                                        stringPair.Second = reader.Value;
                                        earnixAttributesList.Add(stringPair);
                                        pairIndex += 1;
                                    }
                                    break;
                            }
                            break;
                        case XmlNodeType.ProcessingInstruction:
                            if (_debug)
                            {
                                Debug.WriteLine("Processing = \"" + reader.Name + "\"=\"" + reader.Value + "\"");
                            }
                            break;
                        case XmlNodeType.Comment:
                            if (_debug)
                            {
                                Debug.WriteLine("Comment = \"" + reader.Value + "\"");
                            }
                            break;
                        case XmlNodeType.XmlDeclaration:
                            if (_debug)
                            {
                                Debug.WriteLine("Declaration \"<?xml version='1.0'?>\"");
                            }
                            break;
                        case XmlNodeType.Document:
                        case XmlNodeType.DocumentType:
                            if (_debug)
                            {
                                Debug.WriteLine("DOC Type =\"" + reader.Name + "\" Value = \"" + reader.Value +
                                                  "\"");
                            }
                            break;
                        case XmlNodeType.EntityReference:
                            if (_debug)
                            {
                                Debug.WriteLine("Entity Reference = \"" + reader.Name + "\"");
                            }
                            break;
                        case XmlNodeType.EndElement:
                            if (_debug)
                            {
                                Debug.WriteLine("End Element \"" + reader.Name + "\"");
                            }
                            reading = false;
                            elementName = null;
                            break;
                        default:
                            Debug.WriteLine("Unknown Node Type " + reader.NodeType.ToString());
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

        private const bool _debug = true;

        private string _filename = null;

        private string _errorText = null;
        private SortedDictionary<string, string> _earnixAttributesDictionary = new SortedDictionary<string, string>();

    } // EarnixAttributeReader
}
