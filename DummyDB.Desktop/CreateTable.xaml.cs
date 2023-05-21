using Laba5;
using System.Collections.Generic;
using System.Windows;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace DummyDB.Desktop
{
    public partial class CreateTable : Window
    {
        public CreateTable(string folderPath)
        {
            InitializeComponent();
            DataContext = new CreateTableViewModel(folderPath);

        }
    }
}
