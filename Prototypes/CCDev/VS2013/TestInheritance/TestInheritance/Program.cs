using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestInheritance
{
    class Program
    {

        public abstract class BaseClass
        {
            public BaseClass()
            {
                Console.WriteLine("Base Class value is {0}",_baseClassValue);
                _baseClassValue += 1;
                Console.WriteLine("Base Class value is {0}", _baseClassValue);
            }

            public abstract int _baseClassValue
            {
                get; set;
            }
        }

        public class DerivedClass : BaseClass
        {
            public DerivedClass() : base()
            {
                Console.WriteLine("Derived Class value is {0}", _baseClassValue);
            }

            public override int _baseClassValue { get; set; }
        }

        static void Main(string[] args)
        {
            DerivedClass d = new DerivedClass();
        }
    }
}
