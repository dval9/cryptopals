using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cryptopals
{
    public static class BinaryOperations
    {

        public static int Hamming(byte[] one, byte[] two)
        {
            int dist = 0;
            for (int i = 0; i < one.Length; i++)
            {
                byte val = (byte)(one[i] ^ two[i]);
                while (val != 0)
                {
                    dist++;
                    val &=  (byte)(val - 1);
                }
            }
            return dist;
        }

        public static int Hamming(byte[] one, byte[] two, byte[] three, byte[] four)
        {
            int dist = 0;
            for (int i = 0; i < one.Length; i++)
            {
                byte val = (byte)(one[i] ^ two[i] ^ three[i] ^ four[i]);
                while (val != 0)
                {
                    dist++;
                    val &= (byte)(val - 1);
                }
            }
            return dist;
        }

        public static byte[] Xor(byte[] text, byte[] key)
        {
            byte[] retval = new byte[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                retval[i] = (byte)(text[i] ^ key[i % key.Length]);
            }
            return retval;
        }

        public static byte[] Xor(byte[] text, byte key)
        {
            byte[] retval = new byte[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                retval[i] = (byte)(text[i] ^ key);
            }
            return retval;
        }

        public static byte[] HexToBytes(string hex_string)
        {
            byte[] retval = new byte[hex_string.Length / 2];
            for (int i = 0; i < retval.Length; i++)
            {
                retval[i] = Convert.ToByte(hex_string.Substring(i * 2, 2), 16);
            }
            return retval;
        }
    }
}
