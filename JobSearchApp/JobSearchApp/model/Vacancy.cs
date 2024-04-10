using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JobSearchApp.model
{
    public class Vacancy
    {
        private string _companyLogo;
        private string _jobTitle;
        private int _salary;
        private DateTime _date;
        public string CompanyLogo { get; set; }
        public string JobTitle { get; set; }
        public int Salary { get; set; }
        public DateTime DateAdded { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        public Vacancy(string companyLogo, string jobTitle, int salary, DateTime dateAdded)
        {
            CompanyLogo = companyLogo;
            JobTitle = jobTitle;
            Salary = salary;
            DateAdded = dateAdded;
        }

        public object Clone()
        {
            if (this == null)
                return null;
            return new Vacancy(CompanyLogo, JobTitle, Salary, DateAdded);
        }

    }
}
