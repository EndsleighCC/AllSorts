using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.IO;

namespace TmsSectorDeduplicate
{
    /// <summary>
    /// Represents a sector and all associated data
    /// </summary>
    [Serializable]
    public class SectorContainer : List<SectorInformation>
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public SectorContainer(Chassis chassis)
        {
            _chassis = chassis;
            InformationHeader = new SectorInformationHeader();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Read only sector information collection
        /// </summary>
        public ReadOnlyCollection<SectorInformation> ReadOnlyInformationCollection
        {
            get { return new ReadOnlyCollection<SectorInformation>(this); }
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Sector information header
        /// </summary>
        internal SectorInformationHeader InformationHeader { get; private set; }

        /// <summary>
        /// A dictionary of sector information objects sorted by sector number
        /// </summary>
        internal SortedDictionary<int, SectorInformation> SortedInformationCollection
        {
            get
            {
                Dictionary<int, SectorInformation> informationCollection =
                    this.ToDictionary(
                        sectorInformation => sectorInformation.SectorDescription.SectorNumber,
                        sectorInformation => sectorInformation);

                return new SortedDictionary<int, SectorInformation>(informationCollection);
            }
        }

        /// <summary>
        /// Chassis
        /// </summary>
        internal Chassis Chassis
        {
            get { return _chassis; }
            set { _chassis = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves the sector information for the specified Sector Number
        /// </summary>
        /// <param name="sectorNumber">The number of the Sector that is required</param>
        /// <returns></returns>
        /// <exception cref="SectorNotPresentException"><c>SectorNotPresentException</c>.</exception>
        public SectorInformation SectorInformationForSectorNumber(int sectorNumber)
        {
            SectorInformation sectorInformationFound =
                this.FirstOrDefault(
                    sectorInformation => sectorInformation.SectorDescription.SectorNumber == sectorNumber);

            if (sectorInformationFound == null)
                throw new SectorNotPresentException("SectorContainer.SectorInformationForSectorNumber(" +
                                                    Convert.ToString(sectorNumber) + ")");

            return sectorInformationFound;
        }

        /// <summary>
        /// Adds a sector information object to the collection
        /// </summary>
        /// <param name="sectorInformation">The new sector information</param>
        /// <returns>The chassis assigned sector number</returns>
        public int AddSectorInformation(SectorInformation sectorInformation)
        {
            int nextSectorNumber = GetNextSectorNumber();

            sectorInformation.Number = nextSectorNumber;

            Add(sectorInformation);

            return nextSectorNumber;
        }

        /// <summary>
        /// Attempts to add the supplied Sector Information at the specified Sector Number.
        /// If the Sector already exists a DuplicateSectorNumberException exception will be thrown
        /// </summary>
        /// <param name="sectorNumber">The number that the Sector should have</param>
        /// <param name="sectorInformation">The Sector Information to be added</param>
        /// <exception cref="DuplicateSectorNumberException"><c>DuplicateSectorNumberException</c>.</exception>
        public void AddSectorInformation(int sectorNumber, SectorInformation sectorInformation)
        {
            try
            {
                SectorInformationForSectorNumber(sectorNumber);
                // A Sector with the specified Number already exists
                throw new DuplicateSectorNumberException("SectorContainer.AddSectorInformation(" + sectorNumber + ") is duplicate");
            }
            catch (SectorNotPresentException)
            {
                // Sector not currently present so add it
                sectorInformation.Number = sectorNumber;
                Add(sectorInformation);
            }
        }

        /// <summary>
        /// Removes a sector information object from the collection
        /// </summary>
        /// <param name="sectorNumber">The number of the sector to remove</param>
        public void RemoveSectorInformation(int sectorNumber)
        {
            SectorInformation sectorInformation = SectorInformationForSectorNumber(sectorNumber);

            Remove(sectorInformation);
        }

        #endregion Public Methods

        #region Internal Methods

        /// <summary>
        /// Get the next available sector number
        /// </summary>
        /// <returns>The next available sector number</returns>
        internal int GetNextSectorNumber()
        {
            SortedDictionary<int, SectorInformation> sortedInformationCollection = SortedInformationCollection;

            // Merges two sequences, in this case the dictionary key of the sector information collection. The keys
            // are compared to see if the gap between them is bigger than 1. If so there is an empty key in the sequence so
            // the new sector can be assigned this instance number. Otherwise the next available iteration is used.
            var potentialMissingKey =
                sortedInformationCollection.Keys.Zip(sortedInformationCollection.Keys.Skip(1), (a, b) => new { a, b }).
                    Where(x => x.b != x.a + 1).FirstOrDefault();

            int sectorIndex;

            if (potentialMissingKey != null)
                sectorIndex = potentialMissingKey.a + 1;
            else
                sectorIndex = sortedInformationCollection.Last().Key + 1;

            return sectorIndex;
        }

        /// <summary>
        /// Populates properties on the current object reading information from the 
        /// provided binary reader.
        /// </summary>
        internal void Read()
        {
            using (
                BinaryReader binaryReader = FileHelper.GetBinaryReader(Chassis.FullReadPath, TmlSectorInfoFile,
                                                                       Encoding.GetEncoding(
                                                                           Constants.FileEncodingCodePageNumber))
                )
            {
                InformationHeader.Read(binaryReader);

                for (int sectorIndex = 0; sectorIndex < InformationHeader.SectorCount; sectorIndex++)
                {
                    var sectorInformation = new SectorInformation();

                    sectorInformation.Read(binaryReader);

                    Add(sectorInformation);
                }

                DumpSectorData(RiskBasedFilename(TmsSectorDumpReadFilename), RiskBasedFilename(TmsSectorDumpReadBackupFilename));
            }
        }

        /// <summary>
        /// Writes the contents of the sector container objects to the sector information file.
        /// </summary>
        /// <exception cref="SectorContainerWriteException">Error encountered writing sector container</exception>
        internal void Write()
        {
            try
            {
                using (
                    BinaryWriter binaryWriter = FileHelper.GetBinaryWriter(Chassis.FullWritePath, TmlSectorInfoFile,
                                                                           Path.Combine(Chassis.FullWritePath,
                                                                                        TmlSectorInfoBackupFile),
                                                                           Encoding.GetEncoding(
                                                                               Constants.FileEncodingCodePageNumber)))
                {
                    InformationHeader.SectorCount = Count;

                    InformationHeader.Write(binaryWriter);

                    // Calculate length of Sector Information collection
                    int sectorInformationCollectionSize =
                        this.Sum(sectorInformation => sectorInformation.FileRecordSize());

                    long sectorInformationPositionItemsCurrentIndex = binaryWriter.BaseStream.Position +
                                                                      sectorInformationCollectionSize;

                    // Iterate through the sector information collection and set the notes length and offset values
                    foreach (SectorInformation sectorInformation in this)
                    {
                        int notesLength = sectorInformation.Notes.Length;

                        sectorInformation.SectorDescription.NotesOffset =
                            (int)sectorInformationPositionItemsCurrentIndex;

                        sectorInformation.SectorDescription.NotesLength = notesLength;

                        sectorInformationPositionItemsCurrentIndex += notesLength;
                    }

                    foreach (SectorInformation sectorInformation in this)
                    {
                        sectorInformation.Write(binaryWriter);
                    }
                }
                DumpSectorData(RiskBasedFilename(TmsSectorDumpWriteFilename), RiskBasedFilename(TmsSectorDumpWriteBackupFilename));
            }
            catch (Exception eek)
            {
                const string message = "Error encountered writing sector container";

                Chassis.Logger.ErrorException(message, eek);

                throw new SectorContainerWriteException(message, eek);
            }

            try
            {
                DumpSectorData(RiskBasedFilename(TmsSectorDumpWriteFilename), RiskBasedFilename(TmsSectorDumpWriteBackupFilename));
            }
            catch (Exception eek)
            {
                Chassis.Logger.ErrorException("Error encountered dumping sector data", eek);
            }
        }

        #endregion

        #region Private Methods

        private string RiskBasedFilename(string filename)
        {
            return _chassis.RiskName + "." + filename;
        }

        private void DumpSectorData(string filename, string backupFilename)
        {
            using (StreamWriter sectorDataDiagnostics = _chassis.DiagnosticStream(DiagnosticSectorContainerCategory, filename, backupFilename))
            {
                if (sectorDataDiagnostics != null)
                {
                    {
                        sectorDataDiagnostics.WriteLine("Sector Information in File Order");
                        foreach (SectorInformation sectorInformation in this)
                        {
                            sectorDataDiagnostics.WriteLine("Number={0}, Name=\"{1}\", Notes=\"{2}\"",
                                                            sectorInformation.SectorDescription.SectorNumber,
                                                            sectorInformation.SectorDescription.SectorName,
                                                            sectorInformation.Notes);
                        }
                    }
                }
            }

        }

        #endregion

        #region Private Properties

        private Chassis _chassis;

        #endregion Private Properties

        #region Private Constants

        private const string TmlSectorInfoBackupFile = "SECTINFO.BAK";
        private const string TmlSectorInfoFile = "SECTINFO.DAT";

        private const string DiagnosticSectorContainerCategory = "SECTOR";

        #endregion

        #region Public Constants

        public const string TmsSectorDumpReadFilename = "SectorDumpRead.log";
        public const string TmsSectorDumpReadBackupFilename = "SectorDumpRead.bak";

        public const string TmsSectorDumpWriteFilename = "SectorDumpWrite.log";
        public const string TmsSectorDumpWriteBackupFilename = "SectorDumpWrite.bak";

        public const string SectorsArgument = "/sectors";

        #endregion

    }
}
