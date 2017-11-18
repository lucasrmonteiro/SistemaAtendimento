using NetUtil.Util.Helper;
using SGCA.Models.Manager;
using SGCA.Models.Manager.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGCA.Import.Controller
{
    public class ImportController
    {
        private IImportacaoManager importacaoManager;

        public ImportController(IImportacaoManager im)
        {
            this.importacaoManager = im;
        }

        public void ExecutaImportacao()
        {
            try
            {
                importacaoManager.ImportaArquivos();
            }
            catch (ManagerException ex)
            {
                EventViewerHelper.LogBusinessFailure(ex.Message, this.GetType().Name, "ExecutaImportacao");
            }
        }
    }
}
