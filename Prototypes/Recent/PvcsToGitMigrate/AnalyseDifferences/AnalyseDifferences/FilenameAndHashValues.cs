using System.Numerics;

namespace AnalyseDifferences
{
    public class FilenameAndHashValues
    {
        public FilenameAndHashValues(string filename, BigInteger promotionGroupFileHash, BigInteger repositoryFileHash)
        {
            Filename = filename;
            PromotionGroupFileHash = promotionGroupFileHash;
            RepositoryFileHash = repositoryFileHash;
        }

        public string Filename { get; private set; }
        public BigInteger PromotionGroupFileHash { get; private set; }

        public BigInteger RepositoryFileHash { get; private set; }
    }
}
