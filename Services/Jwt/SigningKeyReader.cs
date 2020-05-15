using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LuneauAuthentication.Services.Jwt
{
    public class SigningKeyReader
    {
        public SigningKeyReader()
        {
        }

        private string ReadKey(string path)
        {
            if (!File.Exists(path)) return null;
            return "";
        }
    }
}
