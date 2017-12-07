using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cryptopals
{
    public static class Decoder
    {
        static Dictionary<char, int> frequency = new Dictionary<char, int>(){
            {'A', 6532},
            {'B', 1258},
            {'C', 2233},
            {'D', 3282},
            {'E', 10266},
            {'F', 1983},
            {'G', 1624},
            {'H', 4978},
            {'I', 5668},
            {'J', 97},
            {'K', 560},
            {'L', 3317},
            {'M', 2026},
            {'N', 5712},
            {'O', 6159},
            {'P', 1504},
            {'Q', 83},
            {'R', 4987},
            {'S', 5317},
            {'T', 7516},
            {'U', 2275},
            {'V', 796},
            {'W', 1703},
            {'X', 140},
            {'Y', 1427},
            {'Z', 51},
            {' ', 18288}
        };

        public static int ScoreWord(string str)
        {
            int ret = 0;
            foreach (char c in str.ToCharArray())
            {
                if (frequency.TryGetValue(c, out int val))
                    ret += val;
            }
            return ret;
        }

        public static List<Tuple<int, string>> SingleXorDecrypt(List<byte[]> strings)
        {
            List<Tuple<int, string>> scores = new List<Tuple<int, string>>();
            foreach (byte[] input in strings)
            {
                for (int i = 0; i < 255; i++)
                {
                    var text = Encoding.ASCII.GetString(BinaryOperations.Xor(input, Convert.ToByte(i))).ToUpper();
                    var score = ScoreWord(text);
                    scores.Add(new Tuple<int, string>(score, text));
                }
            }
            scores.Sort((x, y) => y.Item1.CompareTo(x.Item1));
            return scores;
        }

        public static List<byte> SingleXorFindKey(List<byte[]> strings)
        {
            List<Tuple<int, byte>> scores = new List<Tuple<int, byte>>();
            List<byte> key = new List<byte>();
            foreach (byte[] input in strings)
            {
                scores.Clear();
                for (int i = 0; i < 255; i++)
                {
                    var text = Encoding.ASCII.GetString(BinaryOperations.Xor(input, Convert.ToByte(i))).ToUpper();
                    var score = ScoreWord(text);
                    scores.Add(new Tuple<int, byte>(score, Convert.ToByte(i)));
                }
                scores.Sort((x, y) => y.Item1.CompareTo(x.Item1));
                key.Add(scores[0].Item2);
            }
            return key;
        }

        public static byte[] Vigenere(byte[] cyphertext)
        {
            List<Tuple<double, int>> scores = new List<Tuple<double, int>>();
            int keysize;
            for (keysize = 2; keysize <= 40; keysize++)
            {
                int dist = BinaryOperations.Hamming(cyphertext.ToList().GetRange(0, keysize).ToArray(),
                                                    cyphertext.ToList().GetRange(2 * keysize, keysize).ToArray(),
                                                    cyphertext.ToList().GetRange(3 * keysize, keysize).ToArray(),
                                                    cyphertext.ToList().GetRange(4 * keysize, keysize).ToArray());
                scores.Add(new Tuple<double, int>((double)dist / keysize, keysize));
            }
            scores.Sort((x, y) => x.Item1.CompareTo(y.Item1));
            List<Tuple<int, byte[]>> word_scores = new List<Tuple<int, byte[]>>();
            for (int i = 0; i < scores.Count; i++)
            {
                List<List<byte>> blocks = new List<List<byte>>();
                keysize = scores[i].Item2;
                for (int j = 0; j < cyphertext.Length / keysize; j++)
                {
                    blocks.Add(cyphertext.ToList().GetRange(j * keysize, keysize));
                }
                if ((cyphertext.Length % keysize) != 0)
                    blocks.Add(cyphertext.ToList().GetRange(blocks.Count * keysize, cyphertext.Length % keysize));
                blocks = Lists.Transpose(blocks);
                List<byte[]> blocks_t = new List<byte[]>();
                foreach (List<byte> list in blocks)
                    blocks_t.Add(list.ToArray());
                byte[] key = SingleXorFindKey(blocks_t).ToArray();
                int score = ScoreWord(Encoding.ASCII.GetString(BinaryOperations.Xor(cyphertext, key)).Replace("-", string.Empty));
                word_scores.Add(new Tuple<int, byte[]>(score, key));
            }
            word_scores.Sort((x, y) => y.Item1.CompareTo(x.Item1));
            return word_scores[0].Item2;
        }
    }
}
