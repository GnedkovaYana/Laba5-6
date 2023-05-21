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

            var options = new JsonSerializerOptions { WriteIndented = true }; 
            string jsonScheme = JsonSerializer.Serialize<TableScheme>(Scheme, options);
            File.WriteAllText(FilePath + "//" + Scheme.Name + ".json", jsonScheme);
            File.WriteAllText(FilePath + "//" + Scheme.Name + ".csv", GetCsv());

        }

        public void UptadeTableName(string tableName)
        {
            Scheme.Name = tableName;
            Save();
        }

        public void UptadeColumnName(string  newColumnName,  string oldColumnName) 
        {
            Column CurretColumn = null;
            CurretColumn = GetColumnByName(oldColumnName);
            CurretColumn.Name = newColumnName;
            Save();
        }

        public void AddColumn(string columnName, string typeColumn, bool primaryColumn)
        {
            Column column = new Column { Name = columnName, Type = typeColumn, IsPrimary = primaryColumn };
            Scheme.Columns.Add(column);
            foreach (Row row in Rows)
            {
                if (column.Type == "uint")
                {
                    row.Data.Add(column, 0);
                }
                else if (column.Type == "string")
                {
                    row.Data.Add(column, "");
                }
                else if (column.Type == "double")
                {
                    row.Data.Add(column, 0);
                }
                else if (column.Type == "dateTime")
                {
                    row.Data.Add(column, DateTime.MinValue);
                }
            }
            Save();
        }

        public void RemoveColumn(string columnName) 
        {
            Column column = GetColumnByName(columnName);
            if (column.IsPrimary)
            {
                throw new ArgumentException("Вы пытаетесь удалить primary столбец");
            }
            if (column != null)
            {
                Scheme.Columns.Remove(column);
                foreach (Row row in Rows)
                {
                    row.Data.Remove(column);
                }
            }
            Save();
        }

        public void AddRow()
        {
            Row row = new Row();
            foreach (Column column in Scheme.Columns)
            {
                if (column.Type == "uint")
                {
                    row.Data.Add(column, 0);
                }
                else if (column.Type == "string")
                {
                    row.Data.Add(column, "");
                }
                else if (column.Type == "double")
                {
                    row.Data.Add(column, 0);
                }
                else if (column.Type == "dateTime")
                {
                    row.Data.Add(column, DateTime.MinValue);
                }
            }
            Rows.Add(row);
            Save();
        }

        public void DeleteRow(string deleteRow)
        {
            Row row = GetRowByName(deleteRow);
            Rows.Remove(row);
            Save();
        }

        
        private Column GetColumnByName(string oldName)
        {
            foreach (Column column in Scheme.Columns)
            {
                if (column.Name == oldName)
                {
                    return column;
                }
            }

            return null;
        }

        private Row GetRowByName(string deleteRow)
        {
            foreach (Row row in Rows)
            {
                if (row.Data[GetPrimaryColumn()].ToString() == deleteRow)
                {
                    return row;
                }

            }
            return null;
        }

        public Column GetPrimaryColumn()
        {
            foreach (Column column in Scheme.Columns)
            {
                if (column.IsPrimary)
                {
                    return column;
                }
            }
            return null;
        }
    }
}
