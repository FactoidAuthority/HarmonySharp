using System;
using System.Text;
using HarmonySharp;

namespace HarmonyTest
{
    public class EntryData
    {
        public byte[] Content { get; private set; }
        public byte[][] ExtIDs { get; private set; }
        
        public EntryData(byte[] content, byte[][] extIDs = null)
        {
            Content = content;
            ExtIDs = extIDs;
        }
        
        public EntryData(string content, byte[][] extIDs = null)
        {
            Content = Encoding.UTF8.GetBytes(content);
            ExtIDs = extIDs;
        }
        
        
        public string ContentBase64String
        {
          get
          {
                return System.Convert.ToBase64String(Content);
          }
        }
        
        public string[] ExtIDsBase64Strings
        {
          get
          {
            return ExtIDs.ExtIDsToBase64Strings();
          }
        }
        
    }
}
