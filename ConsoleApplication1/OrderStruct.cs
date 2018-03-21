using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
   [Serializable]
    public class OrderStruct
    {

        public int Module { get; set; }
        public string SupplierName { get; set; }
        public int Supplierid { get; set; }
        public string NickName { get; set; }
        public string Ordernumber { get; set; }
        public int Orderstatus { get; set; }
        public string[] Price { get; set; }
        public string Totalprice { get; set; }
        public int Userid { get; set; }
        public int[] Number { get; set; }
        public string[] Pname { get; set; }
        public int[] CommodityId { get; set; }
        public string[] Remark { get; set; }
        public int SourceId { get; set; }
       
    }
}
