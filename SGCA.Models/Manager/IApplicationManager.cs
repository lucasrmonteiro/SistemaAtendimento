using SGCA.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGCA.Models.Manager {
    public interface IApplicationManager 
    {
        ConfigSFtp FindConfigSFtp();

        ConfigEmail FindConfigEmail();
    }
}