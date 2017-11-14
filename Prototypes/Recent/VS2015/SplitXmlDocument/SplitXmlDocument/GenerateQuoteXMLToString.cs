using System;
using System.Xml;
using System.Text;

namespace SplitXmlDocument
{
    partial class Program
    {
        //public static string XmlEscape(string unescaped)
        //{
        //    XmlDocument doc = new XmlDocument();
        //    XmlNode node = doc.CreateElement("root");
        //    node.InnerText = unescaped;
        //    return node.InnerXml;
        //}

        //public static string XmlUnescape(string escaped)
        //{
        //    XmlDocument doc = new XmlDocument();
        //    XmlNode node = doc.CreateElement("root");
        //    node.InnerXml = escaped;
        //    return node.InnerText;
        //}

        private static string XmlEscape(string unescaped)
        {
            return System.Security.SecurityElement.Escape(unescaped);
        }

        private static string GenerateQuoteXMLToString(XmlReader xmlReader, int quoteCount, ref int indentingLevel)
        {
            StringBuilder sbQuote = new StringBuilder();

            bool finishedQuote = false;

            sbQuote.Append(XmlLine(String.Format("{0}<!-- Quote {1} begin -->", Indenting(indentingLevel), quoteCount)));

            if (!xmlReader.HasAttributes)
            {
                sbQuote.Append(XmlLine(String.Format("{0}<{1}>", Indenting(indentingLevel), xmlReader.Name)));
            }
            else
            {
                sbQuote.Append(XmlLine(String.Format("{0}<{1} {2} >",
                    Indenting(indentingLevel), xmlReader.Name, GetXmlAttributeString(xmlReader))));
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
                            if ( ! xmlReader.IsEmptyElement )
                            {
                                sbQuote.Append(XmlLine(String.Format("{0}<{1}>", Indenting(indentingLevel), xmlReader.Name)));
                            }
                            else
                            {
                                sbQuote.Append(XmlLine(String.Format("{0}<{1} />", Indenting(indentingLevel), xmlReader.Name)));
                            }
                        }
                        else
                        {
                            if (!xmlReader.IsEmptyElement)
                            {
                                sbQuote.Append(XmlLine(String.Format("{0}<{1} {2} >",
                                Indenting(indentingLevel), xmlReader.Name, GetXmlAttributeString(xmlReader))));
                            }
                            else
                            {
                                sbQuote.Append(XmlLine(String.Format("{0}<{1} {2} />",
                                Indenting(indentingLevel), xmlReader.Name, GetXmlAttributeString(xmlReader))));
                            }
                        }
                        break;
                    case XmlNodeType.Text:
                        sbQuote.Append(XmlLine(String.Format("{0}{1}", Indenting(indentingLevel + 1), XmlEscape( xmlReader.Value))));
                        break;
                    case XmlNodeType.CDATA:
                        sbQuote.Append(XmlLine(String.Format("<![CDATA[{0}]]>", xmlReader.Value)));
                        break;
                    case XmlNodeType.ProcessingInstruction:
                        sbQuote.Append(XmlLine(String.Format("<?{0} {1}?>", xmlReader.Name, xmlReader.Value)));
                        break;
                    case XmlNodeType.Comment:
                        sbQuote.Append(XmlLine(String.Format("<!--{0}-->", xmlReader.Value)));
                        break;
                    case XmlNodeType.XmlDeclaration:
                        // quoteXml += XmlLine( String.Format("<?xml version='1.0'?>") );
                        sbQuote.Append(XmlLine(String.Format("<?xml {0}?>", xmlReader.Value)));
                        break;
                    case XmlNodeType.Document:
                        break;
                    case XmlNodeType.DocumentType:
                        sbQuote.Append(XmlLine(String.Format("<!DOCTYPE {0} [{1}]", xmlReader.Name, xmlReader.Value)));
                        break;
                    case XmlNodeType.EntityReference:
                        sbQuote.Append(XmlLine(String.Format(xmlReader.Name)));
                        break;
                    case XmlNodeType.EndElement:
                        sbQuote.Append(XmlLine(String.Format("{0}</{1}>", Indenting(indentingLevel), xmlReader.Name)));
                        if (String.Compare(xmlReader.Name, _quoteSubTreeName) == 0)
                        {
                            // Output message before the indenting level changes
                            sbQuote.Append(XmlLine(String.Format("{0}<!-- Quote {1} end -->", Indenting(indentingLevel), quoteCount)));
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
                        sbQuote.Append(XmlLine(String.Format("<!-- Unexpected Node Type \"{0}\" -->", xmlReader.NodeType)));
                        break;
                } // switch (xmlReader.NodeType)

            } // while reading

            string quoteXml = sbQuote.ToString();

            return quoteXml;
        } // GenerateQuoteXMLToString


        private static string GenerateQuoteXMLToStringBuilder(XmlReader xmlReader, int quoteCount, ref int indentingLevel)
        {
            StringBuilder sbQuote = new StringBuilder();

            bool finishedQuote = false;

            sbQuote.Append(XmlLine(String.Format("{0}<!-- Quote {1} begin -->", Indenting(indentingLevel), quoteCount)));

            if (!xmlReader.HasAttributes)
            {
                sbQuote.Append(XmlLine(String.Format("{0}<{1}>", Indenting(indentingLevel), xmlReader.Name)));
            }
            else
            {
                sbQuote.Append(XmlLine(String.Format("{0}<{1} {2} >",
                    Indenting(indentingLevel), xmlReader.Name, GetXmlAttributeString(xmlReader))));
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
                                sbQuote.Append(XmlLine(String.Format("{0}<{1}>", Indenting(indentingLevel), xmlReader.Name)));
                            }
                            else
                            {
                                sbQuote.Append(XmlLine(String.Format("{0}<{1} />", Indenting(indentingLevel), xmlReader.Name)));
                            }
                        }
                        else
                        {
                            if (!xmlReader.IsEmptyElement)
                            {
                                sbQuote.Append(XmlLine(String.Format("{0}<{1} {2} >",
                                Indenting(indentingLevel), xmlReader.Name, GetXmlAttributeString(xmlReader))));
                            }
                            else
                            {
                                sbQuote.Append(XmlLine(String.Format("{0}<{1} {2} />",
                                Indenting(indentingLevel), xmlReader.Name, GetXmlAttributeString(xmlReader))));
                            }
                        }
                        break;
                    case XmlNodeType.Text:
                        sbQuote.Append(XmlLine(String.Format("{0}{1}", Indenting(indentingLevel + 1), XmlEscape(xmlReader.Value))));
                        break;
                    case XmlNodeType.CDATA:
                        sbQuote.Append(XmlLine(String.Format("<![CDATA[{0}]]>", xmlReader.Value)));
                        break;
                    case XmlNodeType.ProcessingInstruction:
                        sbQuote.Append(XmlLine(String.Format("<?{0} {1}?>", xmlReader.Name, xmlReader.Value)));
                        break;
                    case XmlNodeType.Comment:
                        sbQuote.Append(XmlLine(String.Format("<!--{0}-->", xmlReader.Value)));
                        break;
                    case XmlNodeType.XmlDeclaration:
                        // quoteXml += XmlLine( String.Format("<?xml version='1.0'?>") );
                        sbQuote.Append(XmlLine(String.Format("<?xml {0}?>", xmlReader.Value)));
                        break;
                    case XmlNodeType.Document:
                        break;
                    case XmlNodeType.DocumentType:
                        sbQuote.Append(XmlLine(String.Format("<!DOCTYPE {0} [{1}]", xmlReader.Name, xmlReader.Value)));
                        break;
                    case XmlNodeType.EntityReference:
                        sbQuote.Append(XmlLine(String.Format(xmlReader.Name)));
                        break;
                    case XmlNodeType.EndElement:
                        sbQuote.Append(XmlLine(String.Format("{0}</{1}>", Indenting(indentingLevel), xmlReader.Name)));
                        if (String.Compare(xmlReader.Name, _quoteSubTreeName) == 0)
                        {
                            // Output message before the indenting level changes
                            sbQuote.Append(XmlLine(String.Format("{0}<!-- Quote {1} end -->", Indenting(indentingLevel), quoteCount)));
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
                        sbQuote.Append(XmlLine(String.Format("<!-- Unexpected Node Type \"{0}\" -->", xmlReader.NodeType)));
                        break;
                } // switch (xmlReader.NodeType)

            } // while reading

            string quoteXml = sbQuote.ToString();

            return quoteXml;
        } // GenerateQuoteXMLToString

    }
}
