using System.Collections.Generic;

namespace NetUtil.Util.DTO
{
    public class DataTableData
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<object> data { get; set; }

        public DataTableData()
        {
        }

        public DataTableData(List<object> dados, int totalItens)
        {
            this.recordsTotal = totalItens;
            this.recordsFiltered = totalItens;
            this.data = dados;
        }

        public DataTableData (int draw, List<object> dados, int totalItens)
        {
            this.draw = draw;
            this.recordsTotal = totalItens;
            this.recordsFiltered = totalItens;
            this.data = dados;
        }
    }
}