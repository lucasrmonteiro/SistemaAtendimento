using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetUtil.Util.Entity
{
    public class ConnDB : DbContext
    {
        private const string CONNECTION_STRING_KEY = "ConnDB";

        public ConnDB()
            : base(CONNECTION_STRING_KEY)
        {
            Database.SetInitializer<ConnDB>(new CreateDatabaseIfNotExists<ConnDB>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}
