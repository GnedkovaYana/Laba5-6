using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace DummyDB.Desktop
{
    internal class CreateDataBaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private string nameDB;
        public string NameDB
        {
            get { return nameDB; }
            set
            {
                nameDB = value;
                OnPropertyChanged();
            }
        }

        private string folder;
        public string Folder
        {
            get { return folder; }
            set
            {
                folder = value;
                OnPropertyChanged();
            }
        }

        public ICommand ChooseSave => new CommandDelegate(param =>
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string selectedPath = dialog.SelectedPath;
                Folder = selectedPath;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Вы не выбрали папку!");
            }
        });

        public ICommand OK => new CommandDelegate(param =>
        {
            string folderPath = Folder + "\\" + NameDB;
            Folder = folderPath;

            if (!string.IsNullOrEmpty(NameDB))
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

        });


    }

}
