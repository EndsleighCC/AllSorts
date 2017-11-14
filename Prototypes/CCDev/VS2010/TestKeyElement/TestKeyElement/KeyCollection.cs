using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestKeyElement
{
    public class KeyCollection : List<string> /* Key Element Collection */ , IEquatable<KeyCollection>, IComparable<KeyCollection>
    {
        public KeyCollection() : base()
        {
        }

        public KeyCollection(string firstKeyElement) : base()
        {
            AddKeyElement();
            this[0] = firstKeyElement;
        }

        /// <summary>
        /// Add an empty Key Element to the end of the List of string for this KeyCollection
        /// </summary>
        public void AddKeyElement()
        {
            this.Add(String.Empty);
        }

        #region Live Implementation

        #region IEquatable members

        /// <summary>
        /// Determine whether the supplied Key Key Collection is effectively equal to "this" Key Collection
        /// </summary>
        /// <param name="otherKeyCollection">The other Key Collection with which "this" is to be compared</param>
        /// <returns>true if "this" is equal to other, or, false if not</returns>
        public bool Equals(KeyCollection otherKeyCollection)
        {
            bool isEqual = false;
            if (Count == otherKeyCollection.Count)
            {
                // Compare the contents of each List
                isEqual = true;
                for (int keyElementIndex = 0; (isEqual) && (keyElementIndex < Count); ++keyElementIndex)
                {
                    isEqual = this[keyElementIndex].Equals(otherKeyCollection[keyElementIndex], StringComparison.CurrentCultureIgnoreCase);
                }
            } // Compare the contents of each List
            return isEqual;
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. 
        ///                 </param><exception cref="T:System.NullReferenceException">The <paramref name="obj"/> parameter is null.
        ///                 </exception><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            bool isEqual;

            if (obj == null)
                isEqual = false;
            else if (ReferenceEquals(this, obj))
                isEqual = true;
            else if (GetType() != obj.GetType())
                isEqual = false;
            else
                isEqual = Equals(obj as KeyCollection);

            return isEqual;
        }

        #endregion IEquatable members

        #region IComparable members

        /// <summary>
        /// Each Key Element will be composed of either one or two Key Element Components
        /// A Key Element Component may be either a string or a signed integer
        /// If a Key Element contains one Key Element Component then the whole Key Element value represents one value that may be compared.
        /// If a Key Element contains two Key Element Components, then the Key Element must represent a range with one Component representing
        /// the inclusive lower limit of the range and the other representing the inclusive upper limit of the range.
        /// "Key Elements" may be compared according to the following rules.
        ///     Equality
        ///         Two Simple Key Elements:
        ///             The left Simple Key Element is only equal to the right Simple Key Element
        ///             if the left Simple Key Element is equal to the right Simple Key Element
        ///         Two Range Key Elements:
        ///             The left Range Key Element is only equal to the right Range Key Element
        ///             if one range is fully enclosed within the other range
        ///         A Range Key Element and a Simple Key Element:
        ///             The Range Key Element is only equal to a Simple Key Element
        ///             if the Simple Key Element falls within the inclusive range of the limits of the Range Key Element
        ///             i.e. if the Simple Key Element is greater than or equal to the lower limit of the Range Key Element 
        ///             AND less than or equal to the upper limit of the Range Key Element
        ///         A Simple Key Element and a Range Key Element:
        ///             The Simple Key Element is only equal to the Range Key Element
        ///             if the Simple Key Element falls within the inclusive range of the limits of the Range Key Element
        ///             i.e. if the Simple Key Element is greater than or equal to the lower limit of the Range Key Element 
        ///             AND less than or equal to the upper limit of the Range Key Element
        ///     Comparablity
        ///         Two Simple Key Elements:
        ///             The left Simple Key Element is less than or greater than the right Simple Key Element
        ///             if the left Simple Key Element is less than or greater than the right Simple Key Element respectively
        ///             otherwise it is considered equal
        ///         Two Range Key Elements:
        ///             The left Range Key Element is only less than the right Range Key Element
        ///             if the left Key Element Upper Limit is less than the right Key Element Lower Limit.
        ///             The left Range Key Element is only greater than the right Range Key Element
        ///             if the left Key Element Lower Limit is greater than the right Key Element Upper Limit.
        ///             Otherwise the two are considered equal.
        ///             Note that ranges that overlap are considered to be equal.
        ///         A Range Key Element and a Simple Key Element:
        ///             A Range Key Element is only less than a Simple Key Element
        ///             if the upper limit of the Range Key Element is less than the Simple Key Element.
        ///             A Range Key Element is only greater than a Simple Key Element
        ///             if the lower limit of the Range Key Element is greater than the Simple Key Element.
        ///             The two are considered equal if the Range Key Element limits contain the Simple Key Element.
        ///         A Simple Key Element and a Range Key Element:
        ///             A Simple Key Element is only less than a Range Key Element
        ///             if the Simple Key Element is less than the lower limit of the Range Key Element.
        ///             A Simple Key Element is only greater than a Range Key Element
        ///             if the Simple Key Element is greater than the upper limit of the Range Key Element.
        ///             The two are considered equal if the Simple Key Element falls within the range of the Range Key Element.
        ///     Comparing two Key Elements is notionally equivalent to generating the arithmentical difference
        ///     between the two Key Elements i.e. "left Key Element" - "right Key Element"
        ///     
        ///     For Ranges, diagramatically, this is as follows:
        ///     
        ///     Range Left is less than Range Right:
        ///     
        ///         Range Left:  |------|
        ///         Range Right:          |------|
        ///         
        ///     Range Left is equal to Range Right: 
        ///         
        ///         Range Left:  |------|
        ///         Range Right: |------|
        ///     
        ///         Range Left:  |------|
        ///         Range Right:      |-------|
        ///     
        ///         Range Left:       |-------|
        ///         Range Right: |------|
        ///         
        ///         Range Left:  |----|
        ///         Range Right: |------|
        ///         
        ///         Range Left:   |----|
        ///         Range Right: |------|
        ///         
        ///         Range Left:  |------|
        ///         Range Right: |----|
        ///         
        ///         Range Left:  |------|
        ///         Range Right:  |----|
        ///     
        ///     Range Left is greater than Range Right:
        ///     
        ///         Range Left:           |------|
        ///         Range Right: |------|
        ///         
        /// </summary>

        /// <summary>
        /// Contain the details of a particular Component of a Key Element
        /// </summary>
        private class KeyElementComponentDetail : IEquatable<KeyElementComponentDetail> , IComparable<KeyElementComponentDetail>
        {
            #region Constructors

            /// <summary>
            /// Construct from a string value that is a "simple" item i.e. not a range
            /// </summary>
            /// <param name="value">A string that con</param>
            public KeyElementComponentDetail(string value)
            {
                // All Key Element Components have a string representation
                _keyElementComponentStringValue = value;
                if (Int32.TryParse(value, out _keyElementComponentIntegerValue))
                    // As well as having a string representation this Key Element Component also has a valid integer value
                    _keyElemenComponentType = KeyElementComponentTypeEnum.Integer;
                else
                {
                    _keyElemenComponentType = KeyElementComponentTypeEnum.String;
                }
            }

            #endregion Constructors

            #region Public Members

            public enum KeyElementComponentTypeEnum
            {
                Unknown,
                String,
                Integer
            }

            /// <summary>
            /// Identifies the type of Key Element Component
            /// </summary>
            public KeyElementComponentTypeEnum KeyElementComponentType
            {
                get { return _keyElemenComponentType; }
                private set { _keyElemenComponentType = value; }
            }

            #endregion Public Members

            #region IEquatable

            /// <summary>
            /// Determine whether the supplied Key Element Component Detail is effectively equal to "this" Key Element Component Detail
            /// </summary>
            /// <param name="otherKeyElementComponentDetail">The other Key Element Component Detail with which "this" is to be compared</param>
            /// <returns>true if "this" is equal to other, or, false if not</returns>
            public bool Equals(KeyElementComponentDetail otherKeyElementComponentDetail)
            {
                bool isEqual = this.CompareTo(otherKeyElementComponentDetail) == 0 ;
                return isEqual;
            }

            /// <summary>
            /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
            /// </summary>
            /// <returns>
            /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
            /// </returns>
            /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. 
            ///                 </param><exception cref="T:System.NullReferenceException">The <paramref name="obj"/> parameter is null.
            ///                 </exception><filterpriority>2</filterpriority>
            public override bool Equals(object obj)
            {
                bool isEqual;

                if (obj == null)
                    isEqual = false;
                else if (ReferenceEquals(this, obj))
                    isEqual = true;
                else if (GetType() != obj.GetType())
                    isEqual = false;
                else
                    isEqual = Equals(obj as KeyElementComponentDetail);

                return isEqual;
            }

            #endregion

            #region IComparable

            /// <summary>
            /// Determine how the supplied Key Element Component Detail compares to "this" Key Element component Detail
            ///     negative = "this" is less than "other"
            ///     zero     = "this" is equal to "other"
            ///     positive = "this" is greater than other
            /// Notionally this function returns something akin to the arithmetical difference operation
            ///         "this" - "other"
            /// </summary>
            /// <param name="otherKeyElementComponentDetail">The other Key Element Component Collection with which "this" is to be compared</param>
            /// <returns>negative if "this" is less than "other", zero if "this" is equal to "other", positive if "this" is greater than "other"</returns>
            public int CompareTo(KeyElementComponentDetail otherKeyElementComponentDetail)
            {
                int comparisonIndicator = 0; // Default to equal

                if ((this._keyElemenComponentType == KeyElementComponentTypeEnum.Integer)
                     && (this._keyElemenComponentType == otherKeyElementComponentDetail._keyElemenComponentType)
                   )
                    // Integer comparison is possible because both are integers
                    comparisonIndicator = _keyElementComponentIntegerValue - otherKeyElementComponentDetail._keyElementComponentIntegerValue;
                else
                    // Only string comparison is possible because the Components are different types
                    comparisonIndicator = String.Compare(_keyElementComponentStringValue, otherKeyElementComponentDetail._keyElementComponentStringValue, true /* ignore case */);

                return comparisonIndicator;
            }

            #endregion IComparable

            #region Private Member variables

            private int _keyElementComponentIntegerValue = 0;
            private string _keyElementComponentStringValue = null;
            private KeyElementComponentTypeEnum _keyElemenComponentType = KeyElementComponentTypeEnum.Unknown;

            #endregion Private Member variables
        } ;

        /// <summary>
        /// Contain the details of a particular Key Element
        /// </summary>
        private class KeyElementDetail : List<KeyElementComponentDetail>, IEquatable<KeyElementDetail> , IComparable<KeyElementDetail>
        {
            #region Constructors

            public KeyElementDetail(string keyElementData)
                : base()
            {
                string keyElementTrim = keyElementData.Trim();
                // Do not consume a negative sign on the first value as a range indicator
                int firstNonSignCharacter = 0;
                if (keyElementTrim[0] == '-')
                    // Skip the initial sign character (which is the same as a hyphen range separator character)
                    firstNonSignCharacter = 1;
                else
                    firstNonSignCharacter = 0;

                // Look for a "hyphen" which will indicate whether this is a range
                int hyphenPosition = keyElementTrim.IndexOf('-',firstNonSignCharacter) ;
                if (hyphenPosition == -1)
                {
                    // Single simple value

                    this.Add(new KeyElementComponentDetail(keyElementTrim));

                } // Single simple value
                else
                {
                    // Range value

                    // First value before the hyphen
                    KeyElementComponentDetail keyElementComponentDetailFirst = new KeyElementComponentDetail(keyElementTrim.Substring(0,hyphenPosition));
                    // Second value after the hyphen
                    KeyElementComponentDetail keyElementComponentDetailSecond = new KeyElementComponentDetail(keyElementTrim.Substring(hyphenPosition+1));
                    if ( keyElementComponentDetailFirst.CompareTo(keyElementComponentDetailSecond) <= 0 )
                    {
                        // First is less than or equal to second
                        this.Add(keyElementComponentDetailFirst);
                        this.Add(keyElementComponentDetailSecond);
                    } // First is less than or equal to second
                    else
                    {
                        // First is greater than second which needs to be "corrected"
                        this.Add(keyElementComponentDetailSecond);
                        this.Add(keyElementComponentDetailFirst);
                    } // First is greater than second which needs to be "corrected"

                } // Range value
            }

            #endregion Constructors

            #region Public Members

            public enum KeyElementTypeEnum
            {
                Unknown,
                Simple,
                Range
            }

            public KeyElementTypeEnum KeyElementType
            {
                get
                {
                    KeyElementTypeEnum keyElementType = KeyElementTypeEnum.Unknown;
                    switch (this.Count)
                    {
                        case 0: keyElementType = KeyElementTypeEnum.Unknown; break;
                        case 1: keyElementType = KeyElementTypeEnum.Simple; break;
                        default: keyElementType = KeyElementTypeEnum.Range; break;
                    }
                    return keyElementType;
                }
            }

            #endregion Public Members

            #region IEquatable

            /// <summary>
            /// Determine whether the supplied Key Element Detail is effectively equal to "this" Key Element Detail
            /// </summary>
            /// <param name="otherKeyElementDetail">The other Key Element Detail with which "this" is to be compared</param>
            /// <returns>true if "this" is equal to other, or, false if not</returns>
            public bool Equals(KeyElementDetail otherKeyElementDetail)
            {
                bool isEqual = this.CompareTo(otherKeyElementDetail) == 0 ;
                return isEqual;
            }

            /// <summary>
            /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
            /// </summary>
            /// <returns>
            /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
            /// </returns>
            /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. 
            ///                 </param><exception cref="T:System.NullReferenceException">The <paramref name="obj"/> parameter is null.
            ///                 </exception><filterpriority>2</filterpriority>
            public override bool Equals(object obj)
            {
                bool isEqual;

                if (obj == null)
                    isEqual = false;
                else if (ReferenceEquals(this, obj))
                    isEqual = true;
                else if (GetType() != obj.GetType())
                    isEqual = false;
                else
                    isEqual = Equals(obj as KeyElementDetail);

                return isEqual;
            }

            #endregion

            #region IComparable

            /// <summary>
            /// Determine how the supplied Key Element Detail compares to "this" Key Element Detail
            ///     negative = "this" is less than "other"
            ///     zero     = "this" is equal to "other"
            ///     positive = "this" is greater than other
            /// Notionally this function returns something akin to the arithmetical difference operation
            ///         "this" - "other"
            /// </summary>
            /// <param name="otherKeyElementDetail">The other Key Element Detail with which "this" is to be compared</param>
            /// <returns>negative if "this" is less than "other", zero if "this" is equal to "other", positive if "this" is greater than "other"</returns>
            public int CompareTo(KeyElementDetail otherKeyElementDetail)
            {
                int comparisonIndicator = 0; // Default to equal
                if ((this.KeyElementType != KeyElementTypeEnum.Range) && (otherKeyElementDetail.KeyElementType != KeyElementTypeEnum.Range))
                {
                    // No Ranges involved so use simple comparison
                    KeyElementComponentDetail keyElementComponentDetailLeft = this[0];
                    KeyElementComponentDetail keyElementComponentDetailRight = otherKeyElementDetail[0];
                    comparisonIndicator = keyElementComponentDetailLeft.CompareTo(keyElementComponentDetailRight);
                } // No Ranges involved so use simple comparison
                else if ((this.KeyElementType == KeyElementTypeEnum.Range) && (otherKeyElementDetail.KeyElementType == KeyElementTypeEnum.Range))
                {
                    // Two Ranges involved

                    // Ranges are effectively equal if one range is fully enclosed within the other
                    KeyElementComponentDetail keyElementComponentDetailLeftLowerLimit = this[0];
                    KeyElementComponentDetail keyElementComponentDetailLeftUpperLimit = this[1];
                    KeyElementComponentDetail keyElementComponentDetailRightLowerLimit = otherKeyElementDetail[0];
                    KeyElementComponentDetail keyElementComponentDetailRightUpperLimit = otherKeyElementDetail[1];

                    if (keyElementComponentDetailLeftUpperLimit.CompareTo(keyElementComponentDetailRightLowerLimit) < 0)
                        // Less than
                        comparisonIndicator = -1;
                    else if (keyElementComponentDetailLeftLowerLimit.CompareTo(keyElementComponentDetailRightUpperLimit) > 0)
                        // Greater than
                        comparisonIndicator = 1;
                    else
                        // Effectively the same
                        comparisonIndicator = 0;

                } // Two Ranges involved
                else if (this.KeyElementType == KeyElementTypeEnum.Range)
                {
                    // "this" is a range key only
                    // Test for rhs being in range of this
                    KeyElementComponentDetail keyElementComponentDetailLeftLowerLimit = this[0];
                    KeyElementComponentDetail keyElementComponentDetailLeftUpperLimit = this[1];
                    KeyElementComponentDetail keyElementComponentDetailRight = otherKeyElementDetail[0];
                    if ((keyElementComponentDetailLeftLowerLimit.CompareTo(keyElementComponentDetailRight) <= 0)
                         && (keyElementComponentDetailLeftUpperLimit.CompareTo(keyElementComponentDetailRight) >= 0)
                       )
                        // Right is in Range
                        comparisonIndicator = 0;
                    else if (keyElementComponentDetailLeftUpperLimit.CompareTo(keyElementComponentDetailRight) < 0)
                        // Range is less than right
                        comparisonIndicator = -1;
                    else
                        // Range must be greater than right
                        comparisonIndicator = 1;

                } // "this" is a range key only
                else
                {
                    // rhs is a range key only
                    // Test for this being in range of rhs
                    KeyElementComponentDetail keyElementComponentDetailLeft = this[0];
                    KeyElementComponentDetail keyElementComponentDetailRightLowerLimit = otherKeyElementDetail[0];
                    KeyElementComponentDetail keyElementComponentDetailRightUpperLimit = otherKeyElementDetail[1];
                    if ((keyElementComponentDetailLeft.CompareTo(keyElementComponentDetailRightLowerLimit) >= 0)
                        && (keyElementComponentDetailLeft.CompareTo(keyElementComponentDetailRightUpperLimit) <= 0)
                        )
                        // this is in the Range of Right
                        comparisonIndicator = 0;
                    else if (keyElementComponentDetailLeft.CompareTo(keyElementComponentDetailRightLowerLimit) < 0)
                        // this is less than the Range of Right
                        comparisonIndicator = -1;
                    else
                        // this must be greater than the Range of Right
                        comparisonIndicator = 1;

                } // rhs is a range key only

                return comparisonIndicator;
            }

            #endregion IComparable
        }

        /// <summary>
        /// Contain the details of all Key Elements within this Key Collection
        /// </summary>
        private class KeyElementDetailList : List<KeyElementDetail>, IEquatable<KeyElementDetailList>, IComparable<KeyElementDetailList>
        {
            public KeyElementDetailList()
                : base()
            {
            }

            #region IEquatable

            /// <summary>
            /// Determine whether the supplied Key Element Detail List is effectively equal to "this" Key Element Detail List
            /// </summary>
            /// <param name="otherKeyElementDetailList">The other Key Element Detail List with which "this" is to be compared</param>
            /// <returns>true if "this" is equal to other, or, false if not</returns>
            public bool Equals(KeyElementDetailList otherKeyElementDetailList)
            {
                bool isEqual = this.CompareTo(otherKeyElementDetailList) == 0;
                return isEqual;
            }

            /// <summary>
            /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
            /// </summary>
            /// <returns>
            /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
            /// </returns>
            /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. 
            ///                 </param><exception cref="T:System.NullReferenceException">The <paramref name="obj"/> parameter is null.
            ///                 </exception><filterpriority>2</filterpriority>
            public override bool Equals(object obj)
            {
                bool isEqual;

                if (obj == null)
                    isEqual = false;
                else if (ReferenceEquals(this, obj))
                    isEqual = true;
                else if (GetType() != obj.GetType())
                    isEqual = false;
                else
                    isEqual = Equals(obj as KeyElementDetailList);

                return isEqual;
            }

            #endregion

            #region IComparable

            /// <summary>
            /// Determine how the supplied Key Element Detail List compares to "this" Key Element Detail List
            ///     negative = "this" is less than "other"
            ///     zero     = "this" is equal to "other"
            ///     positive = "this" is greater than other
            /// Notionally this function returns something akin to the arithmetical difference operation
            ///         "this" - "other"
            /// For each Key, a comparison of each Key Element Detail in "this" is performed in turn with
            /// the corresponding Key Element in "other" until a Key Element comparison produces "not equal".
            /// The result of the overall comparison is therefore the result of the two Key Elements that are not equal.
            /// This creates the mechanism of "decreasing Key Element Significance" through the Key
            /// </summary>
            /// <param name="otherKeyElementDetailList">The other Key Element Detail List with which "this" is to be compared</param>
            /// <returns>negative if "this" is less than "other", zero if "this" is equal to "other", positive if "this" is greater than "other"</returns>
            public int CompareTo(KeyElementDetailList otherKeyElementDetailList)
            {
                int comparisonIndicator = 0; // Default to equal

                if ( this.Count != otherKeyElementDetailList.Count )
                    comparisonIndicator = this.Count - otherKeyElementDetailList.Count ;
                else
                {
                    // For each Key, a comparison of each Key Element Detail in "this" is performed in turn with
                    // the corresponding Key Element in "other" until a Key Element comparison produces "not equal".
                    // The result of the overall comparison is therefore the result of the two Key Elements that are not equal.
                    // This creates the mechanism of "decreasing Key Element Significance" through the Key
                    for (int keyElementIndex = 0; (comparisonIndicator == 0) && (keyElementIndex < this.Count); ++keyElementIndex)
                    {
                        comparisonIndicator = this[keyElementIndex].CompareTo(otherKeyElementDetailList[keyElementIndex]);
                    }
                }

                return comparisonIndicator;
            }

            #endregion IComparable
        }

        /// <summary>
        /// Determine the structure of all the Key Elements within this Key Collection
        /// </summary>
        private KeyElementDetailList DetermineKeyElementDetail()
        {
            KeyElementDetailList keyElementDetailList = new KeyElementDetailList();
            foreach (string keyElement in this)
            {
                KeyElementDetail keyElementDetail = new KeyElementDetail(keyElement);
                keyElementDetailList.Add(keyElementDetail);
            }
            return keyElementDetailList;
        }

        /// <summary>
        /// Determine how the supplied Key Collection compares to "this" Key Collection
        ///     negative = "this" is less than "other"
        ///     zero     = "this" is equal to "other"
        ///     positive = "this" is greater than other
        /// Notionally this function returns something akin to the arithmetical difference operation
        ///         "this" - "other"
        /// </summary>
        /// <param name="otherKeyCollection">The other Key Collection with which "this" is to be compared</param>
        /// <returns>negative if "this" is less than "other", zero if "this" is equal to "other", positive if "this" is greater than "other"</returns>
        public int CompareTo(KeyCollection otherKeyCollection)
        {
            int comparisonIndicator = 0;

            KeyElementDetailList thisKeyElementDetailList = this.DetermineKeyElementDetail();
            KeyElementDetailList otherKeyElementDetailList = otherKeyCollection.DetermineKeyElementDetail();
            comparisonIndicator = thisKeyElementDetailList.CompareTo(otherKeyElementDetailList);

            return comparisonIndicator;
        }

        #endregion IComparable members

        #endregion Live Implementation

    }
}
