using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSspQuoteXML
{
    class EISExtractsContainer
    {
        public EISExtractsContainer(string xmlFilename)
        {
            try
            {
                XmlSerializer eisExtractsSerializer = new XmlSerializer(typeof(EISExtracts));

                try
                {
                    FileStream eisExtractsFileStream = new FileStream(xmlFilename, FileMode.Open);
                    _eisExtractsObject = (EISExtracts)eisExtractsSerializer.Deserialize(eisExtractsFileStream);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception deserialising \"{0}\" = \"{1}\" = {2}",
                                        xmlFilename,
                                        ex.Message ?? "(no message)",
                                        ex.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception constructing XmlSerializer \"{0}\" = \"{1}\" = {2}",
                                    xmlFilename,
                                    ex.Message??"(no message)",
                                    ex.ToString());
            }
        }

        public EISExtracts eisExtractsObject { get { return _eisExtractsObject; } }

        private EISExtracts _eisExtractsObject = null;
    }

}
