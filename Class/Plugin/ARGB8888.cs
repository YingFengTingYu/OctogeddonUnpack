using System.Drawing;

namespace OctogeddonUnpack.Class
{
    internal static class ARGB8888
    {
        public static Bitmap Read(BinaryStream bs, int width, int height)
        {
            Bitmap image = new Bitmap(width, height);
            LockBitmap pixels = new LockBitmap(image);
            pixels.LockBits();
            byte a, r, g, b;
            for (int i = 0; i < pixels.Length; i++)
            {
                b = bs.ReadByte();
                g = bs.ReadByte();
                r = bs.ReadByte();
                a = bs.ReadByte();
                pixels[i] = new Color(r, g, b, a);
            }
            pixels.UnlockBits();
            return image;
        }

        public static void Write(BinaryStream bs, Bitmap image)
        {
            LockBitmap pixels = new LockBitmap(image);
            pixels.LockBits();
            for (int i = 0; i < pixels.Length; i++)
            {
                bs.WriteByte(pixels[i].Blue);
                bs.WriteByte(pixels[i].Green);
                bs.WriteByte(pixels[i].Red);
                bs.WriteByte(pixels[i].Alpha);
            }
            pixels.UnlockBits();
        }
    }
}