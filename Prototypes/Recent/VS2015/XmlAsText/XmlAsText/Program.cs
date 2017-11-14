using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace XmlAsText
{
    class Program
    {
        const int _indentLength = 4;
        const string _textQualifier = "#text";

        static string Indent( int depth)
        {
            return new string(' ', depth * _indentLength);
        }

        static void ShowAttributes(XmlNode xmlNode , int depth )
        {
            if (xmlNode.Attributes != null )
            {
                foreach (XmlAttribute xmlAttribute in xmlNode.Attributes)
                {
                    Console.WriteLine("{0}Attribute \"{1}\" = \"{2}\"", Indent(depth+2), xmlAttribute.Name, (xmlAttribute.Value == null ? "(unset)" : xmlAttribute.Value));
                }
            }

        } // ShowAttributes

        static void ShowNode( XmlNode xmlNodeBase , int depth )
        {
            Console.WriteLine("{0}<{1}>", Indent(depth), xmlNodeBase.Name);
            if ( xmlNodeBase.Value != null )
            {
                // Console.WriteLine("{0}\"{1}\" = \"{2}\"", Indent(depth), xmlNodeBase.Name, (xmlNodeBase.Value == null ? "(unset)" : xmlNodeBase.Value));
                Console.WriteLine("{0}\"{1}\"", Indent(depth+1), (xmlNodeBase.Value == null ? "(unset)" : xmlNodeBase.Value));
            }
            else
            {
                string value = null;
                if ( xmlNodeBase.HasChildNodes)
                {
                    // Find the value of this node in amongst the children
                    for (int nodeIndex = 0; (value == null) && (nodeIndex < xmlNodeBase.ChildNodes.Count); ++nodeIndex)
                    {
                        if (xmlNodeBase.ChildNodes[nodeIndex].Name == _textQualifier)
                        {
                            value = xmlNodeBase.ChildNodes[nodeIndex].Value;
                        }
                    }
                }
                // Console.WriteLine("{0}\"{1}\" = \"{2}\"", Indent(depth), xmlNodeBase.Name, (value == null ? "(unset)" : value));
                Console.WriteLine("{0}\"{1}\"", Indent(depth+1), (value == null ? "(unset)" : value));
            }
            ShowAttributes(xmlNodeBase,depth);
            if ( xmlNodeBase.HasChildNodes)
            {
                foreach (XmlNode xmlNode in xmlNodeBase.ChildNodes)
                {
                    if ( xmlNode.Name != _textQualifier)
                    {
                        ShowNode(xmlNode, depth + 1);
                    }
                }
            }
            Console.WriteLine("{0}</{1}>", Indent(depth), xmlNodeBase.Name);
        }

        static void Main(string[] args)
        {
            if ( args.Length > 0 )
            {
                for ( int argIndex = 0; argIndex < args.Length; ++argIndex )
                {
                    string xmlFilename = Path.GetFullPath(args[argIndex]);

                    if ( ! File.Exists(xmlFilename))
                    {
                        Console.WriteLine("XML document \"{0}\" does not exist", xmlFilename);
                    }
                    else
                    {
                        try
                        {
                            System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
                            xmlDocument.Load(xmlFilename);
                            XmlNode root = xmlDocument.DocumentElement;
                            ShowNode(root,0);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error reading XML file \"{0}\" = {1}", xmlFilename, ex.ToString());
                        }
                    }
                }
            }
        }
    }
}
