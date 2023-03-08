using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace OctogeddonUnpack.Class
{
    class LockBitmap
    {
        Bitmap source = null;
        IntPtr Iptr = IntPtr.Zero;
        BitmapData bitmapData = null;
        int length = 0;
        bool needDispose = false;

        ~LockBitmap()
        {
            if (needDispose) source?.Dispose();
        }

        public byte[] Pixels { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public int Length { get => length; private set => length = value; }

        public LockBitmap(Bitmap source)
        {
            if (source.PixelFormat == PixelFormat.Format32bppArgb)
            {
                this.source = source;
            }
            else
            {
                this.source = new Bitmap(source.Width, source.Height, PixelFormat.Format32bppArgb);
                using (Graphics g = Graphics.FromImage(this.source))
                {
                    g.DrawImageUnscaled(source, 0, 0);
                }
                needDispose = true;
            }
        }

        public void LockBits()
        {
            try
            {
                Width = source.Width;
                Height = source.Height;
                Length = Width * Height;
                int PixelCount = Width * Height;
                Rectangle rect = new Rectangle(0, 0, Width, Height);
                bitmapData = source.LockBits(rect, ImageLockMode.ReadWrite, source.PixelFormat);
                Pixels = new byte[PixelCount * 4];
                Iptr = bitmapData.Scan0;
                Marshal.Copy(Iptr, Pixels, 0, Pixels.Length);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UnlockBits()
        {
            try
            {
                Marshal.Copy(Pixels, 0, Iptr, Pixels.Length);
                source.UnlockBits(bitmapData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Color GetPixel(int x, int y)
        {
            int i = ((y * Width) + x) << 2;
            return new Color(Pixels[i | 2], Pixels[i | 1], Pixels[i], Pixels[i | 3]);
        }

        public void SetPixel(int x, int y, Color color)
        {
            int i = ((y * Width) + x) << 2;
            Pixels[i] = color.Blue;
            Pixels[i | 1] = color.Green;
            Pixels[i | 2] = color.Red;
            Pixels[i | 3] = color.Alpha;
        }

        /// <summary>
        /// type=0为r，1为g，2为b，3为a，4为l
        /// </summary>
        /// <param name="index"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public byte this[int index, byte type]
        {
            get
            {
                switch (type)
                {
                    case 0:
                        return Pixels[(index << 2) | 2];
                    case 1:
                        return Pixels[(index << 2) | 1];
                    case 2:
                        return Pixels[index << 2];
                    case 3:
                        return Pixels[(index << 2) | 3];
                    default:
                        return (byte)Math.Min(Pixels[(index << 2) | 2] * 0.299 + Pixels[(index << 2) | 1] * 0.587 + Pixels[index << 2] * 0.114, 255);
                }
            }
            set
            {
                switch (type)
                {
                    case 0:
                        Pixels[(index << 2) | 2] = value;
                        break;
                    case 1:
                        Pixels[(index << 2) | 1] = value;
                        break;
                    case 2:
                        Pixels[index << 2] = value;
                        break;
                    case 3:
                        Pixels[(index << 2) | 3] = value;
                        break;
                    default:
                        Pixels[(index << 2) | 2] = Pixels[(index << 2) | 1] = Pixels[index << 2] = value;
                        break;
                }
            }
        }

        public Color this[int index]
        {
            get
            {
                int i = index << 2;
                return new Color(Pixels[i | 2], Pixels[i | 1], Pixels[i], Pixels[i | 3]);
            }
            set
            {
                int i = index << 2;
                Pixels[i] = value.Blue;
                Pixels[i | 1] = value.Green;
                Pixels[i | 2] = value.Red;
                Pixels[i | 3] = value.Alpha;
            }
        }
    }
}
