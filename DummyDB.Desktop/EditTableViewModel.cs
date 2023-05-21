using Laba5;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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

        });

        public ICommand SaveNameColumn => new CommandDelegate(param =>
        {

        });

        public ICommand SaveAddColumn => new CommandDelegate(param =>
        {

        });

        public ICommand SaveDeleteColumn => new CommandDelegate(param =>
        {

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

    }

}
