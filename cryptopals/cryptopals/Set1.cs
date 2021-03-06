﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace cryptopals
{
    public class Set1
    {
        /* Convert hex to base64
         * The string:
         * "49276d206b696c6c696e6720796f757220627261696e206c696b65206120706f69736f6e6f7573206d757368726f6f6d"
         * Should produce:
         * "SSdtIGtpbGxpbmcgeW91ciBicmFpbiBsaWtlIGEgcG9pc29ub3VzIG11c2hyb29t"
         * So go ahead and make that happen. You'll need to use this code for the rest of the exercises. 
         * Cryptopals Rule
         * Always operate on raw bytes, never on encoded strings. Only use hex and base64 for pretty-printing.
         */
        public static void Set1Challenge1()
        {
            string input = "49276d206b696c6c696e6720796f757220627261696e206c696b65206120706f69736f6e6f7573206d757368726f6f6d";
            string expect = "SSdtIGtpbGxpbmcgeW91ciBicmFpbiBsaWtlIGEgcG9pc29ub3VzIG11c2hyb29t";
            string output = Convert.ToBase64String(BinaryOperations.HexToBytes(input));
            Console.WriteLine("Set 1 Challenge 1: " + expect.ToUpper().Equals(output.ToUpper()));
            Console.WriteLine("Set 1 Challenge 1: (input) " + Encoding.ASCII.GetString(BinaryOperations.HexToBytes(input)));
        }

        /* Fixed XOR
         * Write a function that takes two equal-length buffers and produces their XOR combination.
         * If your function works properly, then when you feed it the string:
         * "1c0111001f010100061a024b53535009181c"
         * ... after hex decoding, and when XOR'd against:
         * "686974207468652062756c6c277320657965"
         * ... should produce:
         * "746865206b696420646f6e277420706c6179"
         */
        public static void Set1Challenge2()
        {
            string input = "1c0111001f010100061a024b53535009181c";
            string key = "686974207468652062756c6c277320657965";
            string expect = "746865206b696420646f6e277420706c6179";
            string output = BitConverter.ToString(BinaryOperations.Xor(BinaryOperations.HexToBytes(input), BinaryOperations.HexToBytes(key))).Replace("-", string.Empty);
            Console.WriteLine("Set 1 Challenge 2: " + expect.ToUpper().Equals(output.ToUpper()));
            Console.WriteLine("Set 1 Challenge 2: (key) " + Encoding.ASCII.GetString(BinaryOperations.HexToBytes(key)));
            Console.WriteLine("Set 1 Challenge 2: (expect) " + Encoding.ASCII.GetString(BinaryOperations.HexToBytes(expect)));
        }

        /* Single-byte XOR cipher
         * The hex encoded string:
         * "1b37373331363f78151b7f2b783431333d78397828372d363c78373e783a393b3736"
         * ... has been XOR'd against a single character. Find the key, decrypt the message.
         * You can do this by hand. But don't: write code to do it for you.
         * How? Devise some method for "scoring" a piece of English plaintext. Character frequency is a good metric. Evaluate each output and choose the one with the best score.
         * Achievement Unlocked
         * You now have our permission to make "ETAOIN SHRDLU" jokes on Twitter.
         */
        public static void Set1Challenge3()
        {
            string str = "1b37373331363f78151b7f2b783431333d78397828372d363c78373e783a393b3736";
            List<byte[]> input = new List<byte[]>
            {
                BinaryOperations.HexToBytes(str)
            };
            string output = Crypto.SingleXorDecrypt(input)[0].Item2;
            Console.WriteLine("Set 1 Challenge 3: " + output);
        }

        /* Detect single-character XOR
         * One of the 60-character strings in this file(@"../../set1challenge4.txt") has been encrypted by single-character XOR.
         * Find it.
         * (Your code from #3 should help.)
         */
        public static void Set1Challenge4()
        {
            string[] lines = File.ReadAllLines(@"../../../data/set1/4.txt");
            List<byte[]> input = new List<byte[]>();
            foreach (string line in lines)
                input.Add(BinaryOperations.HexToBytes(line));
            string output = Crypto.SingleXorDecrypt(input)[0].Item2;
            Console.WriteLine("Set 1 Challenge 4: " + output.Trim());
        }

        /* Implement repeating-key XOR
         * Here is the opening stanza of an important work of the English language:
         * "Burning 'em, if you ain't quick and nimble"
         * "I go crazy when I hear a cymbal"
         * Encrypt it, under the key "ICE", using repeating-key XOR.
         * In repeating-key XOR, you'll sequentially apply each byte of the key; 
         * the first byte of plaintext will be XOR'd against I, the next C, the next E, then I again for the 4th byte, and so on.
         * It should come out to:
         * "0b3637272a2b2e63622c2e69692a23693a2a3c6324202d623d63343c2a26226324272765272"
         * "a282b2f20430a652e2c652a3124333a653e2b2027630c692b20283165286326302e27282f"
         * Encrypt a bunch of stuff using your repeating-key XOR function. 
         * Encrypt your mail. Encrypt your password file. Your .sig file. Get a feel for it. I promise, we aren't wasting your time with this.
         */
        public static void Set1Challenge5()
        {
            string input = "Burning 'em, if you ain't quick and nimble\nI go crazy when I hear a cymbal";
            string expect = "0b3637272a2b2e63622c2e69692a23693a2a3c6324202d623d63343c2a26226324272765272a282b2f20430a652e2c652a3124333a653e2b2027630c692b20283165286326302e27282f";
            string key = "ICE";
            string output1 = BitConverter.ToString(BinaryOperations.Xor(Encoding.ASCII.GetBytes(input), Encoding.ASCII.GetBytes(key))).Replace("-", string.Empty);
            Console.WriteLine("Set 1 Challenge 5: " + expect.ToUpper().Equals(output1.ToUpper()));
        }

        /* Break repeating-key XOR
         * It is officially on, now.
         * This challenge isn't conceptually hard, but it involves actual error-prone coding. 
         * The other challenges in this set are there to bring you up to speed. This one is there to qualify you. If you can do this one, you're probably just fine up to Set 6.
         * There's a file(@"../../set1challenge6.txt") here. It's been base64'd after being encrypted with repeating-key XOR.
         * Decrypt it.
         * Here's how:
         * Let KEYSIZE be the guessed length of the key; try values from 2 to (say) 40.
         * Write a function to compute the edit distance/Hamming distance between two strings. The Hamming distance is just the number of differing bits. The distance between:
         * "this is a test"
         * and
         * wokka wokka!!!
         * is 37. Make sure your code agrees before you proceed.
         * For each KEYSIZE, take the first KEYSIZE worth of bytes, and the second KEYSIZE worth of bytes, and find the edit distance between them. Normalize this result by dividing by KEYSIZE.
         * The KEYSIZE with the smallest normalized edit distance is probably the key. 
         * You could proceed perhaps with the smallest 2-3 KEYSIZE values. Or take 4 KEYSIZE blocks instead of 2 and average the distances.
         * Now that you probably know the KEYSIZE: break the ciphertext into blocks of KEYSIZE length.
         * Now transpose the blocks: make a block that is the first byte of every block, and a block that is the second byte of every block, and so on.
         * Solve each block as if it was single-character XOR. You already have code to do this.
         * For each block, the single-byte XOR key that produces the best looking histogram is the repeating-key XOR key byte for that block. Put them together and you have the key.
         * This code is going to turn out to be surprisingly useful later on. 
         * Breaking repeating-key XOR ("Vigenere") statistically is obviously an academic exercise, a "Crypto 101" thing. 
         * But more people "know how" to break it than can actually break it, and a similar technique breaks something much more important.
         * No, that's not a mistake.
         * We get more tech support questions for this challenge than any of the other ones. 
         * We promise, there aren't any blatant errors in this text. In particular: the "wokka wokka!!!" edit distance really is 37.
         */
        public static void Set1Challenge6()
        {
            string input = File.ReadAllText(@"../../../data/set1/6.txt");
            byte[] cyphertext = Convert.FromBase64String(input);
            byte[] key = Crypto.Vigenere(cyphertext);
            Console.WriteLine("Set 1 Challenge 6: used key =>" + Encoding.ASCII.GetString(key));
            Console.WriteLine("Set 1 Challenge 6: " + Encoding.ASCII.GetString(BinaryOperations.Xor(cyphertext, key)).Replace(" - ", string.Empty).Substring(0, 32));
        }

        /* AES in ECB mode
         * The Base64-encoded content in this file(@"../../set1challenge7.txt") has been encrypted via AES-128 in ECB mode under the key
         * "YELLOW SUBMARINE".
         * (case-sensitive, without the quotes; exactly 16 characters; I like "YELLOW SUBMARINE" because it's exactly 16 bytes long, and now you do too).
         * Decrypt it. You know the key, after all.
         * Easiest way: use OpenSSL::Cipher and give it AES-128-ECB as the cipher.
         * Do this with code.
         * You can obviously decrypt this using the OpenSSL command-line tool, but we're having you get ECB working in code for a reason. 
         * You'll need it a lot later on, and not just for attacking ECB.
         */
        public static void Set1Challenge7()
        {
            string key = "YELLOW SUBMARINE";
            string input = File.ReadAllText(@"../../../data/set1/7.txt");
            byte[] cipher = Convert.FromBase64String(input);
            string plaintext;
            plaintext = Encoding.ASCII.GetString(Crypto.AesEcb128Decrypt(cipher, Encoding.ASCII.GetBytes(key)));
            Console.WriteLine("Set 1 Challenge 7: " + plaintext.Substring(0, 32));
        }

        /* Detect AES in ECB mode
         * In this file(@"../../set1challenge8.txt") are a bunch of hex-encoded ciphertexts.
         * One of them has been encrypted with ECB.
         * Detect it.
         * Remember that the problem with ECB is that it is stateless and deterministic; the same 16 byte plaintext block will always produce the same 16 byte ciphertext.
         */
        public static void Set1Challenge8()
        {
            string[] input = File.ReadAllLines(@"../../../data/set1/8.txt");
            string detected = Crypto.DetectAesEcb(input);
            Console.WriteLine("Set 1 Challenge 8: " + detected.Substring(0,32));
        }
    }
}
