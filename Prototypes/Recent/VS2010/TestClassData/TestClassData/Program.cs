using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestClassData
{
    public class SomeOutputData
    {
        public SomeOutputData(int dataItemAfirstParameter, int dataItemB, int dataItemC)
        {
            DataItemA = dataItemAfirstParameter;
            DataItemB = dataItemB;
            DataItemC = dataItemC;
        }

        public void Display()
        {
            Console.WriteLine( "DataItemA = {0}, DataItemB = {1}, DataItemC = {2}",DataItemA,DataItemB,DataItemC);
        }

        int DataItemA { get; set; }
        int DataItemB { get; set; }
        int DataItemC { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            SomeOutputData someOutput = new SomeOutputData(dataItemAfirstParameter: 1, dataItemC: 3, dataItemB: 2);
            someOutput.Display();
        }
    }
}
