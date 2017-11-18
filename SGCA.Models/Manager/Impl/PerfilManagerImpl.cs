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
    public class PerfilManagerImpl : BaseManagerImpl<Perfil>, IPerfilManager
    {

        public PerfilManagerImpl(IGenericDAO dao)
        {
            this._dao = dao;
        }

        public IList<Perfil> FindAll()
        {
            return _dao.FindAll<Perfil>();
        }

        public Perfil FindByPk(int pk)
        {
            return _dao.FindByPK<Perfil>(pk);
        }
    }
}