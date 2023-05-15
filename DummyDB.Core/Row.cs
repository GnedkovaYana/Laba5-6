using System.Collections.Generic;

namespace Laba5
{
    public class Row
    {
        public Dictionary<Column, object> Data { get; set; }
        public Row()
        {
            Data = new Dictionary<Column, object>();
        }
    }
}
