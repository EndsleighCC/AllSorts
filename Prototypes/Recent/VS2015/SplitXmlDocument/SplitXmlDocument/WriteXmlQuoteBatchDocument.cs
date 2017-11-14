using System;
using System.Collections.Generic;
using System.Xml;

namespace SplitXmlDocument
{
    partial class Program
    {

        static void WriteXmlQuoteBatchDocument(string xmlFullDocumentFilename, XmlDocument xmlQuoteDocument, string namespaceName, int batchNumber)
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
                else
                {
                    // All quotes in the XML document should be written to the file

                    string batchNumberText = batchNumber.ToString("D3");

                    // Work out the filename based on the supplied batch number
                    int lastDotPos = xmlFullDocumentFilename.LastIndexOf(".");
                    string quoteDocumentFilename = null;
                    if (lastDotPos < 0)
                    {
                        // No last dot
                        quoteDocumentFilename = xmlFullDocumentFilename + "." + batchNumberText + ".xml";
                    }
                    else
                    {
                        // Last dot found
                        quoteDocumentFilename = xmlFullDocumentFilename.Substring(0, lastDotPos) + "." + batchNumberText + ".xml";
                    }

                    try
                    {
                        Console.WriteLine();
                        Console.WriteLine("WriteXmlQuoteDocument({0}) : Writing {1} quotes to \"{2}\"",
                                            batchNumberText,
                                            xmlQuoteRefList.Count.ToString("#,##0"),
                                            quoteDocumentFilename);
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
                        Console.WriteLine("XmlWriter exception {0}", ex.ToString());
                        throw;
                    }

                    //Console.WriteLine("WriteXmlQuoteDocument {0} Begin", quoteReference);
                    //DisplayXmlDocument(xmlQuoteDocument);
                    //Console.WriteLine("WriteXmlQuoteDocument {0} End", quoteReference);

                } // All quotes in the XML document should be written to the file
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception writing XML file \"{0}\" = {1}",
                    xmlFullDocumentFilename, ex.ToString());
                throw new ApplicationFailureException(
                    String.Format("Application failed to write XML file for Batch Number {0}", batchNumber));
            }

        } // WriteXmlQuoteBatchDocument
    }
}
