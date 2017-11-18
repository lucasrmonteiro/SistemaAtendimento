using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SGCA.Models.Manager.Base;
using SGCA.Models.Entity;
using SGCA.Models.Manager.Base.Impl;
using SGCA.Models.DAO;

namespace SGCA.Models.Manager.Impl
{
    public class StatusManagerImpl : BaseManagerImpl<Status>, IStatusManager
    {

        public StatusManagerImpl(IGenericDAO dao)
        {
            this._dao = dao;
        }

        public IList<Status> FindAll()
        {
            return _dao.FindAll<Status>();
        }
        
        public Status FindByPk(int pk)
        {
            return _dao.FindByPK<Status>(pk);
        }
    }
}