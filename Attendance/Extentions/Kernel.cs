using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Extentions
{
    public class Kernel
    {       
        public static FastRandom Random = new FastRandom();         
        public static bool Rate(int value)
        {
            if (value >= 100) return true;
            return value > ((Random.Next() % 200));
            //var generator = new Random(DateTime.Now.Millisecond);
            //return generator.Next(100) < value;
        }
        public static string RandFromGivingStrings(params string[] nums)
        {
            //return "Banha";
            return nums[Random.Next(0, nums.Length - 1)];
        }
        public static byte[] FinalizeProtoBuf(object proto, ushort packetid)
        {
            try
            {
                byte[] protobuff;
                using (var ms = new System.IO.MemoryStream())
                {
                    ProtoBuf.Serializer.Serialize(ms, proto);
                    protobuff = ms.ToArray();
                    byte[] buffer;
                    buffer = new byte[12 + protobuff.Length];
                    System.Buffer.BlockCopy(protobuff, 0, buffer, 4, protobuff.Length);
                    Network.Writer.Write((ushort)(buffer.Length - 8), 0, buffer);
                    Network.Writer.Write(packetid, 2, buffer);
                    return buffer;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }       
    }
}
