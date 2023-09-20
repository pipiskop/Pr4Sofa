using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyBudget
{
    public partial class AddWindows : Window
    {
        public AddWindows()
        {
            InitializeComponent();
        }
        private void Typebtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(typeTitle.Text))
            {
                MessageBox.Show("Поле не может быть пустым");
            }
            else
            {
                DialogResult = true;
                Close();
            }
        }
    }
}
