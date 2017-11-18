using System;
using SGCA.Models.Manager;
using SGCA.Models.Manager.Impl;
using SGCA.Tests.Mock;
using System.Collections.Generic;
using SGCA.Models.Entity;
using SGCA.Models.Manager.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SGCA.Tests.Managers
{
    [TestClass]
    public class ProcessoManagerTest
    {

        private IProcessoManager _manager;

        [TestInitialize]
        public void SetUp()
        {
            _manager = new ProcessoManagerImpl(new GenericMockDAO());
        }


        /// <summary>
        /// O sucesso deste teste pode ter logs no eventviewer,
        /// pois podem existir linha de arquivos com problema
        /// </summary>
        [TestMethod]
        public void ImportarLegadoComSucesso()
        {
            bool sucesso;
            try
            {
                sucesso = _manager.ImportarLegado();
            }
            catch (Exception)
            {
                sucesso = false;
            }

            Assert.IsTrue(sucesso);
        }

        /// <summary>
        /// Para este tester funcionar, após iniciar o processo pausar e apagar arquivos *.log da pasta raiz
        /// </summary>
        [TestMethod]
        public void ImportarLegadoFalha()
        {
            bool sucesso;
            try
            {
                _manager.ImportarLegado();
                sucesso = false;
            }
            catch (Exception)
            {
                sucesso = true;
            }

            Assert.IsTrue(sucesso);
        }

    }
}
