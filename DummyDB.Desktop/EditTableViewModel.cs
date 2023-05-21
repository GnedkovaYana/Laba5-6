using Laba5;
using System;
using System.Collections.Generic;
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

        public List<Column> Columns { get; set; }
        public Column SelectedColumnToChange { get; set; }
        public List<string> ColumnsTypes { get; set; }
        public string ColumnType { get; set; }
        public Column SelectedColumnToDelete { get; set; }
        public List<Row> Rows { get; set; }
        public Row SelectedRowToDelete { get; set; }

        Table table;

        private DataTable dataTable;
        public DataTable DataTable
        {
            get { return dataTable; }
            set { dataTable = value; OnPropertyChanged(); }
        }

        public EditTableViewModel(Table table) 
        {
            this.table = table;
            Columns = table.Scheme.Columns;
            ColumnsTypes = new List<string>
            {
                "uint",
                "string",
                "dataTime",
                "double"
            };
            Rows = table.Rows;
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
                table.UptadeTableName(ChangeTable);
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
                table.UptadeColumnName(ChangeColumn, SelectedColumnToChange.Name);
                MessageBox.Show("Столбец переименован");
                UpdateView();
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
                table.AddColumn(AddColumn, ColumnType, false );
                MessageBox.Show("Столбец добален");
                UpdateView();
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
                table.RemoveColumn(SelectedColumnToDelete.Name);
                MessageBox.Show("Столбец удален");
                UpdateView();
            }
        });

        public ICommand SaveAddRow => new CommandDelegate(param =>
        {

        });

        public ICommand SaveDeleteRow => new CommandDelegate(param =>
        {

        });

        public ICommand SaveChangeTable => new CommandDelegate(param =>
        {

        });


        public void LoadTable()
        {
            if (DataTable != null)
            {
                DataTable.Clear();
            }

            DataTable dataTable = new DataTable();
            dataTable.TableName = table.Scheme.Name;
            foreach (var column in table.Scheme.Columns)
            {
                dataTable.Columns.Add(column.Name);
            }
            foreach (var row in table.Rows)
            {
                DataRow dataRow = dataTable.NewRow();
                foreach (var item in row.Data)
                {
                    dataRow[item.Key.Name] = item.Value.ToString();
                }
                dataTable.Rows.Add(dataRow);
            }
            DataTable = dataTable;
        }

        public void UpdateView()
        {
            UpdateColumns();

            List<Row> newRows = new List<Row>();
            foreach (Row row in table.Rows)
            {
                newRows.Add(row);
            }
            Rows = newRows;
            LoadTable();
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
