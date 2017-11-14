using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TmsSectorDeduplicate
{
    public interface IBinaryFileHandler
    {
        /// <summary>
        /// Populates properties on the current object reading information from the 
        /// provided binary reader.
        /// </summary>
        /// <param name="binaryReader">The binary reader to read</param>
        void Read(BinaryReader binaryReader);

        /// <summary>
        /// Writes property values to a binary file using the binary writer
        /// </summary>
        /// <param name="binaryWriter">The instantiated binary writer used to write property information</param>
        void Write(BinaryWriter binaryWriter);

        /// <summary>
        /// Returns the file size of the object which is used to calculate file position and offsets
        /// </summary>
        /// <returns>Object size</returns>
        int FileRecordSize();

        /// <summary>
        /// Returns the actual node data size of the object
        /// </summary>
        /// <returns>Object size</returns>
        int DataSize();

    } // interface IBinaryFileHandler
}
