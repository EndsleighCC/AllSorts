using System;
using System.Collections.Generic;
using System.Xml;

namespace SplitXmlDocument
{
    partial class Program
    {
        private static void WriteSingleXmlQuoteDocument(string xmlFullDocumentFilename, XmlDocument xmlQuoteDocument, string namespaceName, int quoteCount)
        {
            try
            {
                List<string> xmlQuoteRefList = GetQuoteRefList(xmlFullDocumentFilename, xmlQuoteDocument, namespaceName);
                if (xmlQuoteRefList.Count == 0)
                {
                    Console.WriteLine("Document \"{0}\" contains no quotes for XML Namespace \"{1}\"",
                                        xmlFullDocumentFilename, namespaceName);
                    throw new NoQuotesDetected(xmlFullDocumentFilename);
                }
                else if (xmlQuoteRefList.Count > 1)
                {
                    Console.WriteLine("The supplied sub-document of \"{0}\" for XML Namespace \"{1}\" contains more than one quote",
                                        xmlFullDocumentFilename, namespaceName);
                    throw new TooManyQuotes(xmlFullDocumentFilename);
                }
                else
                {
                    // Exactly one quote to output

                    string quoteReference = null;

                    int nodeCount = 0;
                    foreach (string quoteRef in xmlQuoteRefList)
                    {
                        nodeCount += 1;
                        //Console.WriteLine("WriteSingleXmlQuoteDocument({0}) : Quote Reference \"{1}\"",
                        //            xmlFullDocumentFilename,
                        //            xmlNodeInner.InnerText);
                        quoteReference = xmlQuoteRefList[nodeCount - 1];
                    }

                    // Work out the filename based on the quote reference
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
                        Console.WriteLine("WriteSingleXmlQuoteDocument({0}) : Writing \"{1}\"", quoteCount, quoteDocumentFilename);
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

        } // WriteSingleXmlQuoteDocument
    }
}
