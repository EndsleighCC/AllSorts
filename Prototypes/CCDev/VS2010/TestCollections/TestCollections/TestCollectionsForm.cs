
// #define _required

using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace TestCollections
{
    public partial class TestCollectionsForm : Form
    {
        public TestCollectionsForm()
        {
            InitializeComponent();

            _testSortedDictionary = new TestSortedDictionary();
        }

#if _required
        public class TableDataItem
        {
            public string Value
            {
                get { return _value ; }
                set { _value = value ; }
            }

            public int RowIndex
            {
                get { return _rowIndex; }
                set { _rowIndex = value; }
            }

            public int ColumnIndex
            {
                get { return _columnIndex; }
                set { _columnIndex = value; }
            }

            string _value = "" ;
            int _rowIndex;
            int _columnIndex;
        }

        public class KeyCollection : List<TableDataItem> ... etc ...
        {
            ... etc ....
        }

        public class RowCellCollection : List<TableDataItem> ... etc ...
        {
            ... etc ....
        }
#endif

        private void btnLoad_Click(object sender, EventArgs e)
        {
            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };

            int intLineNumber = 0 ;

            if ( txtInputData.Lines.Count() < 1 )
                MessageBox.Show(String.Format("Insufficient items", System.Convert.ToString(intLineNumber)), "Error");
            else
            {
                // Got text to insert

                foreach (string TextLine in txtInputData.Lines)
                {
                    intLineNumber += 1;
                    if (!String.IsNullOrEmpty(TextLine))
                    {
                        // Not a blank line

                        string[] TextLineSplit = TextLine.Split(delimiterChars);

                        if (TextLineSplit.Count() < 2)
                        {
                            MessageBox.Show(String.Format("Insufficient items on line {0}", System.Convert.ToString(intLineNumber)), "Error");
                        }
                        else
                        {
                            // Insert into the Collection

                            // Remove the existing one first
                            if ( _testSortedDictionary.StringPairSortedDictionaryContains(TextLineSplit[0]))
                                _testSortedDictionary.StringPairSortedDictionaryRemove(TextLineSplit[0]) ;

                            // Add the entry
                            _testSortedDictionary.StringPairSortedDictionaryAdd(TextLineSplit[0], TextLineSplit[1]);

                        } // Insert into the Collection

                    } // Not a blank line

                } // for

                Redisplay() ;

            } // Got text to insert

        } // btnLoad_Click

        public class StringStringIgnoreCaseSortedDictionaryType : SortedDictionary<string, string>
        {
            public StringStringIgnoreCaseSortedDictionaryType()
                : base(StringComparer.CurrentCultureIgnoreCase)
            {
            }
        }

        public class KeyCollection : List<string>
        {
            public KeyCollection()
            {
                _value = 0;
            }
            public KeyCollection(int value)
            {
                _value = value;
            }

            public int _value;
        }

        public class ColumnKeyCollection : KeyCollection
        {
            public ColumnKeyCollection() : base() { }
            public ColumnKeyCollection(int value) : base(value) { }
        }

        private string ListMembersAsString(List<string> stringList)
        {
            string listAsString = "";
            stringList.ForEach(s => listAsString += ",\"" + s + "\"");
            if (listAsString.Length > 0)
                // Remove the leading comma
                listAsString = listAsString.Remove(0, 1);
            return listAsString;
        }

        private enum TestEnum
        {
            First ,
            Second ,
            Third ,
            Rubbish
        }

        private void Redisplay()
        {
            txtOutputData.Clear() ;

            string firstValue = TestEnum.First.ToString();
            string secondValue = TestEnum.Second.ToString();
            string thirdValue = TestEnum.Third.ToString();
            string fourthValue = TestEnum.Rubbish.ToString();

            List<string> stringList = new List<string>();

            stringList.Add("1");
            stringList.Add("2");
            stringList.Add("3");
            stringList.Add("4");
            stringList.Add("5");

            string listAsString = ListMembersAsString(stringList);

            string listAsString2 = ListMembersAsString(new List<string>());

            TestSortedDictionary.StringPairSortedDictionaryType stringPairSortedDictionary = _testSortedDictionary.StringPairSortedDictionaryGet();

            foreach (TestSortedDictionary.StringPair Entry in stringPairSortedDictionary.Values)
            {
                txtOutputData.Text += Entry.Key + "," + Entry.Value + Environment.NewLine ;
            }

            List<int> intCollection1 = new List<int>();

            // Use Insert when the Collection is empty
            intCollection1.Insert(0,-1);

            intCollection1.Add(1);
            intCollection1.Add(2);

            List<int> intCollection2 = new List<int>();

            intCollection2.Add(3);
            intCollection2.Add(4);

            intCollection1.InsertRange(0, intCollection2);

            intCollection1[2] = 19;

            intCollection1.Insert(intCollection1.Count, 21);

            txtOutputData.Text += "And in reverse:" + Environment.NewLine;
            foreach (TestSortedDictionary.StringPair Entry in stringPairSortedDictionary.Values.Reverse())
            {
                txtOutputData.Text += Entry.Key + "," + Entry.Value + Environment.NewLine;
            }

            StringStringIgnoreCaseSortedDictionaryType stringStringIgnoreCaseSortedDictionary = new StringStringIgnoreCaseSortedDictionaryType();

            stringStringIgnoreCaseSortedDictionary.Add("A","X") ;
            stringStringIgnoreCaseSortedDictionary.Add("C", "Z");
            stringStringIgnoreCaseSortedDictionary.Add("B", "Y");

            txtOutputData.Text += "StringString Sorted Dictionary:" + Environment.NewLine;
            foreach (var stringPair in stringStringIgnoreCaseSortedDictionary)
            {
                txtOutputData.Text += stringPair.Key + "," + stringPair.Value + Environment.NewLine;
            }

            txtOutputData.Text += "StringString Sorted Dictionary in reverse:" + Environment.NewLine;
            foreach (var stringPair in stringStringIgnoreCaseSortedDictionary.Reverse())
            {
                txtOutputData.Text += stringPair.Key + "," + stringPair.Value + Environment.NewLine;
            }

            SortedSet<int> intSortedSet1 = new SortedSet<int>();
            intSortedSet1.Add(1);
            intSortedSet1.Add(2);
            intSortedSet1.Add(3);
            intSortedSet1.Add(4);

            SortedSet<int> intSortedSet2 = new SortedSet<int>();
            intSortedSet2.Add(1);
            intSortedSet2.Add(2);
            intSortedSet2.Add(11);
            intSortedSet2.Add(12);
            intSortedSet2.Add(13);
            intSortedSet2.Add(14);
            intSortedSet2.Add(15);

            string intSortedSet2String = intSortedSet2.ToString();


            try
            {
                if (intSortedSet2.Any(i => i == 1))
                    Debug.WriteLine("There's already a 1");
                else
                    intSortedSet2.Add(1);
            }
            catch (Exception eek)
            {
            }

            intSortedSet2.RemoveWhere(i => i == 1);

            IEnumerable<int> union = intSortedSet1.Union(intSortedSet2);

            SortedSet<int> intSortedSetUnion = new SortedSet<int>() ;

            foreach (int value in union)
            {
                intSortedSetUnion.Add(value);
            }

            intSortedSet1.UnionWith(intSortedSet2);

            // Construct a "base" KeyCollection
            List<KeyCollection> keyCollectionList = new List<KeyCollection>();

            // Construct a "derived" ColumnKeyCollection
            ColumnKeyCollection columnKeyCollection = new ColumnKeyCollection();

            // Try to add the "derived" Column Key Collection to the "base" Key Collection List
            keyCollectionList.Add(columnKeyCollection);

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            _testSortedDictionary.StringPairSortedDictionaryClear();
            Redisplay();
        }

        private TestSortedDictionary _testSortedDictionary;

        private void btnDelete_Click(object sender, EventArgs e)
        {
            _testSortedDictionary.StringPairSortedDictionaryRemove( txtDeleteKey.Text ) ;
            Redisplay();
        }

        private void btnChangeKey_Click(object sender, EventArgs e)
        {
            TestSortedDictionary.StringPair stringPair = _testSortedDictionary.StringPairSortedDictionaryGet(txtFrom.Text);

            if (stringPair != null)
            {
                // Delete the old entry
                if (_testSortedDictionary.StringPairSortedDictionaryRemove(txtFrom.Text))
                {
                    // Removed the entry

                    // Change the key
                    stringPair.Key = txtTo.Text;
                    // Re-insert
                    _testSortedDictionary.StringPairSortedDictionaryAdd( stringPair ) ;

                } // Removed the entry
            }

            Redisplay();
        }

    } // TestCollectionsForm
}
