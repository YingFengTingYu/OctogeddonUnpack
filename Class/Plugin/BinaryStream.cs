using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OctogeddonUnpack.Class
{
    /// <summary>
    /// 提供对二进制文件流操作的基础类，继承自FileStream
    /// </summary>
    public class BinaryStream : FileStream
    {
        //前言：基础变量声明
        BinaryReader BR;
        BinaryWriter BW;
        Encoding encoding = Encoding.UTF8;

        public readonly string FileName;
        /// <summary>
        /// 设置二进制流默认字符串编码
        /// </summary>
        /// <param name="encoding"></param>
        public void SetEncoding(Encoding encoding)
        {
            this.encoding = encoding;
        }
        /// <summary>
        /// 关闭流
        /// </summary>
        public new void Close()
        {
            BW.Close();
            BR.Close();
            base.Close();
        }
        /// <summary>
        /// 释放流
        /// </summary>
        public new void Dispose()
        {
            BW.Dispose();
            BR.Dispose();
            base.Dispose();
        }
        //第一部分：流的构造
        /// <summary>
        /// 用于在类的静态方法中构造二进制流
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileMode"></param>
        BinaryStream(string filePath, FileMode fileMode) : base(filePath, fileMode, FileAccess.ReadWrite)
        {
            BR = new BinaryReader(this);
            BW = new BinaryWriter(this);
            FileName = filePath;
        }
        /// <summary>
        /// 用于在指定目录新建文件
        /// </summary>
        /// <param name="filePath">string型，指明生成的文件所在目录</param>
        /// <returns>BinaryStream型，这个文件的二进制流</returns>
        public static BinaryStream Create(string filePath)
        {
            return new BinaryStream(filePath, FileMode.Create);
        }
        /// <summary>
        /// 用于打开指定目录的文件，若文件不存在则在指定目录新建文件
        /// </summary>
        /// <param name="filePath">string型，指明被打开的文件所在目录</param>
        /// <returns>BinaryStream型，这个文件的二进制流</returns>
        public static BinaryStream OpenOrCreate(string filePath)
        {
            return new BinaryStream(filePath, FileMode.OpenOrCreate);
        }
        /// <summary>
        /// 用于打开指定目录的文件，若文件不存在则报错
        /// </summary>
        /// <param name="filePath">string型，指明被打开的文件所在目录</param>
        /// <returns>BinaryStream型，这个文件的二进制流</returns>
        public static BinaryStream Open(string filePath)
        {
            return new BinaryStream(filePath, FileMode.Open);
        }
        //第二部分：流的位置
        /// <summary>
        /// 获取流的当前位置
        /// </summary>
        /// <returns>long型，流的当前位置</returns>
        public long GetPosition()
        {
            return base.Position;
        }
        /// <summary>
        /// 设置流的当前位置
        /// </summary>
        /// <param name="position">long型，指明设置的文件流位置</param>
        public void SetPosition(long position)
        {
            Seek(position, SeekOrigin.Begin);
        }
        /// <summary>
        /// 增加流的当前位置
        /// </summary>
        /// <param name="positon">long型，指明增加的文件流位置</param>
        public void AddPosition(long positon)
        {
            Position += positon;
        }

        /// <summary>
        /// 减少流的当前位置
        /// </summary>
        /// <param name="positon">long型，指明减少的文件流位置</param>
        public void SubPosition(long positon)
        {
            Position = Math.Max(0, Position - positon);
        }

        /// <summary>
        /// 反向设置流的当前位置
        /// </summary>
        /// <param name="position">long型，指明设置的文件流位置</param>
        public void SetPositionByLast(long position)
        {
            Seek(-Math.Abs(position), SeekOrigin.End);
        }

        //第三部分：文件魔数检测部分
        /// <summary>
        /// 验证下一字节是否与指定值相同
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void IdByte(byte value)
        {
            if (BR.ReadByte() != value) throw new FileCorruptionException("内容不匹配");
        }

        /// <summary>
        /// 验证下一有符号字节是否与指定值相同
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void IdSByte(sbyte value)
        {
            if (BR.ReadSByte() != value) throw new FileCorruptionException("内容不匹配");
        }

        /// <summary>
        /// 验证下一16位整数是否与指定值相同
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void IdInt16(short value)
        {
            if (BR.ReadInt16() != value) throw new FileCorruptionException("内容不匹配");
        }

        /// <summary>
        /// 验证下一16位无符号整数是否与指定值相同
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void IdUInt16(ushort value)
        {
            if (BR.ReadUInt16() != value) throw new FileCorruptionException("内容不匹配");
        }

        /// <summary>
        /// 验证下一32位整数是否与指定值相同
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void IdInt32(int value)
        {
            if (BR.ReadInt32() != value) throw new FileCorruptionException(Position + "内容不匹配");
        }

        /// <summary>
        /// 验证下一32位无符号整数是否与指定值相同
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void IdUInt32(uint value)
        {
            if (BR.ReadUInt32() != value) throw new FileCorruptionException("内容不匹配");
        }

        /// <summary>
        /// 验证下一64位整数是否与指定值相同
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void IdInt64(long value)
        {
            if (BR.ReadInt64() != value) throw new FileCorruptionException("内容不匹配");
        }

        /// <summary>
        /// 验证下一64位无符号整数是否与指定值相同
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void IdUInt64(ulong value)
        {
            if (BR.ReadUInt64() != value) throw new FileCorruptionException("内容不匹配");
        }

        /// <summary>
        /// 验证下一字符是否与指定值相同
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void IdChar(char value)
        {
            if (BR.ReadChar() != value) throw new FileCorruptionException("内容不匹配");
        }

        /// <summary>
        /// 验证下一布尔值是否与指定值相同
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void IdBoolean(bool value)
        {
            if (BR.ReadBoolean() != value) throw new FileCorruptionException("内容不匹配");
        }

        /// <summary>
        /// 验证下一字符串是否与指定值相同
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void IdString(string str)
        {
            if (encoding.GetString(BR.ReadBytes(str.Length)) != str) throw new FileCorruptionException("内容不匹配");
        }

        /// <summary>
        /// 验证下一字节数组是否与指定值相同
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void IdBytes(byte[] bytes)
        {
            var b = BR.ReadBytes(bytes.Length); //保证读错了加的偏移也要一样
            for (int i = 0; i < b.Length; i++)
            {
                if (b[i] != bytes[i])
                {
                    throw new FileCorruptionException("内容不匹配");
                }
            }
        }

        //第四部分：按小端序查找内容
        /// <summary>
        /// 查找指定布尔值
        /// </summary>
        /// <param name="value">bool型，被查找的bool值</param>
        /// <returns>long型，找到则返回第一个值所在位置，未找到则返回-1</returns>
        public long FindNextBoolean(bool value)
        {
            long backoffset = Position;
            long ans = -1;
            long size = Length;
            for (long i = backoffset; i < size; i++)
            {
                if (BR.ReadBoolean() == value)
                {
                    ans = i;
                    break;
                }
            }
            Seek(backoffset, SeekOrigin.Begin);
            return ans;
        }
        /// <summary>
        /// 查找指定字符
        /// </summary>
        /// <param name="value">char型，被查找的字符值</param>
        /// <returns>long型，找到则返回第一个值所在位置，未找到则返回-1</returns>
        public long FindNextChar(char value)
        {
            long backoffset = Position;
            long ans = -1;
            long size = Length;
            for (long i = backoffset; i < size - 1; i++)
            {
                if (BR.ReadChar() == value)
                {
                    ans = i;
                    break;
                }
                Seek(-1, SeekOrigin.Current);
            }
            Seek(backoffset, SeekOrigin.Begin);
            return ans;
        }
        /// <summary>
        /// 查找指定字节
        /// </summary>
        /// <param name="value">byte型，被查找的字节值</param>
        /// <returns>long型，找到则返回第一个值所在位置，未找到则返回-1</returns>
        public long FindNextByte(byte value)
        {
            long backoffset = Position;
            long ans = -1;
            long size = Length;
            for (long i = backoffset; i < size; i++)
            {
                if (BR.ReadByte() == value)
                {
                    ans = i;
                    break;
                }
            }
            Seek(backoffset, SeekOrigin.Begin);
            return ans;
        }
        /// <summary>
        /// 查找指定无符号字节
        /// </summary>
        /// <param name="value">sbyte型，被查找的无符号字节值</param>
        /// <returns>long型，找到则返回第一个值所在位置，未找到则返回-1</returns>
        public long FindNextSByte(sbyte value)
        {
            long backoffset = Position;
            long ans = -1;
            long size = Length;
            for (long i = backoffset; i < size; i++)
            {
                if (BR.ReadSByte() == value)
                {
                    ans = i;
                    break;
                }
            }
            Seek(backoffset, SeekOrigin.Begin);
            return ans;
        }
        /// <summary>
        /// 查找指定16位有符号整数
        /// </summary>
        /// <param name="value">short型，被查找的值</param>
        /// <returns>long型，找到则返回第一个值所在位置，未找到则返回-1</returns>
        public long FindNextInt16(short value)
        {
            long backoffset = Position;
            long ans = -1;
            long size = Length;
            for (long i = backoffset; i < size - 1; i++)
            {
                if (BR.ReadInt16() == value)
                {
                    ans = i;
                    break;
                }
                Seek(-1, SeekOrigin.Current);
            }
            Seek(backoffset, SeekOrigin.Begin);
            return ans;
        }
        /// <summary>
        /// 查找指定16位无符号整数
        /// </summary>
        /// <param name="value">ushort型，被查找的值</param>
        /// <returns>long型，找到则返回第一个值所在位置，未找到则返回-1</returns>
        public long FindNextUInt16(ushort value)
        {
            long backoffset = Position;
            long ans = -1;
            long size = Length;
            for (long i = backoffset; i < size - 1; i++)
            {
                if (BR.ReadUInt16() == value)
                {
                    ans = i;
                    break;
                }
                Seek(-1, SeekOrigin.Current);
            }
            Seek(backoffset, SeekOrigin.Begin);
            return ans;
        }
        /// <summary>
        /// 查找指定32位有符号整数
        /// </summary>
        /// <param name="value">int型，被查找的值</param>
        /// <returns>long型，找到则返回第一个值所在位置，未找到则返回-1</returns>
        public long FindNextInt32(int value)
        {
            long backoffset = Position;
            long ans = -1;
            long size = Length;
            for (long i = backoffset; i < size - 3; i++)
            {
                if (BR.ReadInt32() == value)
                {
                    ans = i;
                    break;
                }
                Seek(-3, SeekOrigin.Current);
            }
            Seek(backoffset, SeekOrigin.Begin);
            return ans;
        }
        /// <summary>
        /// 查找指定32位无符号整数
        /// </summary>
        /// <param name="value">uint型，被查找的值</param>
        /// <returns>long型，找到则返回第一个值所在位置，未找到则返回-1</returns>
        public long FindNextUInt32(uint value)
        {
            long backoffset = Position;
            long ans = -1;
            long size = Length;
            for (long i = backoffset; i < size - 3; i++)
            {
                if (BR.ReadUInt32() == value)
                {
                    ans = i;
                    break;
                }
                Seek(-3, SeekOrigin.Current);
            }
            Seek(backoffset, SeekOrigin.Begin);
            return ans;
        }
        /// <summary>
        /// 查找指定64位有符号整数
        /// </summary>
        /// <param name="value">long型，被查找的值</param>
        /// <returns>long型，找到则返回第一个值所在位置，未找到则返回-1</returns>
        public long FindNextInt64(long value)
        {
            long backoffset = Position;
            long ans = -1;
            long size = Length;
            for (long i = backoffset; i < size - 7; i++)
            {
                if (BR.ReadInt64() == value)
                {
                    ans = i;
                    break;
                }
                Seek(-7, SeekOrigin.Current);
            }
            Seek(backoffset, SeekOrigin.Begin);
            return ans;
        }
        /// <summary>
        /// 查找指定64位无符号整数
        /// </summary>
        /// <param name="value">ulong型，被查找的值</param>
        /// <returns>long型，找到则返回第一个值所在位置，未找到则返回-1</returns>
        public long FindNextUInt64(ulong value)
        {
            long backoffset = Position;
            long ans = -1;
            long size = Length;
            for (long i = backoffset; i < size - 7; i++)
            {
                if (BR.ReadUInt64() == value)
                {
                    ans = i;
                    break;
                }
                Seek(-7, SeekOrigin.Current);
            }
            Seek(backoffset, SeekOrigin.Begin);
            return ans;
        }
        /// <summary>
        /// 查找指定32位浮点小数
        /// </summary>
        /// <param name="value">float型，被查找的值</param>
        /// <returns>long型，找到则返回第一个值所在位置，未找到则返回-1</returns>
        public long FindNextFloat32(float value)
        {
            long backoffset = Position;
            long ans = -1;
            long size = Length;
            for (long i = backoffset; i < size - 3; i++)
            {
                if (BR.ReadSingle() == value)
                {
                    ans = i;
                    break;
                }
                Seek(-3, SeekOrigin.Current);
            }
            Seek(backoffset, SeekOrigin.Begin);
            return ans;
        }
        /// <summary>
        /// 查找指定32位浮点小数
        /// </summary>
        /// <param name="value">float型，被查找的值</param>
        /// <returns>long型，找到则返回第一个值所在位置，未找到则返回-1</returns>
        public long FindNextSingle(float value) => FindNextFloat32(value);
        /// <summary>
        /// 查找指定64位浮点小数
        /// </summary>
        /// <param name="value">double型，被查找的值</param>
        /// <returns>long型，找到则返回第一个值所在位置，未找到则返回-1</returns>
        public long FindNextFloat64(double value)
        {
            long backoffset = Position;
            long ans = -1;
            long size = Length;
            for (long i = backoffset; i < size - 7; i++)
            {
                if (BR.ReadDouble() == value)
                {
                    ans = i;
                    break;
                }
                Seek(-7, SeekOrigin.Current);
            }
            Seek(backoffset, SeekOrigin.Begin);
            return ans;
        }
        /// <summary>
        /// 查找指定64位浮点小数
        /// </summary>
        /// <param name="value">double型，被查找的值</param>
        /// <returns>long型，找到则返回第一个值所在位置，未找到则返回-1</returns>
        public long FindNextDouble(double value) => FindNextDouble(value);
        /// <summary>
        /// 查找指定128位十进制数
        /// </summary>
        /// <param name="value">decimal型，被查找的值</param>
        /// <returns>long型，找到则返回第一个值所在位置，未找到则返回-1</returns>
        public long FindNextDecimal(decimal value)
        {
            long backoffset = Position;
            long ans = -1;
            long size = Length;
            for (long i = backoffset; i < size - 15; i++)
            {
                if (BR.ReadDecimal() == value)
                {
                    ans = i;
                    break;
                }
                Seek(-15, SeekOrigin.Current);
            }
            Seek(backoffset, SeekOrigin.Begin);
            return ans;
        }
        //第五部分：按小端序读取指定内容，并增加流位置
        /// <summary>
        /// 读取字节
        /// </summary>
        /// <returns></returns>
        public new byte ReadByte()
        {
            return BR.ReadByte();
        }

        /// <summary>
        /// 读取有符号字节
        /// </summary>
        /// <returns></returns>
        public sbyte ReadSByte()
        {
            return BR.ReadSByte();
        }

        /// <summary>
        /// 读取16位整数
        /// </summary>
        /// <returns></returns>
        public short ReadInt16()
        {
            return BR.ReadInt16();
        }

        /// <summary>
        /// 读取16位无符号整数
        /// </summary>
        /// <returns></returns>
        public ushort ReadUInt16()
        {
            return BR.ReadUInt16();
        }

        public int ReadUInt24()
        {
            var bt = new byte[4];
            base.Read(bt, 0, 3);
            return BitConverter.ToInt32(bt, 0);
        }
        /// <summary>
        /// 读取32位整数
        /// </summary>
        /// <returns></returns>
        public int ReadInt32()
        {
            return BR.ReadInt32();
        }

        /// <summary>
        /// 读取32位无符号整数
        /// </summary>
        /// <returns></returns>
        public uint ReadUInt32()
        {
            return BR.ReadUInt32();
        }

        public long ReadUInt48()
        {
            var bt = new byte[8];
            base.Read(bt, 0, 6);
            return BitConverter.ToInt64(bt, 0);
        }

        public long ReadUInt48ByBigEndian()
        {
            var bt = new byte[8];
            base.Read(bt, 2, 6);
            Array.Reverse(bt);
            return BitConverter.ToInt64(bt, 0);
        }

        /// <summary>
        /// 读取64位整数
        /// </summary>
        /// <returns></returns>
        public long ReadInt64()
        {
            return BR.ReadInt64();
        }

        /// <summary>
        /// 读取64位无符号整数
        /// </summary>
        /// <returns></returns>
        public ulong ReadUInt64()
        {
            return BR.ReadUInt64();
        }

        /// <summary>
        /// 读取32位单精度浮点数
        /// </summary>
        /// <returns></returns>
        public float ReadFloat32()
        {
            return BR.ReadSingle();
        }

        /// <summary>
        /// 读取32位单精度浮点数
        /// </summary>
        /// <returns></returns>
        public float ReadSingle() => ReadFloat32();

        /// <summary>
        /// 读取64位双精度浮点数
        /// </summary>
        /// <returns></returns>
        public double ReadFloat64()
        {
            return BR.ReadDouble();
        }

        /// <summary>
        /// 读取64位双精度浮点数
        /// </summary>
        /// <returns></returns>
        public double ReadDouble() => ReadFloat64();

        /// <summary>
        /// 读取128位十进制数
        /// </summary>
        /// <returns></returns>
        public decimal ReadDecimal()
        {
            return BR.ReadDecimal();
        }

        /// <summary>
        /// 读取字符
        /// </summary>
        /// <returns></returns>
        public char ReadChar()
        {
            return BR.ReadChar();
        }

        /// <summary>
        /// 读取布尔值
        /// </summary>
        /// <returns></returns>
        public bool ReadBoolean()
        {
            return BR.ReadBoolean();
        }

        /// <summary>
        /// 读取字节数组
        /// </summary>
        /// <returns></returns>
        public byte[] ReadBytes(int count)
        {
            return BR.ReadBytes(count);
        }

        /// <summary>
        /// 读取字符数组
        /// </summary>
        /// <returns></returns>
        public char[] ReadChars(int count)
        {
            return BR.ReadChars(count);
        }
        /// <summary>
        /// 按7位压缩整数字符长度头读取字符串，即字符串之前存储了一个varint型的数值表明此字符串长度
        /// </summary>
        /// <returns>string型，读取所得字符串</returns>
        public string ReadStringByVarintHead()
        {
            return encoding.GetString(BR.ReadBytes(ReadVarint32())).Replace("\0", "");
        }
        /// <summary>
        /// 按1字节字符长度头读取字符串，即字符串之前存储了一个byte型的数值表明此字符串长度
        /// </summary>
        /// <returns>string型，读取所得字符串</returns>
        public string ReadStringByByteHead()
        {
            return encoding.GetString(BR.ReadBytes(BR.ReadByte())).Replace("\0", "");
        }
        /// <summary>
        /// 按指定长度读取字符串
        /// </summary>
        /// <param name="count">int型，字符串长度</param>
        /// <returns>string型，读取所得字符串</returns>
        public string ReadString(int count)
        {
            Console.WriteLine(Position);
            return encoding.GetString(BR.ReadBytes(count)).Replace("\0", "");
        }
        /// <summary>
        /// 按4字节字符长度头读取字符串，即字符串之前存储了一个int型的数值表明此字符串长度
        /// </summary>
        /// <returns>string型，读取所得字符串</returns>
        public string ReadStringByInt32Head()
        {
            return encoding.GetString(BR.ReadBytes(BR.ReadInt32())).Replace("\0", "");
        }
        /// <summary>
        /// 按4字节字符长度头读取字符串，即字符串之前存储了一个int型的数值表明此字符串长度，但此4字节数是按照大端序存储
        /// </summary>
        /// <returns>string型，读取所得字符串</returns>
        public string ReadStringByInt32BigEndianHead()
        {
            return encoding.GetString(BR.ReadBytes(ReadInt32ByBigEndian())).Replace("\0", "");
        }
        /// <summary>
        /// 小端序读取字符串
        /// </summary>
        /// <param name="count">int型，字符串长度</param>
        /// <returns>string型，读取所得字符串</returns>
        public string ReadStringByLittleEndian(int count)
        {
            byte[] bytes = BR.ReadBytes(count);
            Array.Reverse(bytes);
            return encoding.GetString(bytes).Replace("\0", "");
        }

        /// <summary>
        /// 读取字符串直到0x0
        /// </summary>
        /// <returns></returns>
        public string ReadStringByEmpty()
        {
            List<byte> bytes = new List<byte>();
            byte tp;
            while (true)
            {
                if ((tp = BR.ReadByte()) == 0)
                {
                    break;
                }
                bytes.Add(tp);
            }
            return encoding.GetString(bytes.ToArray());
        }

        public int ReadVarint32()
        {
            //方法来源于伟大的微软，只不过微软偷偷把这方法藏起来了
            int num = 0;
            int num2 = 0;
            byte b;
            do
            {
                if (num2 == 35)
                {
                    throw new FormatException("数据过大！");
                }
                b = ReadByte();
                num |= (b & 0x7F) << num2;
                num2 += 7;
            }
            while ((b & 0x80) != 0);
            return num;
        }

        public uint ReadUVarint32()
        {
            //方法来源于伟大的微软，只不过微软偷偷把这方法藏起来了
            int num = 0;
            int num2 = 0;
            byte b;
            do
            {
                if (num2 == 35)
                {
                    throw new FormatException("数据过大！");
                }
                b = ReadByte();
                num |= (b & 0x7F) << num2;
                num2 += 7;
            }
            while ((b & 0x80) != 0);
            return (uint)num;
        }

        public int ReadZigZag32()
        {
            uint n = ReadUVarint32();
            if ((n & 0b1) == 0)
            {
                return (int)(n >> 1);
            }
            return -(int)((n + 1) >> 1);
        }

        public long ReadVarint64()
        {
            //方法来源于伟大的微软，只不过微软偷偷把这方法藏起来了
            long num = 0;
            int num2 = 0;
            byte b;
            do
            {
                if (num2 == 70)
                {
                    throw new FormatException("数据过大！");
                }
                b = ReadByte();
                num |= ((long)(b & 0x7F)) << num2;
                num2 += 7;
            }
            while ((b & 0x80) != 0);
            return num;
        }

        public ulong ReadUVarint64()
        {
            //方法来源于伟大的微软，只不过微软偷偷把这方法藏起来了
            long num = 0;
            int num2 = 0;
            byte b;
            do
            {
                if (num2 == 70)
                {
                    throw new FormatException("数据过大！");
                }
                b = ReadByte();
                num |= ((long)(b & 0x7F)) << num2;
                num2 += 7;
            }
            while ((b & 0x80) != 0);
            return (ulong)num;
        }

        public long ReadZigZag64()
        {
            ulong n = ReadUVarint64();
            if ((n & 0b1) == 0)
            {
                return (long)(n >> 1);
            }
            return -(long)((n + 1) >> 1);
        }

        //第六部分：按大端序读取内容并增加流位置
        /// <summary>
        /// 读取大端序16位整数
        /// </summary>
        /// <returns></returns>
        public short ReadInt16ByBigEndian()
        {
            var bt = BR.ReadBytes(2);
            Array.Reverse(bt);
            return BitConverter.ToInt16(bt, 0);
        }

        /// <summary>
        /// 读取大端序16位无符号整数
        /// </summary>
        /// <returns></returns>
        public ushort ReadUInt16ByBigEndian()
        {
            var bt = BR.ReadBytes(2);
            Array.Reverse(bt);
            return BitConverter.ToUInt16(bt, 0);
        }

        /// <summary>
        /// 读取大端序32位整数
        /// </summary>
        /// <returns></returns>
        public int ReadInt32ByBigEndian()
        {
            var bt = BR.ReadBytes(4);
            Array.Reverse(bt);
            return BitConverter.ToInt32(bt, 0);
        }

        /// <summary>
        /// 读取大端序32位无符号整数
        /// </summary>
        /// <returns></returns>
        public uint ReadUInt32ByBigEndian()
        {
            var bt = BR.ReadBytes(4);
            Array.Reverse(bt);
            return BitConverter.ToUInt32(bt, 0);
        }

        /// <summary>
        /// 读取大端序64位整数
        /// </summary>
        /// <returns></returns>
        public long ReadInt64ByBigEndian()
        {
            var bt = BR.ReadBytes(8);
            Array.Reverse(bt);
            return BitConverter.ToInt64(bt, 0);
        }

        /// <summary>
        /// 读取大端序64位无符号整数
        /// </summary>
        /// <returns></returns>
        public ulong ReadUInt64ByBigEndian()
        {
            var bt = BR.ReadBytes(8);
            Array.Reverse(bt);
            return BitConverter.ToUInt64(bt, 0);
        }

        /// <summary>
        /// 读取大端序32位单精度浮点数
        /// </summary>
        /// <returns></returns>
        public float ReadFloat32ByBigEndian()
        {
            var bt = BR.ReadBytes(4);
            Array.Reverse(bt);
            return BitConverter.ToSingle(bt, 0);
        }

        /// <summary>
        /// 读取大端序32位单精度浮点数
        /// </summary>
        /// <returns></returns>
        public float ReadSingleByBigEndian() => ReadFloat32ByBigEndian();

        /// <summary>
        /// 读取大端序64位双精度浮点数
        /// </summary>
        /// <returns></returns>
        public double ReadFloat64ByBigEndian()
        {
            var bt = BR.ReadBytes(8);
            Array.Reverse(bt);
            return BitConverter.ToDouble(bt, 0);
        }

        /// <summary>
        /// 读取大端序64位双精度浮点数
        /// </summary>
        /// <returns></returns>
        public double ReadDoubleByBigEndian() => ReadFloat64ByBigEndian();

        //第七部分：按小端序写入并增加流位置
        /// <summary>
        /// 写入字节
        /// </summary>
        /// <returns></returns>
        public new void WriteByte(byte value)
        {
            BW.Write(value);
        }

        /// <summary>
        /// 写入有符号字节
        /// </summary>
        /// <returns></returns>
        public void WriteSByte(sbyte value)
        {
            BW.Write(value);
        }

        /// <summary>
        /// 写入16位整数
        /// </summary>
        /// <returns></returns>
        public void WriteInt16(short value)
        {
            BW.Write(value);
        }

        /// <summary>
        /// 写入16位无符号整数
        /// </summary>
        /// <returns></returns>
        public void WriteUInt16(ushort value)
        {
            BW.Write(value);
        }

        /// <summary>
        /// 写入32位整数
        /// </summary>
        /// <returns></returns>
        public void WriteInt32(int value)
        {
            BW.Write(value);
        }

        /// <summary>
        /// 写入32位无符号整数
        /// </summary>
        /// <returns></returns>
        public void WriteUInt32(uint value)
        {
            BW.Write(value);
        }

        /// <summary>
        /// 写入64位整数
        /// </summary>
        /// <returns></returns>
        public void WriteInt64(long value)
        {
            BW.Write(value);
        }

        /// <summary>
        /// 写入64位无符号整数
        /// </summary>
        /// <returns></returns>
        public void WriteUInt64(ulong value)
        {
            BW.Write(value);
        }

        /// <summary>
        /// 写入32位单精度浮点数
        /// </summary>
        /// <returns></returns>
        public void WriteFloat32(float value)
        {
            BW.Write(value);
        }

        /// <summary>
        /// 写入32位单精度浮点数
        /// </summary>
        /// <returns></returns>
        public void WriteSingle(float value) => WriteFloat32(value);

        /// <summary>
        /// 写入64位双精度浮点数
        /// </summary>
        /// <returns></returns>
        public void WriteFloat64(double value)
        {
            BW.Write(value);
        }

        /// <summary>
        /// 写入64位双精度浮点数
        /// </summary>
        /// <returns></returns>
        public void WriteDouble(double value) => WriteFloat64(value);

        /// <summary>
        /// 写入128位十进制数
        /// </summary>
        /// <returns></returns>
        public void WriteDecimal(decimal value)
        {
            BW.Write(value);
        }

        /// <summary>
        /// 写入字符
        /// </summary>
        /// <returns></returns>
        public void WriteChar(char value)
        {
            BW.Write(value);
        }

        /// <summary>
        /// 写入布尔值
        /// </summary>
        /// <returns></returns>
        public void WriteBoolean(bool value)
        {
            BW.Write(value);
        }

        /// <summary>
        /// 写入字节数组
        /// </summary>
        /// <returns></returns>
        public void WriteBytes(byte[] value)
        {
            BW.Write(value);
        }

        /// <summary>
        /// 写入字符数组
        /// </summary>
        /// <returns></returns>
        public void WriteChars(char[] value)
        {
            BW.Write(value);
        }

        /// <summary>
        /// 写入字符串，不带任何其他信息
        /// </summary>
        /// <param name="value"></param>
        public void WriteString(string value)
        {
            BW.Write(encoding.GetBytes(value));
        }
        /// <summary>
        /// 写入带Varint字符串长的信息
        /// </summary>
        /// <param name="value"></param>
        public void WriteStringByVarintHead(string value)
        {
            byte[] b = encoding.GetBytes(value);
            WriteVarint32(b.Length);
            BW.Write(b);
        }
        /// <summary>
        /// 写入带1字节字符串长的信息
        /// </summary>
        /// <param name="value"></param>
        public void WriteStringByByteHead(string value)
        {
            byte[] b = encoding.GetBytes(value);
            BW.Write((byte)b.Length);
            BW.Write(b);
        }
        /// <summary>
        /// 写入带4字节字符串长的信息
        /// </summary>
        /// <param name="value"></param>
        public void WriteStringByInt32Head(string value)
        {
            byte[] b = encoding.GetBytes(value);
            BW.Write(b.Length);
            BW.Write(b);
        }
        /// <summary>
        /// 写入带4字节字符串长的信息，不过这4字节是大端序写的
        /// </summary>
        /// <param name="value"></param>
        public void WriteStringByInt32BigEndianHead(string value)
        {
            byte[] b = encoding.GetBytes(value);
            WriteInt32ByBigEndian(b.Length);
            BW.Write(b);
        }
        /// <summary>
        /// 小端序写入不带任何其他信息的字符串
        /// </summary>
        /// <param name="value"></param>
        public void WriteStringByLittleEndian(string value)
        {
            byte[] bytes = encoding.GetBytes(value);
            Array.Reverse(bytes);
            BW.Write(bytes);
        }
        /// <summary>
        /// 写入字符串，之后写入0x0
        /// </summary>
        /// <param name="value"></param>
        public void WriteStringByEmpty(string value)
        {
            BW.Write(encoding.GetBytes(value));
            BW.Write((byte)0);
        }

        public void WriteVarint32(int value)
        {
            //还是来自伟大的微软
            uint num;
            for (num = (uint)value; num >= 128; num >>= 7)
            {
                BW.Write((byte)(num | 0x80));
            }
            BW.Write((byte)num);
        }

        public void WriteUVarint32(uint value)
        {
            //还是来自伟大的微软
            uint num;
            for (num = value; num >= 128; num >>= 7)
            {
                BW.Write((byte)(num | 0x80));
            }
            BW.Write((byte)num);
        }

        public void WriteZigZag32(int value)
        {
            WriteVarint32((value << 1) ^ (value >> 31));
        }

        public void WriteVarint64(long value)
        {
            //还是来自伟大的微软
            ulong num;
            for (num = (ulong)value; num >= 128; num >>= 7)
            {
                BW.Write((byte)(num | 0x80));
            }
            BW.Write((byte)num);
        }

        public void WriteUVarint64(ulong value)
        {
            //还是来自伟大的微软
            ulong num;
            for (num = value; num >= 128; num >>= 7)
            {
                BW.Write((byte)(num | 0x80));
            }
            BW.Write((byte)num);
        }

        public void WriteZigZag64(long value)
        {
            WriteVarint64((value << 1) ^ (value >> 63));
        }
        //第八部分：按大端序写入并增加流位置

        /// <summary>
        /// 写入大端序16位整数
        /// </summary>
        /// <returns></returns>
        public void WriteInt16ByBigEndian(short value)
        {
            var a = BitConverter.GetBytes(value);
            Array.Reverse(a);
            BW.Write(a);
        }

        /// <summary>
        /// 写入大端序16位无符号整数
        /// </summary>
        /// <returns></returns>
        public void WriteUInt16ByBigEndian(ushort value)
        {
            var a = BitConverter.GetBytes(value);
            Array.Reverse(a);
            BW.Write(a);
        }

        /// <summary>
        /// 写入大端序32位整数
        /// </summary>
        /// <returns></returns>
        public void WriteInt32ByBigEndian(int value)
        {
            var a = BitConverter.GetBytes(value);
            Array.Reverse(a);
            BW.Write(a);
        }

        /// <summary>
        /// 写入大端序32位无符号整数
        /// </summary>
        /// <returns></returns>
        public void WriteUInt32ByBigEndian(uint value)
        {
            var a = BitConverter.GetBytes(value);
            Array.Reverse(a);
            BW.Write(a);
        }

        /// <summary>
        /// 写入大端序64位整数
        /// </summary>
        /// <returns></returns>
        public void WriteInt64ByBigEndian(long value)
        {
            var a = BitConverter.GetBytes(value);
            Array.Reverse(a);
            BW.Write(a);
        }

        /// <summary>
        /// 写入大端序64位无符号整数
        /// </summary>
        /// <returns></returns>
        public void WriteUInt64ByBigEndian(ulong value)
        {
            var a = BitConverter.GetBytes(value);
            Array.Reverse(a);
            BW.Write(a);
        }

        /// <summary>
        /// 写入大端序32位单精度浮点数
        /// </summary>
        /// <returns></returns>
        public void WriteFloat32ByBigEndian(float value)
        {
            var a = BitConverter.GetBytes(value);
            Array.Reverse(a);
            BW.Write(a);
        }

        /// <summary>
        /// 写入大端序32位单精度浮点数
        /// </summary>
        /// <returns></returns>
        public void WriteSingleByBigEndian(float value) => WriteFloat32ByBigEndian(value);

        /// <summary>
        /// 写入大端序64位双精度浮点数
        /// </summary>
        /// <returns></returns>
        public void WriteFloat64ByBigEndian(double value)
        {
            var a = BitConverter.GetBytes(value);
            Array.Reverse(a);
            BW.Write(a);
        }

        /// <summary>
        /// 写入大端序64位双精度浮点数
        /// </summary>
        /// <returns></returns>
        public void WriteDoubleByBigEndian(double value) => WriteFloat64ByBigEndian(value);

        /// <summary>
        /// 读取下一个字节但不增加流位置
        /// </summary>
        /// <returns></returns>
        public byte PeekByte()
        {
            byte ans = BR.ReadByte();
            Seek(-1, SeekOrigin.Current);
            return ans;
        }

        /// <summary>
        /// 读取下一个32位整数但不增加流位置
        /// </summary>
        /// <returns></returns>
        public int PeekInt32()
        {
            int ans = BR.ReadInt32();
            Seek(-4, SeekOrigin.Current);
            return ans;
        }

        /// <summary>
        /// 获取指定位置的字节
        /// </summary>
        /// <returns></returns>
        public byte GetByte(long offset)
        {
            long back = Position;
            Position = offset;
            byte ans = BR.ReadByte();
            Position = back;
            return ans;
        }

        /// <summary>
        /// 获取指定位置的有符号字节
        /// </summary>
        /// <returns></returns>
        public sbyte GetSByte(long offset)
        {
            long back = Position;
            Position = offset;
            sbyte ans = BR.ReadSByte();
            Position = back;
            return ans;
        }

        /// <summary>
        /// 获取指定位置的16位整数
        /// </summary>
        /// <returns></returns>
        public short GetInt16(long offset)
        {
            long back = Position;
            Position = offset;
            short ans = BR.ReadInt16();
            Position = back;
            return ans;
        }

        /// <summary>
        /// 获取指定位置的16位无符号整数
        /// </summary>
        /// <returns></returns>
        public ushort GetUInt16(long offset)
        {
            long back = Position;
            Position = offset;
            ushort ans = BR.ReadUInt16();
            Position = back;
            return ans;
        }

        /// <summary>
        /// 获取指定位置的32位整数
        /// </summary>
        /// <returns></returns>
        public int GetInt32(long offset)
        {
            long back = Position;
            Position = offset;
            int ans = BR.ReadInt32();
            Position = back;
            return ans;
        }

        /// <summary>
        /// 获取指定位置的32位无符号整数
        /// </summary>
        /// <returns></returns>
        public uint GetUInt32(long offset)
        {
            long back = Position;
            Position = offset;
            uint ans = BR.ReadUInt32();
            Position = back;
            return ans;
        }

        /// <summary>
        /// 获取指定位置的64位整数
        /// </summary>
        /// <returns></returns>
        public long GetInt64(long offset)
        {
            long back = Position;
            Position = offset;
            long ans = BR.ReadInt64();
            Position = back;
            return ans;
        }

        /// <summary>
        /// 获取指定位置的64位无符号整数
        /// </summary>
        /// <returns></returns>
        public ulong GetUInt64(long offset)
        {
            long back = Position;
            Position = offset;
            ulong ans = BR.ReadUInt64();
            Position = back;
            return ans;
        }

        /// <summary>
        /// 获取指定位置的32位单精度浮点数
        /// </summary>
        /// <returns></returns>
        public float GetFloat32(long offset)
        {
            long back = Position;
            Position = offset;
            float ans = BR.ReadSingle();
            Position = back;
            return ans;
        }

        /// <summary>
        /// 获取指定位置的32位单精度浮点数
        /// </summary>
        /// <returns></returns>
        public float GetSingle(long offset) => GetFloat32(offset);

        /// <summary>
        /// 获取指定位置的64位双精度浮点数
        /// </summary>
        /// <returns></returns>
        public double GetFloat64(long offset)
        {
            long back = Position;
            Position = offset;
            double ans = BR.ReadDouble();
            Position = back;
            return ans;
        }

        /// <summary>
        /// 获取指定位置的64位双精度浮点数
        /// </summary>
        /// <returns></returns>
        public double GetDouble(long offset) => GetFloat64(offset);

        /// <summary>
        /// 获取指定位置的128位十进制数
        /// </summary>
        /// <returns></returns>
        public decimal GetDecimal(long offset)
        {
            long back = Position;
            Position = offset;
            decimal ans = BR.ReadDecimal();
            Position = back;
            return ans;
        }

        /// <summary>
        /// 获取指定位置的字符
        /// </summary>
        /// <returns></returns>
        public char GetChar(long offset)
        {
            long back = Position;
            Position = offset;
            char ans = BR.ReadChar();
            Position = back;
            return ans;
        }

        /// <summary>
        /// 获取指定位置的布尔值
        /// </summary>
        /// <returns></returns>
        public bool GetBoolean(long offset)
        {
            long back = Position;
            Position = offset;
            bool ans = BR.ReadBoolean();
            Position = back;
            return ans;
        }

        /// <summary>
        /// 获取指定位置的字节数组
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes(long offset, int count)
        {
            long back = Position;
            Position = offset;
            byte[] ans = BR.ReadBytes(count);
            Position = back;
            return ans;
        }

        /// <summary>
        /// 获取指定位置的字符数组
        /// </summary>
        /// <returns></returns>
        public char[] GetChars(long offset, int count)
        {
            long back = Position;
            Position = offset;
            char[] ans = BR.ReadChars(count);
            Position = back;
            return ans;
        }

        //补充按地址读取字符串
        /// <summary>
        /// 按1字节字符长度头在指定地址处获取字符串，即字符串之前存储了一个byte型的数值表明此字符串长度
        /// </summary>
        /// <returns>string型，读取所得字符串</returns>
        public string GetStringByByteHead(long offset)
        {
            long offsetbak = Position;
            Seek(offset, SeekOrigin.Begin);
            string ans = encoding.GetString(BR.ReadBytes(BR.ReadByte())).Replace("\0", "");
            Seek(offsetbak, SeekOrigin.Begin);
            return ans;
        }
        /// <summary>
        /// 按指定长度在指定地址处获取字符串
        /// </summary>
        /// <param name="count">int型，字符串长度</param>
        /// <returns>string型，读取所得字符串</returns>
        public string GetString(long offset, int count)
        {
            long offsetbak = Position;
            Seek(offset, SeekOrigin.Begin);
            string ans = encoding.GetString(BR.ReadBytes(count)).Replace("\0", "");
            Seek(offsetbak, SeekOrigin.Begin);
            return ans;
        }
        /// <summary>
        /// 按4字节字符长度头在指定地址处获取字符串，即字符串之前存储了一个int型的数值表明此字符串长度
        /// </summary>
        /// <returns>string型，读取所得字符串</returns>
        public string GetStringByInt32Head(long offset)
        {
            long offsetbak = Position;
            Seek(offset, SeekOrigin.Begin);
            string ans = encoding.GetString(BR.ReadBytes(BR.ReadInt32())).Replace("\0", "");
            Seek(offsetbak, SeekOrigin.Begin);
            return ans;
        }
        /// <summary>
        /// 按4字节字符长度头在指定地址处获取字符串，即字符串之前存储了一个int型的数值表明此字符串长度，但此4字节数是按照大端序存储
        /// </summary>
        /// <returns>string型，读取所得字符串</returns>
        public string GetStringByInt32BigEndianHead(long offset)
        {
            long offsetbak = Position;
            Seek(offset, SeekOrigin.Begin);
            string ans = encoding.GetString(BR.ReadBytes(ReadInt32ByBigEndian())).Replace("\0", "");
            Seek(offsetbak, SeekOrigin.Begin);
            return ans;
        }
        /// <summary>
        /// 小端序在指定地址处获取字符串
        /// </summary>
        /// <param name="count">int型，字符串长度</param>
        /// <returns>string型，读取所得字符串</returns>
        public string GetStringByLittleEndian(long offset, int count)
        {
            long offsetbak = Position;
            Seek(offset, SeekOrigin.Begin);
            byte[] bytes = BR.ReadBytes(count);
            Array.Reverse(bytes);
            string ans = encoding.GetString(bytes).Replace("\0", "");
            Seek(offsetbak, SeekOrigin.Begin);
            return ans;
        }
        /// <summary>
        /// 在指定地址处获取字符串直到读取到第一个0x0
        /// </summary>
        /// <returns>string型，读取所得字符串</returns>
        public string GetStringByEmpty(long offset)
        {
            long offsetbak = Position;
            Seek(offset, SeekOrigin.Begin);
            List<byte> bytes = new List<byte>();
            byte tp;
            while (true)
            {
                if ((tp = BR.ReadByte()) == 0)
                {
                    break;
                }
                bytes.Add(tp);
            }
            string ans = encoding.GetString(bytes.ToArray());
            Seek(offsetbak, SeekOrigin.Begin);
            return ans;
        }

        /// <summary>
        /// 获取指定位置大端序16位整数
        /// </summary>
        /// <returns></returns>
        public short GetInt16ByBigEndian(long offset)
        {
            long back = Position;
            Position = offset;
            var bt = BR.ReadBytes(2);
            Position = back;
            Array.Reverse(bt);
            return BitConverter.ToInt16(bt, 0);
        }

        /// <summary>
        /// 获取指定位置大端序16位无符号整数
        /// </summary>
        /// <returns></returns>
        public ushort GetUInt16ByBigEndian(long offset)
        {
            long back = Position;
            Position = offset;
            var bt = BR.ReadBytes(2);
            Position = back;
            Array.Reverse(bt);
            return BitConverter.ToUInt16(bt, 0);
        }

        /// <summary>
        /// 获取指定位置大端序32位整数
        /// </summary>
        /// <returns></returns>
        public int GetInt32ByBigEndian(long offset)
        {
            long back = Position;
            Position = offset;
            var bt = BR.ReadBytes(4);
            Position = back;
            Array.Reverse(bt);
            return BitConverter.ToInt32(bt, 0);
        }

        /// <summary>
        /// 获取指定位置大端序32位无符号整数
        /// </summary>
        /// <returns></returns>
        public uint GetUInt32ByBigEndian(long offset)
        {
            long back = Position;
            Position = offset;
            var bt = BR.ReadBytes(4);
            Position = back;
            Array.Reverse(bt);
            return BitConverter.ToUInt32(bt, 0);
        }

        /// <summary>
        /// 获取指定位置大端序64位整数
        /// </summary>
        /// <returns></returns>
        public long GetInt64ByBigEndian(long offset)
        {
            long back = Position;
            Position = offset;
            var bt = BR.ReadBytes(8);
            Position = back;
            Array.Reverse(bt);
            return BitConverter.ToInt64(bt, 0);
        }

        /// <summary>
        /// 获取指定位置大端序64位无符号整数
        /// </summary>
        /// <returns></returns>
        public ulong GetUInt64ByBigEndian(long offset)
        {
            long back = Position;
            Position = offset;
            var bt = BR.ReadBytes(8);
            Position = back;
            Array.Reverse(bt);
            return BitConverter.ToUInt64(bt, 0);
        }

        /// <summary>
        /// 获取指定位置大端序32位单精度浮点数
        /// </summary>
        /// <returns></returns>
        public float GetFloat32ByBigEndian(long offset)
        {
            long back = Position;
            Position = offset;
            var bt = BR.ReadBytes(4);
            Position = back;
            Array.Reverse(bt);
            return BitConverter.ToSingle(bt, 0);
        }

        /// <summary>
        /// 获取指定位置大端序32位单精度浮点数
        /// </summary>
        /// <returns></returns>
        public float GetSingleByBigEndian(long offset) => GetFloat32ByBigEndian(offset);

        /// <summary>
        /// 获取指定位置大端序64位双精度浮点数
        /// </summary>
        /// <returns></returns>
        public double GetFloat64ByBigEndian(long offset)
        {
            long back = Position;
            Position = offset;
            var bt = BR.ReadBytes(8);
            Position = back;
            Array.Reverse(bt);
            return BitConverter.ToDouble(bt, 0);
        }

        /// <summary>
        /// 获取指定位置大端序64位双精度浮点数
        /// </summary>
        /// <returns></returns>
        public double GetDoubleByBigEndian(long offset) => GetFloat64ByBigEndian(offset);

        /// <summary>
        /// 写入字节
        /// </summary>
        /// <returns></returns>
        public void SetByte(long offset, byte value)
        {
            long back = Position;
            Position = offset;
            BW.Write(value);
            Position = back;
        }

        /// <summary>
        /// 写入有符号字节
        /// </summary>
        /// <returns></returns>
        public void SetSbyte(long offset, sbyte value)
        {
            long back = Position;
            Position = offset;
            BW.Write(value);
            Position = back;
        }

        /// <summary>
        /// 写入16位整数
        /// </summary>
        /// <returns></returns>
        public void SetInt16(long offset, short value)
        {
            long back = Position;
            Position = offset;
            BW.Write(value);
            Position = back;
        }

        /// <summary>
        /// 写入16位无符号整数
        /// </summary>
        /// <returns></returns>
        public void SetUInt16(long offset, ushort value)
        {
            long back = Position;
            Position = offset;
            BW.Write(value);
            Position = back;
        }

        /// <summary>
        /// 写入32位整数
        /// </summary>
        /// <returns></returns>
        public void SetInt32(long offset, int value)
        {
            long back = Position;
            Position = offset;
            BW.Write(value);
            Position = back;
        }

        /// <summary>
        /// 写入32位无符号整数
        /// </summary>
        /// <returns></returns>
        public void SetUInt32(long offset, uint value)
        {
            long back = Position;
            Position = offset;
            BW.Write(value);
            Position = back;
        }

        /// <summary>
        /// 写入64位整数
        /// </summary>
        /// <returns></returns>
        public void SetInt64(long offset, long value)
        {
            long back = Position;
            Position = offset;
            BW.Write(value);
            Position = back;
        }

        /// <summary>
        /// 写入64位无符号整数
        /// </summary>
        /// <returns></returns>
        public void SetUInt64(long offset, ulong value)
        {
            long back = Position;
            Position = offset;
            BW.Write(value);
            Position = back;
        }

        /// <summary>
        /// 写入32位单精度浮点数
        /// </summary>
        /// <returns></returns>
        public void SetFloat32(long offset, float value)
        {
            long back = Position;
            Position = offset;
            BW.Write(value);
            Position = back;
        }

        /// <summary>
        /// 写入32位单精度浮点数
        /// </summary>
        /// <returns></returns>
        public void SetSingle(long offset, float value) => SetFloat32(offset, value);

        /// <summary>
        /// 写入64位双精度浮点数
        /// </summary>
        /// <returns></returns>
        public void SetFloat64(long offset, double value)
        {
            long back = Position;
            Position = offset;
            BW.Write(value);
            Position = back;
        }

        /// <summary>
        /// 写入64位双精度浮点数
        /// </summary>
        /// <returns></returns>
        public void SetDouble(long offset, double value) => SetFloat64(offset, value);

        /// <summary>
        /// 写入128位十进制数
        /// </summary>
        /// <returns></returns>
        public void SetDecimal(long offset, decimal value)
        {
            long back = Position;
            Position = offset;
            BW.Write(value);
            Position = back;
        }

        /// <summary>
        /// 写入字符
        /// </summary>
        /// <returns></returns>
        public void SetChar(long offset, char value)
        {
            long back = Position;
            Position = offset;
            BW.Write(value);
            Position = back;
        }

        /// <summary>
        /// 写入布尔值
        /// </summary>
        /// <returns></returns>
        public void SetBoolean(long offset, bool value)
        {
            long back = Position;
            Position = offset;
            BW.Write(value);
            Position = back;
        }

        /// <summary>
        /// 写入字节数组
        /// </summary>
        /// <returns></returns>
        public void SetBytes(long offset, byte[] value)
        {
            long back = Position;
            Position = offset;
            BW.Write(value);
            Position = back;
        }

        /// <summary>
        /// 写入字符数组
        /// </summary>
        /// <returns></returns>
        public void SetChars(long offset, char[] value)
        {
            long back = Position;
            Position = offset;
            BW.Write(value);
            Position = back;
        }

        /// <summary>
        /// 写入字符串，不带任何其他信息
        /// </summary>
        /// <param name="value"></param>
        public void SetString(long offset, string value)
        {
            long back = Position;
            Position = offset;
            BW.Write(encoding.GetBytes(value));
            Position = back;
        }
        /// <summary>
        /// 写入带1字节字符串长的信息
        /// </summary>
        /// <param name="value"></param>
        public void SetStringByByteHead(long offset, string value)
        {
            long back = Position;
            Position = offset;
            BW.Write((byte)value.Length);
            BW.Write(encoding.GetBytes(value));
            Position = back;
        }
        /// <summary>
        /// 写入带4字节字符串长的信息
        /// </summary>
        /// <param name="value"></param>
        public void SetStringByInt32Head(long offset, string value)
        {
            long back = Position;
            Position = offset;
            BW.Write(value.Length);
            BW.Write(encoding.GetBytes(value));
            Position = back;
        }
        /// <summary>
        /// 写入带4字节字符串长的信息，不过这4字节是大端序写的
        /// </summary>
        /// <param name="value"></param>
        public void SetStringByInt32BigEndianHead(long offset, string value)
        {
            long back = Position;
            Position = offset;
            WriteInt32ByBigEndian(value.Length);
            BW.Write(encoding.GetBytes(value));
            Position = back;
        }
        /// <summary>
        /// 小端序写入不带任何其他信息的字符串
        /// </summary>
        /// <param name="value"></param>
        public void SetStringByLittleEndian(long offset, string value)
        {
            long back = Position;
            Position = offset;
            byte[] bytes = encoding.GetBytes(value);
            Array.Reverse(bytes);
            BW.Write(bytes);
            Position = back;
        }
        /// <summary>
        /// 写入字符串，之后写入0x0
        /// </summary>
        /// <param name="value"></param>
        public void SetStringByEmpty(long offset, string value)
        {
            long back = Position;
            Position = offset;
            BW.Write(encoding.GetBytes(value));
            BW.Write((byte)0);
            Position = back;
        }
        //第八部分：按大端序写入并增加流位置

        /// <summary>
        /// 写入大端序16位整数
        /// </summary>
        /// <returns></returns>
        public void SetInt16ByBigEndian(long offset, short value)
        {
            long back = Position;
            Position = offset;
            var a = BitConverter.GetBytes(value);
            Array.Reverse(a);
            BW.Write(a);
            Position = back;
        }

        /// <summary>
        /// 写入大端序16位无符号整数
        /// </summary>
        /// <returns></returns>
        public void SetUInt16ByBigEndian(long offset, ushort value)
        {
            long back = Position;
            Position = offset;
            var a = BitConverter.GetBytes(value);
            Array.Reverse(a);
            BW.Write(a);
            Position = back;
        }

        /// <summary>
        /// 写入大端序32位整数
        /// </summary>
        /// <returns></returns>
        public void SetInt32ByBigEndian(long offset, int value)
        {
            long back = Position;
            Position = offset;
            var a = BitConverter.GetBytes(value);
            Array.Reverse(a);
            BW.Write(a);
            Position = back;
        }

        /// <summary>
        /// 写入大端序32位无符号整数
        /// </summary>
        /// <returns></returns>
        public void SetUInt32ByBigEndian(long offset, uint value)
        {
            long back = Position;
            Position = offset;
            var a = BitConverter.GetBytes(value);
            Array.Reverse(a);
            BW.Write(a);
            Position = back;
        }

        /// <summary>
        /// 写入大端序64位整数
        /// </summary>
        /// <returns></returns>
        public void SetInt64ByBigEndian(long offset, long value)
        {
            long back = Position;
            Position = offset;
            var a = BitConverter.GetBytes(value);
            Array.Reverse(a);
            BW.Write(a);
            Position = back;
        }

        /// <summary>
        /// 写入大端序64位无符号整数
        /// </summary>
        /// <returns></returns>
        public void SetUInt64ByBigEndian(long offset, ulong value)
        {
            long back = Position;
            Position = offset;
            var a = BitConverter.GetBytes(value);
            Array.Reverse(a);
            BW.Write(a);
            Position = back;
        }

        /// <summary>
        /// 写入大端序32位单精度浮点数
        /// </summary>
        /// <returns></returns>
        public void SetFloat32ByBigEndian(long offset, float value)
        {
            long back = Position;
            Position = offset;
            var a = BitConverter.GetBytes(value);
            Array.Reverse(a);
            BW.Write(a);
            Position = back;
        }

        /// <summary>
        /// 写入大端序32位单精度浮点数
        /// </summary>
        /// <returns></returns>
        public void SetSingleByBigEndian(long offset, float value) => SetFloat32ByBigEndian(offset, value);

        /// <summary>
        /// 写入大端序64位双精度浮点数
        /// </summary>
        /// <returns></returns>
        public void SetFloat64ByBigEndian(long offset, double value)
        {
            long back = Position;
            Position = offset;
            var a = BitConverter.GetBytes(value);
            Array.Reverse(a);
            BW.Write(a);
            Position = back;
        }

        /// <summary>
        /// 写入大端序64位双精度浮点数
        /// </summary>
        /// <returns></returns>
        public void SetDoubleByBigEndian(long offset, double value) => SetFloat64ByBigEndian(offset, value);

        /// <summary>
        /// 指定段异或数组
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        public void XorBytes(byte[] bytes, long offset, long length)
        {
            long back = GetPosition();
            SetPosition(offset);
            int index = 0;
            int arysize = bytes.Length;
            for (long i = 0; i < length; i++)
            {
                WriteByte((byte)(PeekByte() ^ bytes[index++]));
                index %= arysize;
            }
            SetPosition(back);
        }

        /// <summary>
        /// 指定段异或数组
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        public void XorByte(byte value, long offset, long length)
        {
            long back = GetPosition();
            SetPosition(offset);
            for (long i = 0; i < length; i++)
            {
                WriteByte((byte)(PeekByte() ^ value));
            }
            SetPosition(back);
        }
    }
}
