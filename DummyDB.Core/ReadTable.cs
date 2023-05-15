using System;
using System.IO;

namespace Laba5
{
    public class ReadTable
    {
        public static Table TableRead(TableScheme tableScheme, string path)
        {
            string[] file = File.ReadAllLines(path + "//" + tableScheme.Name + ".csv");
            Table table = new Table(tableScheme, path);
            for (int i = 0; i < file.Length; i++)
            {
                string[] el = file[i].Split(';');
                if (el.Length != tableScheme.Columns.Count)
                {
                    throw new ArgumentException();
                }
                Row row = RowRead(tableScheme, path, i, el);
                table.Rows.Add(row);
            }
            return table;
        }

        public static Row RowRead(TableScheme tableScheme, string path, int i, string[] el)
        {
            Row row = new Row();

            for (int j = 0; j < el.Length; j++)
            {
                switch (tableScheme.Columns[j].Type)
                {
                    case ("uint"):
                        {
                            if (uint.TryParse(el[j], out uint number))
                                row.Data.Add(tableScheme.Columns[j], number);
                            else
                                throw new ArgumentException();
                        }
                        break;

                    case ("double"):
                        {
                            if (double.TryParse(el[j], out double doubleNumber))
                                row.Data.Add(tableScheme.Columns[j], doubleNumber);
                            else
                                throw new ArgumentException();
                        }
                        break;

                    case ("datetime"):
                        {
                            if (DateTime.TryParse(el[j], out DateTime data))
                                row.Data.Add(tableScheme.Columns[j], data.ToShortDateString());
                            else
                                throw new ArgumentException();
                        }
                        break;

                    default:
                        row.Data.Add(tableScheme.Columns[j], el[j]);
                        break;
                }
            }
            return row;
        }
    }
}
