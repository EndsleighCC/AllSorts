using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TestInheritance
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private class A : BinaryReader
        {
            public A() : base(File.OpenRead(@"c:\st00\tmstables.txt")) { }

            public int P { get { return Avalue; } }

            private const int Avalue = -1;
        }

        private class B : A
        {
            public B()
                : base()
            {
            }
        }

        private class C : B
        {
            public C()
                : base()
            {
            }
        }

        private void Process(BinaryReader binaryReader)
        {
            char[] chars = binaryReader.ReadChars(40);
        }

        interface IDoSomethingCommon
        {
            void DoSomething();
        }

        public class SomethingA : IDoSomethingCommon
        {
            public void DoSomething()
            {
                Debug.WriteLine(String.Format("SomethingA : Doing Something"));
            }
        }

        public class SomethingB : IDoSomethingCommon
        {
            public void DoSomething()
            {
                Debug.WriteLine(String.Format("SomethingB : Doing Something"));
            }
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            List<IDoSomethingCommon> doSomethingCommonList = new List<IDoSomethingCommon>();

            doSomethingCommonList.Add( new SomethingA() );
            doSomethingCommonList.Add( new SomethingB() );

            doSomethingCommonList[0].DoSomething();
            doSomethingCommonList[1].DoSomething();

            C c = new C();

            int value = c.P;

            Process(c);

            B b = new B();

            BinaryReader binaryReader2 = b;
        }
    }
}
