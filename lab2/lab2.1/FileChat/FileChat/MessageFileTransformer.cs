using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;

namespace FileChat
{
    public static class MessageFileTransformer
    {
        public static List<Message> CreateMsgsFromLocalJson()
        {
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<Message>));

            List<Message> messages;
            using (FileStream fs = new FileStream("messageList.json", FileMode.OpenOrCreate))
            {
                 messages = jsonFormatter.ReadObject(fs) as List<Message>;
            }
            return messages;
        }

        public static void CreateLocalJsonFromMsgs(List<Message> messages)
        {
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<Message>));
            using (FileStream fs = new FileStream("messageList.json", FileMode.OpenOrCreate))
            {
                jsonFormatter.WriteObject(fs, messages);
            }
        }

        public static void SendFileToServer()
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://127.0.0.1/messageList.json");
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential("gonstFTP", "gonstFTP");
            FileStream fs = new FileStream("messageList.json", FileMode.Open);
            byte[] fileContents = new byte[fs.Length];
            fs.Read(fileContents, 0, fileContents.Length);
            fs.Close();
            request.ContentLength = fileContents.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            response.Close();
        }

        public static void GetFileFromServer()
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://127.0.0.1/messageList.json");
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential("gonstFTP", "gonstFTP");
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            FileStream fs = new FileStream("messageList.json", FileMode.Create);
            byte[] buffer = new byte[64];
            int size = 0;
            while ((size = responseStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                fs.Write(buffer, 0, size);
            }
            fs.Close();
            response.Close();
        }

    }
}
