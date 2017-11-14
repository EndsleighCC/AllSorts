using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TestSimpleXMLReader
{
    class Program
    {
        public class SimpleNodeReader
        {
            public SimpleNodeReader(string filename)
            {
                _filename = filename;
                Read();
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

                    if (_debug)
                    {
                        Console.WriteLine("{0}", _filename);
                    }

                    reader.WhitespaceHandling = WhitespaceHandling.None;

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
                                    Console.WriteLine("Element = \"" + reader.Name + "\"");
                                    // Console.WriteLine("    " + reader.ReadElementString() ;
                                }
                                break;
                            case XmlNodeType.Text:
                                if (_debug)
                                {
                                    Console.WriteLine("Text = \"" + reader.Value + "\"");
                                }
                                break;
                            case XmlNodeType.CDATA:
                                // Reading a Request
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

            } // Read

            private bool _debug = true;

            private string _filename = null;

            private string _errorText = null;

        } // SimpleNodeReader

        static void Main(string[] args)
        {
            if (args.Count() == 0)
            {
                Console.WriteLine();
                SimpleNodeReader simpleNodeReaderRequest =
                    new SimpleNodeReader(@"I:\UT00\Logs\earnix\2015-04-17-13.09.01.344000-Request.xml");
                Console.WriteLine();
                SimpleNodeReader simpleNodeReaderResponse =
                    new SimpleNodeReader(@"I:\UT00\Logs\earnix\2015-04-17-13.09.01.344000-Response.xml");
            }
            else
            {
                SimpleNodeReader simpleNodeReader = new SimpleNodeReader(args[0]);
            }
        }
    }
}
