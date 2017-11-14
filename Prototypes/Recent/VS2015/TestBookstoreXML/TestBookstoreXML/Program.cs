using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TestBookstoreXML
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                string xmlFilename = Path.GetFullPath(args[0]);
                if (!File.Exists(xmlFilename))
                {
                    Console.WriteLine("XML File \"{0}\" does not exist", xmlFilename);
                }
                else
                {
                    BookstoreContainer bookstoreContainer = new BookstoreContainer(xmlFilename);

                    Console.WriteLine("Bookstore loaded");

                    Console.WriteLine("Count of books is {0}", bookstoreContainer.bookstoreObject.Items.Count());
                    foreach ( bookstoreBook bookstorebookObject in bookstoreContainer.bookstoreObject.Items)
                    {
                        // This is an array because there could be more than one language
                        for (int languageIndex=0; languageIndex< bookstorebookObject.title.Length; ++languageIndex)
                        {
                            Console.WriteLine("    Title {0} = \"{1}\"", languageIndex, bookstorebookObject.title[languageIndex].Value);
                            Console.WriteLine("    Language {0} = \"{1}\"", languageIndex, bookstorebookObject.title[languageIndex].lang);
                        }
                        Console.WriteLine("    Price = \"{0}\"", bookstorebookObject.price);
                    }
                }
            }
        }
    }
}
