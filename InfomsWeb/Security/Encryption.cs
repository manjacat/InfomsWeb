using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace InfomsWeb.Security
{
    public class Encryption
    {
        string LeadingKey = "0x";

        public Encryption()
        {
        }

        private byte[] CreateBinaryPwd(string password, string username)
        {

            //Encrypt the password
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] hashedBytes = null;
            UTF8Encoding encoder = new UTF8Encoding();
            hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(password + username));

            return hashedBytes;
        }

        //Encrypt Password
        public string GetEncrypt(string password, string username)
        {
            byte[] byteArray = CreateBinaryPwd(password, username.ToLower());
            return (LeadingKey + BitConverter.ToString(byteArray).Replace("-", ""));
        }
    }
}