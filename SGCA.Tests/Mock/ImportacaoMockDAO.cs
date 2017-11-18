using SGCA.Models.DAO;
using SGCA.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGCA.Tests.Mock
{
    public class ImportacaoMockDAO : GenericMockDAO
    {

        public override IList<T> FindByFilter<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            IList<Importacao> lista = new List<Importacao>();
            
            Importacao imp = new Importacao();
            imp.Arquivo = "SGCA_INPUT_DATA_201602021805.csv";
            lista.Add(imp);

            return (IList<T>)lista;
        }

    }
}
