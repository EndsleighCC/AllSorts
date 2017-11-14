using System;
using System.Xml;

namespace SplitXmlDocument
{
    public partial class Program
    {
        private static void AddDriverNumberXmlAttribute(string xmlFilename, XmlDocument xmlDocument, string namespaceName)
        {
            // Find all the Quote nodes in the document (there is likely to be only one)
            XmlNodeList quoteNodeList = GetXmlNodeList(xmlFilename, xmlDocument, namespaceName, _quoteSubTreeName);
            if ( quoteNodeList.Count==0)
            {
                Console.WriteLine("AddDriverNumberXmlAttribute : Unable to locate any quotes with XPath \"{0}\"", _quoteSubTreeName);
            }
            else
            {
                // Found the quotes

                // Find all the Driver nodes within each quote
                foreach (XmlNode xmlNodeQuote in quoteNodeList)
                {
                    XmlNodeList xmlNodeDrivers = GetXmlNodeList(xmlFilename, xmlNodeQuote, namespaceName, _quoteDriverXPath);
                    if (xmlNodeDrivers.Count==0)
                    {
                        string quoteRef = GetQuoteRef(xmlNodeQuote, namespaceName);
                        Console.WriteLine("AddDriverNumberXmlAttribute({0}) : No \"Driver\" Nodes found with \"{1}\"",
                                            quoteRef, _quoteDriverXPath);
                    }
                    else
                    {
                        // Update each Driver node with a driver number attribute
                        for (int driverIndex=0; driverIndex<xmlNodeDrivers.Count;++driverIndex)
                        {
                            XmlNode xmlNodeDriver = xmlNodeDrivers[driverIndex];
                            string driverNumber = (driverIndex+1).ToString();
                            string currentDriverNumber = GetXmlAttributeValue(xmlNodeDriver, _quoteDriverNumberAttributeName);
                            if (currentDriverNumber==null)
                            {
                                AddXmlAttribute(xmlDocument, xmlNodeDriver, _quoteDriverNumberAttributeName, driverNumber);
                            }
                            if (driverIndex==0)
                            {
                                // Check that Driver Number 1 is actually the Proposer
                                string relationshipToProposer = null;
                                XmlNode xmlNodeRelationshipToProposer = GetSingleXmlNode(xmlNodeDriver, namespaceName, _quoteDriverRelationshipToProposerDescriptionName);
                                if (xmlNodeRelationshipToProposer == null)
                                {
                                    string quoteRef = GetQuoteRef(xmlNodeQuote, namespaceName);
                                    Console.WriteLine("AddDriverNumberXmlAttribute({0}) : Unable to find Driver Sub-Node \"{1}\"",
                                        quoteRef ,
                                        _quoteDriverRelationshipToProposerDescriptionName);
                                }
                                else
                                {
                                    relationshipToProposer = GetNodeValue(xmlNodeRelationshipToProposer);
                                }
                                if (relationshipToProposer==null)
                                {
                                    string quoteRef = GetQuoteRef(xmlNodeQuote,namespaceName);
                                    Console.WriteLine("AddDriverNumberXmlAttribute({0}) : No data for \"{1}\"",
                                                        quoteRef, _quoteDriverRelationshipToProposerDescriptionName);
                                }
                                else
                                {
                                    if ( String.Compare(relationshipToProposer, _quoteDriverRelationshipToProposerDescriptionProposerValue)!=0)
                                    {
                                        string quoteRef = GetQuoteRef(xmlNodeQuote, namespaceName);
                                        Console.WriteLine("AddDriverNumberXmlAttribute({0}) : Driver 1 relationship \"{1}\" is not \"{2}\"",
                                                            quoteRef,
                                                            relationshipToProposer ,
                                                            _quoteDriverRelationshipToProposerDescriptionName);
                                    }
                                }

                            } // Check that Driver Number 1 is actually the Proposer
                        } // for driver
                    } // Update each Driver node with a driver number attribute
                } // foreach quote
            } // Found the quotes
        } // AddDriverNumberXmlAttribute
    }
}
