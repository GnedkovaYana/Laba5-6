using Laba5;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DummyDB.Desktop
{
    internal class EditTableViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private string changeTable;
        public string ChangeTable
        {
            get { return changeTable; }
            set
            {
                changeTable = value;
                OnPropertyChanged();
            }
        }

        private string changeColumn;
        public string ChangeColumn
        {
            get { return changeColumn; }
            set
            {
                changeColumn = value;
                OnPropertyChanged();
            }
        }

        private string addColumn;
        public string AddColumn
        {
            get { return addColumn; }
            set
            {
                addColumn = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Column> Columns { get; set; }
        public Column SelectedColumnToChange { get; set; }
        public List<string> ColumnsTypes { get; set; }
        public string ColumnType { get; set; }
        public Column SelectedColumnToDelete { get; set; }
        public ObservableCollection<Row> Rows { get; set; }
        public Row SelectedRowToDelete { get; set; }

        Table table;

        private DataTable dataTable = new DataTable();
        public DataTable DataTable
        {
            get { return dataTable; }
            set { dataTable = value; OnPropertyChanged(); }
        }

        public EditTableViewModel(Table table) 
        {
            this.table = table;
            Columns = new ObservableCollection<Column>(table.Scheme.Columns);
            ColumnsTypes = new List<string>
            {
                "uint",
                "string",
                "dataTime",
                "double"
            };
            Rows = new ObservableCollection<Row>(table.Rows);
            LoadTable();
        }

        public ICommand SaveChangeNameTable => new CommandDelegate(param =>
        {
            if (string.IsNullOrEmpty(ChangeTable))
            {
                MessageBox.Show("Вы не ввели назание таблицы");
            }
            else
            {
                table.UpdateTableName(ChangeTable);
                MessageBox.Show("Таблица переименована");
            }
        });

        public ICommand SaveNameColumn => new CommandDelegate(param =>
        {
            if (string.IsNullOrEmpty(ChangeColumn) || SelectedColumnToChange == null)
            {
                MessageBox.Show("Вы не ввели новое название столбца или не выбрали столбец");
            }
            else
            {
                try
                {
                    table.UpdateColumnName(ChangeColumn, SelectedColumnToChange.Name);
                    MessageBox.Show("Столбец переименован");
                    UpdateView();
                }
                catch (Exception ex) 
                {
                    MessageBox.Show(ex.Message);
                }
            }
        });

        public ICommand SaveAddColumn => new CommandDelegate(param =>
        {
            if (string.IsNullOrEmpty(ColumnType) || string.IsNullOrEmpty(AddColumn))
            {
                MessageBox.Show("Вы не выбрали тип столбца или ввели его назание");
            }
            else
            {
                try
                {
                    table.AddColumn(AddColumn, ColumnType, false);
                    MessageBox.Show("Столбец добален");
                    UpdateView();
                }
                catch (Exception ex) 
                {
                    MessageBox.Show(ex.Message);
                }
            }
        });

        public ICommand SaveDeleteColumn => new CommandDelegate(param =>
        {
            if (SelectedColumnToDelete == null) 
            {
                MessageBox.Show("Вы не выбрали столбец");
            }
            else
            {
                try
                {
                    table.RemoveColumn(SelectedColumnToDelete.Name);
                    MessageBox.Show("Столбец удален");
                    UpdateView();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        });

        public ICommand SaveAddRow => new CommandDelegate(param =>
        {
            table.AddRow();
            MessageBox.Show("Строка добалена");
            UpdateView();
        });

        public ICommand SaveDeleteRow => new CommandDelegate(param =>
        {
            if (SelectedRowToDelete == null)
            {
                MessageBox.Show("Вы не выбрали строку");
            }
            else
            {
                table.DeleteRow(SelectedRowToDelete.ToString());
                MessageBox.Show("Строка удалена");
                UpdateView();
            }
        });

        public ICommand SaveChangeTable => new CommandDelegate(param =>
        {
            for (int i = 0; i < DataTable.Rows.Count; i++)
            {
                for (int j = 0; j < DataTable.Columns.Count; j++)
                {
                    ValidateItem(i, j);
                }
            }
            table.Save();
            MessageBox.Show("Строка отредактирована");
        });

        private void ValidateItem(int i, int j)
        {
            switch (table.Scheme.Columns[j].Type)
            {
                case ("uint"):
                    {
                        if (uint.TryParse(DataTable.Rows[i][DataTable.Columns[j]].ToString(), out uint number))
                        {
                            table.Rows[i].Data[table.Scheme.Columns[j]] = number;
                        }
                        else
                        {
                            MessageBox.Show($"Ошибка: в строке {i + 1} в столбце {table.Scheme.Columns[j].Name} неверный тип данных");
                        }
                    }
                    break;

                case ("double"):
                    {
                        if (double.TryParse(DataTable.Rows[i][DataTable.Columns[j]].ToString(), out double doubleNumber))
                        {
                            table.Rows[i].Data[table.Scheme.Columns[j]] = doubleNumber;
                        }
                        else
                        {
                            MessageBox.Show($"Ошибка: в строке {i + 1} в столбце {table.Scheme.Columns[j].Name} неверный тип данных");
                        }
                    }
                    break;

                case ("datatime"):
                    {
                        if (DateTime.TryParse(DataTable.Rows[i][DataTable.Columns[j]].ToString(), out DateTime datetimeNamber))
                        {
                            table.Rows[i].Data[table.Scheme.Columns[j]] = datetimeNamber;
                        }
                        else
                        {
                            MessageBox.Show($"Ошибка: в строке {i + 1} в столбце {table.Scheme.Columns[j].Name} неверный тип данных");
                        }
                    }
                    break;

                default:
                    table.Rows[i].Data[table.Scheme.Columns[j]] = DataTable.Rows[i][DataTable.Columns[j]].ToString();
                    break;
            }
        }

        public void LoadTable()
        {
            DataTable dataTable = new DataTable();
            dataTable.TableName = table.Scheme.Name;
            AddColumns(dataTable);
            AddRows(dataTable);
            DataTable = dataTable;
        }

        private void AddColumns(DataTable dataTable)
        {
            foreach (var column in table.Scheme.Columns)
            {
                dataTable.Columns.Add(column.Name);
            }
        }

        private void AddRows(DataTable dataTable)
        {
            foreach (var row in table.Rows)
            {
                DataRow dataRow = dataTable.NewRow();
                foreach (var item in row.Data)
                {
                    dataRow[item.Key.Name] = item.Value.ToString();
                }
                dataTable.Rows.Add(dataRow);
            }
        }

        public void UpdateView()
        {
            UpdateColumns();
            UpdateRows();
            LoadTable();
        }

        private void UpdateRows()
        {
            Rows.Clear();
            foreach (Row row in table.Rows)
            {
                Rows.Add(row);
            }
        }
        private void UpdateColumns()
        {
            Columns.Clear();
            foreach (Column column in table.Scheme.Columns)
            {
                Columns.Add(column);
            }
        }
    }
}