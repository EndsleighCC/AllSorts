using System;
using System.IO;
using System.Xml;

namespace SplitXmlDocument
{
    partial class Program
    {
        static void SplitXmlDocumentUsingDOM(string xmlFilename)
        {
            XmlDocument xmlDocument = new XmlDocument();

            bool documentLoaded = false;

            try
            {
                xmlDocument.Load(xmlFilename);
                documentLoaded = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception loading XML Document \"{0}\" = {1}", xmlFilename, ex.ToString());
            }

            if (documentLoaded)
            {
                // Document loaded successfully
                string namespaceName = NamespaceOf(xmlDocument);

                ShowQuoteRefs(xmlFilename, xmlDocument, namespaceName);

                NameTable nameTable = new NameTable();
                XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(nameTable);
                // One quote per file
                string namespacePrefix = "SingleQuote";
                string xPath = "//" + namespacePrefix + ":" + _quoteSubTreeName;
                xmlNamespaceManager.AddNamespace(namespacePrefix, namespaceName);
                XmlNodeList xmlBrokerDataNodeList = xmlDocument.DocumentElement.SelectNodes(xPath, xmlNamespaceManager);

                if (xmlBrokerDataNodeList.Count == 0)
                {
                    Console.WriteLine("SplitXmlDocument : No quotes found in \"{0}\"", xmlFilename);
                }
                else
                {
                    int quoteCount = 0;
                    for (int brokerDataIndex = 0; brokerDataIndex < xmlBrokerDataNodeList.Count; ++brokerDataIndex)
                    {
                        quoteCount += 1;
                        XmlDocument quoteXmlDocument = new XmlDocument();
                        try
                        {
                            var quoteNode = quoteXmlDocument.ImportNode(xmlBrokerDataNodeList[brokerDataIndex], true);
                            quoteXmlDocument.AppendChild(quoteNode);

                            try
                            {
                                string quoteRefXPath = "//" + namespacePrefix + ":" + _quoteRefName;
                                XmlNode quoteRefNode = quoteXmlDocument.SelectSingleNode(quoteRefXPath, xmlNamespaceManager);
                                if (quoteRefNode == null)
                                {
                                    Console.WriteLine("Unable to determine the QuoteRef for Quote Count {0}", quoteCount);
                                }
                                else
                                {
                                    WriteSingleXmlQuoteDocument(xmlFilename, quoteXmlDocument, namespaceName, quoteCount);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Exception determining QuoteRef for Quote Count {0} = {1}",
                                                quoteCount, ex.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("SplitXmlDocumentUsingDOM : Exception importing node for Quote Count {0} = {1}",
                                                quoteCount, ex.ToString());
                        }
                    }
                }
            } // Document loaded successfully

        } // SplitXmlDocumentUsingDOM
    }
}
