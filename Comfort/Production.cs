using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comfort
{
    [Table("Production")]
    public class Production
    {

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string? Article { get; set; } = null;
        public string? Type { get; set; } = null;
        public string? Name { get; set; } = null;
        public double MinValue { get; set; }
        public string? Material { get; set; } = null;

        public Production()
        {
            
        }
        public Production(string type, string name, string article, double minValue, string material)
        {
            Type = type; Name = name; Article = article; MinValue = minValue; Material = material;
        }
    }
}
