using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
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
            string selectedPath = folder.Text;
            string folderPath = selectedPath + "\\" + folderName;
            folder.Text = folderPath;

            if (!string.IsNullOrEmpty(folderName))
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                    System.Windows.Forms.MessageBox.Show("Папка успешно создана!");
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Папка с таким названием уже существует!");
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Введите название папки!");
            }
        }

        private void ChooseSave(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string selectedPath = dialog.SelectedPath;
                folder.Text = selectedPath;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Вы не выбрали папку!");
            }
        }
    }
}
