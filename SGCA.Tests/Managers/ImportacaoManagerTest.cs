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
    public class ImportacaoManagerTest
    {

        private IImportacaoManager _manager;

        [TestInitialize]
        public void SetUp()
        {
            _manager = new ImportacaoManagerImpl(new ImportacaoMockDAO());
        }

        [TestMethod]
        public void AdicionaExecutaImportacaoNomeclaturaInvalida()
        {
            Importacao i = new Importacao();
            i.Arquivo = "inexistente.csv";

            bool sucesso;
            try
            {
                _manager.AdicionaExecutaImportacao(i);
                sucesso = false;
            }
            catch (NomeclaturaInvalidaException)
            {
                sucesso = true;
            }
            catch (Exception)
            {
                sucesso = false;
            }

            Assert.IsTrue(sucesso);
        }

        [TestMethod]
        public void AdicionaExecutaImportacaoTimeStapInvalido()
        {
            Importacao i = new Importacao();
            i.Arquivo = "inexistente_201602021604.csv";

            bool sucesso;
            try
            {
                _manager.AdicionaExecutaImportacao(i);
                sucesso = false;
            }
            catch (TimeStampInvalidoException)
            {
                sucesso = true;
            }
            catch (Exception)
            {
                sucesso = false;
            }

            Assert.IsTrue(sucesso);
        }

        [TestMethod]
        public void AdicionaExecutaImportacaoArquivoInexistente()
        {
            //atentar para o time stamp, configurado para ate 1 hora

            Importacao i = new Importacao();
            i.Arquivo = "inexistente_201602021804.csv";

            bool sucesso;
            try
            {
                _manager.AdicionaExecutaImportacao(i);
                sucesso = false;
            }
            catch (ImportacaoIndexadaPeloSistemaException)
            {
                sucesso = true;
            }
            catch (Exception)
            {
                sucesso = false;
            }

            Assert.IsTrue(sucesso);
        }

        [TestMethod]
        public void AdicionaExecutaImportacaoLayoutInvalido()
        {
            ///para este teste necessario que o arquivo esteja 
            ///na pasta raiz configurada para os arquivos a serem importados
            ///com o layout invalido

            Importacao i = new Importacao();
            i.Arquivo = "SGCA_INPUT_DATA_201602021800.csv";

            bool sucesso;
            try
            {
                _manager.AdicionaExecutaImportacao(i);
                sucesso = false;
            }
            catch (LayoutArquivoInvalidoException)
            {
                sucesso = true;
            }
            catch (Exception)
            {
                sucesso = false;
            }

            Assert.IsTrue(sucesso);
        }

        [TestMethod]
        public void AdicionaExecutaImportacao()
        {
            ///para este teste necessario que o arquivo esteja 
            ///na pasta raiz configurada para os arquivos a serem importados
            ///com o layout e timestamp valido

            Importacao i = new Importacao();
            i.Arquivo = "SGCA_INPUT_DATA_201602021805.csv";

            bool sucesso;
            try
            {
                _manager.AdicionaExecutaImportacao(i);
                sucesso = true;
            }
            catch (Exception)
            {
                sucesso = false;
            }

            Assert.IsTrue(sucesso);
        }

        //[TestMethod]
        //public void ImportaArquivo(int? id)
        //{
            //_manager.ImportaArquivo(int? id)
        //}

        [TestMethod]
        public void ImportaArquivos()
        {
            ///para este teste necessario que o arquivo com nome igual ao retornado na classe ImportacaoMockDAO 
            ///esteja na pasta raiz configurada para os arquivos a serem importados
            ///com o layout e timestamp valido
            
            bool sucesso;
            try
            {
                _manager.ImportaArquivos();
                sucesso = true;
            }
            catch (Exception)
            {
                sucesso = false;
            }

            Assert.IsTrue(sucesso);
        }
    }
}
