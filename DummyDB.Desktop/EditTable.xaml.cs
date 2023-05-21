using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Laba5;

namespace DummyDB.Desktop
{
    public partial class EditTable : Window
    {
        Table table { get; set; }
        public EditTable(Table table)
        {
            InitializeComponent();
            DataContext = new EditTableViewModel(table);
            this.table = table;
            //LoadTable();
           //LoadComboBox();
        }
        private Dictionary<TableScheme, Table> schemesTables = new Dictionary<TableScheme, Table>();
        Column primaryColumn = null;
        public void LoadTable()
        {
            DataTable2.Columns.Clear();
            DataTable dataTable = new DataTable();

            List<RowAdapter> rowsData = new List<RowAdapter>();

            foreach (Row row in table.Rows)
            {
                List<object> rowData = new List<object>();
                foreach (object cell in row.Data.Values)
                {
                    rowData.Add(cell);
                }
                rowsData.Add(new RowAdapter() { Data = rowData });
            }

            DataTable2.ItemsSource = rowsData;

            for (int i = 0; i < table.Scheme.Columns.Count; i++)
            {
                DataGridTextColumn tableTextColumn = new DataGridTextColumn()
                {
                    Header = table.Scheme.Columns[i].Name,
                    Binding = new System.Windows.Data.Binding($"Data[{i}]")
                };

                DataTable2.Columns.Add(tableTextColumn);
            }
        }

        public void LoadComboBox()
        {
            foreach (Column column in table.Scheme.Columns)
            {
                OldNameColumn.Items.Add(column.Name);
                DeleteColumn.Items.Add(column.Name);
                if (column.IsPrimary)
                {
                    primaryColumn = column;
                }
            }
            foreach (Row row in table.Rows)
            {
                DeleteRow.Items.Add(row.Data[primaryColumn]);
            }
        }

        private class RowAdapter
        {
            public List<Object> Data { get; set; }
        }

        private void SaveChangeTable(object sender, RoutedEventArgs e)
        {
            List<RowAdapter> rowAdapters = (List<RowAdapter>)DataTable2.ItemsSource;
            
            for (int i = 0; i < rowAdapters.Count; i++)
            {
                for (int j = 0; j < rowAdapters[i].Data.Count; j++)
                {
                    if (table.Scheme.Columns[j].Type == "uint")
                    {
                        if (uint.TryParse(rowAdapters[i].Data[j].ToString(), out uint number))
                        {
                            table.Rows[i].Data[table.Scheme.Columns[j]] = number;
                        }
                        else
                        {
                            MessageBox.Show($"Ошибка: в строке {i+1} в столбце {table.Scheme.Columns[j].Name} неверный тип данных");
                        }

                    }
                    else if (table.Scheme.Columns[j].Type == "double")
                    {
                        if (double.TryParse(rowAdapters[i].Data[j].ToString(), out double doubleNumber))
                        {
                            table.Rows[i].Data[table.Scheme.Columns[j]] = doubleNumber;
                        }
                        else
                        {
                            MessageBox.Show($"Ошибка: в строке {i+1} в столбце {table.Scheme.Columns[j].Name} неверный тип данных");
                        }
                    }
                    else if (table.Scheme.Columns[j].Type == "datatime")
                    {
                        if (DateTime.TryParse(rowAdapters[i].Data[j].ToString(), out DateTime datetimeNamber))
                        {
                            table.Rows[i].Data[table.Scheme.Columns[j]] = datetimeNamber;
                        }
                        else
                        {
                            MessageBox.Show($"Ошибка: в строке {i+1} в столбце {table.Scheme.Columns[j].Name} неверный тип данных");
                        }
                    }
                    else
                    {
                        table.Rows[i].Data[table.Scheme.Columns[j]] = rowAdapters[i].Data[j].ToString();                    
                    }
                }
            }
            table.Save();
        }

        private void SaveChangeNameTable(object sender, RoutedEventArgs e)
        {
            string newName = ChangeTable.Text;
            table.Scheme.Name = newName;
            table.Save();
            MessageBox.Show("Таблица переименована");
        }

        private void SaveNameColumn(object sender, RoutedEventArgs e)
        {
            string oldName = OldNameColumn.Text;
            Column CurretColumn = null;
            CurretColumn = GetColumnByName(oldName);

            if (CurretColumn != null)
            {
                CurretColumn.Name = ChangeColumn.Text;
            }
            else
            {
                MessageBox.Show("Вы не выбрали название столбца");
            }

            table.Save();
            MessageBox.Show("Столбец переименован");
        }

        private Column GetColumnByName(string oldName)
        {
            foreach (Column column in table.Scheme.Columns)
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
            foreach (Row row in table.Rows) 
            {
                if (row.Data[primaryColumn].ToString() == deleteRow)
                {
                    return row;
                }
                
            }
            return null;
        }


        private void SaveAddColumn(object sender, RoutedEventArgs e)
        {
            if (AddColumn.Text == "" || TypeColumn.Text == "")
            {
                MessageBox.Show("Вы не указали навание столбца или его тип");
            }
            else if (IsColumnExist(AddColumn.Text))
            {
                MessageBox.Show("вы хуйло четырежды");
            }

            else
            {
                Column column = new Column { Name = AddColumn.Text, Type = TypeColumn.Text };
                table.Scheme.Columns.Add(column);
                foreach (Row row in table.Rows) 
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
                table.Save();
                MessageBox.Show("Новый столбец добален");
            }
        }

        private void SaveDeleteColumn(object sender, RoutedEventArgs e)
        {
            string deleteColumn = DeleteColumn.Text;
            Column column = GetColumnByName(deleteColumn);
            if (column.IsPrimary)
            {
                MessageBox.Show("Вы не можете ни че го!");
            }
            if (column != null)
            {
                table.Scheme.Columns.Remove(column);
                foreach (Row row in table.Rows)
                {
                    row.Data.Remove(column);
                }
            }
            else
            {
                MessageBox.Show("Вы не выбрали название столбца");
            }

            table.Save();
            MessageBox.Show("Столбец удален");
        }

        public bool IsColumnExist(string columnName)
        {
            foreach (Column column in table.Scheme.Columns)
            { 
                if (column.Name == columnName) 
                {
                    return true;
                } 
            }
            return false;
        }

        private void SaveAddRow(object sender, RoutedEventArgs e)
        {
            Row row = new Row();
            foreach (Column column in table.Scheme.Columns)
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
            table.Rows.Add(row);
            table.Save();
            MessageBox.Show("Строка добавлена");
        }

        private void SaveDeleteRow(object sender, RoutedEventArgs e)
        {
            string deleteRow = DeleteRow.Text;
            Row row = GetRowByName(deleteRow);
            table.Rows.Remove(row);
            table.Save();
            MessageBox.Show("Строка удалена");
        }
    }
}
