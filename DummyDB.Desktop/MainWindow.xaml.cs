using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using Laba5;

namespace DummyDB.Desktop
{

    public partial class MainWindow : Window
    {
        Table table { get; set; }
        public MainWindow()
        {
            InitializeComponent();
        }
        List<Table> tables = new List<Table>();

        private void OpenFile(object sender, RoutedEventArgs e)
        {
            tables = new List<Table>();
            dataTree.Items.Clear();
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Title = "Выберите БД";
            dialog.Filter = "Файл базы данных(*.db)|*.db";

            dialog.ShowDialog();

            string folderPath = File.ReadAllText(dialog.FileName);

            string folderName = folderPath.Split('\\')[folderPath.Split('\\').Length - 1];

            dataTree.Header = folderName;

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


            foreach (Table table in tables)
            {
                TreeViewItem treeItem = new TreeViewItem();
                treeItem.Selected += TableTreeSelected;
                treeItem.Unselected += TableTreeUnselected;
                treeItem.Header = table.Scheme.Name;

                foreach (Column key in table.Scheme.Columns)
                {
                    treeItem.Items.Add(key.Name + " - " + key.Type);
                }
                ((MainWindow)System.Windows.Application.Current.MainWindow).dataTree.Items.Add(treeItem);
            }
        }

        private void TableTreeSelected(object sender, RoutedEventArgs e)
        {
            DataTable.Columns.Clear();
            string tableName = ((TreeViewItem)sender).Header.ToString();
            table = GetTableByName(tableName);
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

            DataTable.ItemsSource = rowsData;

            for (int i = 0; i < table.Scheme.Columns.Count; i++)
            {
                DataGridTextColumn tableTextColumn = new DataGridTextColumn()
                {
                    Header = table.Scheme.Columns[i].Name,
                    Binding = new System.Windows.Data.Binding($"Data[{i}]")
                };

                DataTable.Columns.Add(tableTextColumn);
            }
        }

        public Table GetTableByName(string name)
        {
            foreach (Table table in tables)
            {
                if (table.Scheme.Name == name)
                {
                    return table;
                }
            }
            return null;
        }


        private class RowAdapter
        {
            public List<Object> Data { get; set; }
        }
        private void TableTreeUnselected(object sender, RoutedEventArgs e)
        {
            DataTable.Columns.Clear();
        }

        private void CreateFile(object sender, RoutedEventArgs e)
        {
            CreateDataBase createDataBase = new CreateDataBase();
            createDataBase.Show();
        }

        private void CreateTable(object sender, RoutedEventArgs e)
        {
            CreateTable createTable = new CreateTable();
            createTable.Show();
        }

        private void EditTable(object sender, RoutedEventArgs e)
        {
            if (table == null)
            {
                MessageBox.Show("Вы хуйло дважды");
            }
            else
            {
                EditTable editTable = new EditTable(table);
                editTable.Show();
            }          
        }
    }
}
