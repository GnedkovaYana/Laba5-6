using Laba5;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace DummyDB.Desktop
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        Table table { get; set; }
        List<Table> tables = new List<Table>();
        string folderPath = "";

        private ObservableCollection<TableScheme> schemes;
        public ObservableCollection<TableScheme> Schemes
        {
            get { return schemes; }
            set { schemes = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Table> Tables { get; set; }

        public TableScheme SelectedScheme { get; set; }

        public DataTable DataTable { get; set; }

        public Table SelectedTable { get; set; }



        public ICommand OpenFile => new CommandDelegate(param =>
        {
            tables = new List<Table>();
            FolderBrowserDialog openFolderDialog = new FolderBrowserDialog();


            if (openFolderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                folderPath = openFolderDialog.SelectedPath;
                string folderName = folderPath.Split('\\')[folderPath.Split('\\').Length - 1];
                UpdateView();
                Schemes = new ObservableCollection<TableScheme>(tables.Select(table => table.Scheme).ToArray());
            }
        });

        private void ReadTables()
        {
            tables.Clear();
            foreach (string filePath in Directory.EnumerateDirectories(folderPath))
            {
                TableScheme tableScheme = null;
                string csvData = null;

                foreach (string file in Directory.EnumerateFiles(filePath))
                {
                    if (file.Contains("json"))
                    {
                        string jsonScheme = File.ReadAllText(file);
                        tableScheme = JsonSerializer.Deserialize<TableScheme>(jsonScheme);
                    }

                    if (file.Contains("csv"))
                    {
                        csvData = file;
                    }
                }

                Table table = ReadTable.TableRead(tableScheme, filePath);
                tables.Add(table);
            }
        }

        public ICommand CreateFile => new CommandDelegate(param =>
        {
            CreateDataBase createDataBase = new CreateDataBase();
            createDataBase.Show();
        });


        public ICommand CreateTable => new CommandDelegate(param =>
        {
            if (folderPath == "")
            {
                System.Windows.MessageBox.Show("Вы не выбрали базу данных");
            }
            else
            {
                CreateTable createTable = new CreateTable(folderPath);
                createTable.Closed += windowClosed;
                createTable.Show();
            }
        });

        public ICommand EditTable => new CommandDelegate(param =>
        {
            if (table == null)
            {
                System.Windows.MessageBox.Show("Вы не выбрали таблицу");
            }
            else
            {
                EditTable editTable = new EditTable(table);
                editTable.Closed += windowClosed;
                editTable.Show();
            }
        });

        public ICommand SelectTable => new CommandDelegate(param =>
        {
            Table table = SelectedTable;
            LoadTable();
        
        });

        private void windowClosed(object sender, EventArgs e)
        {
            UpdateView();
        }

        public void UpdateView()
        {
            ReadTables();
            LoadTreeView();
            LoadTable();
            UpdateTables();
        }

        private void LoadTable()
        {
            if (DataTable != null)
            {
                DataTable.Clear();
            }

            if (table != null)
            {
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

        private void LoadTreeView()
        {
            if (Schemes == null)
            {
                Schemes = new ObservableCollection<TableScheme> { };
            }
            Schemes.Clear();
            foreach (var table in tables)
            {
                Schemes.Add(table.Scheme);
            }
        }

        public void UpdateTables()
        {
            if (Tables == null)
            {
                Tables = new ObservableCollection<Table> { };
            }
            Tables.Clear();
            foreach (var table in tables)
            {
                Tables.Add(table);
            }
        }
    }
}