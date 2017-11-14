using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace TestReflection
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        public class TestCrappyAttribute : Attribute
        {
            public TestCrappyAttribute(TestCrappyEnum crappyEnum)
            {
                CrappyEnum = crappyEnum;
            }

            public TestCrappyEnum CrappyEnum { get; set; }

            public enum TestCrappyEnum
            {
                String,
                Number
            }
        }

        public class SearchableAttribute : Attribute
        {
            public SearchableAttribute()
            {
            }
        }

        public class TestReflectionClass
        {
            public TestReflectionClass() { }

            [TestCrappyAttribute(TestCrappyAttribute.TestCrappyEnum.String),Searchable]
            public string PropertyString { get; set; }

            [TestCrappyAttribute(TestCrappyAttribute.TestCrappyEnum.Number)]
            public int PropertyInt { get; set; }

            public int AnyOldMethod(int value)
            {
                return value;
            }
        }

        public void Display(int indent, string format, params object[] param)
        {
            txtDisplay.Text += new string(' ', indent * 4);
            txtDisplay.Text += String.Format( format, param);
            txtDisplay.Text += Environment.NewLine;
        }

        public void DisplayCustomAttributes(int indent, MemberInfo mi)
        {
            // Get the set of custom attributes; if none exist, just return.
            object[] attributes = mi.GetCustomAttributes(false);
            Display(indent, "Number of Attributes = {0}", attributes.Length);
            if (attributes.Length > 0)
            {

                // Display the custom attributes applied to this member.
                Display(indent + 1, "Attributes:");
                foreach (object o in attributes)
                {
                    Display(indent + 2, "Attribute={0}", o.GetType().Name);
                    if (o.GetType() == typeof(TestCrappyAttribute))
                    {
                        TestCrappyAttribute testCrappyAttribute = (TestCrappyAttribute)o;
                        Display(indent + 3, "Value of Attribute {0} = {1}", o.GetType().Name, testCrappyAttribute.CrappyEnum);
                    }
                    else if ( o.GetType() == typeof(SearchableAttribute) )
                    {
                        Display(indent + 3, "Member {0} is Searchable", mi.Name);
                    }
                }

            }
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            int indent = 0;
            Assembly a = System.Reflection.Assembly.GetExecutingAssembly();
            Display(indent, "Assembly identity={0}", a.FullName);
            Display(indent + 1, "Codebase={0}", a.CodeBase);

            TestReflectionClass testReflectionObject = new TestReflectionClass();

            testReflectionObject.PropertyInt = 4;
            testReflectionObject.PropertyString = "crap";

            Type typeTestReflectionType = testReflectionObject.GetType() ;
            Module moduleTestReflectionObject = typeTestReflectionType.Module;

            Display( 0 , "Module Name is \"{0}\"" , moduleTestReflectionObject.Name ) ;
            Display(0, "Class Name is \"{0}\"", typeTestReflectionType.Name);

            indent = 1;
            foreach ( MemberInfo mi in typeTestReflectionType.GetMembers() )
            {
                Display(indent, "Member \"{0}\" of Type {1}", mi.Name,mi.GetType().Name);
                DisplayCustomAttributes(indent, mi);

                // If the member is a method, display information about its parameters.
                if (mi.MemberType == MemberTypes.Method)
                {
                    foreach (ParameterInfo pi in ((MethodInfo)mi).GetParameters())
                    {
                        Display(indent + 1, "Parameter: Type={0}, Name={1}", pi.ParameterType, pi.Name);
                    }
                }

                // If the member is a property, display information about the property's accessor methods.
                if (mi.MemberType == MemberTypes.Property)
                {
                    PropertyInfo propertyInfo = ((PropertyInfo)mi);
                    if (propertyInfo.CanRead)
                    {
                        Object propertyObject = propertyInfo.GetValue(testReflectionObject, null);
                        string propertyValue = propertyObject.ToString();
                        Display(indent, "Member \"{0}\" of Type {1} is a Property with value \"{2}\"", mi.Name, mi.GetType().Name, propertyValue);
                    }
                    else
                    {
                        Display(indent, "Member \"{0}\" of Type {1} is a Property which cannot be read", mi.Name, mi.GetType().Name);
                    }

                    foreach (MethodInfo am in ((PropertyInfo)mi).GetAccessors())
                    {
                        Display(indent + 1, "Accessor method: {0} of Type {1}", am.Name , am.ReturnType.Name);
                        // Display( indent , "Return Type: {0}" , mi.
                    }
                }
 
            }

        }
    }
}
