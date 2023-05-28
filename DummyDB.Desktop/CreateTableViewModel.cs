using Laba5;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace DummyDB.Desktop
{
    internal class CreateTableViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        TableScheme scheme;
        bool IsAddPrimary = false;
        string folderPath;

        public CreateTableViewModel(string folderPath)
        {
            scheme = new TableScheme();
            scheme.Columns = new List<Column>();
            this.folderPath = folderPath;
            NameType = new List<string>
            {
                "uint",
                "string",
                "dataTime",
                "double"
            };
        }

        private string nameColumn;
        public string NameColumn
        {
            get { return nameColumn; }
            set
            {
                nameColumn = value;
                OnPropertyChanged();
            }
        }

        private string nameTable;
        public string NameTable
        {
            get { return nameTable; }
            set
            {
                nameTable = value;
                OnPropertyChanged();
            }
        }

        private List<string> nameType;
        public List<string> NameType
        {
            get { return nameType; }
            set
            {
                nameType = value;
                OnPropertyChanged();
            }
        }

        private bool primary;
        public bool Primary
        {
            get { return primary; }
            set
            {
                primary = value;
                OnPropertyChanged();
            }
        }

        public string SelectedType { get; set; }

        public ICommand SaveTable => new CommandDelegate(param =>
        {
            if (!(IsAddPrimary))
            {
                MessageBox.Show("Вы не добавили Primary");
            }
            else if (string.IsNullOrEmpty(NameTable))
            {
                MessageBox.Show("Вы не ввели название таблицы!");
            }
            else
            {
                Directory.CreateDirectory(folderPath + "\\" + NameTable);
                scheme.Name = NameTable;
                Table table = new Table(scheme, folderPath + "\\" + NameTable);
                table.Save();
                MessageBox.Show("Файл создан!");
            }
        });

        public ICommand AddColumn => new CommandDelegate(param =>
        {
            if (string.IsNullOrEmpty(NameColumn) && string.IsNullOrEmpty(SelectedType))
            {
                MessageBox.Show("Вы не выбрали имя столбца или его тип");
                return;
            }

            if (IsAddPrimary && Primary)
            {
                MessageBox.Show("Вы уже создали Primary столбец");
            }
            else
            {
                scheme.Columns.Add(new Column { Name = NameColumn, Type = SelectedType, IsPrimary = (bool)Primary });
                IsAddPrimary = Primary || IsAddPrimary;
                NameColumn = "";
                Primary = false;
            }
        });
    }
}
