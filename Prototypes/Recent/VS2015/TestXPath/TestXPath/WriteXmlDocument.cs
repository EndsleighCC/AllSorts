using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestXPath
{
    partial class Program
    {
        private static void WriteXmlDocument(string xmlFullDocumentFilename, XmlDocument xmlDocument, string namespaceName, string extraName)
        {
            try
            {
                int lastDotPos = xmlFullDocumentFilename.LastIndexOf(".");
                string newDocumentFilename = null;
                if (lastDotPos < 0)
                {
                    // No last dot
                    newDocumentFilename = xmlFullDocumentFilename + "." + extraName + ".xml";
                }
                else
                {
                    // Last dot found
                    newDocumentFilename = xmlFullDocumentFilename.Substring(0, lastDotPos) + "." + extraName + ".xml";
                }

                try
                {
                    Console.WriteLine("WriteXmlDocument : Writing \"{0}\"", newDocumentFilename);
                    XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                    xmlWriterSettings.Indent = true;
                    using (XmlWriter xmlWriter = XmlWriter.Create(newDocumentFilename, xmlWriterSettings))
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
                        xmlDocument.WriteContentTo(xmlWriter);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Write exception {0}", ex.ToString());
                }

                //Console.WriteLine("WriteXmlQuoteDocument {0} Begin", quoteReference);
                //DisplayXmlDocument(xmlQuoteDocument);
                //Console.WriteLine("WriteXmlQuoteDocument {0} End", quoteReference);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception writing {0}", ex.ToString());
            }

        } // WriteXmlDocument
    }
}
