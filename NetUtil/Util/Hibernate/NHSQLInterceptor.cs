using NHibernate;
using NHibernate.Event;
using NHibernate.SqlCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetUtil.Util.Hibernate
{
    public class NHSQLInterceptor : EmptyInterceptor, IInterceptor
    {
        public override SqlString OnPrepareStatement(SqlString sql)
        {
            // para visualizar o SQL ou imprimir antes de retornar ao fluxo normal
            //Console.WriteLine(sql.ToString());
            System.Diagnostics.Debug.WriteLine(sql.ToString());

            //retornando ao fluxo normal
            return base.OnPrepareStatement(sql);
        }
    }
}
