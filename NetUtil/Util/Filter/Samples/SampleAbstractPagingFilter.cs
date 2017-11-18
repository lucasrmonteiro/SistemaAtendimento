using NetUtil.Util.Enums;
using NetUtil.Util.Filter.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetUtil.Util.Filter
{
    //alias para entidade consultada
    [Alias("A")]
    //join com propriedade 'Propriedade1' da entidade consultada
    [Join("A.Propriedade1", "B", JoinType.LeftOuterJoin)]
    //join com propriedade 'Propriedade2' da entidade consultada
    [Join("A.Propriedade2", "C", JoinType.InnerJoin)]
    public class SampleAbstractPagingFilter : AbstractPagingFilter
    {
        public SampleAbstractPagingFilter(int pageSize, int currentPage)
        {
            base.PageSize = pageSize;
            base.CurrentPage = currentPage;
        }

        [Restriction("C.Create", Order.Asc, 0)]
        public object Order_Only { get; set; }

        [Restriction("A.Id", Restriction.Between, true)]
        public int? Id_start { get; set; }

        [Restriction("A.Id", Restriction.Between, Order.Asc, 1, false)]
        public int? Id_end { get; set; }

        [Restriction("B.Date", Restriction.Between)]
        public DateTime? Date_start { get; set; }

        [Restriction("B.Date", Restriction.Between)]
        public DateTime? Date_end { get; set; }

        [Restriction("C.Nome", Restriction.Like)]
        public string nome { get; set; }

        [Restriction("A.Valor", Restriction.Ge)]
        public int? Valor { get; set; }

        [Restriction("A.Valor", Restriction.NotIn)]
        public List<int> Not_in { get; set; }

        [Restriction("A.Ativo")]
        public bool? Ativo { get; set; }
    }
}
