using System.IO;
using System.IO.Compression;

namespace OctogeddonUnpack.Class
{
    internal class Zlib
    {
        public static void Decompress(string inFile, string outFile)
        {
            using (FileStream infileStream = new FileStream(inFile, FileMode.Open))
            {
                if (infileStream.Length <= 0x6)
                {
                    using (FileStream outfileStream = new FileStream(outFile, FileMode.Create))
                    {

                    }
                    return;
                }
                infileStream.Seek(0x2, SeekOrigin.Begin);
                using (DeflateStream zlibStream = new DeflateStream(infileStream, CompressionMode.Decompress))
                {
                    using (FileStream outfileStream = new FileStream(outFile, FileMode.Create))
                    {
                        zlibStream.CopyTo(outfileStream);
                        outfileStream.Flush();
                    }
                }
            }
        }
    }
}
