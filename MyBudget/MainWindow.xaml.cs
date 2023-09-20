using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace MyBudget
{
    public partial class MainWindow : Window
    {
        private List<string> types = new List<string>();
        private List<Budgeting> budget;
        private List<Budgeting> daybudget;
        public string filename = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\budget.json";
        private int result;
        private Budgeting selectedbudget;
        public MainWindow()
        {
            InitializeComponent();
            daybudget = new List<Budgeting>();
            datePicker.SelectedDate = DateTime.Today;
            if (File.Exists(filename))
            {
                budget = JsonClass<List<Budgeting>>.Deserialize<List<Budgeting>>("budget.json");
                DisplayBudgets();

            }
            else
            {
                budget = new List<Budgeting>();

            }
        }
        private void DisplayBudgets()
        {
            if (budget != null)
            {
                types.Clear();
                budgType.Items.Clear();
                foreach (Budgeting budget in budget)
                {
                    types.Add(budget.Type);

                }
                foreach (var type in types)
                {
                    budgType.Items.Add(type);
                }

                daybudget.Clear();
                result = 0;
                MyDataGrid.ItemsSource = null;
                foreach (Budgeting entry in budget)
                {
                    if (entry.Date.DayOfYear == Convert.ToDateTime(datePicker.SelectedDate).DayOfYear)
                    {
                        daybudget.Add(entry);
                        if (entry.isAdd == true)
                            result += entry.Amount;
                        else
                            result -= entry.Amount;
                    }

                }
                MyDataGrid.ItemsSource = daybudget;
                TextAmount.Text = "Итог: " + result.ToString();
            }
        }
        private void SaveNotesToFile()
        {
            JsonClass<List<Budgeting>>.Serialize(budget, "budget.json");
        }

        private void datePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayBudgets();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            bool isAdd;
            string type = "";
            if (budgAmount.Text.Contains("-"))
                isAdd = false;
            else
                isAdd = true;

            if (budgType.SelectedItem != null)
                type = budgType.SelectedItem.ToString();

            if (string.IsNullOrEmpty(budgTitle.Text) || string.IsNullOrEmpty(budgType.Text) || string.IsNullOrEmpty(budgAmount.Text))
            {
                MessageBox.Show("Поля не заполнены");
            }
            else
            {
                budget.Add(new Budgeting(budgTitle.Text, type, Int32.Parse(budgAmount.Text), isAdd, datePicker.SelectedDate.GetValueOrDefault()));
                DisplayBudgets();
                SaveNotesToFile();
            }
        }

        private void AddTypeButton_Click(object sender, RoutedEventArgs e)
        {
            AddWindows window = new AddWindows();
            window.ShowDialog();
            if (window.DialogResult == true)
            {
                budgType.Items.Add(window.typeTitle.Text);
            }
        }

        private void MyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedbudget = MyDataGrid.SelectedItem as Budgeting;
            if (selectedbudget != null)
            {
                budgTitle.Text = selectedbudget.Title;
                budgType.SelectedItem = selectedbudget.Type;
                budgAmount.Text = selectedbudget.Amount.ToString();
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            selectedbudget = MyDataGrid.SelectedItem as Budgeting;

            if (selectedbudget != null)
            {
                selectedbudget.Title = budgTitle.Text;
                selectedbudget.Type = budgType.Text;
                selectedbudget.Amount = Int32.Parse(budgAmount.Text);
                if (budgAmount.Text.Contains("-"))
                    selectedbudget.isAdd = false;
                else
                    selectedbudget.isAdd = true;
                DisplayBudgets();
                SaveNotesToFile();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            selectedbudget = MyDataGrid.SelectedItem as Budgeting;

            if (selectedbudget != null)
            {
                budget.Remove(selectedbudget);
                DisplayBudgets();
                SaveNotesToFile();
            }
        }
    }
    public class Budgeting
    {
        public string Title { get; set; }
        public string Type { get; set; }

        public int Amount;
        public bool isAdd { get; set; }
        public DateTime Date;
        //public int amount
        //{
        //    get { return Amount; }
        //    set
        //    {
        //        if (value < 0)
        //        {
        //            Amount = -value;
        //            isAdd = false;
        //        }
        //        else
        //        {
        //            Amount = value;
        //            isAdd = true;
        //        }
        //    }
        //}
        [JsonIgnore]
        public int amount
        {
            get { return isAdd ? Amount : -Amount; }
            set { Amount = isAdd ? value : value; }
        }
        public Budgeting(string title, string type, int amount, bool IsAdd, DateTime date)
        {
            Title = title;
            Type = type;
            Amount = amount;
            isAdd = IsAdd;
            Date = date;
        }
    }
}
