using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetUtil.Util.Helper
{
    /// <summary>
    ///     Helper para log através do Event Viewer.
    ///     Atenção: Caso o nome do sistema de origem da mensagem
    ///     ainda não esteja registrado no Event Viewer, é necessário
    ///     executar o código como Administrador.
    ///     Abrir o Visual Studio como Administrador.
    ///     Carece de testes quando em produção para avaliar o comportamento.
    /// 
    ///     Executar os comandos a seguir com permissão de administrador:
    ///    i.	eventcreate /t information /SO APPLICATION_NAME /l application /id 1 /D "Creation of APPLICATION_NAME Event Source"
    /// 
    /// </summary>
    public static class EventViewerHelper
    {
        /// <summary>
        ///     Atributo constante de classe que armazena
        ///     valor necessário para adicionar entrada no Event Viewer.
        /// </summary>
        private const string _EVLOG = "Application";
        
        /// <summary>
        /// 
        /// </summary>
        private const string APP_NAME = "application_name";

        /// <summary>
        ///     Adiciona entrada no Event Viewer.
        /// </summary>
        /// <param name="message">
        ///     Deve conter a mensagem a ser gravada.
        /// </param>
        /// <param name="source">
        ///     Deve conter o nome do sistema de origem.
        /// </param>
        /// <param name="logType">
        ///     Tipo de entrada do Event Viewer.
        ///     Ex.: ERROR, WARNING, INFORMATION, etc..
        /// </param>
        private static void LogOnEventViewer(string message, string source,
                                                EventLogEntryType logType)
        {
            /// Verifica se o nome do sistema de origem está cadastrado
            /// no Event Viewer. Se não estiver, cadastra.
            /// Pode lançar Security Exception se executante
            /// não possuir autorização.
            if (!EventLog.SourceExists(source))
            {
                EventLog.CreateEventSource(source, _EVLOG);
            }

            /// Efetivamente escreve a entrada no EventViewer.
            EventLog.WriteEntry(source, message, logType, 1);
        }

        /// <summary>
        ///     Adiciona entrada no Event Viewer como ERROR.
        ///     Uso planejado para casos de falhas impeditivas.
        /// </summary>
        /// <param name="message">
        ///     Deve conter a mensagem a ser gravada.
        /// </param>
        /// <param name="source">
        ///     Deve conter o nome do sistema de origem.
        ///     Parâmetro com valor default "MetaVaultWS".
        /// </param>
        public static void LogException(string message, string source)
        {
            LogOnEventViewer(message, source, EventLogEntryType.Error);
        }

        /// <summary>
        ///     Adiciona entrada no Event Viewer como WARNING.
        ///     Uso planejado para casos de falhas não impeditivas.
        /// </summary>
        /// <param name="message">
        ///     Deve conter a mensagem a ser gravada.
        /// </param>
        /// <param name="source">
        ///     Deve conter o nome do sistema de origem.
        ///     Parâmetro com valor default "MetaVaultWS".
        /// </param>
        public static void LogBusinessWarning(string message, string source)
        {
            LogOnEventViewer(message, source, EventLogEntryType.Warning);
        }

        public static void LogBusinessSucess(string message, string source)
        {
            LogOnEventViewer(message, source, EventLogEntryType.SuccessAudit);
        }

        public static void LogBusinessFailure(string message, string source)
        {
            LogOnEventViewer(message, source, EventLogEntryType.FailureAudit);
        }

        public static void LogInformation(string message, string source)
        {
            LogOnEventViewer(message, source, EventLogEntryType.Information);
        }

        #region - utilizando app name do web/app config

        public static void LogException(string message, string currentClass, string currentMethod) 
        {
            LogException(currentClass + " - " + currentMethod + ": " + message);
        }

        public static void LogException(string message)
        {
            string source = ConfigurationManager.AppSettings[APP_NAME];
            LogOnEventViewer(message, source, EventLogEntryType.Error);
        }

        public static void LogBusinessFailure(string message, string currentClass, string currentMethod)
        {
            LogBusinessFailure(currentClass + " - " + currentMethod + ": " + message);
        }

        public static void LogBusinessFailure(string message)
        {
            string source = ConfigurationManager.AppSettings[APP_NAME];
            LogOnEventViewer(message, source, EventLogEntryType.FailureAudit);
        }

        public static void LogInformation(string message, string currentClass, string currentMethod)
        {
            LogInformation(currentClass + " - " + currentMethod + ": " + message);
        }

        public static void LogInformation(string message)
        {
            string source = ConfigurationManager.AppSettings[APP_NAME];
            LogOnEventViewer(message, source, EventLogEntryType.Information);
        }

        public static void LogBusinessWarning(string message, string currentClass, string currentMethod)
        {
            LogBusinessWarning(currentClass + " - " + currentMethod + ": " + message);
        }

        public static void LogBusinessWarning(string message)
        {
            string source = ConfigurationManager.AppSettings[APP_NAME];
            LogOnEventViewer(message, source, EventLogEntryType.Warning);
        }

        public static void LogBusinessSucess(string message, string currentClass, string currentMethod)
        {
            LogBusinessSucess(currentClass + " - " + currentMethod + ": " + message);
        }

        public static void LogBusinessSucess(string message)
        {
            string source = ConfigurationManager.AppSettings[APP_NAME];
            LogOnEventViewer(message, source, EventLogEntryType.SuccessAudit);
        }

        #endregion

        public static bool CheckEventMinutesAgo(string message,
                                                string source,
                                                int minutesSpan)
        {
            EventLog[] logs = EventLog.GetEventLogs();
            EventLog l = new EventLog();

            foreach (EventLog log in logs)
            {
                if (log.Log.Equals(_EVLOG))
                {
                    l = log;
                    break;
                }
            }

            foreach (EventLogEntry entry in l.Entries)
            {
                if (entry.Source.Equals(source)
                    && entry.Message.Equals(message))
                {
                    if (entry.TimeGenerated > DateTime.Now.AddMinutes(-1.0 * minutesSpan))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}