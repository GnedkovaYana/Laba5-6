using Laba5;
using System.Collections.Generic;
using System.Windows;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace DummyDB.Desktop
{
    public partial class CreateTable : Window
    {
        TableScheme scheme;
        public CreateTable()
        {
            InitializeComponent();
            scheme = new TableScheme();
            scheme.Columns = new List<Column>();
        }

        private void SaveTable(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(nameTable.Text) && !string.IsNullOrEmpty(nameColumn.Text) && !string.IsNullOrEmpty(nameType.Text))
            {
                scheme.Name = nameTable.Text;
                string jsonScheme = JsonSerializer.Serialize<TableScheme>(scheme);

                string folderPath = folderPathTextBox.Text;
                Table table = new Table(scheme, folderPath);
                table.Save();

                MessageBox.Show("Файл создан!");

            }
        }

        private void AddColumn(object sender, RoutedEventArgs e)
        {
            scheme.Columns.Add(new Column { Name = nameColumn.Text, Type = nameType.Text });
        }

    }
}
