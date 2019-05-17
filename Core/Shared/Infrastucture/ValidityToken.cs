namespace SAC.Stock.Infrastucture
{
    using System;
    using System.Linq;
    internal class ValidityToken
    {
        public static string CreateToken(string email, string alterValue, int? validPeriodInDays = null)
        {
            var time = BitConverter.GetBytes(DateTime.Today.AddDays(validPeriodInDays != null ? (int)validPeriodInDays : 1).ToBinary());
            var emailCode = BitConverter.GetBytes(email.GetHashCode());
            var alterValueCode = BitConverter.GetBytes(alterValue.GetHashCode());
            var token = time.Concat(emailCode.Concat(alterValueCode));
            var tokenResult = Convert.ToBase64String(token.ToArray());

            return tokenResult;
        }
        public static bool DecodeToken(string token, bool validatePeriod, int? validPeriodInDays = null)
        {
            try
            {
                var hours = validPeriodInDays != null ? ((int)validPeriodInDays * -24) : -24;
                var data = BitConverter.ToInt64(Convert.FromBase64String(token.Replace(" ", "+")), 0);
                var when = DateTime.FromBinary(data);
                return !validatePeriod || when >= DateTime.UtcNow.AddHours(hours);
            }
            catch (Exception)
            {
                throw new Exception("Código de seguridad inválido.");
            }
        }
    }
}
