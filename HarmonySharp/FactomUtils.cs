using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
//using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using RestSharp;

namespace Harmony {

    public static class FactomUtils
    {
        
    /// <summary>
    ///     Convience function to emulate Java's CopyOfRange
    /// </summary>
    /// <param name="src">The byte array to copfrom</param>
    /// <param name="start">The index to cut from</param>
    /// <param name="end">The index to cut to</param>
    /// <returns></returns>
    public static byte[] CopyOfRange(this byte[] src, int start, int end) {
        var len = end - start + 1;
        var dest = new byte[len];
        Array.Copy(src, start, dest, 0, len);
        return dest;
    }

    /// <summary>
    ///     Converts byte[] to hex string
    /// </summary>
    /// <param name="ba"></param>
    /// <returns></returns>
    public static string ToHexString(this byte[] ba) {
        var hex = BitConverter.ToString(ba);
        return hex.Replace("-", "").ToLower();
    }

    /// <summary>
    ///     Will correct a little endian byte[]
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static byte[] SetEndian(this byte[] bytes)
    {
        if (BitConverter.IsLittleEndian) return bytes.Reverse().ToArray();
        return bytes;
    }

  

    public static byte[] MilliTime() {
        var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        // 6 Byte millisec unix time
        var unixMilliLong = (long) (DateTime.UtcNow - unixEpoch).TotalMilliseconds;
        var unixBytes = BitConverter.GetBytes(unixMilliLong).SetEndian();
        unixBytes = unixBytes.CopyOfRange(2, unixBytes.Length-1);
        return unixBytes;
    }

   

    private static readonly byte[,] ByteLookup = {
        // low nibble
        {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f},
        // high nibble
        {0x00, 0x10, 0x20, 0x30, 0x40, 0x50, 0x60, 0x70, 0x80, 0x90, 0xa0, 0xb0, 0xc0, 0xd0, 0xe0, 0xf0}
    };

    /// <summary>
    ///     Converts string hex into byte[]
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static byte[] DecodeHexIntoBytes(this string input) {
        var result = new byte[(input.Length + 1) >> 1];
        var lastcell = result.Length - 1;
        var lastchar = input.Length - 1;
        // count up in characters, but inside the loop will
        // reference from the end of the input/output.
        for (var i = 0; i < input.Length; i++) {
            // i >> 1    -  (i / 2) gives the result byte offset from the end
            // i & 1     -  1 if it is high-nibble, 0 for low-nibble.
            result[lastcell - (i >> 1)] |= ByteLookup[i & 1, HexToInt(input[lastchar - i])];
        }
        return result;
    }

    public static byte[] ToHexBytes(this byte[] data)
    {
        byte[] bytesOut = new byte[data.Length * 2];
        int b;
        for (int i = 0; i < data.Length; i++)
        {
            b = data[i] >> 4;
            bytesOut[i * 2] = (byte)(87 + b + (((b - 10) >> 31) & -39));
            b = data[i] & 0xF;
            bytesOut[i * 2 + 1] = (byte)(87 + b + (((b - 10) >> 31) & -39));
        }
        return bytesOut;
    }


   
    /// <summary>
    ///     Helper function of Hex functions
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    private static int HexToInt(char c)
    {
        switch (c) {
            case '0':
                return 0;
            case '1':
                return 1;
            case '2':
                return 2;
            case '3':
                return 3;
            case '4':
                return 4;
            case '5':
                return 5;
            case '6':
                return 6;
            case '7':
                return 7;
            case '8':
                return 8;
            case '9':
                return 9;
            case 'a':
            case 'A':
                return 10;
            case 'b':
            case 'B':
                return 11;
            case 'c':
            case 'C':
                return 12;
            case 'd':
            case 'D':
                return 13;
            case 'e':
            case 'E':
                return 14;
            case 'f':
            case 'F':
                return 15;
            default:
                throw new FormatException("Unrecognized hex char " + c);
            }   
        }
        
       
        
        static public string[] MakeRandomExtIDsHex()
        {
            string[] extIDs = new string[2];
            extIDs[0] = BitConverter.ToString(Guid.NewGuid().ToByteArray()).Replace("-", string.Empty);
            extIDs[1] = BitConverter.ToString(Guid.NewGuid().ToByteArray()).Replace("-", string.Empty);
            return extIDs;
        }
        
        static public byte[][] MakeRandomExtIDs()
        {
            byte[][] extIDs = new byte[2][];
            extIDs[0] = Guid.NewGuid().ToByteArray();
            extIDs[1] = Guid.NewGuid().ToByteArray();
            return extIDs;
        }
        
        
        public static string[] ExtIDsToHexStrings(this byte[][] ExtIDs)
        {
            var exid = new string[ExtIDs.Length];
            for (int i=0; i < ExtIDs.Length; i++)
            {
                exid[i]=ExtIDs[i].ToHexString();
            }
            return exid;
        }
        
        public static string[] ExtIDsToBase64Strings(this byte[][] ExtIDs)
        {
            var exid = new string[ExtIDs.Length];
            for (int i=0; i < ExtIDs.Length; i++)
            {
                exid[i] = System.Convert.ToBase64String(ExtIDs[i]);
            }
            return exid;
        }

        public static string[] ToBase64StringArray(this string[] text)
        {
            var text64 = new string[text.Length];
            for (int f = 0; f < text.Length; f++)
            {
                text64[f] = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(text[f]));
            }

            return text64;
        }
 
    }
}