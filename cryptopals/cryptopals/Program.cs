using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace cryptopals
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "1b37373331363f78151b7f2b783431333d78397828372d363c78373e783a393b3736";

            string output = Encoding.ASCII.GetString(hex_to_bytes(input));

            Console.WriteLine(output);
            
            Console.ReadLine();
        }

        static byte[] xor(byte[] one, byte[] two)
        {
            byte[] retval = new byte[one.Length];
            for (int i = 0; i < one.Length; i++)
            {
                retval[i] = (byte)(one[i] ^ two[i]);
            }
            return retval;
        }

        static byte[] hex_to_bytes(string hex_string)
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
