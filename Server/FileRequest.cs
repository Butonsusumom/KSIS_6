using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    [Serializable]
    public class Request
    {
        public string RequestType { get; set; }
        public Request (string type)
        {
            RequestType = type;
        }
    }
    [Serializable]
    public class RequestFile:Request
    {
        public int Offset { get; set; }
        public int Length { get; set; }
        public string FileName { get; set; }
        public string Text { get; set; }
        public RequestFile(string filename, int offset, int length, string text) :
            base("RequestFile")
        {
            FileName = filename;
            Offset = offset;
            Length = length;
            Text = text;
        }

    }
    [Serializable]
    public class RequestDisconnect : Request
    {
        public RequestDisconnect() : base("RequestDisconnect") {}

    }
}
