using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CompareSourceVolumes
{
    public class PvcsPromotionGroupHierarchy : SortedSet<PvcsPromotionGroupHierarchy.PvcsPromotionGroupHierarchyEntry>
    {
        public PvcsPromotionGroupHierarchy()
        {
            if (!File.Exists(_pvcsConfigurationPathAndFilename))
            {
                Console.WriteLine("PvcsPromotionGroupHierarchy : PVCS Project Configuration File \"{0}\" was not found", _pvcsConfigurationPathAndFilename);
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("{0} : Reading PVCS Project Configuration File \"{1}\"",
                                        DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss"), _pvcsConfigurationPathAndFilename);
                try
                {
                    using (StreamReader fileStream = new StreamReader(_pvcsConfigurationPathAndFilename))
                    {
                        while (!fileStream.EndOfStream)
                        {
                            string fileLine = fileStream.ReadLine().Trim();

                            string [] configToken = fileLine.Split(' ');
                            if ( ( configToken != null ) && ( configToken[0] == "PROMOTE") )
                            {
                                // Found a Promotion Group entry

                                // Archive Path, Promotion Group Name
                                this.Add(new PvcsPromotionGroupHierarchyEntry( configToken[1], configToken[2] ));

                            } // Found a Promotion Group entry
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("PvcsPromotionGroupHierarchy : Exception {0}", ex.ToString());
                }
            }
        }

        public class PvcsPromotionGroupHierarchyEntry : IComparable<PvcsPromotionGroupHierarchyEntry>
        {
            public PvcsPromotionGroupHierarchyEntry(string promotionGroupName, string nextHigherPromotionGroup)
            {
                PromotionGroupName = promotionGroupName;
                NextHigherPromotionGroupName = nextHigherPromotionGroup;
            }

            public string PromotionGroupName { get; private set; }
            public string NextHigherPromotionGroupName { get; private set; }

            public int CompareTo( PvcsPromotionGroupHierarchyEntry that )
            {
                return String.Compare(this.PromotionGroupName, that.PromotionGroupName);
            }

        }

        private const string _pvcsConfigurationPathAndFilename = @"\\adebs03\SysPVCS80\PVCS\EIS\ProjCfg\EISProjectDB.cfg";

    } // PvcsPromotionGroupHierarchy
}
