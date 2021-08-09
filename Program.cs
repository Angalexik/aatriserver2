using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using NetMQ;
using NetMQ.Sockets;
using static aatriserver2.Unpackpw.Unpackpw;

namespace aatriserver2
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var server = new ResponseSocket("@tcp://*:4747"))
            {
                Console.WriteLine("AATriServer 2.0 started");
                while (true)
                {
                    List<string> message = server.ReceiveMultipartStrings();
                    string response;
                    try
                    {
                        Console.WriteLine(DateTime.Now.ToString());
                        Console.WriteLine(message[0]);
                        Console.WriteLine(message[1]);
                        if (message[0] == "mdtDecode")
                        {
                            response = DecodeMdt(message[1]);
                        }
                        else if (message[0] == "mdtEncode")
                        {
                            response = EncodeMdt(message[1]);
                        }
                        else if (message[0] == "textDecode")
                        {
                            response = DecodeBin(message[1], true);
                        }
                        else if (message[0] == "linesDecode")
                        {
                            response = DecodeBin(message[1], false);
                        }
                        else if (message[0] == "linesEncode" ||
                                message[0] == "textEncode")
                        {
                            response = EncodeBin(message[1]);
                        }
                        else
                        {
                            response = "epic fail";
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        response = "epic fail";
                    }
                    server.SendFrame(response);
                }
            }
        }

        static byte[] Decrypt(string base64)
        {
            byte[] file = Convert.FromBase64String(base64);
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            rijndaelManaged.KeySize = 128;
            rijndaelManaged.BlockSize = 128;
            string password = "u8DurGE2";
            string s = "6BBGizHE";
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, bytes);
            rfc2898DeriveBytes.IterationCount = 1000;
            rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
            rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);
            using (ICryptoTransform cryptoTransform = rijndaelManaged.CreateDecryptor())
            {
                return cryptoTransform.TransformFinalBlock(file, 0, file.Length);
            }
        }

        static string Encrypt(byte[] file)
        {
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            rijndaelManaged.KeySize = 128;
            rijndaelManaged.BlockSize = 128;
            string password = "u8DurGE2";
            string s = "6BBGizHE";
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, bytes);
            rfc2898DeriveBytes.IterationCount = 1000;
            rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
            rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);
            using (ICryptoTransform cryptoTransform = rijndaelManaged.CreateEncryptor())
            {
                byte[] dataBytes = cryptoTransform.TransformFinalBlock(file, 0, file.Length);
                return Convert.ToBase64String(dataBytes);
            }
        }

        static string DecodeMdt(string base64)
        {
            byte[] mdtData = Decrypt(base64);
            MemoryStream jsonStream = new MemoryStream();

            GSMdtTools.Decoders.MdtDecoder decoder = new GSMdtTools.Decoders.MdtDecoder(new MemoryStream(mdtData));
            List<GSMdtTools.IGSToken> tokens = decoder.DecodeStream();
            GSMdtTools.Encoders.JsonEncoder jsonEncoder = new GSMdtTools.Encoders.JsonEncoder(jsonStream, tokens);
            jsonEncoder.EncodeTokens();
            string jsonString = Encoding.UTF8.GetString(jsonStream.ToArray());
            return jsonString;
        }

        static string EncodeMdt(string json)
        {
            MemoryStream mdtStream = new MemoryStream();
            byte[] jsonBytes = Encoding.UTF8.GetBytes(json);

            GSMdtTools.Decoders.JsonDecoder decoder = new GSMdtTools.Decoders.JsonDecoder(new MemoryStream(jsonBytes));
            List<GSMdtTools.IGSToken> tokens = decoder.DecodeStream();
            GSMdtTools.Encoders.MdtEncoder mdtEncoder = new GSMdtTools.Encoders.MdtEncoder(mdtStream, tokens);
            mdtEncoder.EncodeTokens();
            return Encrypt(mdtStream.ToArray());
        }

        static string DecodeBin(string base64, bool text)
        {
            byte[] bytes = Decrypt(base64);
            return ExtractBin(bytes, text);
        }
        static string EncodeBin(string json)
        {
            byte[] bytes = CreateBin(json);
            return Encrypt(bytes);
        }
    }
}
