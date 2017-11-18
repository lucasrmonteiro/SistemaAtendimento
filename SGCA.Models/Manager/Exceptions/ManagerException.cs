using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGCA.Models.Manager.Exceptions
{
    public class ManagerException : Exception
    {
        public ManagerException(string message)
            : base(message)
        {
        }

        public ManagerException(string message, Exception ex)
            : base(message, ex)
        {
        }

    }
}
