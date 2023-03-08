namespace OctogeddonUnpack.Class
{
    class Color
    {
        public byte Red = 0xFF;
        public byte Green = 0xFF;
        public byte Blue = 0xFF;
        public byte Alpha = 0xFF;

        public byte Luminance { get => (byte)System.Math.Min(Red * 0.299 + Green * 0.587 + Blue * 0.114, 255); set => Red = Green = Blue = value; }

        public Color()
        {

        }

        public Color(byte luminance, byte alpha)
        {
            Luminance = luminance;
            Alpha = alpha;
        }

        public Color(byte red, byte green, byte blue, byte alpha)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }

        public Color(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public void Reset(byte luminance, byte alpha)
        {
            Luminance = luminance;
            Alpha = alpha;
        }

        public void Reset(byte red, byte green, byte blue, byte alpha)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }

        public void Reset(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }
    }
}
