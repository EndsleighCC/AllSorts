using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;
using System.Text;
using System.Reflection;

namespace SplitXmlDocument
{
    partial class Program
    {
        private class NoQuotesDetected : ApplicationException
        {
            public NoQuotesDetected(string xmlFilename) : base(xmlFilename)
            {
            }
        }

        private class TooManyQuotes : ApplicationException
        {
            public TooManyQuotes(string xmlFilename) : base(xmlFilename)
            {
            }
        }

        private class ApplicationFailureException : ApplicationException
        {
            public ApplicationFailureException(string message) : base(message)
            {
            }
        }

        static string PrefixSimpleXPath(string prefix, string xPath)
        {
            StringBuilder sbFullyPrefixedXPath = new StringBuilder();

            const char xPathDelimiter = '/';
            const char xPathQueryOpen = '[';

            if (!xPath.Contains(xPathDelimiter))
            {
                // The supplied XPath does not contain any XPath path delimiters

                if (!String.IsNullOrEmpty(xPath))
                {
                    sbFullyPrefixedXPath.Append(prefix);
                    sbFullyPrefixedXPath.Append(":");
                    sbFullyPrefixedXPath.Append(xPath);
                }

            } // The supplied XPath does not contain any XPath path delimiters
            else
            {
                // The supplied XPath contains XPath path delimiters

                char currentToken = '\0';
                char previousToken = '\0';
                for (int charIndex = 0; charIndex < xPath.Length; ++charIndex)
                {
                    previousToken = currentToken;
                    currentToken = xPath[charIndex];
                    if (currentToken == xPathDelimiter)
                    {
                        // Add just the token (delimiter)
                        sbFullyPrefixedXPath.Append(currentToken);
                    }
                    else
                    {
                        // Not an XPath delimiter

                        if ((previousToken == xPathDelimiter)
                             || (charIndex == 0)
                           )
                        {
                            // Prefix non-delimiter tokens that:
                            // - Are preceded by a delimiter,
                            // or
                            // - Are at the start of the string
                            sbFullyPrefixedXPath.Append(prefix);
                            sbFullyPrefixedXPath.Append(":");
                            sbFullyPrefixedXPath.Append(currentToken);
                        }
                        else if ((previousToken == xPathQueryOpen)
                                  && (!char.IsNumber(currentToken))
                                )
                        {
                            // Prefix non-numeric tokens to the right of an open query
                            // Note that this does *not* (yet?) cope with "XPath Functions" e.g. last(), position(), etc..
                            sbFullyPrefixedXPath.Append(prefix);
                            sbFullyPrefixedXPath.Append(":");
                            sbFullyPrefixedXPath.Append(currentToken);
                        }
                        else
                        {
                            // Add just the token
                            sbFullyPrefixedXPath.Append(currentToken);
                        }

                    } // Not an XPath delimiter
                } // for charIndex
            } // The supplied XPath contains XPath path delimiters

            return sbFullyPrefixedXPath.ToString();
        } // PrefixSimpleXPath

        private static string GetNodeValue(XmlNode xmlNode)
        {
            string value = null;
            switch (xmlNode.NodeType)
            {
                case XmlNodeType.Text:
                    value = xmlNode.Value;
                    break;
                case XmlNodeType.Element:
                    if (!xmlNode.HasChildNodes)
                    {
                        value = null;
                    }
                    else
                    {
                        if (xmlNode.FirstChild.NodeType == XmlNodeType.Text)
                        {
                            value = xmlNode.FirstChild.Value;
                        }
                        else
                        {
                            value = null;
                        }
                    }
                    break;
                default:
                    Console.WriteLine("GetNodeValue : Unprocessed NodeType {0}", xmlNode.NodeType);
                    break;
            }
            return value;
        } // GetNodeValue

        private static string GetFirstQuoteRef( string xmlFilename, XmlDocument xmlDocument, string namespaceName)
        {
            string quoteRef = null;

            // Find all QuoteRef elements from the root of the document
            XmlNodeList xmlQuoteRefNodeList = GetXmlNodeList(xmlFilename, xmlDocument, namespaceName, "//" + _quoteRefName);
            if (xmlQuoteRefNodeList.Count == 0)
            {
                Console.WriteLine("GetFirstQuoteRef : File \"{0}\" contains no quotes", xmlFilename);
            }
            else
            {
                foreach (XmlNode xmlQuoteRefNode in xmlQuoteRefNodeList)
                {
                    if (quoteRef == null)
                    {
                        quoteRef = GetNodeValue(xmlQuoteRefNode); // xmlQuoteRefNode.InnerText.Trim(new char[] { '\r', '\n', ' ' })
                    }
                }
            }
            return quoteRef;
        } // GetFirstQuoteRef

        private static string GetQuoteRef(XmlNode xmlNodeQuote, string namespaceName)
        {
            string quoteRef = null;

            XmlNode xmlNodeQuoteRef = GetSingleXmlNode(xmlNodeQuote, namespaceName, _quoteRefName);
            if (xmlNodeQuoteRef==null)
            {
                Console.WriteLine("GetQuoteRef : No \"[0}\" Node found", _quoteRefName);
            }
            else
            {
                quoteRef = GetNodeValue(xmlNodeQuoteRef);
            }
            return quoteRef;
        } // GetQuoteRef

        private static void ShowQuoteRefs( string xmlFilename, XmlDocument xmlDocument , string namespaceName )
        {
            // Find all QuoteRef elements from the root of the document
            XmlNodeList xmlQuoteRefNodeList = GetXmlNodeList(xmlFilename, xmlDocument, namespaceName, "//" + _quoteRefName);
            if (xmlQuoteRefNodeList.Count == 0)
            {
                Console.WriteLine("ShowQuoteRefs : File \"{0}\" contains no quotes", xmlFilename);
            }
            else
            {
                Console.WriteLine("ShowQuoteRefs : File \"{0}\" contains {1} quotes",
                                    xmlFilename, xmlQuoteRefNodeList.Count);
                int nodeCount = 0;
                foreach (XmlNode xmlQuoteRefNode in xmlQuoteRefNodeList)
                {
                    nodeCount += 1;
                    Console.WriteLine("    {0} : Quote \"{1}\" \"{2}\"",
                                nodeCount,
                                xmlQuoteRefNode.Name,
                                GetNodeValue(xmlQuoteRefNode)); // xmlQuoteRefNode.InnerText.Trim(new char[] { '\r' , '\n' , ' ' })
                }
            }
        } // ShowQuoteRefs

        private static List<string> GetQuoteRefList(string xmlFilename, XmlDocument xmlDocument, string namespaceName)
        {
            List<string> quoteRefList = new List<string>();

            // Find all QuoteRef elements from the root of the document
            XmlNodeList xmlQuoteRefNodeList = GetXmlNodeList(xmlFilename, xmlDocument, namespaceName, "//" + _quoteRefName);
            if (xmlQuoteRefNodeList.Count == 0)
            {
                Console.WriteLine("GetQuoteRefList : File \"{0}\" contains no quotes", xmlFilename);
            }
            else
            {
                //Console.WriteLine("GetQuoteRefList : File \"{0}\" contains {1} quotes",
                //                    xmlFilename, xmlQuoteRefNodeList.Count);
                foreach (XmlNode xmlQuoteRefNode in xmlQuoteRefNodeList)
                {
                    // quoteRefList.Add(xmlQuoteRefNode.InnerText.Trim(new char[] { '\r', '\n', ' ' }));
                    quoteRefList.Add(GetNodeValue(xmlQuoteRefNode));
                }
            }
            return quoteRefList;
        } // GetQuoteRefList

        private static XmlNode GetSingleXmlNode(XmlNode xmlNode, string namespaceName, string xPathSelect)
        {
            XmlNode xmlNodeResult = null;

            if (namespaceName == null)
            {
                try
                {
                    xmlNodeResult = xmlNode.SelectSingleNode(xPathSelect);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("GetSingleXmlNode : Exception {0}", ex.ToString());
                }
            }
            else
            {
                // There is a namespace
                try
                {
                    NameTable nameTable = new NameTable();
                    XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(nameTable);
                    string namespacePrefix = _rootNodeName + "Lookup";
                    string xPath = PrefixSimpleXPath(namespacePrefix, xPathSelect);
                    xmlNamespaceManager.AddNamespace(namespacePrefix, namespaceName);
                    xmlNodeResult = xmlNode.SelectSingleNode(xPath, xmlNamespaceManager);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("GetSingleXmlNode({0}) : Exception {1}", namespaceName, ex.ToString());
                }
            } // There is a namespace

            return xmlNodeResult;

        } // GetSingleXmlNode

        private static XmlNodeList GetXmlNodeList(string xmlFilename, XmlNode xmlNode, string namespaceName, string xPathSelect)
        {
            XmlNodeList xmlNodeList = null;

            if (namespaceName == null)
            {
                try
                {
                    xmlNodeList = xmlNode.SelectNodes(xPathSelect);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("GetXmlNodeList(Node) : Exception {0}", ex.ToString());
                }
            }
            else
            {
                // There is a namespace
                try
                {
                    NameTable nameTable = new NameTable();
                    XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(nameTable);
                    string namespacePrefix = _rootNodeName + "Lookup";
                    string xPath = PrefixSimpleXPath(namespacePrefix, xPathSelect);
                    xmlNamespaceManager.AddNamespace(namespacePrefix, namespaceName);
                    xmlNodeList = xmlNode.SelectNodes(xPath, xmlNamespaceManager);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("GetXmlNodeList(Node,\"{0}\") : Exception {1}", namespaceName, ex.ToString());
                }
            } // There is a namespace

            return xmlNodeList;

        } // GetXmlNodeList

        private static XmlNodeList GetXmlNodeList(string xmlFilename, XmlDocument xmlDocument, string namespaceName, string xPathSelect)
        {
            XmlNodeList xmlNodeList = null;

            if (namespaceName == null)
            {
                try
                {
                    // xmlDocument.DocumentElement appears to point to the first non-comment node
                    // which may not be the root of the document itself so use all the document nodes
                    // xmlNodeList = xmlDocument.DocumentElement.SelectNodes(xPathSelect);
                    xmlNodeList = xmlDocument.SelectNodes(xPathSelect);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("GetXmlNodeList(Document) : Exception {0}", ex.ToString());
                }
            }
            else
            {
                // There is a namespace
                try
                {
                    NameTable nameTable = new NameTable();
                    XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(nameTable);
                    string namespacePrefix = _rootNodeName + "Lookup";
                    string xPath = PrefixSimpleXPath(namespacePrefix, xPathSelect);
                    xmlNamespaceManager.AddNamespace(namespacePrefix, namespaceName);
                    // xmlDocument.DocumentElement appears to point to the first non-comment node
                    // which may not be the root of the document itself so use all the document nodes
                    // xmlNodeList = xmlDocument.DocumentElement.SelectNodes(xPath, xmlNamespaceManager);
                    xmlNodeList = xmlDocument.SelectNodes(xPath, xmlNamespaceManager);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("GetXmlNodeList(Document,\"{0}\") : Exception {1}", namespaceName, ex.ToString());
                }
            } // There is a namespace

            return xmlNodeList;

        } // GetXmlNodeList

        private static void ShowQuoteNodeWithChildren(string xmlFilename, XmlDocument xmlDocument, string namespaceName, string xPath)
        {
            try
            {
                XmlNodeList xmlQuoteNodeList = GetXmlNodeList(xmlFilename, xmlDocument, namespaceName, xPath);
                if ( (xmlQuoteNodeList==null) || (xmlQuoteNodeList.Count == 0))
                {
                    Console.WriteLine("ShowQuoteNodeWithChildren : File \"{0}\" contains no \"{1}\"", xmlFilename, xPath);
                }
                else
                {
                    Console.WriteLine("ShowQuoteNodeWithChildren : File \"{0}\" contains {1} \"{2}\"",
                                        xmlFilename, xmlQuoteNodeList.Count, xPath);
                    List<string> quoteRefList = GetQuoteRefList(xmlFilename, xmlDocument, namespaceName);
                    int quoteRefIndex = -1;
                    foreach (XmlNode xmlQuoteNode in xmlQuoteNodeList)
                    {
                        quoteRefIndex += 1;
                        if (xmlQuoteNode.HasChildNodes)
                        {
                            Console.WriteLine("    {0} : Quote \"{1}\" {2} child nodes called \"{3}\"",
                                        quoteRefIndex,
                                        quoteRefList[quoteRefIndex],
                                        (xmlQuoteNode.HasChildNodes) ? "has" : "has no",
                                        xPath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ShowQuoteNodeWithChildren Exception Message \"{0}\"", ex.Message ?? "(no message)");
                Console.WriteLine("ShowQuoteNodeWithChildren Exception {0}", ex.ToString());
            }
        } // ShowQuoteNodeWithChildren

        private static void ShowQuoteRefs(string xmlFilename, XmlDocument xmlDocument, string namespaceName, int batchNumber)
        {
            // Find all QuoteRef elements from the root of the document
            XmlNodeList xmlQuoteRefNodeList = GetXmlNodeList(xmlFilename, xmlDocument, namespaceName, "//" + _quoteRefName);
            if (xmlQuoteRefNodeList.Count == 0)
            {
                Console.WriteLine("ShowQuoteRefs : File \"{0}\" contains no quotes");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("ShowQuoteRefs : File \"{0}\" batch number {1} contains {2} quotes",
                                    xmlFilename, batchNumber, xmlQuoteRefNodeList.Count);
                int nodeCount = 0;
                foreach (XmlNode xmlQuoteRefNode in xmlQuoteRefNodeList)
                {
                    nodeCount += 1;
                    Console.WriteLine("    {0} : Quote \"{1}\" \"{2}\"",
                                nodeCount,
                                xmlQuoteRefNode.Name,
                                GetNodeValue(xmlQuoteRefNode)); // xmlQuoteRefNode.InnerText.Trim(new char[] { '\r', '\n', ' ' })
                }
            }
        } // ShowQuoteRefs

        private static void ShowQuoteXml( string quoteXml )
        {
            int beginIndex = 0;
            int endIndex = 0;
            int lineNumber = 0;
            int newLineLength = Environment.NewLine.Length;
            do
            {
                lineNumber += 1;
                endIndex = quoteXml.IndexOf(Environment.NewLine, beginIndex);
                string xmlLine = null;
                if ( endIndex >= 0 )
                {
                    xmlLine = quoteXml.Substring(beginIndex, (endIndex - beginIndex));
                    beginIndex = endIndex + newLineLength;
                }
                else
                {
                    xmlLine = quoteXml.Substring(beginIndex);
                }
                Console.WriteLine("{0} : {1}", lineNumber, xmlLine);
            }
            while (endIndex >= 0) ;
        } // WriteQuoteXml

        static string Indenting(int indentingLevel)
        {
            if (_formatIntermediateXml)
                return new string(' ', indentingLevel * 2);
            else
                return String.Empty;
        }

        private static string GetXmlAttributeString( XmlReader xmlReader )
        {
            string attributes = null;

            if ( xmlReader.HasAttributes )
            {
                for (int attributeIndex = 0; attributeIndex < xmlReader.AttributeCount; ++attributeIndex)
                {
                    xmlReader.MoveToAttribute(attributeIndex);
                    string thisAttribute = String.Format("{0}=\"{1}\"", xmlReader.Name, xmlReader.Value);
                    if (attributes == null )
                    {
                        attributes = thisAttribute;
                    }
                    else
                    {
                        attributes += " " + thisAttribute;
                    }
                }
            }
            return attributes;
        } // GetXmlAttributeString

        private static string GetXmlAttributeValue(XmlReader xmlReader, string attributeName)
        {
            string value = null;

            if (xmlReader.HasAttributes)
            {
                bool found = false;
                for (int attributeIndex = 0;
                    (!found)
                    && (attributeIndex < xmlReader.AttributeCount);
                    ++attributeIndex)
                {
                    xmlReader.MoveToAttribute(attributeIndex);

                    //Console.WriteLine("GetXmlAttributeValue({0}) : {1}=\"{2}\"",
                    //                    attributeName, xmlReader.Name, xmlReader.Value);
                    if (String.Compare(xmlReader.Name,attributeName)==0)
                    {
                        found = true;
                        value = xmlReader.Value;
                    }
                }
            }

            return value;
        } // GetXmlAttributeValue

        private static string GetXmlAttributeValue(XmlNode xmlNode, string attributeName)
        {
            string value = null;
            try
            {
                value = xmlNode.Attributes[attributeName].Value;
            }
            catch (NullReferenceException)
            {
                // Attribute does not exist
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetXmlAttributeValue(XmlNode) : Unexpected exception {0}", ex.ToString());
            }
            return value;
        } // GetXmlAttributeValue

        private static bool AddXmlAttribute(XmlDocument xmlDocument, XmlNode xmlNode, string name, string value)
        {
            bool success = false;

            if (xmlNode != null)
            {
                try
                {
                    XmlAttribute xmlAttribute = xmlDocument.CreateAttribute(name);
                    xmlAttribute.Value = value;
                    xmlNode.Attributes.Append(xmlAttribute);
                    success = true;
                }
                catch (Exception ex)
                {
                    success = false;
                    Console.WriteLine("XmlAddAttribute exception {0}", ex.ToString());
                }
            }
            return success;
        } // AddXmlAttribute

        private static string XmlLine( string xml )
        {
            StringBuilder sb = new StringBuilder(xml);
            if (_formatIntermediateXml)
            {
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        private static void MergeXmlDocuments( XmlDocument xmlDocumentMaster , XmlDocument xmlDocument )
        {
            try
            {
                foreach (XmlNode child in xmlDocument.ChildNodes)
                {
                    var newNode = xmlDocumentMaster.ImportNode(child, true);
                    xmlDocumentMaster.DocumentElement.AppendChild(newNode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception merging documents {0}", ex.ToString());
                throw new ApplicationFailureException(
                    String.Format("Application failed merging quote document into master document"));
            }
        }
        static string NamespaceOf(XmlDocument xmlDocument)
        {
            string namespaceName = null;

            // Find all namespace specifications that are held on nodes that are children of the root
            XmlNodeList xmlNamespaceList = xmlDocument.SelectNodes(@"/*/namespace::*[name()='']");

            if (xmlNamespaceList.Count == 0)
            {
                Console.WriteLine("NamespaceOf : Unable to determine the namespace for \"{0}\". No namespaces were detected",
                    xmlDocument.BaseURI);
            }
            else if (xmlNamespaceList.Count > 1)
            {
                Console.WriteLine("NamespaceOf : Unable to determine a unique namespace for \"{0}\". Count of namespaces detected was {1}",
                    xmlDocument.BaseURI,
                    xmlNamespaceList.Count);
                foreach (XmlNode xmlNodeNamespace in xmlNamespaceList)
                {
                    Console.WriteLine("    {0}", xmlNodeNamespace.Value);
                }
            }
            else
            {
                namespaceName = xmlNamespaceList[0].Value;
                //Console.WriteLine("Default namespace for \"{0}\" is \"{1}\"",
                //    xmlDocument.BaseURI,
                //    namespaceName);
            }

            return namespaceName;

        } // NamespaceOf

        private const int _defaultBatchMemberCount = 50000;

        private const string _xmlVersionAttributeName = "version";
        private static string _xmlVersion = null;
        private const string _xmlVersionDefault = "1.0";

        private const string _documentEncodingAttributeName = "encoding";
        private static string _documentEncoding = null;
        private const string _documentEncodingDefault = "UTF-8";

        private const string _namespaceAttributeName = "xmlns";

        private static string _defaultNamespace = null;
        // Default hardcoded value when the namespace cannot be determined from the document
        private const string _defaultNamespaceHardcoded = "http://ssp.web.services/MotorAggregator/schemas/PMQuoteData.xsd";

        private static bool _formatIntermediateXml = false;

        private const string _rootNodeName = "EISExtracts";
        private const string _quoteSubTreeName = "BrokerDataExtract";
        private const string _quoteRefName = "QuoteRef";

        private const string _quoteDriverXPath = "Risk/Drivers/Driver";
        private const string _quoteDriverNumberAttributeName = "number";
        private const string _quoteDriverRelationshipToProposerDescriptionName = "RelationshipToProposer/Description";
        private const string _quoteDriverRelationshipToProposerDescriptionProposerValue = "Proposer";

        private const string _quoteVehicleXPath = "Risk/Vehicles/Vehicle";
        private const string _quoteVehicleNumberAttributeName = "number";

        private static bool _generateDriverAndVehicleNumberXmlAttributes = false;
        private static bool _debugOutputTopLevelXML = false;

        private static void ShowApplicationBanner(string applicationTitle, string applicationAuthor)
        {
            string version = Assembly.GetEntryAssembly().GetName().Version.ToString();
            DateTime buildDateTime = new FileInfo(Assembly.GetEntryAssembly().Location).LastWriteTime;

            int topLineMinLength = applicationTitle.Length;

            string appInfoVersionDateTime = String.Format("v{0} {1}", version, buildDateTime.ToString("dd-MMMM-yyyy HH:mm:ss"));
            int bottomLineMinLength = appInfoVersionDateTime.Length + 1 /* space */ + applicationAuthor.Length;

            int leftTopFillerLength = 0;
            int rightTopFillerLength = 0;

            if (topLineMinLength <= bottomLineMinLength)
            {
                // Top line is shorter than the bottom line

                leftTopFillerLength = (bottomLineMinLength - topLineMinLength) / 2;
                if (leftTopFillerLength < 2)
                {
                    // Always have at least one fill character and one space
                    leftTopFillerLength = 2;
                }
                rightTopFillerLength = bottomLineMinLength - topLineMinLength - leftTopFillerLength;
                if (rightTopFillerLength < 2)
                {
                    // Always have at least one fill character and one space
                    rightTopFillerLength = 2;
                }

            } // Top line is shorter than the bottom line
            else
            {
                // Bottom line is shorter than the top line

                leftTopFillerLength = (topLineMinLength - bottomLineMinLength) / 2;
                if (leftTopFillerLength < 2)
                {
                    // Always have at least one fill character and one space
                    leftTopFillerLength = 2;
                }
                rightTopFillerLength = topLineMinLength - bottomLineMinLength - leftTopFillerLength;
                if (rightTopFillerLength < 2)
                {
                    // Always have at least one fill character and one space
                    rightTopFillerLength = 2;
                }

            } // Bottom line is shorter than the top line

            string topLineText = new string('*', leftTopFillerLength - 1) + " " + applicationTitle + " " + new string('*', rightTopFillerLength - 1);
            int bottomMiddleFiller = topLineText.Length - appInfoVersionDateTime.Length - applicationAuthor.Length;
            string bottomLineText = appInfoVersionDateTime + new string(' ', bottomMiddleFiller) + applicationAuthor;

            Console.WriteLine("{0}", topLineText);
            Console.WriteLine("{0}", bottomLineText);

        } // ShowApplicationBanner

        private static void ShowUsage()
        {
            Console.WriteLine();
            ShowApplicationBanner("SSP XML Document Splitter", "C. Cornelius");
            Console.WriteLine();
            Console.WriteLine("Usage: SplitXmlDocument {/Switches} Filename {BatchSize}");
            Console.WriteLine();
            Console.WriteLine("Where:");
            Console.WriteLine("    Filename = Name of the XML text document that should be split which may be specified");
            Console.WriteLine("               with full, relative or no path");
            Console.WriteLine("    BatchSize = (Optional) The number of quotes to appear in each component text file.");
            Console.WriteLine("                           The default batch size is {0}", _defaultBatchMemberCount.ToString("#,##0"));
            Console.WriteLine();
            Console.WriteLine("Switches:");
            Console.WriteLine("    d = Display debug processing of input XML");
            Console.WriteLine("    n = Generate number XML attributes on \"{0}\" and \"{1}\" nodes",
                                _quoteDriverXPath, _quoteVehicleXPath);
            Console.WriteLine();
            Console.WriteLine("Function:");
            Console.WriteLine("    Split an SSP Quote XML text document into smaller, component XML text documents with");
            Console.WriteLine("    each component text document containing BatchSize quotes with the default being {0}.",
                                _defaultBatchMemberCount.ToString("#,##0"));
            Console.WriteLine();
            Console.WriteLine("    Each component text document will appear in the same directory as the original text");
            Console.WriteLine("    document and will be named according to its batch number with the right zero filled");
            Console.WriteLine("    batch number placed prior to the file extension of the original text document.");
            Console.WriteLine();
            Console.WriteLine("    If required, in the output, \"{0}\" Nodes and \"{1}\" Nodes",
                                    _quoteDriverXPath, _quoteVehicleXPath);
            Console.WriteLine("    will be augmented with XML Attributes that indicate the one-based Driver Number and the");
            Console.WriteLine("    one-based Vehicle Number respectively.");
        }

        private static void ProcessCommandLine( string [] args , ref string xmlFilename, ref int batchMemberMaxCount )
        {
            for (int argIndex=0; argIndex<args.Length;++argIndex)
            {
                string argValue = args[argIndex];
                if (argValue.StartsWith("/"))
                {
                    // A switch

                    if (argValue.Length>1)
                    {
                        switch (argValue.ToLower()[1])
                        {
                            case 'd':
                                _debugOutputTopLevelXML = true;
                                Console.WriteLine("Debug will be generated for top level XML");
                                break;
                            case 'n':
                                _generateDriverAndVehicleNumberXmlAttributes = true;
                                Console.WriteLine("Number XML attributes will be generated on nodes \"{0}\" and \"{1}\"",
                                                    _quoteDriverXPath, _quoteVehicleXPath);
                                break;
                            default:
                                Console.WriteLine("Unknown switch \"{0}\"", argValue);
                                break;
                        } // switch
                    }

                } // A switch
                else
                {
                    // An argument

                    if (xmlFilename == null)
                    {
                        xmlFilename = argValue;
                        Console.WriteLine("Input filename is \"{0}\"", xmlFilename);
                    }
                    else
                    {
                        // Must be a batch size
                        int batchMemberMaxCountSupplied = 0;
                        if ( int.TryParse(argValue, out batchMemberMaxCountSupplied)
                            && (batchMemberMaxCountSupplied>0)
                           )
                        {
                            batchMemberMaxCount = batchMemberMaxCountSupplied;
                            Console.WriteLine("Specified Quote Batch Size is {0}", batchMemberMaxCount.ToString("#,##0"));
                        }
                        else
                        {
                            Console.WriteLine("Invalid Quote Batch Size \"{0}\"", argValue);
                        }

                    } // Must be a batch size
                } // An argument
            } // for argIndex
        } // ProcessCommandLine

        private static int NoXmlFilenameSupplied()
        {
            int error = ErrorDefinitions.Success;
            ShowUsage();
            Console.WriteLine();
            Console.WriteLine("SplitXmlDocument: At least a filename with or without full or relative path must be supplied");
            error = ErrorDefinitions.InsufficientCommandLineParameters;
            return error;
        }

        static int Main(string[] args)
        {
            int error = ErrorDefinitions.Success;

            if (args.Length < 1)
            {
                error = NoXmlFilenameSupplied();
            }
            else
            {
                // Command line switches or arguments were supplied

                string xmlFilename = null;
                int batchMemberMaxCount = _defaultBatchMemberCount;

                ProcessCommandLine(args, ref xmlFilename, ref batchMemberMaxCount);

                // the only mandatory parameter is the filename
                if (xmlFilename == null)
                {
                    error = NoXmlFilenameSupplied();
                }
                else
                {
                    // Sufficient arguments

                    // Generate a filename with full path
                    xmlFilename = Path.GetFullPath(xmlFilename);

                    if (!File.Exists(xmlFilename))
                    {
                        Console.WriteLine();
                        Console.WriteLine("XML document \"{0}\" does not exist", xmlFilename);
                        error = ErrorDefinitions.FileNotFound;
                    }
                    else
                    {
                        Console.WriteLine("Processing XML document \"{0}\"", xmlFilename);
                        Console.WriteLine("Batch size is {0}", batchMemberMaxCount.ToString("#,##0"));
                        try
                        {
                            SplitXmlDocumentFileUsingXmlReader(xmlFilename, batchMemberMaxCount);
                        }
                        catch (NoQuotesDetected ex)
                        {
                            Console.WriteLine();
                            Console.WriteLine("XML Text Document \"{0}\" contains no quotes", ex.Message);
                            error = ErrorDefinitions.NoQuotesDetected;
                        }
                        catch (ApplicationFailureException ex)
                        {
                            Console.WriteLine();
                            Console.WriteLine("Application Failure processing XML file \"{0}\". Message = \"{1}\"",
                                                    xmlFilename,
                                                    ex.Message ?? "(no message)");
                            Console.WriteLine("Exception:");
                            Console.WriteLine("{0}", ex.ToString());
                            error = ErrorDefinitions.FailureProcessingXML;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine();
                            Console.WriteLine("Unexpected error processing XML file \"{0}\". Message = \"{1}\"",
                                                    xmlFilename,
                                                    ex.Message ?? "(no message)");
                            Console.WriteLine("Exception:");
                            Console.WriteLine("{0}", ex.ToString());
                            error = ErrorDefinitions.UnexpectedFailure;
                        }
                    }

                    //if (error != ErrorDefinitions.Success)
                    //{
                    //    Console.WriteLine();
                    //    Console.WriteLine("Returning error {0}", error);
                    //}

                } // Sufficient arguments

            } // Command line switches or arguments were supplied

            return error;
        } // Main
    } // Program
}

