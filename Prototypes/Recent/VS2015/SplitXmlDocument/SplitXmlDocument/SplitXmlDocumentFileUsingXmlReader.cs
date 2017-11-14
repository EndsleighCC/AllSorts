using System;
using System.IO;
using System.Xml;
using System.Diagnostics;

namespace SplitXmlDocument
{
    partial class Program
    {

        private static XmlDocument CreateNewXmlDocument()
        {
            XmlDocument xmlDocumentMaster = new XmlDocument();

            xmlDocumentMaster.PreserveWhitespace = false;

            // The XML declaration is recommended but not mandatory
            XmlDeclaration xmlDeclaration = xmlDocumentMaster.CreateXmlDeclaration(_xmlVersion, _documentEncoding, null);
            XmlElement rootElement = xmlDocumentMaster.CreateElement(_rootNodeName, _defaultNamespace);
            xmlDocumentMaster.AppendChild(rootElement);
            XmlElement root = xmlDocumentMaster.DocumentElement;
            xmlDocumentMaster.InsertBefore(xmlDeclaration, root);

            return xmlDocumentMaster;
        }
        static void SplitXmlDocumentFileUsingXmlReader(string xmlFilename, int batchMemberMaxCount)
        {
            using (StreamReader fileStream = new StreamReader(xmlFilename))
            {
                XmlReaderSettings xmlSettings = new XmlReaderSettings();
                xmlSettings.ConformanceLevel = ConformanceLevel.Fragment;
                xmlSettings.DtdProcessing = DtdProcessing.Parse;
                xmlSettings.IgnoreWhitespace = true;
                xmlSettings.IgnoreComments = false;

                using (XmlReader xmlReader = XmlReader.Create(fileStream, xmlSettings))
                {
                    int quoteCount = 0;
                    int batchMemberNumber = 0;
                    int batchNumber = 1;
                    int indentingLevel = 0;

                    bool errorOccurred = false;
                    int quoteMaxCountTimer = 1000;

                    if (quoteMaxCountTimer > batchMemberMaxCount)
                    {
                        // Ensure there is some progress output
                        quoteMaxCountTimer = batchMemberMaxCount / 2;
                        if ( quoteMaxCountTimer < 1 )
                        {
                            quoteMaxCountTimer = 1;
                        }
                    }

                    Stopwatch totalTimeStopwatch = new Stopwatch();
                    totalTimeStopwatch.Start();

                    // Create the document when the XML Declaration is encountered in the supplied document
                    XmlDocument xmlDocumentMaster = null;
                    // Determine this from the supplied document
                    string namespaceName = null;

                    // xmlReader.MoveToContent();
                    while ((!errorOccurred) && (xmlReader.Read()))
                    {
                        switch (xmlReader.NodeType)
                        {
                            case XmlNodeType.Element:
                                if (_debugOutputTopLevelXML)
                                {
                                    Console.WriteLine("<{0}>", xmlReader.Name);
                                }
                                // Look for the start of a quote
                                if (String.Compare(xmlReader.Name, _quoteSubTreeName) == 0)
                                {
                                    if ( batchMemberNumber == 0 )
                                    {
                                        Console.WriteLine();
                                        Console.WriteLine("{0} : Batch {1} begin",
                                            DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss.fff"), batchNumber);
                                        Console.WriteLine();
                                    }
                                    indentingLevel += 1;
                                    quoteCount += 1;
                                    batchMemberNumber += 1;
                                    string quoteXml = null;
                                    try
                                    {
                                        // GenerateQuoteXML(xmlReader, quoteCount, ref indentingLevel);
                                        quoteXml = GenerateQuoteXMLToString(xmlReader, quoteCount, ref indentingLevel);
                                        // End Element of quote sub-tree name will have been encountered
                                        if (_debugOutputTopLevelXML)
                                        {
                                            Console.WriteLine("</{0}>", xmlReader.Name);
                                        }
                                        // Console.WriteLine(quoteXml);
                                        //using (StreamWriter sw = new StreamWriter("quoteXML.xml"))
                                        //{
                                        //    sw.WriteLine(quoteXml);
                                        //}
                                        
                                        // Generate a new XmlDocument with just this quote
                                        XmlDocument xmlDocument = new XmlDocument();
                                        xmlDocument.LoadXml(quoteXml);
                                        namespaceName = NamespaceOf(xmlDocument);

                                        if (_generateDriverAndVehicleNumberXmlAttributes)
                                        {
                                            AddDriverNumberXmlAttribute(xmlFilename, xmlDocument, namespaceName);
                                            AddVehicleNumberXmlAttribute(xmlFilename, xmlDocument, namespaceName);
                                        }
                                        
                                        //string quoteRef = GetQuoteRef(xmlFilename, xmlDocument, namespaceName);
                                        //Console.WriteLine("Individual Document QuoteRef = \"{0}\"", quoteRef ?? "unknown" );
                                        //ShowQuoteNodeWithChildren(xmlFilename, xmlDocument, namespaceName, "//Drivers/Driver/Claims");
                                        //Console.WriteLine();
                                        //Console.WriteLine("Master Document QuoteRefs prior to merge");
                                        //ShowQuoteRefs(xmlFilename, xmlDocumentMaster, namespaceName);

                                        // If the master document has not been created, create it here.
                                        // Creation needs to occur late so that the namespace has been determined
                                        if (xmlDocumentMaster == null)
                                        {
                                            xmlDocumentMaster = CreateNewXmlDocument();
                                        }

                                        // Merge the quote that was just read from the input into the "Master"
                                        // XML document that will contain all the quotes for this batch
                                        MergeXmlDocuments(xmlDocumentMaster, xmlDocument);
                                        //Console.WriteLine();
                                        //Console.WriteLine("Master Document QuoteRefs after merge");
                                        //ShowQuoteRefs(xmlFilename, xmlDocumentMaster, namespaceName);
                                        if (quoteCount % quoteMaxCountTimer == 0 )
                                        {
                                            // Display progress
                                            double averageQuoteTimeMilliseconds = (double)totalTimeStopwatch.ElapsedMilliseconds / quoteCount;
                                            Console.WriteLine("    {0:#,##0} quotes. Average quote copy time is {1} seconds. Total time is {2} seconds",
                                                                quoteCount,
                                                                (averageQuoteTimeMilliseconds / 1000.0).ToString("0.000000"),
                                                                (((double)totalTimeStopwatch.ElapsedMilliseconds)/1000.0).ToString("0.000"));
                                        }
                                        if ( batchMemberNumber >= batchMemberMaxCount )
                                        {
                                            // Show all QuoteRef elements from the root of the document
                                            // ShowQuoteRefs(xmlFilename, xmlDocumentMaster, namespaceName, batchNumber);
                                            // ShowQuoteNodeWithChildren(xmlFilename, xmlDocumentMaster, namespaceName, "//Drivers/Driver/Claims");
                                            // throw new Exception("Testing");
                                            Console.WriteLine();
                                            Console.WriteLine("{0} : Batch {1} end",
                                                DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss.fff"), batchNumber);
                                            WriteXmlQuoteBatchDocument(xmlFilename, xmlDocumentMaster, namespaceName, batchNumber);
                                            // Discard the XML Document that has been written to disc and create a new empty one for the next batch
                                            xmlDocumentMaster = CreateNewXmlDocument();
                                            batchNumber += 1;
                                            batchMemberNumber = 0;
                                            // Try to ensure that memory doesn't grow excessively
                                            GC.Collect();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("Exception loading XML string = {0}", ex.ToString());
                                        Console.WriteLine("XML is:");
                                        ShowQuoteXml(quoteXml);
                                        //using (StreamWriter sw = new StreamWriter("quoteXML.xml"))
                                        //{
                                        //    sw.WriteLine(quoteXml);
                                        //}
                                        errorOccurred = true;
                                        throw new ApplicationFailureException(
                                            String.Format("Application failed processing XML for Quote sequence number {0}", quoteCount));
                                    }
                                }
                                else
                                {
                                    if (String.Compare(xmlReader.Name, _rootNodeName) == 0)
                                    {
                                        // Try to determine the namespace
                                        _defaultNamespace = GetXmlAttributeValue(xmlReader, _namespaceAttributeName);
                                        if (_defaultNamespace == null )
                                        {
                                            _defaultNamespace = _defaultNamespaceHardcoded;
                                            Console.WriteLine();
                                            Console.WriteLine("Defaulting the namespace to \"{0}\"", _defaultNamespace);
                                        }
                                        else
                                        {
                                            Console.WriteLine();
                                            Console.WriteLine("The namespace was determined to be \"{0}\"", _defaultNamespace);
                                        }
                                    } // Try to determine the namespace

                                    if (!xmlReader.IsEmptyElement)
                                    {
                                        indentingLevel += 1;
                                        // Console.WriteLine("{0}Embedded Node {1} begin", Indenting(indentingLevel), indentingLevel);
                                    }
                                    else
                                    {
                                        // Console.WriteLine("{0}Embedded Empty Node {1}", Indenting(indentingLevel), indentingLevel);
                                    }

                                    //if (!xmlReader.HasAttributes)
                                    //{
                                    //    Console.WriteLine("{0}<{1}>", Indenting(indentingLevel), xmlReader.Name);
                                    //}
                                    //else
                                    //{
                                    //    Console.WriteLine("{0}<{1} {2} >",
                                    //        Indenting(indentingLevel), xmlReader.Name, AttributeString(xmlReader));
                                    //}
                                }
                                break; // XmlNodeType.Element
                            case XmlNodeType.Text:
                                if (_debugOutputTopLevelXML)
                                {
                                    Console.WriteLine("{0}{1}", Indenting(indentingLevel + 1), xmlReader.Value);
                                }
                                break;
                            case XmlNodeType.CDATA:
                                if (_debugOutputTopLevelXML)
                                {
                                    Console.WriteLine("<![CDATA[{0}]]>", xmlReader.Value);
                                }
                                break;
                            case XmlNodeType.ProcessingInstruction:
                                if (_debugOutputTopLevelXML)
                                {
                                    Console.WriteLine("<?{0} {1}?>", xmlReader.Name, xmlReader.Value);
                                }
                                break;
                            case XmlNodeType.Comment:
                                if (_debugOutputTopLevelXML)
                                {
                                    Console.WriteLine("<!--{0}-->", xmlReader.Value);
                                }
                                break;
                            case XmlNodeType.XmlDeclaration:
                                {
                                    if (_debugOutputTopLevelXML)
                                    {
                                        Console.WriteLine("<?xml {0}?>", xmlReader.Value);
                                    }
                                    _xmlVersion = GetXmlAttributeValue(xmlReader, _xmlVersionAttributeName);
                                    if (_xmlVersion==null)
                                    {
                                        _xmlVersion = _xmlVersionDefault;
                                        Console.WriteLine("Defaulting XML Version to \"{0}\"", _xmlVersion);
                                    }
                                    _documentEncoding = GetXmlAttributeValue(xmlReader, _documentEncodingAttributeName);
                                    if (_documentEncoding==null)
                                    {
                                        _documentEncoding = _documentEncodingDefault;
                                        Console.WriteLine("Defaulting Document Encoding to \"{0}\"", _documentEncoding);
                                    }
                                }
                                break;
                            case XmlNodeType.Document:
                                if (_debugOutputTopLevelXML)
                                {
                                    Console.WriteLine("{0}", XmlNodeType.Document);
                                }
                                break;
                            case XmlNodeType.DocumentType:
                                if (_debugOutputTopLevelXML)
                                {
                                    Console.WriteLine("<!DOCTYPE {0} [{1}]", xmlReader.Name, xmlReader.Value);
                                }
                                break;
                            case XmlNodeType.EntityReference:
                                if (_debugOutputTopLevelXML)
                                {
                                    Console.WriteLine("Entity Reference \"{0}\"",xmlReader.Name);
                                }
                                break;
                            case XmlNodeType.EndElement:
                                if (_debugOutputTopLevelXML)
                                {
                                    Console.WriteLine("</{0}>", xmlReader.Name);
                                }
                                if (String.Compare(xmlReader.Name, _quoteSubTreeName) == 0)
                                {
                                    indentingLevel -= 1;
                                }
                                else
                                {
                                    // Console.WriteLine("{0}Embedded Node {1} end", Indenting(indentingLevel), indentingLevel);
                                    indentingLevel -= 1;
                                }
                                break;
                            default:
                                Console.WriteLine("Unexpected Node Type \"{0}\"", xmlReader.NodeType);
                                break;
                        } // switch (xmlReader.NodeType)

                    } // while reading

                    if ( batchMemberNumber > 0 )
                    {
                        Console.WriteLine();
                        Console.WriteLine("{0} : Batch {1} end",
                            DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss.fff"), batchNumber);
                        WriteXmlQuoteBatchDocument(xmlFilename, xmlDocumentMaster, namespaceName, batchNumber);
                    }

                    Console.WriteLine();
                    if ( quoteCount < 1 )
                    {
                        Console.WriteLine("Total count of quotes processed was {0}", quoteCount.ToString("#,##0"));
                    }
                    else
                    {
                        double averageQuoteTimeMilliseconds = (double)totalTimeStopwatch.ElapsedMilliseconds / quoteCount;
                        Console.WriteLine("Total count of quotes processed was {0}. Average quote copy time was {1} seconds",
                                            quoteCount.ToString("#,##0"),
                                            (averageQuoteTimeMilliseconds / 1000.0).ToString("0.000000"));
                    }
                    Console.WriteLine();
                    TimeSpan totalElapsedTime = totalTimeStopwatch.Elapsed;
                    Console.WriteLine("Total time taken was {0:00}:{1:00}:{2:00}.{3:000}",
                                            totalElapsedTime.Hours,
                                            totalElapsedTime.Minutes,
                                            totalElapsedTime.Seconds,
                                            totalElapsedTime.Milliseconds);

                    if ( quoteCount < 1 )
                    {
                        throw new NoQuotesDetected(xmlFilename);
                    }

                } // using XmlReader

            } // using StreamReader

        } // SplitXmlDocumentFileUsingXmlReader
    } // Program
}
