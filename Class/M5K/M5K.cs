using System.IO;

namespace OctogeddonUnpack.Class.M5K
{
    internal static class M5K
    {
        public static void M5KPng(string inFolder)
        {
            string[] a = Dir.GetFiles(inFolder);
            string tempFile = Path.GetTempFileName();
            string tempFile2 = Path.GetTempFileName();
            try
            {
                foreach (string s in a)
                {
                    if (Path.GetExtension(s) != ".m5k") continue;
                    using (BinaryStream bs = BinaryStream.Open(s))
                    {
                        bs.SetPosition(0x70);
                        long b = bs.FindNextUInt16(55928);
                        if (b == -1) continue;
                        bs.Position = b - 12;
                        if (bs.ReadInt16() != 5)
                        {
                            //Console.WriteLine(s + "not 5");
                            //continue;
                        }
                        if (bs.ReadInt16() != 0)
                        {
                            //Console.WriteLine(s + "not 0");
                            //continue;
                        }
                        ushort c = bs.ReadUInt16();
                        ushort d = bs.ReadUInt16();
                        int size = bs.ReadInt32();
                        using (BinaryStream bs2 = BinaryStream.Create(tempFile))
                        {
                            bs2.WriteBytes(bs.ReadBytes(size));
                        }
                        Zlib.Decompress(tempFile, tempFile2);
                        using (BinaryStream bs2 = BinaryStream.Open(tempFile2))
                        {
                            using (var k = ARGB8888.Read(bs2, c, d))
                            {
                                var w = Path.ChangeExtension(s, ".png");
                                if (File.Exists(w))
                                {
                                    File.Delete(w);
                                }
                                k.Save(w);
                            }
                        }
                    }
                }
            }
            finally
            {
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
                if (File.Exists(tempFile2))
                {
                    File.Delete(tempFile2);
                }
            }
        }
    }
}
