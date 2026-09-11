using OfficeOpenXml.Export.HtmlExport.StyleCollectors.StyleContracts;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.DataAccess.Entities
{
    [SugarTable(IsDisabledUpdateAll = true)]
    public class Order
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        public string Name { get; set; }
        public decimal Price { get; set; }
        [SugarColumn(DefaultValue = "(strftime('%Y-%m-%d %H:%M:%S', 'now', 'localtime'))")]
        public DateTime CreateTime { get; set; }
        [SugarColumn(IsNullable = true)]
        public int CustomId { get; set; }
    }
}
