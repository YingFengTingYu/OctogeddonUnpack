namespace OctogeddonUnpack.Class.AYGP
{
    internal static class AYGP
    {
        public static void UnPack(string inFile, string outFolder)
        {
            Dir.NewDir(outFolder);
            using (BinaryStream bs = BinaryStream.Open(inFile))
            {
                bs.IdString("AYGP");
                bs.Position = 0x20;
                UnpackRes(outFolder + "\\", bs);
            }
        }

        static void UnpackRes(string p, BinaryStream bs)
        {
            bs.Position += 0x4;
            p += bs.ReadStringByByteHead() + "\\";
            int folderNumber = bs.ReadInt32();
            for (int i = 0; i < folderNumber; i++)
            {
                UnpackRes(p, bs);
            }
            int fileNumber = bs.ReadInt32();
            for (int i = 0; i < fileNumber; i++)
            {
                int size = bs.ReadInt32();
                string name = bs.ReadStringByByteHead();
                size--;
                size -= name.Length;
                name = p + name;
                Dir.NewDir(name, false);
                uint key = (uint)(0x01020304 + size);
                int index = 0;
                using (BinaryStream bs2 = BinaryStream.Create(name))
                {
                    for (int j = 0; j < size; j++)
                    {
                        bs2.WriteByte((byte)(bs.ReadByte() ^ GetByte(ref key, ref index)));
                    }
                }
            }
        }

        static byte GetByte(ref uint lastKey, ref int index)
        {
            byte ans = 0;
            switch (index)
            {
                case 0:
                    ans = (byte)(lastKey & 0xff);
                    break;
                case 1:
                    ans = (byte)((lastKey & 0xff00) >> 8);
                    break;
                case 2:
                    ans = (byte)((lastKey & 0xff0000) >> 16);
                    break;
                case 3:
                    ans = (byte)((lastKey & 0xff000000) >> 24);
                    lastKey++;
                    break;
            }
            index = (index + 1) % 4;
            return ans;
        }
    }
}
