using System.Collections.Generic;
using System.Xml.Linq;

namespace Laba5
{
    public class Row
    {
        public Dictionary<Column, object> Data { get; set; }
        public Row()
        {
            Data = new Dictionary<Column, object>();
        }

        public override string ToString()
        {
            foreach (var item in Data)
            {
                if (item.Key.IsPrimary)
                {
                    return item.Value.ToString();
                }
            }
            return null;
        }
    }
}
