using System.IO;
using System.Windows;
using Laba5;


namespace DummyDB.Desktop
{
    public partial class CreateDataBase : Window
    {
        public CreateDataBase()
        {
            InitializeComponent();
        }

        private void OK(object sender, RoutedEventArgs e)
        {
            string folderName = nameDB.Text;
            string selectedPath = SpasiISoxrani.Text;
            string folderPath = selectedPath + "\\" + folderName;
            folder.Text = folderPath;

            if (!string.IsNullOrEmpty(folderName))
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                    File.WriteAllText(folderPath, folderName + ".db");
                    MessageBox.Show("Папка успешно создана!");
                }
                else
                {
                    MessageBox.Show("Папка с таким названием уже существует!");
                }
            }
            else
            {
                MessageBox.Show("Введите название папки!");
            }
        }
    }
}
