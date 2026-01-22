using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace YMM.Application.Helper
{
    public class GenerateOtp
    {
        public static string Otp()
        {
            Random rnd = new Random();
            int otp= rnd.Next(100000,999999);
            return otp.ToString();
        }
        public static string Hash(string otp)
        {
            var sHA = SHA256.Create();
            var hash=sHA.ComputeHash(Encoding.Unicode.GetBytes(otp));
            return Convert.ToBase64String(hash);
        }
    }
}
