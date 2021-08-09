using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace aatriserver2.Unpackpw {
    class Unpackpw {
        public struct Data {
            public ushort id;
            public string text;
        }
        public static string ExtractBin(byte[] bytes, bool textOrLine) {
            StringBuilder stringBuilder = new StringBuilder();
            List<Data> list = new List<Data>();
            /* Console.WriteLine(BitConverter.ToString(bytes)); */
            int num = 0;
            if (textOrLine) {
                while (num + 2 <= bytes.Length) {
                    Data item = new Data {
                        id = BitConverter.ToUInt16(bytes, num)
                    };
                    num += 2;
                    while (num + 2 <= bytes.Length) {
                        char c = BitConverter.ToChar(bytes, num);
                        num += 2;
                        if (c != 0) {
                            stringBuilder.Append(c);
                            continue;
                        } else {
                            /* Console.WriteLine("Empty lol"); */
                        }
                        break;
                    }
                    string text;
                    text = EnToHalf(stringBuilder.ToString());
                    text = text.Replace('φ', ' ');
                    item.text = text;
                    stringBuilder.Length = 0;
                    list.Add(item);
                }
            } else {
                while (num + 2 <= bytes.Length) {
                    Data item = default(Data);
                    List<string> list2 = new List<string>();
                    item.id = BitConverter.ToUInt16(bytes, num);
                    num += 2;
                    bool flag = true;
                    while (flag && num + 2 <= bytes.Length) {
                        char c;
                        for (; num + 2 <= bytes.Length; stringBuilder.Append(c)) {
                            c = BitConverter.ToChar(bytes, num);
                            num += 2;
                            switch (c) {
                                case '\0':
                                    flag = false;
                                    break;
                                default:
                                    continue;
                                case ',':
                                    c = '\n';
                                    continue;
                            }
                            break;
                        }
                        string text = EnToHalf(stringBuilder.ToString());
                        text = text.Replace('φ', ' ');
                        list2.Add(text);
                        stringBuilder.Length = 0;
                    }
                    item.text = string.Join("", list2.ToArray());
                    list.Add(item);
                }
            }

            /* return list; */
            return JsonConvert.SerializeObject(list);
        }
        public static byte[] /*void*/ CreateBin(string json) {
            /* int num = 0; */
            List<Data> data = JsonConvert.DeserializeObject<List<Data>>(json);
            List<byte> bytes = new List<byte>();
            /* while (num < data.Length) { */
            /*  string number = data[num].Replace("[", "").Replace("]", ""); */
            /*  /1* Console.WriteLine(number); *1/ */
            /*  bytes.AddRange(BitConverter.GetBytes(UInt16.Parse(number))); */
            /*  string text = data[num + 1]; */
            /*  text = HalfToEn(text); */
            /*  foreach (var c in text) { */
            /*      bytes.AddRange(BitConverter.GetBytes(c)); */
            /*  } */
            /*  bytes.Add(0x00); */
            /*  bytes.Add(0x00); */
            /*  /1* Console.WriteLine(text); *1/ */
            /*  /1* Console.WriteLine(BitConverter.ToString(bytes.ToArray())); *1/ */
            /*  num += 2; */
            /* } */
            foreach (Data d in data) {
                bytes.AddRange(BitConverter.GetBytes(d.id));
                string text = HalfToEn(d.text);
                foreach (var c in text) {
                    bytes.AddRange(BitConverter.GetBytes(c));
                }
                bytes.Add(0x00);
                bytes.Add(0x00);
            }
            return bytes.ToArray();
        }
        public static string EnToHalf(string in_text) {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("１", "1");
            dictionary.Add("２", "2");
            dictionary.Add("３", "3");
            dictionary.Add("４", "4");
            dictionary.Add("５", "5");
            dictionary.Add("６", "6");
            dictionary.Add("７", "7");
            dictionary.Add("８", "8");
            dictionary.Add("９", "9");
            dictionary.Add("０", "0");
            dictionary.Add("Ａ", "A");
            dictionary.Add("Ｂ", "B");
            dictionary.Add("Ｃ", "C");
            dictionary.Add("Ｄ", "D");
            dictionary.Add("Ｅ", "E");
            dictionary.Add("Ｆ", "F");
            dictionary.Add("Ｇ", "G");
            dictionary.Add("Ｈ", "H");
            dictionary.Add("Ｉ", "I");
            dictionary.Add("Ｊ", "J");
            dictionary.Add("Ｋ", "K");
            dictionary.Add("Ｌ", "L");
            dictionary.Add("Ｍ", "M");
            dictionary.Add("Ｎ", "N");
            dictionary.Add("Ｏ", "O");
            dictionary.Add("Ｐ", "P");
            dictionary.Add("Ｑ", "Q");
            dictionary.Add("Ｒ", "R");
            dictionary.Add("Ｓ", "S");
            dictionary.Add("Ｔ", "T");
            dictionary.Add("Ｕ", "U");
            dictionary.Add("Ｖ", "V");
            dictionary.Add("Ｗ", "W");
            dictionary.Add("Ｘ", "X");
            dictionary.Add("Ｙ", "Y");
            dictionary.Add("Ｚ", "Z");
            dictionary.Add("ａ", "a");
            dictionary.Add("ｂ", "b");
            dictionary.Add("ｃ", "c");
            dictionary.Add("ｄ", "d");
            dictionary.Add("ｅ", "e");
            dictionary.Add("ｆ", "f");
            dictionary.Add("ｇ", "g");
            dictionary.Add("ｈ", "h");
            dictionary.Add("ｉ", "i");
            dictionary.Add("ｊ", "j");
            dictionary.Add("ｋ", "k");
            dictionary.Add("ｌ", "l");
            dictionary.Add("ｍ", "m");
            dictionary.Add("ｎ", "n");
            dictionary.Add("ｏ", "o");
            dictionary.Add("ｐ", "p");
            dictionary.Add("ｑ", "q");
            dictionary.Add("ｒ", "r");
            dictionary.Add("ｓ", "s");
            dictionary.Add("ｔ", "t");
            dictionary.Add("ｕ", "u");
            dictionary.Add("ｖ", "v");
            dictionary.Add("ｗ", "w");
            dictionary.Add("ｘ", "x");
            dictionary.Add("ｙ", "y");
            dictionary.Add("ｚ", "z");
            dictionary.Add("\u3000", " ");
            dictionary.Add("．", ".");
            dictionary.Add("，", ",");
            dictionary.Add("＇", "'");
            dictionary.Add("！", "!");
            dictionary.Add("（", "(");
            dictionary.Add("）", ")");
            dictionary.Add("－", "-");
            dictionary.Add("／", "/");
            dictionary.Add("？", "?");
            dictionary.Add("∠", "_");
            dictionary.Add("［", "[");
            dictionary.Add("］", "]");
            dictionary.Add("“", "\"");
            dictionary.Add("”", "\"");
            dictionary.Add("＂", "\"");
            dictionary.Add("―", "-");
            dictionary.Add("‘", "'");
            dictionary.Add("’", "'");
            dictionary.Add("：", ":");
            dictionary.Add("＊", "*");
            dictionary.Add("；", ";");
            dictionary.Add("＄", "$");
            dictionary.Add("Ы", "©");
            dictionary.Add("∋", "è");
            dictionary.Add("∈", "é");
            dictionary.Add("∀", "á");
            dictionary.Add("∧", "à");
            dictionary.Add("⊆", "ç");
            dictionary.Add("⊂", "Ç");
            dictionary.Add("Ц", "û");
            dictionary.Add("↑", "î");
            dictionary.Add("α", "â");
            dictionary.Add("л", "ñ");
            dictionary.Add("↓", "ï");
            dictionary.Add("ε", "ê");
            string text = string.Empty;
            for (int i = 0; i < in_text.Length; i++) {
                text = ((!dictionary.ContainsKey(in_text[i].ToString())) ? (text + in_text[i].ToString()) : (text + dictionary[in_text[i].ToString()]));
            }
            return text;
        }
        public static string HalfToEn(string in_text) {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("1", "１");
            dictionary.Add("2", "２");
            dictionary.Add("3", "３");
            dictionary.Add("4", "４");
            dictionary.Add("5", "５");
            dictionary.Add("6", "６");
            dictionary.Add("7", "７");
            dictionary.Add("8", "８");
            dictionary.Add("9", "９");
            dictionary.Add("0", "０");
            dictionary.Add("A", "Ａ");
            dictionary.Add("B", "Ｂ");
            dictionary.Add("C", "Ｃ");
            dictionary.Add("D", "Ｄ");
            dictionary.Add("E", "Ｅ");
            dictionary.Add("F", "Ｆ");
            dictionary.Add("G", "Ｇ");
            dictionary.Add("H", "Ｈ");
            dictionary.Add("I", "Ｉ");
            dictionary.Add("J", "Ｊ");
            dictionary.Add("K", "Ｋ");
            dictionary.Add("L", "Ｌ");
            dictionary.Add("M", "Ｍ");
            dictionary.Add("N", "Ｎ");
            dictionary.Add("O", "Ｏ");
            dictionary.Add("P", "Ｐ");
            dictionary.Add("Q", "Ｑ");
            dictionary.Add("R", "Ｒ");
            dictionary.Add("S", "Ｓ");
            dictionary.Add("T", "Ｔ");
            dictionary.Add("U", "Ｕ");
            dictionary.Add("V", "Ｖ");
            dictionary.Add("W", "Ｗ");
            dictionary.Add("X", "Ｘ");
            dictionary.Add("Y", "Ｙ");
            dictionary.Add("Z", "Ｚ");
            dictionary.Add("a", "ａ");
            dictionary.Add("b", "ｂ");
            dictionary.Add("c", "ｃ");
            dictionary.Add("d", "ｄ");
            dictionary.Add("e", "ｅ");
            dictionary.Add("f", "ｆ");
            dictionary.Add("g", "ｇ");
            dictionary.Add("h", "ｈ");
            dictionary.Add("i", "ｉ");
            dictionary.Add("j", "ｊ");
            dictionary.Add("k", "ｋ");
            dictionary.Add("l", "ｌ");
            dictionary.Add("m", "ｍ");
            dictionary.Add("n", "ｎ");
            dictionary.Add("o", "ｏ");
            dictionary.Add("p", "ｐ");
            dictionary.Add("q", "ｑ");
            dictionary.Add("r", "ｒ");
            dictionary.Add("s", "ｓ");
            dictionary.Add("t", "ｔ");
            dictionary.Add("u", "ｕ");
            dictionary.Add("v", "ｖ");
            dictionary.Add("w", "ｗ");
            dictionary.Add("x", "ｘ");
            dictionary.Add("y", "ｙ");
            dictionary.Add("z", "ｚ");
            dictionary.Add(" ", "\u3000");
            dictionary.Add(".", "．");
            dictionary.Add(",", "，");
            dictionary.Add("!", "！");
            dictionary.Add("(", "（");
            dictionary.Add(")", "）");
            dictionary.Add("-", "－");
            dictionary.Add("/", "／");
            dictionary.Add("?", "？");
            dictionary.Add("_", "∠");
            dictionary.Add("[", "［");
            dictionary.Add("]", "］");
            dictionary.Add("\"", "“");
            dictionary.Add("'", "‘");
            dictionary.Add(":", "：");
            dictionary.Add("*", "＊");
            dictionary.Add("\n", ",");
            dictionary.Add("$", "＄");
            dictionary.Add("©", "Ы");
            dictionary.Add("è", "∋");
            dictionary.Add("é", "∈");
            dictionary.Add("á", "∀");
            dictionary.Add("à", "∧");
            dictionary.Add("ç", "⊆");
            dictionary.Add("Ç", "⊂");
            dictionary.Add("û", "Ц");
            dictionary.Add("î", "↑");
            dictionary.Add("â", "α");
            dictionary.Add("ñ", "л");
            dictionary.Add("ï", "↓");
            dictionary.Add("ê", "ε");
            string text = string.Empty;
            for (int i = 0; i < in_text.Length; i++) {
                text = ((!dictionary.ContainsKey(in_text[i].ToString())) ? (text + in_text[i].ToString()) : (text + dictionary[in_text[i].ToString()]));
            }
            return text;
        }
    }
}
