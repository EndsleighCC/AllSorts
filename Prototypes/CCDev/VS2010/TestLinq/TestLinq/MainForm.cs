using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestLinq
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        class SimpleClass
        {
            public SimpleClass(SelectionTypeEnum selectionType, string stringValue)
            {
                SelectionType = selectionType;
                StringValue = stringValue;
            }

            public enum SelectionTypeEnum
            {
                First,
                Second,
                Third ,
                Fourth
            }

            public SelectionTypeEnum SelectionType { get; set; }

            public string StringValue { get; set; }

        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            List<SimpleClass> simpleClassList = new List<SimpleClass>();

            simpleClassList.Add(new SimpleClass(SimpleClass.SelectionTypeEnum.First, "first"));
            simpleClassList.Add( new SimpleClass(SimpleClass.SelectionTypeEnum.Second,"second"));
            simpleClassList.Add( new SimpleClass(SimpleClass.SelectionTypeEnum.Third,"third"));
            
            SimpleClass simpleClass2 = simpleClassList.Find( sc => sc.SelectionType == SimpleClass.SelectionTypeEnum.Second );
            SimpleClass simpleClass4 = simpleClassList.Find( sc => sc.SelectionType == SimpleClass.SelectionTypeEnum.Fourth );

        }
    }
}
