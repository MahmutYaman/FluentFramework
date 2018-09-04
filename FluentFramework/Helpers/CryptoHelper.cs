using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace FluentFramework.Helpers
{
    public static class CryptoHelper
    {
        private const int TokenSizeInBytes = 16;
        private const int Pbkdf2Count = 1000;
        private const int Pbkdf2SubkeyLength = 256 / 8;
        private const int SaltSize = 128 / 8;

        public static string GenerateSalt(int byteLength = SaltSize)
        {
            var buff = new byte[byteLength];
            using (var prng = new RNGCryptoServiceProvider())
                prng.GetBytes(buff);

            return Convert.ToBase64String(buff);
        }

        public static string Hash(string value, string algorithm = "sha256")
            => Hash(Encoding.UTF8.GetBytes(value), algorithm);

        public static string Hash(byte[] value, string algorithm = "sha256")
        {
            using (var alg = HashAlgorithm.Create(algorithm))
            {
                if (alg != null)
                {
                    var computedHash = alg.ComputeHash(value);
                    var hex = new char[computedHash.Length * 2];

                    for (int i = 0; i < computedHash.Length; i++)
                    {
                        var hexChar = (byte)(computedHash[i] >> 4);
                        hex[i * 2] = (char)(hexChar > 9 ? hexChar + 0x37 : hexChar + 0x30);
                        hexChar = (byte)(computedHash[i] & 0xF);
                        hex[i * 2 + 1] = (char)(hexChar > 9 ? hexChar + 0x37 : hexChar + 0x30);
                    }

                    return new string(hex);
                }
                else throw new InvalidOperationException(string.Format("Not supported hash algorithm: {0}", algorithm));
            }
        }

        public static string SHA1(string value)
            => Hash(value, "sha1");

        public static string SHA256(string value)
            => Hash(value, "sha256");

        /// <summary>
        /// PBKDF2 with HMAC-SHA1, 128-bit salt, 256-bit subkey, 1000 iterations.
        /// (See also: SDL crypto guidelines v5.1, Part III)
        /// Format: { 0x00, salt, subkey }
        /// </summary>
        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] subkey;
            using (var deriveBytes = new Rfc2898DeriveBytes(password, SaltSize, Pbkdf2Count))
            {
                salt = deriveBytes.Salt;
                subkey = deriveBytes.GetBytes(Pbkdf2SubkeyLength);
            }

            var outputBytes = new byte[1 + SaltSize + Pbkdf2SubkeyLength];
            Buffer.BlockCopy(salt, 0, outputBytes, 1, SaltSize);
            Buffer.BlockCopy(subkey, 0, outputBytes, 1 + SaltSize, Pbkdf2SubkeyLength);
            return Convert.ToBase64String(outputBytes);
        }

        /// <param name="hashedPassword">This must be of the format of HashWithPassword (salt + Hash(salt+value)</param>
        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            var hashedPasswordBytes = Convert.FromBase64String(hashedPassword);
            if (hashedPasswordBytes.Length != (1 + SaltSize + Pbkdf2SubkeyLength) || hashedPasswordBytes[0] != (byte)0x00)
                return false;

            var salt = new byte[SaltSize];
            Buffer.BlockCopy(hashedPasswordBytes, 1, salt, 0, SaltSize);
            var storedSubkey = new byte[Pbkdf2SubkeyLength];
            Buffer.BlockCopy(hashedPasswordBytes, 1 + SaltSize, storedSubkey, 0, Pbkdf2SubkeyLength);

            byte[] generatedSubkey;
            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, Pbkdf2Count))
                generatedSubkey = deriveBytes.GetBytes(Pbkdf2SubkeyLength);

            return ByteArraysEqual(storedSubkey, generatedSubkey);
        }

        // Compares two byte arrays for equality. The method is specifically written so that the loop is not optimized.
        [MethodImpl(MethodImplOptions.NoOptimization)]
        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (a == null || b == null || a.Length != b.Length)
                return false;

            var areSame = true;
            for (int i = 0; i < a.Length; i++)
                areSame &= (a[i] == b[i]);

            return areSame;
        }
    }
}
