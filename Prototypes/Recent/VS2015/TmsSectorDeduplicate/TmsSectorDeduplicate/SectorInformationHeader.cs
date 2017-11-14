using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TmsSectorDeduplicate
{
    [Serializable]
    public class SectorInformationHeader : IBinaryFileHandler
    {
        #region Public Properties

        /// <summary>
        /// Number of sectors
        /// </summary>
        public Int32 SectorCount { get; set; }

        /// <summary>
        /// Date last modified
        /// </summary>
        public UInt32 DateLastModified { get; set; }

        /// <summary>
        /// Time last modified
        /// </summary>
        public UInt32 TimeLastModified { get; set; }

        #endregion

        #region IBinaryFileHandler Members

        /// <summary>
        /// Populates properties on the current object reading information from the 
        /// provided binary reader.
        /// </summary>
        /// <param name="binaryReader">The binary reader to read</param>
        public void Read(BinaryReader binaryReader)
        {
            SectorCount = binaryReader.ReadInt32();
            DateLastModified = binaryReader.ReadUInt32();
            TimeLastModified = binaryReader.ReadUInt32();
        }

        /// <summary>
        /// Writes property values to a binary file using the binary writer
        /// </summary>
        /// <param name="binaryWriter">The instantiated binary writer used to write property information</param>
        /// <exception cref="SectorInformationHeaderWriteException"></exception>
        public void Write(BinaryWriter binaryWriter)
        {
            try
            {
                binaryWriter.Write(SectorCount);
                binaryWriter.Write(DateLastModified);
                binaryWriter.Write(TimeLastModified);
            }
            catch (Exception eek)
            {
                Console.WriteLine("Error encountered writing sector information header = \"{0}\"",
                    eek.ToString());
            }
        }

        /// <summary>
        /// Returns the size of the object used to calculate file position and offsets
        /// TML_SECTOR_INFO_HEADER
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
            int sectorInformationHeaderSize = 0;

            // SectorCount
            sectorInformationHeaderSize += sizeof(Int32);

            // DateLastModified
            sectorInformationHeaderSize += sizeof(UInt32);

            // TimeLastModified
            sectorInformationHeaderSize += sizeof(UInt32);

            return sectorInformationHeaderSize;
        }
        #endregion
    }
}
