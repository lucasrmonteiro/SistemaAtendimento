using NetUtil.Util.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace NetUtil.Util.Filter
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AbstractPagingFilter
    {
        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public virtual int? CurrentPage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public virtual int? PageSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public virtual int? TotalItems { get; set; }

        /// <summary>
        /// Total de páginas do Grid
        /// </summary>
        [NotMapped]
        public virtual int? TotalPages
        {
            get
            {
                if (TotalItems != null && PageSize != null)
                {
                    return (int)Math.Ceiling((decimal)TotalItems.Value / PageSize.Value);

                }
                else
                {
                    return null;
                }
            }
        }

    }
}
