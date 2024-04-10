using JobSearchApp.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace JobSearchApp
{
    /// <summary>
    /// Логика взаимодействия для AddVacancyWindow.xaml
    /// </summary>
    public partial class AddVacancyWindow : Window
    {
        public event EventHandler<VacancyAddedEventArgs> VacancyAdded;

        public AddVacancyWindow()
        {
            InitializeComponent();

            Cursor = new Cursor(
                Application.GetResourceStream(
                    new Uri("photo/dog.cur", UriKind.Relative)).Stream);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string projectFolder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

            string fullPath = "";
            if (!(string.IsNullOrEmpty(this.JobTitle_textBox.Text) ||
               string.IsNullOrEmpty(this.Logo_textbox.Text) ||
               string.IsNullOrEmpty(this.Salary_textBox.Text)))
            {
                fullPath = System.IO.Path.Combine(projectFolder, this.Logo_textbox.Text);
            }
            else
            {
                MessageBox.Show("Все поля должны быть заполнены");
                return;
            }

            if (!Regex.IsMatch(this.Salary_textBox.Text, @"^\d+$") || !File.Exists(fullPath))
            {
                MessageBox.Show("Поля заполнены некорректно");
                return;
            }
            Vacancy newVacancy = new Vacancy(
                this.Logo_textbox.Text,
                this.JobTitle_textBox.Text,
                Convert.ToInt32(this.Salary_textBox.Text),
                DateTime.Now
                );
            
            // Вызываем событие для сообщения о добавлении вакансии
            OnVacancyAdded(newVacancy);

            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        protected virtual void OnVacancyAdded(Vacancy newVacancy)
        {
            VacancyAdded?.Invoke(this, new VacancyAddedEventArgs(newVacancy));
        }

        public class VacancyAddedEventArgs : EventArgs
        {
            public Vacancy NewVacancy { get; }

            public VacancyAddedEventArgs(Vacancy newVacancy)
            {
                NewVacancy = newVacancy;
            }
        }

    }
}
