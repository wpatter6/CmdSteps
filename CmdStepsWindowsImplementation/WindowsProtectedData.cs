using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace CmdStepsWindowsImplementation
{
    static class WindowsProtectedData
    {
        public static byte[] ProtectString(string input, string entropy = null, DataProtectionScope scope = DataProtectionScope.CurrentUser)
        {
            var inputBytes = input == null ? null : Encoding.ASCII.GetBytes(input);
            var entropyBytes = entropy == null ? null : Encoding.ASCII.GetBytes(entropy);

            var resultBytes = ProtectedData.Protect(inputBytes, entropyBytes, scope);
            var result = Encoding.ASCII.GetString(resultBytes);

            return resultBytes;
        }
        public static string UnprotectString(byte[] input, string entropy = null, DataProtectionScope scope = DataProtectionScope.CurrentUser)
        {
            var inputBytes = input;// == null ? null : Encoding.ASCII.GetBytes(input);
            var entropyBytes = entropy == null ? null : Encoding.ASCII.GetBytes(entropy);

            var resultBytes = ProtectedData.Unprotect(inputBytes, entropyBytes, scope);
            var result = Encoding.ASCII.GetString(resultBytes);

            return result;
        }
    }
}
