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
    /// Represents sector information
    /// </summary>
    [Serializable]
    public class SectorInformation : IBinaryFileHandler, ICloneable
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public SectorInformation()
        {
            SectorDescription = new SectorDescription();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The sector number
        /// </summary>
        public Int32 Number
        {
            get { return SectorDescription.SectorNumber; }
            set { SectorDescription.SectorNumber = value; }
        }

        /// <summary>
        /// The sector name
        /// </summary>
        public string Name
        {
            get { return SectorDescription.SectorName; }
            set { SectorDescription.SectorName = value; }
        }

        /// <summary>
        /// Notes
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Sector description
        /// </summary>
        public SectorDescription SectorDescription { get; set; }


        /// <summary>
        /// The item name (e.g. Message 123, Scheme ABC)
        /// </summary>
        public string ItemName
        {
            get { return String.Format("Sector {0} : {1}", Number, Name); }
        }

        public string DisplayTitle
        {
            get { return String.Format("Sector {0}  {1}", Number, Name); }
        }

        #endregion Public Properties

        #region Public Members

        /// <summary>
        /// Returns a SectorInformation Object that is a copy of "this"
        /// with the Sector Number set to zero.
        /// The Sector Number will be assigned when the Sector
        /// is added to a SectorContainer
        /// </summary>
        /// <returns>The new SectorInformation Object with no Sector Number</returns>
        public SectorInformation GenerateClone()
        {
            var sectorInformation = (SectorInformation)Clone();

            // Clear the existing Sector Number because this should become a new Sector
            // if it is ever added to the Sector Container Collection
            sectorInformation.Number = 0;

            return sectorInformation;
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
            SectorDescription.Read(binaryReader);

            long currentPosition = binaryReader.BaseStream.Position;

            binaryReader.BaseStream.Position = SectorDescription.NotesOffset;
            Notes = new String(binaryReader.ReadChars(SectorDescription.NotesLength));

            // Position back to after the end of the SectorDescriptor
            binaryReader.BaseStream.Position = currentPosition;
        }

        /// <summary>
        /// Writes property values to a binary file using the binary writer
        /// </summary>
        /// <param name="binaryWriter">The instantiated binary writer used to write property information</param>
        /// <exception cref="SectorInformationWriteException"><c>SectorInformationWriteException</c>.</exception>
        public void Write(BinaryWriter binaryWriter)
        {
            try
            {
                SectorDescription.Write(binaryWriter);

                long currentPosition = binaryWriter.BaseStream.Position;

                binaryWriter.BaseStream.Position = SectorDescription.NotesOffset;

                binaryWriter.Write(BinaryFileHelper.StringToCharArray(Notes, SectorDescription.NotesLength));

                // Position back to after the end of the SectorDescriptor
                binaryWriter.BaseStream.Position = currentPosition;
            }
            catch (Exception eek)
            {
                var message = String.Format("Error encountered writing sector information: {0} {1} = {2}", Number, Name,eek.ToString());
            }
        }

        /// <summary>
        /// Returns the size of the object used to calculate file position and offsets
        /// TML_SECTOR_INFO::TML_SECTOR_DESCRIPTOR
        /// with Notes held elsewhere in the file
        /// </summary>
        /// <returns>Object size</returns>
        public int FileRecordSize()
        {
            return DataSize();
        }

        /// <summary>
        /// Returns the size of the object data
        /// </summary>
        /// <returns>Object size</returns>
        public int DataSize()
        {
            int sectorInformationSize = 0;

            sectorInformationSize += SectorDescription.FileRecordSize();

            return sectorInformationSize;
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Returns a generic Object that is a copy of "this"
        /// </summary>
        /// <returns>The new SectorInformation Object disguised as an Object</returns>
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
    }
}
