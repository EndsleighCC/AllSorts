using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBookstoreXML
{
    public class BookstoreContainer
    {
        public BookstoreContainer(string xmlFilename)
        {
            try
            {
                XmlSerializer bookstoreSerializer = new XmlSerializer(typeof(bookstore));
                FileStream bookstoreFileStream = new FileStream(xmlFilename, FileMode.Open);

                _bookstoreObject = (bookstore)bookstoreSerializer.Deserialize(bookstoreFileStream);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception deserialising \"{0}\" = {1}", xmlFilename, ex.ToString());
            }
        }

        public bookstore bookstoreObject { get { return _bookstoreObject; } }

        private bookstore _bookstoreObject = null;
    }
}
