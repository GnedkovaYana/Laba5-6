using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Laba5.

namespace Laba5
{
    public partial class EditTable : Window
    {
        Table table { get; set; }
        public EditTable(Table table)
        {
            InitializeComponent();
            this.table = table;
            LoadTable();

        }

        private Dictionary<TableScheme, Table> schemesTables = new Dictionary<TableScheme, Table>();
        private void OpenFile(object sender, RoutedEventArgs e)
        {


            //TreeViewItem tableTree = new TreeViewItem();

            //tableTree.Header = table.Scheme.Name;

            //tableTree.Selected += TableTreeSelected;

            //tableTree.Unselected += TableTreeUnselected;

            //dataTree.Items.Add(tableTree);

        }

        //private void TableTreeSelected(object sender, RoutedEventArgs e)
        //{
        //    DataTable.Columns.Clear();
        //    string tableName = ((TreeViewItem)sender).Header.ToString();
        //    DataTable dataTable = new DataTable();


        //    List<RowAdapter> rowsData = new List<RowAdapter>();

        //    foreach (Row row in table.Rows)
        //    {
        //        List<object> rowData = new List<object>();
        //        foreach (object cell in row.Data.Values)
        //        {
        //            rowData.Add(cell);
        //        }
        //        rowsData.Add(new RowAdapter() { Data = rowData });
        //    }

        //    DataTable.ItemsSource = rowsData;

        //    for (int i = 0; i < table.Scheme.Columns.Count; i++)
        //    {
        //        DataGridTextColumn tableTextColumn = new DataGridTextColumn()
        //        {
        //            Header = table.Scheme.Columns[i].Name,
        //            Binding = new System.Windows.Data.Binding($"Data[{i}]")
        //        };

        //        DataTable.Columns.Add(tableTextColumn);
        //    }
        //    Columns.Add(tableTextColumn);
        //}

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
        private class RowAdapter
        {
            public List<Object> Data { get; set; }
        }

        private void SaveChangeTable(object sender, RoutedEventArgs e)
        {

        }


        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SaveChangeTable2(object sender, RoutedEventArgs e)
        {
            string newName = ChangeTable.Text;
            table.Scheme.Name = newName;
            table.Save();
        }
    }

}
