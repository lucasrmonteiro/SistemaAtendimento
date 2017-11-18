using SGCA.Models.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGCA.Models.Manager.Exceptions
{
    public class TimeStampInvalidoException : ManagerException
    {
        public TimeStampInvalidoException()
            : base(Resources.mensagem_erro_timestamp_arquivo_invalido)
        {
        }
    }
}
