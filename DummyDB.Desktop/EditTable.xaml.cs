using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Laba5;

namespace DummyDB.Desktop
{
    public partial class EditTable : Window
    {
        public EditTable(Table table)
        {
            InitializeComponent();
            DataContext = new EditTableViewModel(table);
        }
    }    
}
