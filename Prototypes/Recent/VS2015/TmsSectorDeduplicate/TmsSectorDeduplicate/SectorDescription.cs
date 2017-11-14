using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TmsSectorDeduplicate
{
    /// <summary>
    /// Represents a sector description
    /// </summary>
    [Serializable]
    public class SectorDescription : IBinaryFileHandler, ICloneable
    {

        #region Constructors

        public SectorDescription()
        {
            SectorName = "";
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// The sector number
        /// </summary>
        public Int32 SectorNumber { get; set; }

        /// <summary>
        /// The sector name
        /// </summary>
        public string SectorName { get; set; }

        /// <summary>
        /// The notes offset position in the Sector Data file
        /// </summary>
        public Int32 NotesOffset { get; set; }

        /// <summary>
        /// The notes length in the Sector Data file
        /// </summary>
        public Int32 NotesLength { get; set; }

        #endregion

        #region Public Members

        /// <summary>
        /// Returns a SectorDescription Object that is a copy of "this"
        /// with the Sector Number set to zero.
        /// The Sector Number will be assigned when the Sector
        /// is added to a SectorContainer
        /// </summary>
        /// <returns>The new SectorDescription Object with no Sector Number</returns>
        public SectorDescription GenerateClone()
        {
            var sectorDescription = (SectorDescription)Clone();

            // Clear the existing Sector Number because this should become a new Sector
            // if it is ever added to the Sector Container Collection
            sectorDescription.SectorNumber = 0;

            return sectorDescription;
        }

        #endregion Public Members

        #region IBinaryFileHandler Members

        /// <summary>
        /// Populates properties on the current object reading information from the 
        /// provided binary reader.
        /// </summary>
        /// <param name="binaryReader">The binary reader to read</param>
        public void Read(BinaryReader binaryReader)
        {
            SectorNumber = binaryReader.ReadInt32();
            // Read the full sized field from the file and tidy it up
            SectorName = BinaryFileHelper.CharArrayToString(binaryReader.ReadChars(SectorNameFieldSize));
            NotesOffset = binaryReader.ReadInt32();
            NotesLength = binaryReader.ReadInt32();
        }

        /// <summary>
        /// Writes property values to a binary file using the binary writer
        /// </summary>
        /// <param name="binaryWriter">The instantiated binary writer used to write property information</param>
        /// <exception cref="SectorDescriptionWriteException"><c>SectorDescriptionWriteException</c>.</exception>
        public void Write(BinaryWriter binaryWriter)
        {
            try
            {
                binaryWriter.Write(SectorNumber);
                // Write the contents of the string containing the Sector Name and any trailing zeros
                binaryWriter.Write(BinaryFileHelper.StringToCharArray(SectorName, SectorNameFieldSize));
                binaryWriter.Write(NotesOffset);
                binaryWriter.Write(NotesLength);
            }
            catch (Exception eek)
            {
                Console.WriteLine("Error encountered writing sector description: {0} {1} = \"{2}\"", SectorNumber, SectorName,eek.ToString());
            }
        }

        /// <summary>
        /// Returns the size of the object used to calculate file position and offsets
        /// </summary>
        /// <returns>Object size</returns>
        public int FileRecordSize()
        {
            return DataSize();
        }

        /// <summary>
        /// Returns the size of the object data
        /// sizeof(TML_SECTOR_DESCRIPTOR)
        /// </summary>
        /// <returns>Object size</returns>
        public int DataSize()
        {
            int sectorDescriptionSize = 0;

            // SectorNumber
            sectorDescriptionSize += sizeof(Int32);

            // SectorName
            sectorDescriptionSize += 124;

            // NotesOffset
            sectorDescriptionSize += sizeof(Int32);

            // NotesLength
            sectorDescriptionSize += sizeof(Int32);

            return sectorDescriptionSize;
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Returns a generic Object that is a copy of "this"
        /// </summary>
        /// <returns>The new SectorDescription Object disguised as an Object</returns>
        public Object Clone()
        {
            var memoryStream = new MemoryStream();
            var binaryformatter = new BinaryFormatter();
            binaryformatter.Serialize(memoryStream, this);
            // Position back to the beginning of the stream
            memoryStream.Position = 0;
            // Read the object back as an exact copy
            object obj = binaryformatter.Deserialize(memoryStream);
            memoryStream.Close();
            return obj;
        }

        #endregion

        #region Private Members

        // The original TMS always writes this number of characters as a NUL terminated string to the Sector Name file field
        private const int SectorNameFieldSize = 124;

        #endregion Private Members
    }
}
