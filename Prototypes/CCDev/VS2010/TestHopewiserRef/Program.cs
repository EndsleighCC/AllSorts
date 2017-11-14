using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestHopewiserRef
{
    class Program
    {
        static void Main(string[] args)
        {
            Type typeHopewiserApplication = Type.GetTypeFromProgID("HPW.RPlus3Svr");

            if (typeHopewiserApplication == null)
            {
                Console.WriteLine("The Hopewiser Application is not Registered on this machine");
            }
            else
            {
                System.Object typeHopewiserObject = Activator.CreateInstance(typeHopewiserApplication);
                    // CoCreateInstance
                RPlus3Svr.IServer hopewiserApplication = (RPlus3Svr.IServer) typeHopewiserObject; // QueryInterface
            }

        }
    }
}
