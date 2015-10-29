using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace RealEstate.Classes
{
    class Hashing
    {
        public string HashFile(string path)
        {
            HashAlgorithm ha = HashAlgorithm.Create();
            FileStream file = new FileStream(path, FileMode.Open);
            byte[] hash = ha.ComputeHash(file);
            file.Close();
            return BitConverter.ToString(hash);

        }
        
    }
}
