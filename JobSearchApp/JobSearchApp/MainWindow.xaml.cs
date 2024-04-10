using JobSearchApp.Command;
using JobSearchApp.Memento;
using JobSearchApp.model;
//using JobSearchApp.styles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static JobSearchApp.AddVacancyWindow;

namespace JobSearchApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Vacancy> _vacancies;
        public ObservableCollection<Vacancy> Vacancies {
            get
            {
                return _vacancies;
            } 
            set
            {
                this.listBox.ItemsSource = value;
                _vacancies = value;
                OnPropertyChanged("Vacancies");

                Serializer.Serializer.SerializationList(_vacancies, "Serializer/serializeList.json");
            }

        }
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly History _operationHistory;


        public void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        public MainWindow()
        {
            InitializeComponent();
            Cursor = new Cursor(
                Application.GetResourceStream(
                    new Uri("photo/dog.cur", UriKind.Relative)).Stream);

            Uri iconUri = new Uri("photo/icon.ico", UriKind.RelativeOrAbsolute);
            this.Resources["ApplicationIcon"] = new System.Windows.Media.Imaging.BitmapImage(iconUri);

            string projectFolder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            Vacancies = Serializer.Serializer.DeserializationList(System.IO.Path.Combine(projectFolder, "Serializer/serializeList.json"));
            /*Vacancies = new ObservableCollection<Vacancy>()
            {
                new Vacancy("photo/pict3.jpg", "Software Engineer", 50000, DateTime.Now),
                new Vacancy("photo/pict4.jpg", "Data Analyst", 45000, DateTime.Now.AddDays(-2)),
                new Vacancy("photo/pict5.jpg", "Web Developer", 55000, DateTime.Now.AddDays(-5))
            };*/
            DataContext = this;
            _operationHistory = new History(new List<Memento.Memento>());
            SaveState(_vacancies);


        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddVacancyWindow addVacancyWindow = new AddVacancyWindow();
            addVacancyWindow.Owner = this;
            addVacancyWindow.VacancyAdded += AddVacancyWindow_VacancyAdded; // Подписываемся на событие
            addVacancyWindow.ShowDialog();
        }

        private void AddVacancyWindow_VacancyAdded(object sender, VacancyAddedEventArgs e)
        {
            Vacancies.Add(e.NewVacancy);
            Serializer.Serializer.SerializationList(Vacancies, "Serializer/serializeList.json");
            SaveState(_vacancies);


        }
        
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Vacancy selectedVacancy = listBox.SelectedItem as Vacancy;

            if (selectedVacancy != null)
            {
                EditVacancyWindow editVacancyWindow = new EditVacancyWindow(selectedVacancy);
                editVacancyWindow.Owner = this;

                editVacancyWindow.VacancyEdited += (source, editedArgs) =>
                {
                    int index = Vacancies.IndexOf(selectedVacancy);
                    if (index != -1)
                    {
                        Vacancies[index] = editedArgs.EditedVacancy;

                        listBox.Items.Refresh();

                        SaveState(_vacancies);
                    }
                };
                editVacancyWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a vacancy to edit.", "No Vacancy Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            Serializer.Serializer.SerializationList(Vacancies, "Serializer/serializeList.json");
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Vacancy selectedVacancy = listBox.SelectedItem as Vacancy;

            if (selectedVacancy != null)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this vacancy?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    Vacancies.Remove(selectedVacancy);

                    SaveState(_vacancies);

                }
            }
            else
            {
                MessageBox.Show("Please select a vacancy to delete.", "No Vacancy Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            Serializer.Serializer.SerializationList(Vacancies, "Serializer/serializeList.json");
        }
        private void ApplySalaryFilter_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(minSalaryTextBox.Text, out int minSalary) && int.TryParse(maxSalaryTextBox.Text, out int maxSalary))
            {
                var filteredVacancies = Vacancies.Where(v => v.Salary >= minSalary && v.Salary <= maxSalary);
                listBox.ItemsSource = filteredVacancies;
            }
            else
            {
                MessageBox.Show("Please enter valid salary values.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ApplyJobTypeFilter_Click(object sender, RoutedEventArgs e)
        {
            string selectedJobType = (jobTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (selectedJobType == "All")
            {
                listBox.ItemsSource = Vacancies;
            }
            else
            {
                var filteredVacancies = Vacancies.Where(v => v.JobTitle == selectedJobType);
                listBox.ItemsSource = filteredVacancies;

            }
        }
        private void ApplySort_Click(object sender, RoutedEventArgs e)
        {
            string selectedSortBy = (sortByComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            switch (selectedSortBy)
            {
                case "Alphabetical Order":
                    listBox.Items.SortDescriptions.Clear();
                    listBox.Items.SortDescriptions.Add(new SortDescription("JobTitle", ListSortDirection.Ascending));
                    break;
                case "Price: Low to High":
                    listBox.Items.SortDescriptions.Clear();
                    listBox.Items.SortDescriptions.Add(new SortDescription("Salary", ListSortDirection.Ascending));
                    break;
                case "Price: High to Low":
                    listBox.Items.SortDescriptions.Clear();
                    listBox.Items.SortDescriptions.Add(new SortDescription("Salary", ListSortDirection.Descending));
                    break;
                case "Date Added: Newest First":
                    listBox.Items.SortDescriptions.Clear();
                    listBox.Items.SortDescriptions.Add(new SortDescription("DateAdded", ListSortDirection.Descending));
                    break;
                case "Date Added: Oldest First":
                    listBox.Items.SortDescriptions.Clear();
                    listBox.Items.SortDescriptions.Add(new SortDescription("DateAdded", ListSortDirection.Ascending));
                    break;
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = searchTextBox.Text.ToLower();

            var filteredVacancies = Vacancies.Where(v => v.JobTitle.ToLower().Contains(searchText));
            listBox.ItemsSource = filteredVacancies;
        }

        private ObservableCollection<Vacancy> favorites = new ObservableCollection<Vacancy>();

        private void AddToFavoritesButton_Click(object sender, RoutedEventArgs e)
        {
            Vacancy selectedVacancy = listBox.SelectedItem as Vacancy;

            if (selectedVacancy != null)
            {
                if (!favorites.Contains(selectedVacancy))
                {
                    favorites.Add(selectedVacancy);
                    MessageBox.Show("Vacancy added to favorites successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("This vacancy is already in favorites!", "Already in Favorites", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select a vacancy to add to favorites.", "No Vacancy Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void FavoritesButton_Click(object sender, RoutedEventArgs e)
        {
            FavoritesWindow favoritesWindow = new FavoritesWindow(favorites);
            favoritesWindow.Owner = this;
            favoritesWindow.ShowDialog();
        }

        private void listBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Vacancy selectedVacancy = listBox.SelectedItem as Vacancy;

            if (selectedVacancy != null && e.ChangedButton == MouseButton.Left)
            {
                VacancyDetailsWindow vacancyDetailsWindow = new VacancyDetailsWindow(selectedVacancy);
                vacancyDetailsWindow.Owner = this;
                vacancyDetailsWindow.ShowDialog();
            }
        }

        private void EnglishButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceDictionary dict = new ResourceDictionary();
            dict.Source = new Uri("Dictionary\\Eng_Dictionary.xaml", UriKind.Relative);
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }

        private void RussianButton_Click(object sender, RoutedEventArgs e)
        {
            // Загружаем ресурсы на русском языке
            ResourceDictionary dict = new ResourceDictionary();
            dict.Source = new Uri("Dictionary\\Ru_Dictionary.xaml", UriKind.Relative);
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }

        public void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            if (_operationHistory.CurrentOperation == 0)
                    return;

                _operationHistory.CurrentOperation--;

            LoadState(_operationHistory.GetCurrentOperationMemento());
        }

        public void RedoButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (_operationHistory.Operations.Count == 0)
                return;
            if (_operationHistory.CurrentOperation == _operationHistory.Operations.Count - 1)
                return;

            _operationHistory.CurrentOperation++;

            LoadState(_operationHistory.GetCurrentOperationMemento());
        }

        private void LoadState(Memento.Memento operation)
        {
            Vacancies = operation.VacancyList;
        }

        private void SaveState(ObservableCollection<Vacancy> list)
        {
            _operationHistory.Operations.Add(new Memento.Memento(CloneList(list)));
            _operationHistory.CurrentOperation = _operationHistory.Operations.Count - 1;
        }

        private ObservableCollection<Vacancy> CloneList(ObservableCollection<Vacancy> Kettles)
        {
            ObservableCollection<Vacancy> clonedAccounts = new ObservableCollection<Vacancy>();
            foreach (Vacancy kettle in Kettles)
                clonedAccounts.Add(kettle.Clone() as Vacancy);
            return clonedAccounts;
        }

        private void SetBlueThemeButton_OnClick(object sender, RoutedEventArgs e)
            => LoadTheme("Blue");

        private void SetPinkThemeButton_OnClick(object sender, RoutedEventArgs e)
            => LoadTheme("Pink");

        private void LoadTheme(string theme)
        {
            ResourceDictionary dict = new ResourceDictionary();
            dict.Source = new Uri("Styles/" + theme + "Theme.xaml", UriKind.Relative);
            //Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dict);

            /*ResourceDictionary dict = new ResourceDictionary();
            dict.Source = new Uri("Styles/" + theme + "ButtonStyle.xaml", UriKind.Relative);
            //Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dict);
            dict.Source = new Uri("Styles/" + theme + "TextBoxStyle.xaml", UriKind.Relative);
            Application.Current.Resources.MergedDictionaries.Add(dict);*/
        }

    }
}
