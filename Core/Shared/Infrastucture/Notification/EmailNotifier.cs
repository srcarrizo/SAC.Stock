namespace SAC.Stock.Infrastucture.Notification
{
    using SAC.Membership.Service.BaseDto;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Mail;
    using System.Text;
    public static class EmailNotifier
    {
        public const string ExampleText =
            "Estimado/a \n" + "{0} \n\n" + "Has solicitado la datos freeedsw . \n\n"
            + "Recordá: \n"
            + "- La ddess en vigencia a los {2} días, a partir de hoy. \n"
            + "- Si deseas cancelar la solicitud desaaaaaaaa, deberás ingresar en el siguiente enlace: \n"
            + "{1} \n\n\n" + "-------------------------------------------------------------------------------------------\n"
            + "En caso de que el enlace no responda \n" + "-------------------------------------------------------------------------------------------\n"
            + "Copia y pega la dirección completa en tu navegador \n" + "{1} \n\n"
            + "IMPORTANTE: \n"
            + "Sólo podrás cancelar la solicitud de asdasdasdasd, si ésta aún no ha entrado en vigencia. \n"
            + "Una vez procesada la baja, esta no podrá ser cancelada. \n\n"
            + "Atentamente, \n" + "SAC \n";

        public const string UserName = "Usuario: ";        

        public const string Password = "Contraseña: ";
        public static void TestMailing(string testingEmail, string testingFirstName, string testingUrlUnsusbcription, string testingSubject, string testingBody, string testingImageFileName)
        {
            var htmlFile = new StringBuilder(testingBody);

            htmlFile.Replace("[[Customer.FirstName]]", testingFirstName);
            htmlFile.Replace("[[Customer.UrlUnsubscription]]", testingUrlUnsusbcription);
            htmlFile.Replace("img src=", "img src='" + testingImageFileName + "'");

            var body = htmlFile.ToString();

            SendEmailHtml(
                testingSubject,
                body,
                new[] { testingEmail },
                null,
                null,
                null,
                "El correo no se visualiza correctamente porque su visualizador no soporta mensajes en formato HTML.\n");
        }

        private static void SendEmailPlainText(string subject, string body, string[] to = null, string[] cc = null, string[] bcc = null)
        {
            to = to ?? new[] { Settings.DefaultEmailTo };

            var msg = new MailMessage(Settings.DefaultEmailFrom, string.Join(",", to)) { Body = body, Subject = subject, IsBodyHtml = false };
            var smtp = new SmtpClient(Settings.SmtpServerHost, Settings.SmtpMailPort) { EnableSsl = Settings.SmtpEnableSsl };
            if (!string.IsNullOrWhiteSpace(Settings.SmtpUser))
            {
                smtp.Credentials = new NetworkCredential(Settings.SmtpUser, Settings.SmtpPassword);
            }

            if (cc != null)
            {
                msg.CC.Add(string.Join(",", cc));
            }

            if (bcc != null)
            {
                msg.Bcc.Add(string.Join(",", bcc));
            }

            smtp.Send(msg);
        }

        public static void NewAccountCreated(UserDto user, string url)
        {
            ////Created Account: La cuenta ha sido creada. Hay que enviarle una especie de bienvenida escueta, ya que se le va a enviar otro correo con toda la bienvenida y el reglamento. Aca hay que darle el instructivo para abrir por primera vez su cuenta.
            var token = ValidityToken.CreateToken(user.Email, user.UserName);
            url = url + "/Account/PasswordConfirm?" + "t=" + token + "&user=" + user.Id;

            var prefixUserName = user.UserName.Substring(0, 2);
            var scope = prefixUserName.ToUpper() == "GS" ? "al xx" : (prefixUserName.ToUpper() == "FS" ? "a yy" : string.Empty);
            var body = string.Format(Settings.FirstLoginMessage, user.GetFullName(), url, token, prefixUserName, scope);

            SendEmailPlainText("SAC: Sesión iniciada por primera vez", body, new[] { user.Email });
        }

        private static void SendEmailHtml(
          string subject, string htmlBody, string[] to = null, string[] cc = null, string[] bcc = null, IEnumerable<LinkedResource> resources = null, string plainBody = null)
        {
            to = to ?? new[] { Settings.DefaultEmailTo };
            var msg = new MailMessage(Settings.DefaultEmailFrom, string.Join(",", to)) { Subject = subject, IsBodyHtml = true };
            var smtp = new SmtpClient(Settings.SmtpServerHost, Settings.SmtpMailPort) { EnableSsl = Settings.SmtpEnableSsl };
            if (!string.IsNullOrWhiteSpace(Settings.SmtpUser))
            {
                smtp.Credentials = new NetworkCredential(Settings.SmtpUser, Settings.SmtpPassword);
            }

            var plain = plainBody;
            if (string.IsNullOrWhiteSpace(plainBody))
            {
                plain = "El contenido de este correo solo puede leerse en programas que soportan formato html.";
            }

            var htmlView = AlternateView.CreateAlternateViewFromString(htmlBody, null, "text/html");
            if (resources != null)
            {
                foreach (var res in resources)
                {
                    htmlView.LinkedResources.Add(res);
                }
            }

            if (cc != null)
            {
                msg.CC.Add(string.Join(",", cc));
            }

            if (bcc != null)
            {
                msg.Bcc.Add(string.Join(",", bcc));
            }

            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(plain, null, "text/plain"));
            msg.AlternateViews.Add(htmlView);
            smtp.Send(msg);
        }
    }
}
