using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlLoadFromNode
{
    class Program
    {
        const int _indentLength = 4;
        const string _textQualifier = "#text";

        static string Indent(int depth)
        {
            return new string(' ', depth * _indentLength);
        }

        static void ShowAttributes(XmlNode xmlNode, int depth)
        {
            if (xmlNode.Attributes != null)
            {
                foreach (XmlAttribute xmlAttribute in xmlNode.Attributes)
                {
                    Console.WriteLine("{0}Attribute \"{1}\" = \"{2}\"", Indent(depth + 2), xmlAttribute.Name, (xmlAttribute.Value == null ? "(unset)" : xmlAttribute.Value));
                }
            }

        } // ShowAttributes

        static void ShowNode(XmlNode xmlNodeBase, int depth)
        {
            Console.WriteLine("{0}<{1}>", Indent(depth), xmlNodeBase.Name);
            if (xmlNodeBase.Value != null)
            {
                // Console.WriteLine("{0}\"{1}\" = \"{2}\"", Indent(depth), xmlNodeBase.Name, (xmlNodeBase.Value == null ? "(unset)" : xmlNodeBase.Value));
                Console.WriteLine("{0}\"{1}\"", Indent(depth + 1), (xmlNodeBase.Value == null ? "(unset)" : xmlNodeBase.Value));
            }
            else
            {
                string value = null;
                if (xmlNodeBase.HasChildNodes)
                {
                    // Find the value of this node in amongst the children
                    for (int nodeIndex = 0; (value == null) && (nodeIndex < xmlNodeBase.ChildNodes.Count); ++nodeIndex)
                    {
                        if (xmlNodeBase.ChildNodes[nodeIndex].Name == _textQualifier)
                        {
                            value = xmlNodeBase.ChildNodes[nodeIndex].Value;
                        }
                    }
                }
                // Console.WriteLine("{0}\"{1}\" = \"{2}\"", Indent(depth), xmlNodeBase.Name, (value == null ? "(unset)" : value));
                Console.WriteLine("{0}\"{1}\"", Indent(depth + 1), (value == null ? "(unset)" : value));
            }
            ShowAttributes(xmlNodeBase, depth);
            if (xmlNodeBase.HasChildNodes)
            {
                foreach (XmlNode xmlNode in xmlNodeBase.ChildNodes)
                {
                    if (xmlNode.Name != _textQualifier)
                    {
                        ShowNode(xmlNode, depth + 1);
                    }
                }
            }
            Console.WriteLine("{0}</{1}>", Indent(depth), xmlNodeBase.Name);
        }

        static void DisplayXmlDocument( XmlDocument xmlDocument)
        {
            XmlNode root = xmlDocument.DocumentElement;
            ShowNode(root, 0);
        }

        private class NamespaceNotPresent : ApplicationException
        {
            public NamespaceNotPresent(string name) : base(name)
            {
            }
        }

        private class InsufficientQuoteCount : ApplicationException
        {
            public InsufficientQuoteCount(string name) : base(name)
            {
            }
        }

        private class TooManyQuotes : ApplicationException
        {
            public TooManyQuotes(string name) : base(name)
            {
            }
        }

        static void WriteXmlQuoteDocument( string xmlFullDocumentFilename, XmlDocument xmlQuoteDocument, string namespaceName , ref int quoteCount )
        {
            try
            {
                string quoteReference = "None";
                NameTable nameTable = new NameTable();
                XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(nameTable);
                string namespacePrefix = "SingleQuote";
                string xPath = "//" + namespacePrefix + ":" +_quoteRefText;
                xmlNamespaceManager.AddNamespace(namespacePrefix, namespaceName);
                XmlNodeList xmlQuoteRefNodeList = xmlQuoteDocument.DocumentElement.SelectNodes(xPath, xmlNamespaceManager);
                if (xmlQuoteRefNodeList.Count == 0)
                {
                    Console.WriteLine("Document \"{0}\" contains no quotes for XML Namespace \"{1}\"",
                                        xmlFullDocumentFilename, namespaceName);
                    throw new InsufficientQuoteCount(xmlFullDocumentFilename);
                }
                else if (xmlQuoteRefNodeList.Count > 1)
                {
                    Console.WriteLine("The supplied sub-document of \"{0}\" for XML Namespace \"{1}\" contains more than one quote",
                                        xmlFullDocumentFilename, namespaceName);
                    throw new TooManyQuotes(xmlFullDocumentFilename);
                }
                else
                {
                    // Exactly one quote to output

                    quoteCount += 1;

                    int nodeCount = 0;
                    foreach (XmlNode xmlNodeInner in xmlQuoteRefNodeList)
                    {
                        nodeCount += 1;
                        //Console.WriteLine("WriteXmlQuoteDocument({0}) : Quote Reference \"{1}\"",
                        //            xmlFullDocumentFilename,
                        //            xmlNodeInner.InnerText);
                        quoteReference = xmlNodeInner.InnerText;
                    }

                    // Work out the filename based on the the quote reference
                    int lastDotPos = xmlFullDocumentFilename.LastIndexOf(".");
                    string quoteDocumentFilename = null;
                    if (lastDotPos < 0)
                    {
                        // No last dot
                        quoteDocumentFilename = xmlFullDocumentFilename + "." + quoteReference + ".xml";
                    }
                    else
                    {
                        // Last dot found
                        quoteDocumentFilename = xmlFullDocumentFilename.Substring(0, lastDotPos) + "." + quoteReference + ".xml";
                    }

                    try
                    {
                        Console.WriteLine("WriteXmlQuoteDocument({0}) : Writing \"{1}\"", quoteCount, quoteDocumentFilename);
                        XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                        xmlWriterSettings.Indent = true;
                        using (XmlWriter xmlWriter = XmlWriter.Create(quoteDocumentFilename, xmlWriterSettings))
                        {
                            //Console.WriteLine("    Check characters is {0}", xmlWriter.Settings.CheckCharacters);
                            //Console.WriteLine("    Close output is {0}", xmlWriter.Settings.CloseOutput);
                            //Console.WriteLine("    Conformance level is {0}", xmlWriter.Settings.ConformanceLevel);
                            //Console.WriteLine("    Encoding is {0}", xmlWriter.Settings.Encoding);
                            //Console.WriteLine("    Indent is {0}", xmlWriter.Settings.Indent);
                            //Console.WriteLine("    IndentChars is \"{0}\"", xmlWriter.Settings.IndentChars);
                            //Console.WriteLine("    Namespace handling is {0}", xmlWriter.Settings.NamespaceHandling);
                            //Console.WriteLine("    Newline chars is \"{0}\"", xmlWriter.Settings.NewLineChars);
                            //Console.WriteLine("    Newline handling is {0}", xmlWriter.Settings.NewLineHandling);
                            //Console.WriteLine("    Newline On Attributes is {0}", xmlWriter.Settings.NewLineOnAttributes);
                            //Console.WriteLine("    Omit XML Declaration is {0}", xmlWriter.Settings.OmitXmlDeclaration);
                            //Console.WriteLine("    Output method is {0}", xmlWriter.Settings.OutputMethod);
                            //Console.WriteLine("    Write End Document On Close is {0}", xmlWriter.Settings.WriteEndDocumentOnClose);
                            xmlQuoteDocument.WriteContentTo(xmlWriter);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Write exception {0}", ex.ToString());
                    }

                    //Console.WriteLine("WriteXmlQuoteDocument {0} Begin", quoteReference);
                    //DisplayXmlDocument(xmlQuoteDocument);
                    //Console.WriteLine("WriteXmlQuoteDocument {0} End", quoteReference);

                } // Exactly one quote to output
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception writing {0}", ex.ToString());
            }

        } // WriteXmlQuoteDocument

        //private static string NamespaceOf(XmlDocument xmlDocument)
        //{
        //    string namespaceName = null ;

        //    NameTable nameTable = new NameTable();
        //    XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(nameTable);
        //    string namespacePrefix = "eisex";
        //    string xPath = "//" + namespacePrefix + ":EISExtracts";
        //    xmlNamespaceManager.AddNamespace(namespacePrefix, "http://ssp.web.services/MotorAggregator/schemas/PMQuoteData.xsd");
        //    XmlNode xmlNode = xmlDocument.DocumentElement.SelectSingleNode(xPath, xmlNamespaceManager);
        //    if ( ( xmlNode != null ) && (xmlNode.Attributes != null ) )
        //    {
        //        for ( int attributeIndex = 0;
        //                 ( namespaceName == null )
        //              && ( attributeIndex < xmlNode.Attributes.Count ) ;
        //              ++attributeIndex
        //            )
        //        {
        //            XmlAttribute xmlAttribute = xmlNode.Attributes[attributeIndex];
        //            if ( String.Compare( xmlAttribute.Name , "xmlns" ) == 0 )
        //            {
        //                namespaceName = xmlAttribute.Value;
        //            }
        //        }

        //    }
        //    return namespaceName;

        //} // NamespaceOf

        private static void ProcessXmlStream(string xmlFilename, Stream xmlFileStream)
        {
            Console.WriteLine();
            Console.WriteLine("Using XML Document");
            Int64 initialStreamPosition = xmlFileStream.Position;
            string namespaceName = null;
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(xmlFileStream);
                namespaceName = NamespaceOf(xmlDocument);
                if ( namespaceName == null )
                {
                    Console.WriteLine("ProcessXmlStream({1}) : Unable to determine the name of the default namespace",
                                        xmlFilename);
                    throw new NamespaceNotPresent(xmlFilename);
                }
                NameTable nameTable = new NameTable();
                XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(nameTable);
                string namespacePrefix = "SingleQuote";
                string xPath = "//" + namespacePrefix + ":" + _quoteRefText;
                xmlNamespaceManager.AddNamespace(namespacePrefix, namespaceName);
                XmlNodeList xmlQuoteRefNodeList = xmlDocument.DocumentElement.SelectNodes(xPath, xmlNamespaceManager);
                if ( xmlQuoteRefNodeList.Count == 0 )
                {
                    Console.WriteLine("ProcessXmlStream : File \"{0}\" contains no quotes");
                }
                else
                {
                    Console.WriteLine("ProcessXmlStream : File \"{0}\" contains {1} quotes",
                        xmlFilename, xmlQuoteRefNodeList.Count);
                    int nodeCount = 0;
                    foreach ( XmlNode xmlNodeInner in xmlQuoteRefNodeList)
                    {
                        nodeCount += 1;
                        Console.WriteLine("    {0} : Quote Reference \"{1}\" \"{2}\"",
                                    nodeCount ,
                                    xmlNodeInner.Name,
                                    xmlNodeInner.InnerText);
                    }
                }
                //Console.WriteLine("*** Full document begin");
                //DisplayXmlDocument(xmlDocument);
                //Console.WriteLine("*** Full document end");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception loading XML document \"{0}\" = {1}",
                                    xmlFilename, ex.ToString());
            }

            Console.WriteLine();
            Console.WriteLine("Using XML Reader");
            Console.WriteLine();

            // Move to the original location in the stream
            xmlFileStream.Position = initialStreamPosition;

            XmlReaderSettings xmlSettings = new XmlReaderSettings();
            xmlSettings.ConformanceLevel = ConformanceLevel.Fragment;
            xmlSettings.DtdProcessing = DtdProcessing.Parse;
            xmlSettings.IgnoreWhitespace = true;
            xmlSettings.IgnoreComments = false;
            NameTable nameTableReader = new NameTable();
            nameTableReader.Add("http://ssp.web.services/MotorAggregator/schemas/PMQuoteData.xsd");
            xmlSettings.NameTable = nameTableReader;

            int quoteCount = 0;

            using (XmlReader xmlReader = XmlReader.Create(xmlFileStream, xmlSettings))
            {

                xmlReader.MoveToContent();
                while (xmlReader.Read())
                {
                    // Console.WriteLine("ReadState is {0}", xmlReader.ReadState);
                    // Parse the file and display each of the nodes.
                    switch (xmlReader.NodeType)
                    {
                        case XmlNodeType.Element:
                            Console.WriteLine("<{0}>", xmlReader.Name);
                            // Console.WriteLine("Reader state is {0}", xmlReader.ReadState);
                            // Look for the start of a quote
                            string subTreeName = "BrokerDataExtract";
                            if ( String.Compare( xmlReader.Name, subTreeName) == 0 )
                            {
                                // Console.WriteLine("*** Sub-tree \"{0}\" begin", xmlReader.Name);
                                // Construct an XML Document from the sub-tree on which the file is positioned
                                XmlDocument xmlQuoteDocument = new XmlDocument();
                                try
                                {
                                    // xmlQuoteDocument.Load(xmlFileStream);
                                    xmlQuoteDocument.Load(xmlReader);
                                }
                                catch (InvalidOperationException ex)
                                {
                                    // Processing a fragment causes this exception and appears to be normal
                                    // Console.WriteLine("*** Sub-tree \"{0}\" end", subTreeName);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Exception loading XML document \"{0}\". ReadState {1} = {2}",
                                                        xmlFilename ,
                                                        xmlReader.ReadState ,
                                                        ex.ToString());
                                }
                                // Console.WriteLine();
                                //Console.WriteLine("+++ Document content for \"{0}\" ReadState {1}",
                                //                    subTreeName, xmlReader.ReadState);
                                // DisplayXmlDocument(xmlQuoteDocument);
                                WriteXmlQuoteDocument(xmlFilename, xmlQuoteDocument,namespaceName,ref quoteCount);
                                // Console.WriteLine("---");
                            }
                            break;
                        case XmlNodeType.Text:
                            Console.WriteLine(xmlReader.Value);
                            break;
                        case XmlNodeType.CDATA:
                            Console.WriteLine("<![CDATA[{0}]]>", xmlReader.Value);
                            break;
                        case XmlNodeType.ProcessingInstruction:
                            Console.WriteLine("<?{0} {1}?>", xmlReader.Name, xmlReader.Value);
                            break;
                        case XmlNodeType.Comment:
                            Console.WriteLine("<!--{0}-->", xmlReader.Value);
                            break;
                        case XmlNodeType.XmlDeclaration:
                            Console.WriteLine("<?xml version='1.0'?>");
                            break;
                        case XmlNodeType.Document:
                            break;
                        case XmlNodeType.DocumentType:
                            Console.WriteLine("<!DOCTYPE {0} [{1}]", xmlReader.Name, xmlReader.Value);
                            break;
                        case XmlNodeType.EntityReference:
                            Console.WriteLine(xmlReader.Name);
                            break;
                        case XmlNodeType.EndElement:
                            Console.WriteLine("</{0}>", xmlReader.Name);
                            break;
                        default:
                            Console.WriteLine("Unexpected Node Type \"{0}\"", xmlReader.NodeType);
                            break;
                    } // switch (xmlReader.NodeType)

                } // while reading

            } // using

        } // ProcessXmlStream

        static void Main(string[] args)
        {
            for (int argIndex = 0; argIndex < args.Length; ++argIndex)
            {
                string xmlFilename = Path.GetFullPath(args[argIndex]);

                if (!File.Exists(xmlFilename))
                {
                    Console.WriteLine("XML document \"{0}\" does not exist", xmlFilename);
                }
                else
                {
                    try
                    {
                        using (StreamReader fileStream = new StreamReader(xmlFilename))
                        {
                            ProcessXmlStream(xmlFilename, fileStream.BaseStream);
                        }
                    }
                    catch (NamespaceNotPresent ex)
                    {
                        Console.WriteLine("Exception determining document XML Namespace \"{0}\"", ex.Message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error reading XML file \"{0}\" = {1}", xmlFilename, ex.ToString());
                    }
                }
            }
        }
    }
}
