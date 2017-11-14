using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Diagnostics;

namespace TestXPath
{
    partial class Program
    {
        static string NamespaceOf(XmlDocument xmlDocument)
        {
            string namespaceName = null;

            // Find all namespace specifications that are held on nodes that are children of the root
            XmlNodeList xmlNamespaceList = xmlDocument.SelectNodes(@"/*/namespace::*[name()='']");

            //Console.WriteLine("Namespace count is {0}", xmlNamespaceList.Count);
            //foreach (XmlNode xmlNamespaceNode in xmlNamespaceList)
            //{
            //    Console.WriteLine("Node Name \"{0}\", Value \"{1}\", InnerXml \"{2}\"",
            //        xmlNamespaceNode.Name, xmlNamespaceNode.Value, xmlNamespaceNode.InnerXml);
            //}

            if (xmlNamespaceList.Count == 0)
            {
                Console.WriteLine("Unable to determine the namespace for \"{0}\". No namespaces were detected",
                    xmlDocument.BaseURI);
            }
            else if (xmlNamespaceList.Count > 1)
            {
                Console.WriteLine("Unable to determine a unique namespace for \"{0}\". Number of namespaces detected was {1}",
                    xmlDocument.BaseURI,
                    xmlNamespaceList.Count);
                foreach ( XmlNode xmlNodeNamespace in xmlNamespaceList)
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

                        if (    (previousToken==xPathDelimiter)
                             || (charIndex==0)
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
                        else if (    (previousToken == xPathQueryOpen)
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

        private static string GetValueOfNode( XmlNode xmlNode )
        {
            string value = null;
            if ( xmlNode == null )
            {
                value = "(undefined)";
            }
            else
            {
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
                        Console.WriteLine("GetValueOfNode: Unprocessed NodeType {0}", xmlNode.NodeType);
                        break;
                }
            }
            return value;
        } // GetValueOfNode

        private static void ShowDescriptiveXmlNode( XmlNode xmlNode , int indentLevel)
        {
            string indent = new string(' ', indentLevel*2);
            //Console.WriteLine("\"{0}\" = \"{1}\" -> \"{2}\"",
            //    xmlNode.Name, xmlNode.Value, xmlNode.InnerXml);
            if ( indentLevel == 0 )
            {
                Console.Write("{0}\"{1}\" ({2})", indent, xmlNode.Name, xmlNode.NodeType);
            }
            else
            {
                Console.Write("{0}{1}:\"{2}\" ({3})", indent, indentLevel, xmlNode.Name, xmlNode.NodeType);
            }
            if (xmlNode.Value == null)
            {
                Console.Write(" has no value");
                string value = GetValueOfNode(xmlNode);
                // string value = null;
                if (value == null)
                {
                    Console.Write(" and has no node value");
                }
                else
                {
                    Console.Write(" and has node value \"{0}\"", value);
                }
            }
            else
            {
                Console.Write(" and has value \"{0}\"", xmlNode.Value);
            }
            if (xmlNode.InnerText == null)
            {
                Console.Write(" and has no InnerText");
            }
            else
            {
                Console.Write(" and has InnerText \"{0}\"", xmlNode.InnerText);
            }

            if (!xmlNode.HasChildNodes)
            {
                Console.WriteLine(" and no children");
            }
            else
            {
                Console.WriteLine(" and has {0} {1}",
                    xmlNode.ChildNodes.Count,
                    (xmlNode.ChildNodes.Count==1?"child":"children"));
                foreach (XmlNode xmlNodeChild in xmlNode.ChildNodes)
                {
                    ShowDescriptiveXmlNode(xmlNodeChild, indentLevel+1);
                }
            }
        } // ShowDescriptiveXmlNode

        private static void ShowXmlNode(XmlNode xmlNode, int indentLevel)
        {
            string indent = new string(' ', indentLevel * 2);
            //Console.WriteLine("\"{0}\" = \"{1}\" -> \"{2}\"",
            //    xmlNode.Name, xmlNode.Value, xmlNode.InnerXml);
            if (indentLevel == 0)
            {
                Console.Write("{0}\"{1}\" ({2})", indent, xmlNode.Name, xmlNode.NodeType);
            }
            else
            {
                Console.Write("{0}{1}:\"{2}\" ({3})", indent, indentLevel, xmlNode.Name, xmlNode.NodeType);
            }
            if (xmlNode.Value == null)
            {
                string value = GetValueOfNode(xmlNode);
                // string value = null;
                if (value != null)
                {
                    Console.Write(" and has node value \"{0}\"", value);
                }
            }
            else
            {
                Console.Write(" and has value \"{0}\"", xmlNode.Value);
            }

            if (!xmlNode.HasChildNodes)
            {
                Console.WriteLine();
            }
            else
            {
                Console.Write(" and has {0}", xmlNode.ChildNodes.Count);
                if (xmlNode.ChildNodes.Count == 1)
                {
                    Console.WriteLine(" child of NodeType {0}", xmlNode.FirstChild.NodeType);
                    if ( xmlNode.FirstChild.NodeType != XmlNodeType.Text)
                    {
                        ShowXmlNode(xmlNode.FirstChild, indentLevel + 1);
                    }
                }
                else
                {
                    Console.WriteLine(" children");
                    foreach (XmlNode xmlNodeChild in xmlNode.ChildNodes)
                    {
                        ShowXmlNode(xmlNodeChild, indentLevel + 1);
                    }
                }
            }
        } // ShowXmlNode

        private static XmlNodeList SelectXmlNodes( string namespaceName, XmlNode xmlNode, string xPath)
        {
            XmlNodeList xmlNodeList = null;

            if (namespaceName != null)
            {
                NameTable nameTable = new NameTable();
                XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(nameTable);
                string namespacePrefix = "test";
                xmlNamespaceManager.AddNamespace(namespacePrefix, namespaceName);

                xPath = PrefixSimpleXPath(namespacePrefix, xPath);
                xmlNodeList = xmlNode.SelectNodes(xPath, xmlNamespaceManager);
            }
            else
            {
                xmlNodeList = xmlNode.SelectNodes(xPath);
            }
            return xmlNodeList;

        } // SelectXmlNodes

        private static XmlNode SelectSingleXmlNode(string namespaceName, XmlNode xmlNode, string xPath)
        {
            XmlNode xmlNodeResult = null;

            if (namespaceName != null)
            {
                NameTable nameTable = new NameTable();
                XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(nameTable);
                string namespacePrefix = "test";
                xmlNamespaceManager.AddNamespace(namespacePrefix, namespaceName);

                xPath = PrefixSimpleXPath(namespacePrefix, xPath);
                xmlNodeResult = xmlNode.SelectSingleNode(xPath, xmlNamespaceManager);
            }
            else
            {
                xmlNodeResult = xmlNode.SelectSingleNode(xPath);
            }
            return xmlNodeResult;

        } // SelectXmlNodes

        private static void ShowClaim(string namespaceName, XmlNode xmlNodeClaims, ref int driverClaimSetCount)
        {
            XmlNode xmlNodeBrokerDataExtract = xmlNodeClaims.ParentNode.ParentNode.ParentNode.ParentNode;
            string xPathQuoteRef = _quoteRefName;
            try
            {
                XmlNode xmlNodeQuoteRef = SelectSingleXmlNode(namespaceName, xmlNodeBrokerDataExtract, xPathQuoteRef );
                if (xmlNodeQuoteRef == null )
                {
                    Console.WriteLine("No XmlNode {0}", xPathQuoteRef);
                }
                else
                {
                    XmlNode xmlNodeDriver = xmlNodeClaims.ParentNode;
                    XmlNode xmlNodeDriverNameX = SelectSingleXmlNode(namespaceName, xmlNodeDriver, "NameX");
                    string driverName = GetValueOfNode(xmlNodeDriverNameX);

                    if (!xmlNodeClaims.HasChildNodes)
                    {
                        Console.WriteLine("QuoteRef \"{0}\" for Driver \"{1}\" has no Claims",
                                            GetValueOfNode(xmlNodeQuoteRef),
                                            driverName);
                    }
                    else
                    {
                        driverClaimSetCount += 1;
                        Console.WriteLine("{0} : QuoteRef \"{1}\" for Driver \"{2}\" has {3} Claims",
                                            driverClaimSetCount ,
                                            GetValueOfNode(xmlNodeQuoteRef),
                                            driverName ,
                                            xmlNodeClaims.ChildNodes.Count);
                        XmlNodeList xmlNodeListClaim = SelectXmlNodes(namespaceName, xmlNodeClaims, "Claim");
                        if ( xmlNodeListClaim.Count == 0 )
                        {
                            Console.WriteLine("    Unable to find Claims");
                        }
                        else
                        {
                            for ( int claimIndex = 0; claimIndex < xmlNodeListClaim.Count; ++claimIndex)
                            {
                                XmlNode xmlNodeClaim = xmlNodeListClaim[claimIndex];

                                XmlNode xmlNodeDateMade = SelectSingleXmlNode(namespaceName, xmlNodeClaim, "DateMade");
                                Console.WriteLine("    DateMade = \"{0}\"", GetValueOfNode(xmlNodeDateMade));

                                XmlNode xmlNodeClaimType = SelectSingleXmlNode(namespaceName, xmlNodeClaim, "ClaimType");
                                XmlNode xmlNodeClaimTypeDescription = SelectSingleXmlNode(namespaceName, xmlNodeClaimType, "Description");
                                Console.WriteLine("    ClaimType = \"{0}\"", GetValueOfNode(xmlNodeClaimTypeDescription));

                                XmlNode xmlFaultType = SelectSingleXmlNode(namespaceName, xmlNodeClaim, "FaultType");
                                XmlNode xmlNodeFaultTypeDescription = SelectSingleXmlNode(namespaceName, xmlFaultType, "Description");
                                Console.WriteLine("    FaultType = \"{0}\"", GetValueOfNode(xmlNodeFaultTypeDescription));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ShowClaim: Exception finding \"{0}\" {1}", xPathQuoteRef, ex.ToString());
            }
        } // ShowClaim

        private static void ShowQuote(string namespaceName, XmlNode xmlNodeBrokerDataExtract)
        {
            string xPathQuoteRef = _quoteRefName;
            try
            {
                XmlNode xmlNodeQuoteRef = SelectSingleXmlNode(namespaceName, xmlNodeBrokerDataExtract, xPathQuoteRef);
                if (xmlNodeQuoteRef == null)
                {
                    Console.WriteLine("No XmlNode {0}", xPathQuoteRef);
                }
                else
                {
                    Console.WriteLine("{0} = \"{1}\"", _quoteRefName, GetValueOfNode(xmlNodeQuoteRef));
                    XmlNodeList xmlNodeListDrivers = SelectXmlNodes(namespaceName, xmlNodeBrokerDataExtract, "Risk/Drivers/Driver");
                    Console.WriteLine("    Driver Count is {0}", xmlNodeListDrivers.Count);
                    for ( int driverIndex = 0; driverIndex < xmlNodeListDrivers.Count; ++driverIndex)
                    {
                        Console.WriteLine("    Driver Index {0} = Driver Number {1}", driverIndex, driverIndex + 1);
                        XmlNode xmlNodeDriver = SelectSingleXmlNode(
                                                    namespaceName,
                                                    xmlNodeBrokerDataExtract,
                                                    String.Format("Risk/Drivers/Driver[{0}]",driverIndex+1));
                        if ( xmlNodeDriver == null )
                        {
                            Console.WriteLine("    Driver Index {0} does not exist",driverIndex);
                        }
                        else
                        {
                            XmlNode xmlNodeDriverName = SelectSingleXmlNode(namespaceName, xmlNodeDriver, "NameX");
                            Console.WriteLine("        Name = \"{0}\"", GetValueOfNode(xmlNodeDriverName));

                            XmlNode xmlNodeDriverAge = SelectSingleXmlNode(namespaceName, xmlNodeDriver, "Age");
                            Console.WriteLine("        Age = \"{0}\"", GetValueOfNode(xmlNodeDriverAge));

                            XmlNode xmlNodeDriverDOBDay = SelectSingleXmlNode(namespaceName, xmlNodeDriver, "DOBDay");
                            int dobDay = 0;
                            int.TryParse(GetValueOfNode(xmlNodeDriverDOBDay),out dobDay);

                            XmlNode xmlNodeDriverDOBMonth = SelectSingleXmlNode(namespaceName, xmlNodeDriver, "DOBMonth");
                            int dobMonth = 0;
                            int.TryParse(GetValueOfNode(xmlNodeDriverDOBMonth), out dobMonth);

                            XmlNode xmlNodeDriverDOBYear = SelectSingleXmlNode(namespaceName, xmlNodeDriver, "DOBYear");
                            int dobYear = 0;
                            int.TryParse(GetValueOfNode(xmlNodeDriverDOBYear), out dobYear);

                            Console.WriteLine("            Date of birth = {0}-{1}-{2}",
                                dobDay.ToString("00"), dobMonth.ToString("00"), dobYear.ToString("####"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ShowQuote: Exception finding \"{0}\" {1}", xPathQuoteRef, ex.ToString());
            }
        }

        private static XmlNodeList SelectXmlNodes( XmlDocument xmlDocument, string namespaceName, string xPath )
        {
            XmlNodeList xmlNodeList = null;

            if (namespaceName != null)
            {
                NameTable nameTable = new NameTable();
                XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(nameTable);
                string namespacePrefix = "test";
                xmlNamespaceManager.AddNamespace(namespacePrefix, namespaceName);

                xPath = PrefixSimpleXPath(namespacePrefix, xPath);
                xmlNodeList = xmlDocument.DocumentElement.SelectNodes(xPath, xmlNamespaceManager);
            }
            else
            {
                xmlNodeList = xmlDocument.SelectNodes(xPath);
            }
            return xmlNodeList;
        } // SelectXmlNodes

        private static bool AddXmlAttribute(XmlDocument xmlDocument, XmlNode xmlNode , string name, string value)
        {
            bool success = false;

            if ( xmlNode != null )
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
                    Console.WriteLine("AddXmlAttribute exception {0}", ex.ToString());
                }
            }
            return success;
        } // AddXmlAttribute

        private static void ProcessXmlDocument(string fullDocumentName, string searchXPath)
        {
            if (!File.Exists(fullDocumentName))
            {
                Console.WriteLine("XML Document \"{0}\" does not exist", fullDocumentName);
            }
            else
            {
                XmlDocument xmlDocument = new XmlDocument();

                try
                {
                    Console.WriteLine();
                    Console.WriteLine("Loading XML Document \"{0}\"", fullDocumentName);
                    Stopwatch stopwatchLoad = new Stopwatch();
                    stopwatchLoad.Start();
                    xmlDocument.Load(fullDocumentName);
                    stopwatchLoad.Stop();
                    Console.WriteLine("Load time {0} seconds", (stopwatchLoad.ElapsedMilliseconds/1000.0).ToString("0.000"));
                    Console.WriteLine();
                    Console.WriteLine("Specified search XPath is \"{0}\"", searchXPath);

                    string namespaceName = NamespaceOf(xmlDocument);

                    string xPath = null;
                    XmlNodeList xmlNodeList = null;

                    if (namespaceName!=null)
                    {
                        NameTable nameTable = new NameTable();
                        XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(nameTable);
                        string namespacePrefix = "test";
                        xmlNamespaceManager.AddNamespace(namespacePrefix, namespaceName);

                        xPath = PrefixSimpleXPath(namespacePrefix, searchXPath);
                        xmlNodeList = xmlDocument.DocumentElement.SelectNodes(xPath, xmlNamespaceManager);
                    }
                    else
                    {
                        xPath = searchXPath;
                        xmlNodeList = xmlDocument.SelectNodes(xPath);
                    }

                    if (xmlNodeList.Count == 0)
                    {
                        Console.WriteLine("No {0} nodes satisfy XPath \"{1}\"",
                                            (namespaceName!=null) ? "namespace" : "", xPath);
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("{0} {1}nodes satisfy XPath \"{2}\"",
                                            xmlNodeList.Count, namespaceName!=null ? "namespace " : "", xPath);
                        if ( xPath.IndexOf("bookstore") > 0 )
                        {
                            XmlNode xmlNode = SelectSingleXmlNode(namespaceName, xmlDocument.DocumentElement, "/bookstore/book[1]/title");
                            if (AddXmlAttribute(xmlDocument, xmlNode, "type", "fantasy"))
                            {
                                WriteXmlDocument(fullDocumentName, xmlDocument, namespaceName, "attribute");
                            }
                            XmlNodeList xmlNodeListBooks = SelectXmlNodes(namespaceName, xmlDocument.DocumentElement, "/bookstore/book");
                        }
                        else if ( xPath.IndexOf("Claims") > 0)
                        {
                            // Process Claims
                            int driverClaimSetCount = 0;
                            foreach (XmlNode xmlNodeClaims in xmlNodeList)
                            {
                                ShowClaim(namespaceName, xmlNodeClaims, ref driverClaimSetCount);
                            }
                        }
                        else if ( xPath.IndexOf("BrokerDataExtract")>0)
                        {
                            // Process each quote
                            foreach (XmlNode xmlNodeQuote in xmlNodeList)
                            {
                                ShowQuote(namespaceName, xmlNodeQuote);
                            }
                        }
                        else
                        {
                            // Display generic node tree
                            foreach (XmlNode xmlNode in xmlNodeList)
                            {
                                // ShowDescriptiveXmlNode(xmlNode, 0);
                                ShowXmlNode(xmlNode, 0);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("ProcessXmlDocument: Exception {0}", ex.ToString());
                }

            }

        } // ProcessXmlDocument

        private const string _quoteRefName = "QuoteRef";

        static int Main(string[] args)
        {
            int error = 0;

            if (args.Length < 2)
            {
                error = 1;
            }
            else
            {
                string xmlFilenameNoNamespace = Path.GetFullPath(args[0]);
                string xmlFilenameWithNamespace = Path.GetFullPath(args[1]);

                string xmlFilenameWithNamespaceAndLotsOfData = null;

                if (args.Length>2)
                {
                    xmlFilenameWithNamespaceAndLotsOfData = Path.GetFullPath(args[2]);
                }

                string searchXPath = "//bookstore/book";
                // string searchXPath = "*";
                // string searchXPath = "*/book";
                ProcessXmlDocument(xmlFilenameNoNamespace, searchXPath );
                ProcessXmlDocument(xmlFilenameWithNamespace, searchXPath );
                // searchXPath = "/";
                searchXPath = "//bookstore//book";
                ProcessXmlDocument(xmlFilenameWithNamespace, searchXPath);

                if ( xmlFilenameWithNamespaceAndLotsOfData != null )
                {
                    // "//BrokerDataExtract/Risk/Drivers/Driver/Claims"
                    // "/BrokerDataExtract/Risk/Drivers/Driver/Claims"
                    // "//BrokerDataExtract"
                    // ProcessXmlDocument(xmlFilenameWithNamespaceAndLotsOfData, "/");
                    // ProcessXmlDocument(xmlFilenameWithNamespaceAndLotsOfData, "/EISExtracts/BrokerDataExtract/Risk/Drivers/Driver/Claims");
                    // ProcessXmlDocument(xmlFilenameWithNamespaceAndLotsOfData, "//BrokerDataExtract/Risk/Drivers/Driver/Claims");
                    // ProcessXmlDocument(xmlFilenameWithNamespaceAndLotsOfData, "//BrokerDataExtract");
                    ProcessXmlDocument(xmlFilenameWithNamespaceAndLotsOfData, "//BrokerDataExtract[QuoteRef=920227302]");
                }

            }

            return error;
        }
    }
}
