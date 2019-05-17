namespace SAC.Stock.Infrastucture.Notification
{
    using System.Configuration;
    public static class Settings
    {
        public const string FirstLoginMessage =
  "Estimado/a \n" + "{0} \n\n" + "Ud. ha ingresado por primera vez a SAC Stock. \n\n"
  + "Ahora está en condiciones de activar su cuenta para comenzar a trabajar con el sistema. \n\n"
  + "Más abajo encontrará información detallada del proceso de activación. \n\n" + "Para ACTIVAR su cuenta deberá ingresar en el siguiente enlace: \n"
  + "{1} \n\n\n" + "-------------------------------------------------------------------------------------------\n"
  + "En caso de que el enlace no responda \n" + "-------------------------------------------------------------------------------------------\n"
  + "Copie y pegue la dirección completa en su navegador \n" + "{1} \n\n" + "El enlace incluye el siguiente código de seguridad: \n" + "{2} \n"
  + "el cual es requerido por el sistema. \n\n" + "IMPORTANTE: \n" + "El código de seguridad tiene vigencia por 48hs. \n"
  + "Pasado tal período perderá validez y será necesario generarlo nuevamente. \n" + "Contáctese con el administrador del sistema que le dió el alta.\n"
  + "(consulte mail de bienvenida) \n\n\n" + "-------------------------------------------------------------------------------------------\n"
  + "Activación de Cuenta \n" + "-------------------------------------------------------------------------------------------\n"
  + "El proceso de activación le demandará que cambie su nombre de usuario. \n\n"
  + "Note que su nombre de usuario siempre será precedido por el prefijo \"{3}\", \n" + "indicando que Ud. es staff perteneciente {4}. \n\n\n"
  + "Atentamente, \n" + "SAC Stock \n";

        public static string DefaultEmailTo
        {
            get
            {
                return ConfigurationManager.AppSettings["DefaultEmailTo"];
            }
        }
        public static string DefaultEmailFrom
        {
            get
            {
                return ConfigurationManager.AppSettings["DefaultEmailFrom"];
            }
        }
        public static string SmtpUser
        {
            get
            {
                return ConfigurationManager.AppSettings["SmtpUser"];
            }
        }
        public static string SmtpPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["SmtpPassword"];
            }
        }
        public static string SmtpServerHost
        {
            get
            {
                return ConfigurationManager.AppSettings["SmtpServerHost"];
            }
        }
        public static bool SmtpEnableSsl
        {
            get
            {
                bool result;
                bool.TryParse(ConfigurationManager.AppSettings["SmtpEnableSsl"], out result);
                return result;
            }
        }
        public static int SmtpMailPort
        {
            get
            {
                int result;
                int.TryParse(ConfigurationManager.AppSettings["SmtpMailPort"], out result);

                return result;
            }
        }

        public static string IsSecureConnection
        {
            get
            {
                return GetValue("IsSecureConnection", false.ToString());
            }
        }

        private static string GetValue(string key, string defaultValue)
        {
            return string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[key]) ? defaultValue : ConfigurationManager.AppSettings[key];
        }
    }
}
