using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Laba5
{
    public class Table
    {
        public List<Row> Rows { get; set; }
        public TableScheme Scheme { get; set; }

        public string FilePath { get; set; }
        public Table(TableScheme scheme, string filePath)
        {
            Rows = new List<Row>();
            Scheme = scheme;
            FilePath = filePath;
        }

        public string GetCsv()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var row in Rows)
            {
                foreach (var item in row.Data)
                {
                    sb.Append(item.Value);
                    if (item.Key != row.Data.Last().Key)
                    {
                        sb.Append(";");
                    }
                }
                sb.Append("\n");
            }

            return sb.ToString().Trim('\n');
        }

        public void Save()
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(FilePath);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }

            string jsonScheme = JsonSerializer.Serialize<TableScheme>(Scheme);
            File.WriteAllText(FilePath + "//" + Scheme.Name + ".json", jsonScheme);
            File.WriteAllText(FilePath + "//" + Scheme.Name + ".csv", GetCsv());

        }
    }
}
