using System.Collections.Generic;

namespace Laba5
{
    public class Table
    {
        public List<Row> Rows { get; set; }

        public Table()
        {
            Rows = new List<Row>();
        }
    }
}
