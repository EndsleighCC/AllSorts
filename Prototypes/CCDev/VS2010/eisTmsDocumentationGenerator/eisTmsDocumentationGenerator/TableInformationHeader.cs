using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eisTmsDocumentationGenerator
{
    public class TableInformationHeader : IBinaryFileHandler
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableContainer"></param>
        public TableInformationHeader(/*TableContainer tableContainer*/)
        {
            /* TableContainer = tableContainer; */
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Number of base tables
        /// </summary>
        public Int32 BaseTableCount { get; set; }

        /// <summary>
        /// Number of instances
        /// </summary>
        public Int32 InstanceCount { get; set; }

        /// <summary>
        /// Date last modified
        /// </summary>
        public UInt32 DateLastModified { get; set; }

        /// <summary>
        /// Time last modified
        /// </summary>
        public UInt32 TimeLastModified { get; set; }

        /// <summary>
        /// The owning table container
        /// </summary>
        // public TableContainer TableContainer { get; set; }

        #endregion

        #region IBinaryFileHandler Members

        /// <summary>
        /// Populates properties on the current object reading information from the 
        /// provided binary reader.
        /// </summary>
        /// <param name="binaryReader">The binary reader to read</param>
        public void Read(BinaryReader binaryReader)
        {
            BaseTableCount = binaryReader.ReadInt32();
            InstanceCount = binaryReader.ReadInt32();
            DateLastModified = binaryReader.ReadUInt32();
            TimeLastModified = binaryReader.ReadUInt32();
        }

        /// <summary>
        /// Writes property values to a binary file using the binary writer
        /// </summary>
        /// <param name="binaryWriter">The instantiated binary writer used to write property information</param>
        /// <exception cref="TableInformationHeaderWriteException">Error encountered writing table information header</exception>
        public void Write(BinaryWriter binaryWriter)
        {
            //try
            //{
            //    binaryWriter.Write(TableContainer.TableBaseInformationCount);
            //    binaryWriter.Write(TableContainer.TableInstanceInformationCount);
            //    binaryWriter.Write(DateLastModified);
            //    binaryWriter.Write(TimeLastModified);
            //}
            //catch (Exception eek)
            //{
            //    const string message = "Error encountered writing table information header";

            //    Chassis.Logger.ErrorException(message, eek);

            //    throw new TableInformationHeaderWriteException(message, eek);
            //}
        }

        /// <summary>
        /// Returns the size of the object used to calculate file position and offsets
        /// TML_TABLE_INFO_HEADER
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
            int tableInformationHeaderSize = 0;

            // BaseTableCount
            tableInformationHeaderSize += sizeof(Int32);

            // InstanceCount
            tableInformationHeaderSize += sizeof(Int32);

            // DateLastModified
            tableInformationHeaderSize += sizeof(UInt32);

            // TimeLastModified
            tableInformationHeaderSize += sizeof(UInt32);

            return tableInformationHeaderSize;
        }

        #endregion
    }
}
