using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestCollections
{
    public class TestSortedDictionary
    {
        public TestSortedDictionary()
        {
            _stringPairSortedDictionary = new StringPairSortedDictionaryType();
        }

        public class StringPair
        {
            public StringPair()
            {
                _Key = "";
                _Value = "";
            }

            public StringPair(StringPair stringPair)
            {
                _Key = stringPair.Key;
                _Value = stringPair.Value;
            }

            public StringPair(string Key, string Value)
            {
                _Key = Key;
                _Value = Value;
            }

            public string Key
            {
                get { return _Key;  }
                set { _Key = value; }
            }

            public string Value
            {
                get { return _Value; }
                set { _Value = value; }
            }

            private string _Key;
            private string _Value;

        } // StringPair

        public void StringPairSortedDictionaryClear()
        {
            _stringPairSortedDictionary.Clear();
        }

        public bool StringPairSortedDictionaryAdd(string Key, string Value)
        {
            bool Success = true;

            try
            {
                _stringPairSortedDictionary.Add(Key, new StringPair(Key, Value));
            }
            catch (Exception)
            {
                Success = false;
            }
            return Success;
        }

        public bool StringPairSortedDictionaryAdd(StringPair stringPair )
        {
            bool Success = true;

            try
            {
                _stringPairSortedDictionary.Add(stringPair.Key, new StringPair(stringPair));
            }
            catch (Exception)
            {
                Success = false;
            }
            return Success;
        }

        public bool StringPairSortedDictionaryContains(string Key)
        {
            return _stringPairSortedDictionary.ContainsKey(Key);
        }

        public bool StringPairSortedDictionaryRemove(string Key)
        {
            bool Success = true;

            try
            {
                _stringPairSortedDictionary.Remove(Key);
            }
            catch (Exception)
            {
                Success = false;
            }

            return Success;
        }

        public StringPair StringPairSortedDictionaryGet(string key)
        {
            StringPair stringPair = null ;
            try
            {
                stringPair = _stringPairSortedDictionary[key];
            }
            catch (Exception)
            {
            }

            return stringPair;
        }

        public StringPairSortedDictionaryType StringPairSortedDictionaryGet()
        {
            return _stringPairSortedDictionary ;
        }

        public class StringPairSortedDictionaryType : SortedDictionary<string, StringPair>
        {}

        private StringPairSortedDictionaryType _stringPairSortedDictionary ;

    } // TestSortedDictionary
}
