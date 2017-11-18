
using SGCA.Models.Entity;
using SGCA.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Net;

namespace SGCA.Models.Helpers
{
    public class EmailHelper
    {
         /// <summary>
        /// Método responsável por enviar email
        /// </summary>
        /// <param name="emailDestinatarios">Ilist contendo a lista de destinatários</param>
        /// <param name="titulo">Título do email</param>
        /// <param name="mensagem">Mensagem a ser enviada é permitido o envio de mensagens HTML</param>
        public static void EnviaEmail(ConfigEmail configEmail, IList<string> emailDestinatarios, string assunto, string mensagem)
        {

            // Define o nome do remetente
            string nomeRemetente = configEmail.Dsc_nome_remetente;

            // Define o email do remente
            string emailRemetente = configEmail.Dsc_email_remetente;

            // Senha de acesso
            string senha = configEmail.Dsc_senha_email;

            // Endereço do servidor de email
            string servidor = configEmail.Dsc_servidor_email;

            // Porta para envio de email
            int porta = configEmail.Num_porta_email;

            bool ssl = false;

            // Verifica se usaremos ou não SSL
            if (configEmail.Char_ssl.Equals("s"))
            {
                ssl = true;
            }
              
            // Título do email
            string assuntoMensagem = assunto;

            // Conteudo do email, permitido HTML
            string conteudoMensagem = mensagem;

            //Cria objeto com dados do e-mail.
            MailMessage objEmail = new MailMessage();

            //Define o Campo From e ReplyTo do e-mail.
            objEmail.From = new System.Net.Mail.MailAddress(nomeRemetente + "<" + emailRemetente + ">");


            foreach (string destinatario in emailDestinatarios)
            {
                //Define os destinatários do e-mail.
                objEmail.To.Add(destinatario);
            }
    
            //Define a prioridade do e-mail.
            objEmail.Priority = System.Net.Mail.MailPriority.Normal;

            //Define o formato do e-mail HTML
            objEmail.IsBodyHtml = true;

            //Define título do e-mail.
            objEmail.Subject = assuntoMensagem;

            //Define o corpo do e-mail.
            objEmail.Body = conteudoMensagem;

            //Para evitar problemas de caracteres "estranhos", configuramos o charset para "ISO-8859-1"
            objEmail.SubjectEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
            objEmail.BodyEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");

            //Cria objeto com os dados do SMTP
            System.Net.Mail.SmtpClient objSmtp = new System.Net.Mail.SmtpClient();

            objSmtp.Host = servidor;

            objSmtp.Port = porta;

            objSmtp.EnableSsl = false;

            objSmtp.UseDefaultCredentials = false;

            //Alocamos o endereço do host para enviar os e-mails  
            //objSmtp.Credentials = new System.Net.NetworkCredential("lucasrmonteiroooo@gmail.com", "807472");


            //Enviamos o e-mail através do método .send()
            try
            {
                objSmtp.Send(objEmail);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocorreram problemas no envio do e-mail. Erro = " + ex.Message);
            }
            finally
            {
                //Excluímos o objeto de e-mail da memória
                objSmtp.Dispose();
                objEmail.Dispose();
                //anexo.Dispose();
            }
        }
    }
}