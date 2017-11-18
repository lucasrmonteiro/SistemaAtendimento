using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Reflection;
using System.ComponentModel;

namespace SGCA.Models.Enums
{
    public enum EnumDemanda
    {
        [Description("CAP")]
        CAP = 1,
        [Description("E-mail")]
        EMAIL,
        [Description("Inbox Workflow")]
        INBOX_WORKFLOW,
        [Description("Inbox Encerramento")]
        INBOX_ENCERRAMENTO,
    }
}