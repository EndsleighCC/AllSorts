using System;
using System.Xml;

namespace SplitXmlDocument
{
    partial class Program
    {
        private static void AddVehicleNumberXmlAttribute(string xmlFilename, XmlDocument xmlDocument, string namespaceName)
        {
            // Find all the Quote nodes in the document (there is likely to be only one)
            XmlNodeList quoteNodeList = GetXmlNodeList(xmlFilename, xmlDocument, namespaceName, _quoteSubTreeName);
            if (quoteNodeList.Count == 0)
            {
                Console.WriteLine("AddVehicleNumberXmlAttribute : Unable to locate any quotes with XPath \"{0}\"", _quoteSubTreeName);
            }
            else
            {
                // Found the quotes

                // Find all the Vehicle nodes within each quote
                foreach (XmlNode xmlNodeQuote in quoteNodeList)
                {
                    XmlNodeList xmlNodeVehicles = GetXmlNodeList(xmlFilename, xmlNodeQuote, namespaceName, _quoteVehicleXPath);
                    if (xmlNodeVehicles.Count == 0)
                    {
                        string quoteRef = GetQuoteRef(xmlNodeQuote, namespaceName);
                        Console.WriteLine("AddVehicleNumberXmlAttribute({0}) : No \"Vehicle\" Nodes found with \"{1}\"",
                                            quoteRef, _quoteVehicleXPath);
                    }
                    else
                    {
                        if (xmlNodeVehicles.Count > 1)
                        {
                            string quoteRef = GetQuoteRef(xmlNodeQuote, namespaceName);
                            Console.WriteLine("AddVehicleNumberXmlAttribute({0}) : Count of vehicles is greater than one = {1}",
                                                quoteRef, xmlNodeVehicles.Count);
                        }
                        // Update each Vehicle node with a vehicle number attribute
                        for (int vehicleIndex = 0; vehicleIndex < xmlNodeVehicles.Count; ++vehicleIndex)
                        {
                            XmlNode xmlNodeVehicle = xmlNodeVehicles[vehicleIndex];
                            string vehicleNumber = (vehicleIndex + 1).ToString();
                            string currentVehicleNumber = GetXmlAttributeValue(xmlNodeVehicle, _quoteVehicleNumberAttributeName);
                            if (currentVehicleNumber == null)
                            {
                                AddXmlAttribute(xmlDocument, xmlNodeVehicle, _quoteVehicleNumberAttributeName, vehicleNumber);
                            }
                        } // for vehicle
                    } // Update each Vehicle node with a vehicle number attribute
                } // foreach quote
            } // Found the quotes
        } // AddVehicleNumberXmlAttribute
    }
}
