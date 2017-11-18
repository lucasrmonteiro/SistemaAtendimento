using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGCA.Models.Entity
{
    [Serializable]
    public class ConfigSFtp
    {
        /// <summary>
        /// Id Config
        /// </summary>
        public virtual int Id_configSftp { get; set; }

        /// <summary>
        /// Host
        /// </summary>
        public virtual string Dsc_host { get; set; }

        /// <summary>
        /// Port
        /// </summary>
        public virtual int Num_port { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        public virtual string Dsc_username { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public virtual string Dsc_password { get; set; }

        /// <summary>
        /// Path
        /// </summary>
        public virtual string Dsc_path { get; set; }
    }
}