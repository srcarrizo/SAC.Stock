namespace SAC.Stock.Domain.Infrastructure
{
    using SAC.Seed.Cryptography;
    using System;
    using System.Globalization;

    internal static class TokenIdentity
    {
        private const int HashId = 0;

        public static string New()
        {
            var rand = new Random();
            return rand.Next(100, 999).ToString(CultureInfo.InvariantCulture) + rand.Next(256, 4096).ToString("X3")
                   + rand.Next(100, 999).ToString(CultureInfo.InvariantCulture);
        }

        public static string NewCrypto()
        {
            return Encode(New());
        }

        public static string Decode(string crypto)
        {
            return Crypto.Decode(crypto, HashId);
        }


        public static string Encode(string token)
        {
            return Crypto.Encode(token, HashId);
        }
    }
}