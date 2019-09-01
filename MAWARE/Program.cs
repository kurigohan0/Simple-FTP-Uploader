using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Net;   
using System.Xml.Serialization;

namespace MAWARE
{
    class Program
    {
        public static FTP_Data FTPdata;

        static void Main(string[] args)
        {

            if (args.Length == 0)
            {
                Console.WriteLine("Drag and drop the necessary files into the .exe folder and drag them into the executable file of the program");
            }
            else
            {
                GetFTPData();

                Console.WriteLine($"Trying to connect {FTPdata.Name}:{FTPdata.Port} Username:{FTPdata.Username} Password:{FTPdata.Password}...");

                foreach (var item in args)
                {
                    Console.WriteLine($"Sending {item} to server... ");
                    FTPUploadFile("ftp://" + FTPdata.Name + ":" + FTPdata.Port, item, "/" + Path.GetFileName(item));
                }
                Console.WriteLine("It's all.");
            }
            Console.ReadLine();

        }

        static void GetFTPData()
        {
            try
            {
                using (FileStream sw = new FileStream(@"data.xml", FileMode.Open))
                {
                    XmlSerializer formatter = new XmlSerializer(typeof(FTP_Data));

                    FTPdata = (FTP_Data)formatter.Deserialize(sw);
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("FTP data file not found... ");
            }
        }

        protected static string FTPUploadFile(string uri, string filePath, string fileName)
        {
            FileInfo fileInf = new FileInfo(filePath);
            FtpWebRequest reqFTP;
            Uri reqUri = new Uri(uri + fileName);
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(reqUri);
            reqFTP.Credentials = new NetworkCredential(FTPdata.Username, FTPdata.Password);
            reqFTP.KeepAlive = false;
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            reqFTP.UseBinary = true;
            reqFTP.ContentLength = fileInf.Length;
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;
            try
            {
                FileStream fs = fileInf.OpenRead();
                try
                {
                    Stream strm = reqFTP.GetRequestStream();
                    contentLen = fs.Read(buff, 0, buffLength);
                    while (contentLen != 0)
                    {
                        strm.Write(buff, 0, contentLen);
                        contentLen = fs.Read(buff, 0, buffLength);
                    }
                    strm.Close();
                    fs.Close();
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine("Закройте " + ex.Message);
            }

            return uri + fileName;
        }
    }
}
