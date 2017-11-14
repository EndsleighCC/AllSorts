using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestAddressService.AddressWebService;

namespace TestAddressService
{
    class Program
    {
        static void LookupAddress(string postCode, string houseNameOrNumber)
        {

            LookupAddressRequest lookupAddressRequest = new LookupAddressRequest();

            if (houseNameOrNumber != null)
            {
                lookupAddressRequest.HouseNameOrNumber = houseNameOrNumber;
            }
            else
            {
                lookupAddressRequest.HouseNameOrNumber = "1";
            }
            lookupAddressRequest.Postcode = postCode;
            lookupAddressRequest.ExtensionData = null;

            LookupAddressResponse lookupAddressResponse = new LookupAddressResponse();

            try
            {
                ServiceClient serviceClient = new ServiceClient();

                lookupAddressResponse = serviceClient.LookupAddress(lookupAddressRequest);

                Console.WriteLine("{0} = {1}, {2}, {3}, {4}, {5}",
                                    postCode,
                                    lookupAddressResponse.Address.Line1,
                                    lookupAddressResponse.Address.Line2,
                                    lookupAddressResponse.Address.Line3,
                                    lookupAddressResponse.Address.Line4,
                                    lookupAddressResponse.Address.Postcode);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception calling serviceClient.LookupAddress = \"{0}\"", ex.ToString());
            }

        } // LookupAddress

        private class AddressDetail
        {
            public AddressDetail(string postCode, string houseNameOrNumber)
            {
                PostCode = postCode;
                HouseNameOrNumber = houseNameOrNumber;
            }
            public string PostCode { get; set; }
            public string HouseNameOrNumber { get; set; }
        }

        static void Main(string[] args)
        {
            switch (args.Count())
            {
                case 2 : // Fall through to ...
                case 3 :
                    string postCode = null ;
                    string houseNameOrNumber = null;
                    int postCodeItemCount = 0;

                    for (int argId = 0; argId < args.Count(); ++argId)
                    {
                        if (String.IsNullOrEmpty(postCode))
                        {
                            postCode += args[argId];
                            postCodeItemCount += 1;
                        }
                        else
                        {
                            if (postCodeItemCount <= 1)
                            {
                                postCode += " " + args[argId];
                                postCodeItemCount += 1;
                            }
                            else
                            {
                                houseNameOrNumber = args[argId];
                            }
                        }

                    } // for

                    LookupAddress(postCode,houseNameOrNumber);

                    break;
                default :
                    {
                        List<AddressDetail> addressDetailList = new List<AddressDetail>();

                        addressDetailList.Add(new AddressDetail("GL11 5LJ" , "22"));
                        addressDetailList.Add(new AddressDetail("GL51 4UE", "Endsleigh House"));
                        addressDetailList.Add(new AddressDetail("GL51 4UG", "Greenway Hotel"));
                        addressDetailList.Add(new AddressDetail("GL51 7AY", "Premier Inn"));
                        addressDetailList.Add(new AddressDetail("GL50 4FA", "Prezzo Cheltenham Brewery"));
                        addressDetailList.Add(new AddressDetail("GL53 8EA", "cheltenham Park Hotel"));
                        addressDetailList.Add(new AddressDetail("GL20 7DN", "Tewkesbury Park Hotel"));
                        addressDetailList.Add(new AddressDetail("GL52 2BD", "Cheltenham Townhouse Hotel"));
                        addressDetailList.Add(new AddressDetail("GL53 0JE", "Beaumont House Hotel"));

                        int iterationCount = 50;

                        for (int iterationId = 1; iterationId <= iterationCount; ++iterationId)
                        {
                            Console.WriteLine();
                            Console.WriteLine("Begin iteration {0} of {1}",iterationId,iterationCount);

                            Parallel.ForEach(addressDetailList,
                                (addressDetail) =>
                                    LookupAddress(addressDetail.PostCode, addressDetail.HouseNameOrNumber));
                        }
                    }
                    break;
            } // switch
        }
    }
}
