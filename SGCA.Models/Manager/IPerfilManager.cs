using SGCA.Models.Entity;
using SGCA.Models.Manager.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGCA.Models.Manager
{
    public interface IPerfilManager : IBaseManager<Perfil>
    {
        IList<Perfil> FindAll();

        Perfil FindByPk(int p);
    }
}