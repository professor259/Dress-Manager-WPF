namespace Attendance.Network
{
    using System;
    using System.Text;
    using System.Collections.Generic;

    public class Writer
    {
        public static void Write(List<string> arg, int offset, byte[] buffer)
        {
            if (arg == null)
                return;
            if (buffer == null)
                return;
            if (offset > buffer.Length - 1)
                return;
            buffer[offset] = (byte)arg.Count;
            offset++;
            foreach (string str in arg)
            {
                buffer[offset] = (byte)str.Length;
                offset += 1;
                Writer.Write(str, offset + 1, buffer);
                offset += str.Length + 1;
            }
        }
        public static void Write(int arg, int offset, byte[] buffer)
        {
            if (buffer == null)
            {
                return;
            }
            if (offset > buffer.Length - 1)
            {
                return;
            }
            if (buffer.Length >= offset + sizeof(uint))
            {
                unsafe
                {
#if UNSAFE
                    fixed (byte* Buffer = buffer)
                    {
                        *((int*)(Buffer + offset)) = arg;
                    }
#else 
                    buffer[offset] = (byte)(arg);
                    buffer[offset + 1] = (byte)(arg >> 8);
                    buffer[offset + 2] = (byte)(arg >> 16);
                    buffer[offset + 3] = (byte)(arg >> 24);
#endif
                }
            }
        }
        public static void Uint(uint arg, int offset, byte[] buffer)
        {
            if (buffer == null)
                return;
            if (offset > buffer.Length - 1)
                return;
            if (buffer.Length >= offset + sizeof(uint))
            {
                unsafe
                {
#if UNSAFE
                    fixed (byte* Buffer = buffer)
                    {
                        *((uint*)(Buffer + offset)) = arg;
                    }
#else
                    buffer[offset] = (byte)arg;
                    buffer[offset + 1] = (byte)(arg >> 8);
                    buffer[offset + 2] = (byte)(arg >> 16);
                    buffer[offset + 3] = (byte)(arg >> 24);
#endif
                }
            }
        }
        public static void Write(string arg, int offset, byte[] buffer)
        {
            if (buffer == null)
                return;
            if (offset > buffer.Length - 1)
                return;
            byte[] argEncoded = Program.Encoding.GetBytes(arg);
            if (buffer.Length >= offset + arg.Length)
                Array.Copy(argEncoded, 0, buffer, offset, arg.Length);
        }
        public static void Write(ushort arg, int offset, byte[] buffer)
        {
            if (buffer == null)
                return;
            if (offset > buffer.Length - 1)
                return;
            if (buffer.Length >= offset + sizeof(ushort))
            {
                unsafe
                {
#if UNSAFE
                    fixed (byte* Buffer = buffer)
                    {
                        *((ushort*)(Buffer + offset)) = arg;
                    }
#else
                    buffer[offset] = (byte)arg;
                    buffer[offset + 1] = (byte)(arg >> 8);
#endif
                }
            }
        }
        public static void Write(bool arg, int offset, byte[] buffer)
        {
            if (buffer == null)
                return;
            if (offset > buffer.Length - 1)
                return;
            Write(arg == true ? (byte)1 : (byte)0, offset, buffer);
        }
        public static void Write(uint arg, int offset, byte[] buffer)
        {
            if (buffer == null)
                return;
            if (offset > buffer.Length - 1)
                return;
            if (buffer.Length >= offset + sizeof(uint))
            {
                unsafe
                {
#if UNSAFE
                    fixed (byte* Buffer = buffer)
                    {
                        *((uint*)(Buffer + offset)) = arg;
                    }
#else
                    buffer[offset] = (byte)arg;
                    buffer[offset + 1] = (byte)(arg >> 8);
                    buffer[offset + 2] = (byte)(arg >> 16);
                    buffer[offset + 3] = (byte)(arg >> 24);
#endif
                }
            }
        }
        public static void Write(ulong arg, int offset, byte[] buffer)
        {
            if (buffer == null)
                return;
            if (offset > buffer.Length - 1)
                return;
            if (buffer.Length >= offset + sizeof(ulong))
            {
                unsafe
                {
#if UNSAFE
                    fixed (byte* Buffer = buffer)
                    {
                        *((ulong*)(Buffer + offset)) = arg;
                    }
#else
                    buffer[offset] = (byte)(arg);
                    buffer[offset + 1] = (byte)(arg >> 8);
                    buffer[offset + 2] = (byte)(arg >> 16);
                    buffer[offset + 3] = (byte)(arg >> 24);
                    buffer[offset + 4] = (byte)(arg >> 32);
                    buffer[offset + 5] = (byte)(arg >> 40);
                    buffer[offset + 6] = (byte)(arg >> 48);
                    buffer[offset + 7] = (byte)(arg >> 56);
#endif
                }
            }
        }
        public static void Byte(byte arg, int offset, byte[] buffer)
        {
            if (buffer == null)
                return;
            if (offset > buffer.Length - 1)
                return;
            buffer[offset] = arg;
        }
        public static void Ulong(ulong arg, int offset, byte[] buffer)
        {
            if (buffer == null)
                return;
            if (offset > buffer.Length - 1)
                return;
            if (buffer.Length >= offset + sizeof(ulong))
            {
                unsafe
                {
#if UNSAFE
                    fixed (byte* Buffer = buffer)
                    {
                        *((ulong*)(Buffer + offset)) = arg;
                    }
#else
                    buffer[offset] = (byte)(arg);
                    buffer[offset + 1] = (byte)(arg >> 8);
                    buffer[offset + 2] = (byte)(arg >> 16);
                    buffer[offset + 3] = (byte)(arg >> 24);
                    buffer[offset + 4] = (byte)(arg >> 32);
                    buffer[offset + 5] = (byte)(arg >> 40);
                    buffer[offset + 6] = (byte)(arg >> 48);
                    buffer[offset + 7] = (byte)(arg >> 56);
#endif
                }
            }
        }
        public static void Ushort(ushort arg, int offset, byte[] buffer)
        {
            if (buffer == null)
                return;
            if (offset > buffer.Length - 1)
                return;
            if (buffer.Length >= offset + sizeof(ushort))
            {
                unsafe
                {
#if UNSAFE
                    fixed (byte* Buffer = buffer)
                    {
                        *((ushort*)(Buffer + offset)) = arg;
                    }
#else
                    buffer[offset] = (byte)arg;
                    buffer[offset + 1] = (byte)(arg >> 8);
#endif
                }
            }
        }
        public static void smethod_0(ushort arg, int offset, byte[] buffer)
        {
            if (buffer != null && offset <= (int)buffer.Length - 1 && (int)buffer.Length >= offset + 2)
            {
                buffer[offset] = (byte)arg;
                buffer[offset + 1] = (byte)(arg >> 8);
            }
        }

        public static void smethod_1(uint arg, int offset, byte[] buffer)
        {
            if (buffer != null && offset <= (int)buffer.Length - 1 && (int)buffer.Length >= offset + 4)
            {
                buffer[offset] = (byte)arg;
                buffer[offset + 1] = (byte)(arg >> 8);
                buffer[offset + 2] = (byte)(arg >> 16);
                buffer[offset + 3] = (byte)(arg >> 24);
            }
        }
        public static void smethod_2(uint arg, int offset, byte[] buffer)
        {
            if (((buffer != null) && (offset <= (buffer.Length - 1))) && (buffer.Length >= (offset + 4)))
            {
                buffer[offset] = (byte)arg;
                buffer[offset + 1] = (byte)(arg >> 8);
                buffer[offset + 2] = (byte)(arg >> 0x10);
                buffer[offset + 3] = (byte)(arg >> 0x18);
            }
        }
        public static void smethod_2(int arg, int offset, byte[] buffer)
        {
            if (buffer != null && offset <= (int)buffer.Length - 1 && (int)buffer.Length >= offset + 4)
            {
                buffer[offset] = (byte)arg;
                buffer[offset + 1] = (byte)(arg >> 8);
                buffer[offset + 2] = (byte)(arg >> 16);
                buffer[offset + 3] = (byte)(arg >> 24);
            }
        }

        public static void smethod_3(ulong arg, int offset, byte[] buffer)
        {
            if (buffer != null && offset <= (int)buffer.Length - 1 && (int)buffer.Length >= offset + 8)
            {
                buffer[offset] = (byte)arg;
                buffer[offset + 1] = (byte)(arg >> 8);
                buffer[offset + 2] = (byte)(arg >> 16);
                buffer[offset + 3] = (byte)(arg >> 24);
                buffer[offset + 4] = (byte)(arg >> 32);
                buffer[offset + 5] = (byte)(arg >> 40);
                buffer[offset + 6] = (byte)(arg >> 48);
                buffer[offset + 7] = (byte)(arg >> 56);
            }
        }
        public static void WriteUshort(ushort arg, int offset, byte[] buffer)
        {
            if (buffer == null)
                return;
            if (offset > buffer.Length - 1)
                return;
            if (buffer.Length >= offset + sizeof(ushort))
            {
                unsafe
                {
#if UNSAFE
                    fixed (byte* Buffer = buffer)
                    {
                        *((ushort*)(Buffer + offset)) = arg;
                    }
#else
                    buffer[offset] = (byte)arg;
                    buffer[offset + 1] = (byte)(arg >> 8);
#endif
                }
            }
        }
        public static void WriteUint(uint arg, int offset, byte[] buffer)
        {
            if (buffer == null)
                return;
            if (offset > buffer.Length - 1)
                return;
            if (buffer.Length >= offset + sizeof(uint))
            {
                unsafe
                {
#if UNSAFE
                    fixed (byte* Buffer = buffer)
                    {
                        *((uint*)(Buffer + offset)) = arg;
                    }
#else
                    buffer[offset] = (byte)arg;
                    buffer[offset + 1] = (byte)(arg >> 8);
                    buffer[offset + 2] = (byte)(arg >> 16);
                    buffer[offset + 3] = (byte)(arg >> 24);
#endif
                }
            }
        }
        public static void WriteStringWithLength(string arg, int offset, byte[] buffer)
        {
            if (buffer == null)
                return;
            if (offset > buffer.Length - 1)
                return;
            int till = buffer.Length - offset;
            till = Math.Min(arg.Length, till);
            buffer[offset] = (byte)arg.Length;
            offset++;
            byte[] argEncoded = Program.Encoding.GetBytes(arg);
            Array.Copy(argEncoded, 0, buffer, offset, till);
        }
        public static void WriteString(string arg, int offset, byte[] buffer)
        {
            try
            {
                if (buffer == null)
                    return;
                if (offset > buffer.Length - 1)
                    return;
                byte[] argEncoded = Program.Encoding.GetBytes(arg);
                if (buffer.Length >= offset + arg.Length)
                    Array.Copy(argEncoded, 0, buffer, offset, arg.Length);
            }
            catch
            {
                Console.WriteLine(arg);
            }
        }
        public static void WriteByte(byte arg, int offset, byte[] buffer)
        {
            if (buffer == null)
            {
                return;
            }
            if (offset > buffer.Length - 1)
            {
                return;
            }
            buffer[offset] = arg;
        }
        public static void WriteBoolean(bool arg, int offset, byte[] buffer)
        {
            if (buffer == null)
            {
                return;
            }
            if (offset > buffer.Length - 1)
            {
                return;
            }
            WriteByte(arg == true ? (byte)1 : (byte)0, offset, buffer);
        }
        public static void WriteUInt16(ushort arg, int offset, byte[] buffer)
        {
            if (buffer == null)
            {
                return;
            }
            if (offset > buffer.Length - 1)
            {
                return;
            }
            if (buffer.Length >= offset + sizeof(ushort))
            {
                unsafe
                {
#if UNSAFE
                    fixed (byte* Buffer = buffer)
                    {
                        *((ushort*)(Buffer + offset)) = arg;
                    }
#else
                    buffer[offset] = (byte)arg;
                    buffer[offset + 1] = (byte)(arg >> 8);
#endif
                }
            }
        }
        public static void WriteUInt32(uint arg, int offset, byte[] buffer)
        {
            if (buffer == null)
            {
                return;
            }
            if (offset > buffer.Length - 1)
            {
                return;
            }
            if (buffer.Length >= offset + sizeof(uint))
            {
                unsafe
                {
#if UNSAFE
                    fixed (byte* Buffer = buffer)
                    {
                        *((uint*)(Buffer + offset)) = arg;
                    }
#else
                    buffer[offset] = (byte)arg;
                    buffer[offset + 1] = (byte)(arg >> 8);
                    buffer[offset + 2] = (byte)(arg >> 16);
                    buffer[offset + 3] = (byte)(arg >> 24);
#endif
                }
            }
        }
        public static unsafe void WriteUInt128(decimal arg, int offset, byte[] Buffer)
        {
            try
            {
                fixed (byte* buffer = Buffer)
                {
                    if (arg.GetType() == typeof(decimal))
                    {
                        *((decimal*)(buffer + offset)) = arg;
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
        public static void WriteInt32(int arg, int offset, byte[] buffer)
        {
            if (buffer == null)
            {
                return;
            }
            if (offset > buffer.Length - 1)
            {
                return;
            }
            if (buffer.Length >= offset + sizeof(uint))
            {
                unsafe
                {
#if UNSAFE
                    fixed (byte* Buffer = buffer)
                    {
                        *((int*)(Buffer + offset)) = arg;
                    }
#else
                    buffer[offset] = (byte)(arg);
                    buffer[offset + 1] = (byte)(arg >> 8);
                    buffer[offset + 2] = (byte)(arg >> 16);
                    buffer[offset + 3] = (byte)(arg >> 24);
#endif
                }
            }
        }
        public static void WriteUInt64(ulong arg, int offset, byte[] buffer)
        {
            if (buffer == null)
            {
                return;
            }
            if (offset > buffer.Length - 1)
            {
                return;
            }
            if (buffer.Length >= offset + sizeof(ulong))
            {
                unsafe
                {
#if UNSAFE
                    fixed (byte* Buffer = buffer)
                    {
                        *((ulong*)(Buffer + offset)) = arg;
                    }
#else
                    buffer[offset] = (byte)(arg);
                    buffer[offset + 1] = (byte)(arg >> 8);
                    buffer[offset + 2] = (byte)(arg >> 16);
                    buffer[offset + 3] = (byte)(arg >> 24);
                    buffer[offset + 4] = (byte)(arg >> 32);
                    buffer[offset + 5] = (byte)(arg >> 40);
                    buffer[offset + 6] = (byte)(arg >> 48);
                    buffer[offset + 7] = (byte)(arg >> 56);
#endif
                }
            }
        }
        public static void WriteStringList(List<string> arg, int offset, byte[] buffer)
        {
            if (arg == null)
                return;
            if (buffer == null)
                return;
            if (offset > buffer.Length - 1)
                return;
            buffer[offset] = (byte)arg.Count;
            offset++;
            foreach (string str in arg)
            {
                if (str != null)
                {
                    var Buffer = Encoding.UTF8.GetBytes(str);
                    Write((ushort)Buffer.Length, offset, buffer);
                    offset += 2;
                    for (int x = 0; x < Buffer.Length; x++)
                    {
                        Write((byte)Buffer[x], offset, buffer);
                        offset += 1;
                    }
                }
            }

        }
        public static byte[] ReadBytes(int size, byte[] buf, ref int index)
        {
            byte[] res = new byte[size];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = buf[index];
                index++;

            }
            return res;
        }
        public static string ReadCString(int size, byte[] buf, ref int index)
        {
            index += 2;
            string result = Encoding.UTF8.GetString(ReadBytes(size, buf, ref index));
            int idx = result.IndexOf('\0');
            return (idx > -1) ? result.Substring(0, idx) : result;
        }
        public static string ReadString(int size, byte[] buf, ref int index)
        {
            string result = Encoding.UTF8.GetString(ReadBytes(size, buf, ref index));
            int idx = result.IndexOf('\0');
            return (idx > -1) ? result.Substring(0, idx) : result;
        }

        public static void WriteStringListName(List<string> arg, int offset, byte[] buffer)
        {
            if (arg == null)
                return;
            if (buffer == null)
                return;
            if (offset > buffer.Length - 1)
                return;
            buffer[offset] = (byte)arg.Count;
            offset++;
            foreach (string str in arg)
            {
                if (str != null)
                {
                    var Buffer = Encoding.UTF8.GetBytes(str);
                    Write((ushort)Buffer.Length, offset, buffer);
                    offset += 2;
                    for (int x = 0; x < Buffer.Length; x++)
                    {
                        Write((byte)Buffer[x], offset, buffer);
                        offset += 1;
                    }
                }
            }

        }
        public static void WriteStringListName(string[] arg, int offset, byte[] buffer)
        {
            if (arg == null)
                return;
            if (buffer == null)
                return;
            if (offset > buffer.Length - 1)
                return;
            buffer[offset] = (byte)arg.Length;
            offset++;
            foreach (string str in arg)
            {
                if (str != null)
                {
                    var Buffer = Encoding.UTF8.GetBytes(str);
                    Write((ushort)Buffer.Length, offset, buffer);
                    offset += 2;
                    for (int x = 0; x < Buffer.Length; x++)
                    {
                        Write((byte)Buffer[x], offset, buffer);
                        offset += 1;
                    }
                }
            }

        }
        public static void WriteStringList(int offset, byte[] buffer, params string[] arg)
        {
            if (arg == null)
                return;
            if (buffer == null)
                return;
            if (offset > buffer.Length - 1)//
                return;
            buffer[offset] = (byte)arg.Length;
            offset++;
            foreach (string str in arg)
            {
                buffer[offset] = (byte)str.Length;
                WriteString(str, offset + 1, buffer);
                offset += str.Length + 1;
            }
        }
        public static void WriteStringList(string[] arg, int offset, byte[] buffer)
        {
            if (arg == null)
                return;
            if (buffer == null)
                return;
            if (offset > buffer.Length - 1)//
                return;
            buffer[offset] = (byte)arg.Length;
            offset++;
            foreach (string str in arg)
            {
                buffer[offset] = (byte)str.Length;
                WriteString(str, offset + 1, buffer);
                offset += str.Length + 1;
            }
        }
    }
}
