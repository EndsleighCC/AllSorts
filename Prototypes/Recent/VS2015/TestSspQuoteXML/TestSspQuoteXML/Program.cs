using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSspQuoteXML
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
                    EISExtractsContainer eisExtractsContainer = new EISExtractsContainer(xmlFilename);

                    EISExtracts eisExtracts = eisExtractsContainer.eisExtractsObject;

                    Console.WriteLine("EISExtract QuoteRef is \"{0}\"", eisExtracts.Items[0].QuoteRef);
                    Console.WriteLine("EISExtract QuoteRef is \"{0}\"", eisExtracts.Items[0].Result[0][0].TotalPremium );
                }
            }
        } // Main
    }
}
