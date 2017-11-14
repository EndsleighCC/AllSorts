using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TmsSectorDeduplicate
{
    [Serializable]
    public class BinaryFileHelper
    {
        /// <summary>
        /// Construct a string from a char array by ensuring that
        /// the resulting string is only as long as the valid data.
        /// Valid data for 'C' style strings precedes the NUL termination character
        /// </summary>
        public static string CharArrayToString(char[] charArray)
        {
            var charArrayAsString = new string(charArray);

            int nulPosition;

            if ((nulPosition = charArrayAsString.IndexOf('\0')) != -1)
                // Use only that part of the string prior to the NUL
                charArrayAsString = charArrayAsString.Substring(0, nulPosition);

            return charArrayAsString;
        }

        /// <summary>
        /// Construct a char array from a string by ensuring that the resulting
        /// char array is as long as required by padding on the right with '\0'
        /// </summary>
        public static char[] StringToCharArray(string str, int charArrayLength)
        {
            char[] stringAsCharArray = null;

            if (str != null)
            {
                stringAsCharArray = str.Length >= charArrayLength
                                           ? str.ToCharArray(0, charArrayLength)
                                           : str.PadRight(charArrayLength, '\0').ToCharArray();
            }
            else
            {
                stringAsCharArray = new char[charArrayLength];
            }

            return stringAsCharArray;
        }

        /// <summary>
        /// Construct a char array from a string with the char array containing
        /// exactly the same count of characters as the string
        /// </summary>
        /// <param name="str">The string for which to generate a char array</param>
        /// <returns></returns>
        public static char[] StringToCharArray(string str)
        {
            return str.ToCharArray();
        }

        /// <summary>
        /// Construct a byte array of the required length filled with zero ('\0')
        /// </summary>
        public static byte[] EmptyByteArrayOfSize(int byteCount)
        {
            return new byte[byteCount];
        }

        /// <summary>
        /// Returns true if the supplied character is a whitespace character
        /// </summary>
        /// <param name="character">The character being tested for being whitespace</param>
        /// <param name="whitespaceChars">A string containing all the characters that may be considered as whitespace</param>
        /// <returns>true if whitespace or false if not</returns>
        public static bool IsWhiteSpace(string whitespaceChars, char character)
        {
            return (whitespaceChars.IndexOf(character) != -1);
        }

        /// <summary>
        /// Determines whether the supplied character is a member of the end-of-line (NewLine) sequence
        /// </summary>
        /// <param name="character">The character to tbe checked</param>
        /// <returns></returns>
        public static bool IsPartOfEndOfLine(char character)
        {
            bool isPartOfEndOfLine = Environment.NewLine.IndexOf(character) != -1;
            return isPartOfEndOfLine;
        } // IsPartOfEndOfLine

        /// <summary>
        /// Determines whether the supplied Binary Reader is currently positioned over an end-of-line sequence
        /// </summary>
        /// <param name="binaryReader"></param>
        /// <returns></returns>
        public static bool AtEndOfLine(BinaryReader binaryReader)
        {
            return IsPartOfEndOfLine((char)binaryReader.PeekChar());
        }

        /// <summary>
        /// Move the supplied Binary Reader to the next non-whitespace character
        /// Witespace characters include spaces and tabs
        /// </summary>
        /// <param name="binaryReader">The Binary Reader to move to the next non-whitespace character</param>
        /// <param name="whitespaceChars">A string containing all characters that may be considered as whitespace</param>
        public static bool ConsumeWhiteSpace(BinaryReader binaryReader, string whitespaceChars)
        {
            bool success = true;
            try
            {
                // Skip all whitespace
                while (IsWhiteSpace(whitespaceChars, (char)binaryReader.PeekChar()))
                    binaryReader.ReadChar();
            }
            catch (Exception eek)
            {
                success = false;
            }
            return success;
        } // ConsumeWhiteSpace

        /// <summary>
        /// Skips over all whitespace and comments starting at the current location of the Binary Reader
        /// </summary>
        /// <param name="binaryReader">The Binary Reader in which all whitespace is to be "skipped over" starting at the current position</param>
        /// <param name="whitespaceChars">A string containing all characters that may be considered to be whitespace</param>
        /// <param name="commentDeclarator">The character that may be considered to preced a comment</param>
        /// <returns>false if no more data is available from the Binary Reader or true otherwise</returns>
        public static bool ConsumeWhiteSpaceAndComments(BinaryReader binaryReader, string whitespaceChars, char commentDeclarator)
        {
            bool success = true;
            try
            {
                do
                {
                    BinaryFileHelper.ConsumeWhiteSpace(binaryReader, whitespaceChars);
                    if (binaryReader.PeekChar() == commentDeclarator)
                        BinaryFileHelper.ReadTextToEndOfLine(binaryReader);
                } while ((binaryReader.PeekChar() == ' ') || (binaryReader.PeekChar() == commentDeclarator));

                success = !IsWhiteSpace(whitespaceChars, (char)binaryReader.PeekChar());
            }
            catch (Exception eek)
            {
                success = false;
            }
            return success;
        } // ConsumeWhiteSpaceAndComments

        /// <summary>
        /// If and only if the supplied Binary Reader is positioned at the start of an end-of-line (NewLine)
        /// sequence this method will skip over that end-of-line sequence.
        /// </summary>
        /// <param name="binaryReader">The Binary Reader from which the end-of-line (NewLine) sequence will be consumed starting at the current position</param>
        public static void ConsumeEndOfLine(BinaryReader binaryReader)
        {
            // Comsume all end-of-line characters if there are any
            while (IsPartOfEndOfLine((char)binaryReader.PeekChar()))
                binaryReader.ReadChar();
        } // ConsumeEndOfLine

        /// <summary>
        /// Read and discard from from the current Binary Reader position upto and including the next end-of-line
        /// </summary>
        /// <param name="binaryReader">The Binary Reader from which the characters and end-of-line (NewLine) sequence will be consumed starting at the current position</param>
        public static void ConsumeUpToAndIncludingEndOfLine(BinaryReader binaryReader)
        {
            // Consume everything up to but not including the end-of-line
            while (!IsPartOfEndOfLine((char)binaryReader.PeekChar()))
                binaryReader.ReadChar();
            // Consume the end-of-line
            ConsumeEndOfLine(binaryReader);
        }

        /// <summary>
        /// Read a single delimited item from a text line
        /// </summary>
        /// <param name="binaryReader">The Binary Reader from which to read the item</param>
        /// <param name="whitespaceChars">A string containing all the characters that may be considered to be whitespace</param>
        /// <param name="dataDelimiter">The character that delimits the data items within the record</param>
        /// <returns></returns>
        public static string ReadDelimitedDataItem(BinaryReader binaryReader, string whitespaceChars, char dataDelimiter)
        {
            string itemValue = null;

            if (!IsPartOfEndOfLine((char)binaryReader.PeekChar()))
            {
                // Read the contents of the item starting at the current Stream Position
                StringBuilder stringBuilderItemValue = new StringBuilder("", _defaultStringBuilderCapacity);

                try
                {
                    char dataStopChar = dataDelimiter;

                    ConsumeWhiteSpace(binaryReader, whitespaceChars);
                    if (binaryReader.PeekChar() == '\"')
                    {
                        // Item is delimited by double quotes
                        // Read the first double quote
                        dataStopChar = binaryReader.ReadChar();
                    }

                    char nextChar = (char)binaryReader.PeekChar();
                    while ((nextChar != dataStopChar) // End at end of a field indicator (data stop)
                           && (!IsPartOfEndOfLine(nextChar)) // End at end-of-line
                           && (nextChar != (char)Constants.NUL))
                    {
                        stringBuilderItemValue.Append(binaryReader.ReadChar());
                        nextChar = (char)binaryReader.PeekChar();
                    }

                    if ((dataStopChar == '\"') && (nextChar == dataStopChar))
                    {
                        // Consume the double quote at the end of the field value
                        binaryReader.ReadChar();
                        nextChar = (char)binaryReader.PeekChar();
                    }

                    if ((nextChar == dataDelimiter) || (nextChar == (char)Constants.NUL))
                    {
                        // Read and discard the trailing data delimiter or NUL
                        binaryReader.ReadChar();
                    }

                    itemValue = stringBuilderItemValue.ToString();

                } // try
                catch (EndOfStreamException eek)
                {
                    // Return data if any was read
                    if (stringBuilderItemValue.Length > 0)
                    {
                        itemValue = stringBuilderItemValue.ToString();
                    }
                    else
                    {
                        // No data was actually read so re-throw the EndOfStreamException
                        throw;
                    }
                }

            } // Read the contents of the item starting at the current Stream Position

            return itemValue;

        } // ReadDelimitedDataItem

        /// <summary>
        /// Atempts to read the specified number of delimited data values starting
        /// from the current BinaryReader position.
        /// If the function reaches the end of the text line before reading the specified
        /// count of items, the returned collection will have fewer values than requested
        /// If the end-of-line character sequence terminates the read, the end-of-line character sequence will not be read
        /// </summary>
        /// <param name="binaryReader">The Binary Reader from the current position of which the data will be read</param>
        /// <param name="whitespaceChars">A string containing all the characters that may be considered to be whitespace</param>
        /// <param name="itemCount">The count of items to be read</param>
        /// <param name="dataDelimiter">The character that delimits the data items within the record</param>
        /// <returns>A Collection of values read from the stream</returns>
        public static List<string> ReadDelimitedDataItemCollection(BinaryReader binaryReader, string whitespaceChars, int itemCount, char dataDelimiter)
        {
            List<string> stringList = new List<string>();
            try
            {
                bool finishedReadingAllItems = false;
                for (int itemIndex = 0; (itemIndex < itemCount) && (!finishedReadingAllItems); ++itemIndex)
                {
                    string itemValue = ReadDelimitedDataItem(binaryReader, whitespaceChars, dataDelimiter);
                    if (itemValue != null)
                    {
                        stringList.Add(itemValue);
                    }
                    else
                    {
                        finishedReadingAllItems = true;
                    }
                }
            }
            catch (EndOfStreamException eek)
            {
                // Clear whatever was in the string List to indicate and incomplete set of data
                stringList = null;
            }
            return stringList;
        }

        /// <summary>
        /// Reads the data from the current position of the supplied BinaryReader
        /// up to the next end-of-line or end of file (whichever comes first) and generates
        /// a List of strings representing the identified comma separated entries
        /// </summary>
        /// <param name="binaryReader">The Binary Reader from the current position of which the data will be read</param>
        /// <param name="whitespaceChars">A string containing all characters that may be considered to be whitespace</param>
        /// <param name="dataDelimiter">The character that delimits the data items within the record</param>
        /// <returns></returns>
        public static List<string> ReadDelimitedDataRecord(BinaryReader binaryReader, string whitespaceChars, char dataDelimiter)
        {
            List<string> stringList = null;
            try
            {
                string itemValue = null;
                while ((itemValue = ReadDelimitedDataItem(binaryReader, whitespaceChars, dataDelimiter)) != null)
                {
                    if (stringList == null)
                        stringList = new List<string>();
                    stringList.Add(itemValue);
                }
                ConsumeEndOfLine(binaryReader);
            }
            catch (EndOfStreamException eek)
            {
                // Return whatever items have been added to the string List (if any)
            }
            return stringList;
        } // ReadDelimitedDataRecord

        /// <summary>
        /// Atempts to read the specified number of delimited data values starting
        /// from the current BinaryReader position.
        /// If the function reaches the end of the text line before reading the specified
        /// count of items, the returned collection will have fewer values than requested
        /// </summary>
        /// <param name="binaryReader">The Binary Reader from the current position of which the data will be read</param>
        /// <param name="whitespaceChars">A string containing all the characters that may be considered to be whitespace</param>
        /// <param name="itemCount">The count of items to be read</param>
        /// <param name="dataDelimiter">The character that delimits the data items within the record</param>
        /// <returns>A Collection of values read from the stream</returns>
        public static List<string> ReadDelimitedDataRecord(BinaryReader binaryReader, string whitespaceChars, int itemCount, char dataDelimiter)
        {
            List<string> dataRecordItems = ReadDelimitedDataItemCollection(binaryReader, whitespaceChars, itemCount, dataDelimiter);
            // If this is a text record then ignore the rest of the record
            // If this is not a text record then leave the file in its current position
            ConsumeEndOfLine(binaryReader);
            return dataRecordItems;
        } // ReadDelimitedDataRecord

        /// <summary>
        /// Reads the data from the current position of the supplied BinaryReader
        /// to the end of the current text line or end of file whichever comes first
        /// </summary>
        /// <param name="binaryReader">The Binary Reader from the current position of which the data will be read</param>
        /// <returns></returns>
        public static string ReadTextToEndOfLine(BinaryReader binaryReader)
        {
            StringBuilder stringBuilderTextLine = new StringBuilder("", _defaultStringBuilderCapacity);

            try
            {
                while (!IsPartOfEndOfLine((char)binaryReader.PeekChar()))
                {
                    stringBuilderTextLine.Append(binaryReader.ReadChar());
                }
                // Consume the New Line string too
                ConsumeEndOfLine(binaryReader);
            }
            catch (Exception eek)
            {
                // Leave the return as it has been accumulated so far
            }

            return stringBuilderTextLine.ToString();

        } // ReadTextToEndOfLine

        /// <summary>
        /// Write the beginning of a delimited data record to the supplied Binary Writer
        /// </summary>
        /// <param name="binaryWriter">The Binary Writer to which the "begin record" should be written</param>
        /// <param name="dataDelimiter">The character that should delimit the data items within the record</param>
        /// <returns></returns>
        public static bool WriteBeginDelimitedDataRecord(BinaryWriter binaryWriter, char dataDelimiter)
        {
            bool success = true;
            binaryWriter.Write(dataDelimiter);
            return success;
        }

        /// <summary>
        /// Write the supplied list of string to the supplied Binary Writer with each item separated by commas
        /// </summary>
        /// <param name="binaryWriter">The Binary Writer to be written</param>
        /// <param name="stringList">The list of strings to be written</param>
        /// <param name="dataDelimiter">The character that should delimit the data items within the record</param>
        /// <returns>Whether (true) or not (false) the write succeeded</returns>
        public static bool WriteDelimitedDataRecord(BinaryWriter binaryWriter, List<string> stringList, char dataDelimiter)
        {
            return WriteDelimitedDataRecord(binaryWriter, stringList, 0, stringList.Count, dataDelimiter);
        }

        /// <summary>
        /// Write the specified items from the supplied list to the supplied Binary Writer
        /// </summary>
        /// <param name="binaryWriter">The Binary Writer to be written</param>
        /// <param name="stringList">The list of strings to be written</param>
        /// <param name="firstItemIndex">The index of the first item within the List to be written</param>
        /// <param name="itemCount">The count of items from the List to be written</param>
        /// <param name="dataDelimiter">The character that should delimit the data items within the record</param>
        /// <returns></returns>
        public static bool WriteDelimitedDataRecord(BinaryWriter binaryWriter, List<string> stringList, int firstItemIndex, int itemCount, char dataDelimiter)
        {
            bool success = true;

            for (int listItemCount = 0, index = firstItemIndex; listItemCount < itemCount; ++listItemCount, ++index)
            {
                string itemValue = stringList[index];
                string itemValueWrite = itemValue;
                // Only add a delimiter at the beginning of items that need it
                if (index > firstItemIndex)
                {
                    // Precede the data with a delimiter
                    itemValueWrite = dataDelimiter + itemValueWrite;
                }
                if (!String.IsNullOrEmpty(itemValueWrite))
                {
                    binaryWriter.Write(BinaryFileHelper.StringToCharArray(itemValueWrite));
                }
            }

            return success;
        }

        /// <summary>
        /// Write the supplied list of string to the supplied Binary Writer with each item separated by commas
        /// and, if supplied, surrounded by a field delimiter e.g. a double quote (")
        /// </summary>
        /// <param name="binaryWriter">The Binary Writer to be written</param>
        /// <param name="stringList">The list of strings to be written</param>
        /// <param name="delimiter">The delimiter with which to surround each data item string</param>
        /// <param name="dataDelimiter">The character that should delimit the data items within the record</param>
        /// <returns>Whether (true) or not (false) the write succeeded</returns>
        public static bool WriteCommaSeparatedRecord(BinaryWriter binaryWriter, List<string> stringList, char itemDelimiter, char dataDelimiter)
        {
            bool success = true;

            for (int index = 0; index < stringList.Count; ++index)
            {
                string itemValue = stringList[index];
                string itemValueWrite = itemDelimiter + itemValue + itemDelimiter;
                // Only add a comma at the beginning of items that need it
                if (index > 0)
                    // Precede the data with a comma
                    itemValueWrite = dataDelimiter + itemValueWrite;
                binaryWriter.Write(BinaryFileHelper.StringToCharArray(itemValueWrite));
            }

            return success;
        }

        /// <summary>
        /// Write all the characters of an end-of-line (NewLine) sequence to the supplied Binary Writer
        /// </summary>
        /// <param name="binaryWriter">The Binary Writer to which to write the end-of-line sequence</param>
        /// <returns></returns>
        public static bool WriteEndOfLine(BinaryWriter binaryWriter)
        {
            bool success = true;
            try
            {
                foreach (char character in Environment.NewLine)
                {
                    binaryWriter.Write(character);
                }
            }
            catch (Exception eek)
            {
                success = false;
            }
            return success;
        }

        public const char CommaSeparatedVariableSeparatorChar = ',';
        public const char TabSeparatedVariableSeparatorChar = '\t';

        private const int _defaultStringBuilderCapacity = 100;

    } // class BinaryFileHelper
}
