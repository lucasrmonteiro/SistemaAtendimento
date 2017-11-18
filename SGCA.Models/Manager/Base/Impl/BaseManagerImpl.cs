using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetUtil.Util.Hibernate.Interfaces;
using NetUtil.Util.Hibernate;
using NetUtil.Util.Enums;
using NHibernate;
using NHibernate.Criterion;
using SGCA.Models.DAO;

namespace SGCA.Models.Manager.Base.Impl {

    [Serializable]
    public class BaseManagerImpl<T> : IBaseManager<T> {

        protected IGenericDAO _dao;
    }
}
