using System;
using System.IO;
using System.Xml;

namespace SplitXmlDocument
{
    partial class Program
    {

        static void SplitXmlDocumentUsingXmlReader(string xmlFilename)
        {
            using (StreamReader fileStream = new StreamReader(xmlFilename))
            {
                XmlReaderSettings xmlSettings = new XmlReaderSettings();
                xmlSettings.ConformanceLevel = ConformanceLevel.Fragment;
                xmlSettings.DtdProcessing = DtdProcessing.Parse;
                xmlSettings.IgnoreWhitespace = true;
                xmlSettings.IgnoreComments = false;
                NameTable nameTableReader = new NameTable();
                nameTableReader.Add(_defaultNamespace);
                xmlSettings.NameTable = nameTableReader;

                using (XmlReader xmlReader = XmlReader.Create(fileStream, xmlSettings))
                {
                    XmlDocument xmlDocument = new XmlDocument();

                    try
                    {
                        xmlReader.MoveToContent();
                        // xmlReader.MoveToElement();
                        xmlReader.Read();
                        xmlDocument.Load(xmlReader);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("SplitXmlDocumentUsingXmlReader({0}) : Exception \"{1}\" = {2}",
                                            xmlFilename, ex.Message, ex.ToString());
                    }
                } // using XmlReader

            } // using StreamReader

        } // SplitXmlDocumentUsingXmlReader

    }
}
