using System;
using System.IO;
using System.Xml;

namespace SplitXmlDocument
{
    partial class Program
    {
        private static void GenerateQuoteXML(XmlReader xmlReader, int quoteCount, ref int indentingLevel)
        {
            bool finishedQuote = false;

            Console.WriteLine("{0}<!-- Quote {1} begin -->", Indenting(indentingLevel), quoteCount);

            if (!xmlReader.HasAttributes)
            {
                Console.WriteLine("{0}<{1}>", Indenting(indentingLevel), xmlReader.Name);
            }
            else
            {
                Console.WriteLine("{0}<{1} {2} >",
                    Indenting(indentingLevel), xmlReader.Name, GetXmlAttributeString(xmlReader));
            }

            while ((!finishedQuote) && (xmlReader.Read()))
            {
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (!xmlReader.IsEmptyElement)
                        {
                            indentingLevel += 1;
                            // Console.WriteLine("{0}Embedded Node {1} begin", Indenting(indentingLevel), indentingLevel);
                        }
                        else
                        {
                            // Console.WriteLine("{0}Embedded Empty Node {1}", Indenting(indentingLevel), indentingLevel);
                        }
                        if (!xmlReader.HasAttributes)
                        {
                            if (!xmlReader.IsEmptyElement)
                            {
                                Console.WriteLine("{0}<{1}>", Indenting(indentingLevel), xmlReader.Name);
                            }
                            else
                            {
                                Console.WriteLine("{0}<{1} />", Indenting(indentingLevel), xmlReader.Name);
                            }
                        }
                        else
                        {
                            if (!xmlReader.IsEmptyElement)
                            {
                                Console.WriteLine("{0}<{1} {2} >",
                                    Indenting(indentingLevel), xmlReader.Name, GetXmlAttributeString(xmlReader));
                            }
                            else
                            {
                                Console.WriteLine("{0}<{1} {2} />",
                                    Indenting(indentingLevel), xmlReader.Name, GetXmlAttributeString(xmlReader));
                            }
                        }
                        break;
                    case XmlNodeType.Text:
                        Console.WriteLine("{0}{1}", Indenting(indentingLevel + 1), xmlReader.Value);
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
                        // Console.WriteLine("<?xml version='1.0'?>");
                        Console.WriteLine("<?xml {0}?>", xmlReader.Value);
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
                        Console.WriteLine("{0}</{1}>", Indenting(indentingLevel), xmlReader.Name);
                        if (String.Compare(xmlReader.Name, _quoteSubTreeName) == 0)
                        {
                            // Output message before the indenting level changes
                            Console.WriteLine("{0}<!-- Quote {1} end -->", Indenting(indentingLevel), quoteCount);
                            indentingLevel -= 1;
                            finishedQuote = true;
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

        } // GenerateQuoteXML
    }
}
