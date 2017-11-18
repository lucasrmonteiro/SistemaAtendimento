using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGCA.Models.Entity
{
    [Serializable]
    public class Mobilidade
    {
        #region Propriedades

        public virtual int CodigoMobilidade { get; set; }
        public virtual int? ZonaAtendimento { get; set; }
        public virtual int? Quantidade { get; set; }
        public virtual decimal? Valor { get; set; }
        public virtual string StatusOs { get; set; }
        public virtual string SubCategoriaOs { get; set; }
        public virtual string RegistroGasista { get; set; }
        public virtual string NomeGasista { get; set; }
        public virtual string Viatura { get; set; }
        public virtual string DescricaoMaterial { get; set; }
        public virtual Nota Nota { get; set; }

        #endregion Propriedades

        #region Construtor

        public Mobilidade()
        {

        }

        public Mobilidade(Nota nota)
        {
            Nota = nota;
        }

        #endregion Propriedades
    }
}