using System;
using System.Numerics;
using System.IO;
using System.Security.Cryptography;

namespace AnalyseDifferences
{
    public class NonEolHash
    {
        public NonEolHash(string filename)
        {
            _filename = filename;
        }

        string _filename = null;

        public BigInteger Generate()
        {
            BigInteger bigHash = 0;

            if (!File.Exists(_filename))
            {
                Console.WriteLine("File \"{0}\" does not exist", _filename);
            }
            else
            {
                try
                {
                    // Write the contents of the file excluding all end-of-line characters to a temporary file

                    using (BinaryReader binaryReader = new BinaryReader(File.Open(_filename, FileMode.Open, FileAccess.Read, FileShare.None)))
                    {
                        string tempFilename = Path.GetTempFileName();
                        using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(tempFilename, FileMode.Open, FileAccess.Write, FileShare.None)))
                        {
                            while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
                            {
                                byte byteValue = 0;
                                try
                                {
                                    byteValue = binaryReader.ReadByte();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("ReadByte exception: \"{0}\"", ex.ToString());
                                    byteValue = 0;
                                }
                                switch ((int)byteValue)
                                {
                                    case '\r':
                                    case '\n':
                                        // Don't count EOL characters
                                        break;
                                    default:
                                        binaryWriter.Write(byteValue);
                                        break;
                                } // switch
                            } // while

                        } // BinaryWriter

                        try
                        {
                            byte[] byteHash = null;

                            try
                            {
                                using (MD5 md5 = MD5.Create())
                                {
                                    using (FileStream fileStream = File.OpenRead(tempFilename))
                                    {
                                        byteHash = md5.ComputeHash(fileStream);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("    Exception calculating MD5 Hash for file \"{0}\" = \"{1}\"",
                                                    _filename, ex.ToString());
                            }

                            if (byteHash != null)
                            {
                                // Generate a Big Integer containing the hash
                                bigHash = new BigInteger(byteHash);
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Exception: Calculating MD5 Hash on \"{0}\" = \"{1}\"", tempFilename, ex.ToString());
                        }

                        // Delete the temporary file
                        File.Delete(tempFilename);

                    } // BinaryReader

                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("\"{0}\" does not exist", _filename);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception reading \"{0}\" = \"{1}\"", _filename, ex.ToString());
                }
            }

            return bigHash;
        } // Generate
    } // NonEolHash
}
