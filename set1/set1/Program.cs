using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace set1
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "49276d206b696c6c696e6720796f757220627261696e206c696b65206120706f69736f6e6f7573206d757368726f6f6d";
            Console.WriteLine(Convert.ToBase64String(hex_to_base64(input)));
            Console.ReadLine();
        }

        //string in hex
        static byte[] hex_to_base64(string input)
        {
            byte[] retval = new byte[input.Length / 2];
            for (int i = 0; i < retval.Length; i++)
            {
                retval[i] = Convert.ToByte(input.Substring(i * 2, 2), 16);
            }
            return retval;  
        }
    }
}
