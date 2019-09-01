using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAWARE
{
    [Serializable]
    public class FTP_Data
    {
        public FTP_Data(string name, string port, string username, string password)
        {
            Name = name;
            Port = port;
            Username = username;
            Password = password;
        }

        public FTP_Data()
        {

        }

        public string Name { get; set; }
        public string Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
