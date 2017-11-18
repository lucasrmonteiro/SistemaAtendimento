using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGCA.Models.Manager.Base;
using NetUtil.Util.Hibernate;
using NHibernate;
using NHibernate.Criterion;
using SGCA.Models.DAO;
using SGCA.Models.Entity;

namespace SGCA.Models.Manager.Impl {
    public class ApplicationManagerImpl : IApplicationManager
    {
        private IGenericDAO _dao;

        public ApplicationManagerImpl(IGenericDAO dao)
        {
            this._dao = dao;
        }

        public ConfigSFtp FindConfigSFtp()
        {
            return _dao.FindByPK<ConfigSFtp>(1);
        }


        public ConfigEmail FindConfigEmail()
        {
            return _dao.FindByPK<ConfigEmail>(1);
        }
    }
}
