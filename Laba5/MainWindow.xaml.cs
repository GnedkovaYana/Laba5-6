using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Laba5
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private Dictionary<TableScheme, Table> schemesTables = new Dictionary<TableScheme, Table>();

        private void OpenFile(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            string folderPath = "";
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                folderPath = folderBrowserDialog.SelectedPath;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Вы не выбрали папку");
            }

            string folderName = folderPath.Split('\\')[folderPath.Split('\\').Length - 1];

            dataTree.Header = folderName;

            foreach (string filePath in Directory.EnumerateFiles(folderPath))
            {
                if (filePath.Substring(filePath.Length - 4, 4) == "json")
                {
                    TableScheme schemeOfTable = TableScheme.ReadFile(filePath);

                    string tableName = schemeOfTable.Name;
                    string pathTable = filePath.Substring(0, filePath.Length - 12) + ".csv";

                    Table table = ReadTable.TableRead(schemeOfTable, pathTable);

                    schemesTables.Add(schemeOfTable, table);

                    TreeViewItem tableTree = new TreeViewItem();

                    tableTree.Header = tableName;

                    tableTree.Selected += TableTreeSelected;

                    tableTree.Unselected += TableTreeUnselected;

                    foreach (Column key in schemeOfTable.Columns)
                    {
                        tableTree.Items.Add(key.Name + " — " + key.Type);
                    }

                    dataTree.Items.Add(tableTree);
                }
            }
        }
        private void TableTreeSelected(object sender, RoutedEventArgs e)
        {
            DataTable.Columns.Clear();
            string tableName = ((TreeViewItem)sender).Header.ToString();

            foreach (var schemeAndTable in schemesTables)
            {
                if (schemeAndTable.Key.Name == tableName)
                {
                    List<RowAdapter> rowsData = new List<RowAdapter>();

                    foreach (Row row in schemeAndTable.Value.Rows)
                    {
                        List<object> rowData = new List<object>();
                        foreach (object cell in row.Data.Values)
                        {
                            rowData.Add(cell);
                        }
                        rowsData.Add(new RowAdapter() { Data = rowData });
                    }

                    DataTable.ItemsSource = rowsData;

                    for (int i = 0; i < schemeAndTable.Key.Columns.Count; i++)
                    {
                        DataGridTextColumn tableTextColumn = new DataGridTextColumn()
                        {
                            Header = schemeAndTable.Key.Columns[i].Name,
                            Binding = new System.Windows.Data.Binding($"Data[{i}]")
                        };

                        DataTable.Columns.Add(tableTextColumn);
                    }
                    break;
                }
            }
        }
        private class RowAdapter
        {
            public List<Object> Data { get; set; }
        }
        private void TableTreeUnselected(object sender, RoutedEventArgs e)
        {
            DataTable.Columns.Clear();
        }
    }
}
