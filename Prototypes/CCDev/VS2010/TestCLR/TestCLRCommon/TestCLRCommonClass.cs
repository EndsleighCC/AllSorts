using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestCLRCommon
{
    public class TestCLRCommonClass
    {

        public TestCLRCommonClass( int value )
        {
            IntegerValue = value;
        }

        public int IntegerValue { get; set; }
    }

    public class TestCLRCommonInput
    {
        public TestCLRCommonInput( int intProperty , string stringProperty )
        {
            IntProperty = intProperty;
            StringProperty = stringProperty;
        }

        public int IntProperty { set; get; }
        public string StringProperty { set; get; }
    }

    public class TestCLRCommonOutput
    {
        public TestCLRCommonOutput()
        {
        }

        public double Result { get; set; }

        public List<string> DescriptionData = new List<string>();
    }

}
