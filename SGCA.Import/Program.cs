using NetUtil.Util.Helper;
using NetUtil.Util.Spring;
using SGCA.Import.Controller;
using SGCA.Models.DAO;
using SGCA.Models.DAO.Impl;
using SGCA.Models.Manager;
using SGCA.Models.Manager.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGCA.Import
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IGenericDAO dao = new GenericDAOImpl();
                IImportacaoManager manager = new ImportacaoManagerImpl(dao);
                ImportController controller = new ImportController(manager);
                //ImportController controller = ServiceLocator.GetObject<ImportController>();
                controller.ExecutaImportacao();
            }
            catch (Exception ex)
            {
                EventViewerHelper.LogException(ex.Message);
            }
        }
    }
}
