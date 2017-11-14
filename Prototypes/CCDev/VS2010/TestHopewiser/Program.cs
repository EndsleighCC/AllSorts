using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RPlus3Svr;

namespace TestHopewiser
{
    class TestHopewiser
    {
        static string UdprnOf(string postCode, string premise)
        {
            string udprn = null;

            try
            {
                Type typeHopewiserApplication = Type.GetTypeFromProgID("HPW.RPlus3Svr");

                if (typeHopewiserApplication == null)
                {
                    Console.WriteLine("The Hopewiser Application is not Registered on this machine");
                }
                else
                {
                    System.Object typeHopewiserObject = Activator.CreateInstance(typeHopewiserApplication); // CoCreateInstance
                    RPlus3Svr.IServer hopewiserApplication = (RPlus3Svr.IServer)typeHopewiserObject; // QueryInterface

                    string hopewiserDataSetPath = null;

                    string hopewiserDataSetPath32bit = @"C:\Program Files (x86)\Hopewiser\datasets\atlas3";
                    string hopewiserDataSetPath64bit = @"C:\Program Files\Hopewiser\datasets\atlas3";

                    if (Directory.Exists(hopewiserDataSetPath64bit))
                    {
                        Console.WriteLine("WARNING : A Hopewiser installation has been installed in a 64-bit path \"{0}\"",
                                            hopewiserDataSetPath64bit);
                        hopewiserDataSetPath = hopewiserDataSetPath64bit;
                    }
                    else
                    {
                        Console.WriteLine("Using Hopewiser 32-bit path \"{0}\"", hopewiserDataSetPath32bit);
                        hopewiserDataSetPath = hopewiserDataSetPath32bit;
                    }


                    hopewiserApplication.PAFPath = hopewiserDataSetPath;

                    if (hopewiserApplication.Open())
                    {
                        // Hopewiser database is open

                        hopewiserApplication.pcixPostcode = postCode;

                        if (premise != null)
                        {
                            hopewiserApplication.pcixPremise = premise;
                        }

                        hopewiserApplication.pcixPremise = "";

                        try
                        {
                            hopewiserApplication.AH21PCIXReadFirst();
                            string readStatus = null;
                            while ((readStatus = hopewiserApplication.pcixStatus) != RPlus3Svr.Constants.pcixEnd)
                            {
                                if (readStatus != RPlus3Svr.Constants.pcixHeartbeat)
                                {
                                    udprn = "UDPRN=" + hopewiserApplication.moaUdprn
                                            + "," + hopewiserApplication.moaStreet1
                                            + "," + hopewiserApplication.moaStreet2
                                            + "," + hopewiserApplication.moaTown
                                            + "," + hopewiserApplication.moaCounty
                                            + "," + hopewiserApplication.moaCountry
                                            + "," + hopewiserApplication.moaPremiseIdLine;
                                }

                                hopewiserApplication.AH21PCIXReadNext();
                            } // while
                        }
                        catch (Exception eek)
                        {
                            Console.WriteLine("Exception = {0}", eek.ToString());
                        }

                        hopewiserApplication.Close();

                    } // Hopewiser database is open
                    else
                    {
                        Console.WriteLine("Hopewiser failed to open from \"{0}\"", hopewiserApplication.PAFPath);
                    }

                }
            }
            catch (Exception eek)
            {
                Console.WriteLine("Exception \"{0}\"", eek.ToString());
            }
            return udprn;

        } // UdprnOf

        static void ShowUdprnOf(string postCode,string premise)
        {
            string udprn = UdprnOf(postCode, premise);
            if (udprn == null)
            {
                Console.WriteLine("Hopewiser lookup failed");
            }
            else
            {
                Console.WriteLine("UDPRN of Postcode \"{0}\" is \"{1}\"", postCode, udprn);
            }
        }

        static void Main(string[] args)
        {
            ShowUdprnOf("AB4 1AB",null);
            ShowUdprnOf("GL11 5LJ",null);
            ShowUdprnOf("GL51 4UE","James 1 House");
            ShowUdprnOf("GL50 4SU",null);
            ShowUdprnOf("GL53 8RA",null);
            ShowUdprnOf("WS12 3YG",null);
            ShowUdprnOf("WA16 8QF",null);
        }
    }
}
