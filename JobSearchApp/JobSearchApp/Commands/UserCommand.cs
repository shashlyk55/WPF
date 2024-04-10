using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JobSearchApp.Commands
{
    public class UserCommand : RoutedUICommand
    {
        private static RoutedUICommand _openAddVacancyWindowCommand;
        private static RoutedUICommand _openEditVacancyWindowCommand;
        private static RoutedUICommand _deleteVacancyCommand;
        private static RoutedUICommand _undoCommand;
        private static RoutedUICommand _redoCommand;
        private static RoutedUICommand _setPinkThemeCommand;
        private static RoutedUICommand _setBlueThemeCommand;
        private static RoutedUICommand _addToFavoritesCommand;
        private static RoutedUICommand _showFavoritesCommand;
        private static RoutedUICommand _searchCommand;
        private static RoutedUICommand _applySalaryFilterCommand;
        private static RoutedUICommand _applyJobTypeFilterCommand;
        private static RoutedUICommand _applySortCommand;


        
        static UserCommand()
        {
            _openAddVacancyWindowCommand = new RoutedUICommand("OpenAddVacancyWindow", "OpenAddVacancyWindowCommand", typeof(UserCommand));
            _openEditVacancyWindowCommand = new RoutedUICommand("OpenEditVacancyWindow", "OpenEditVacancyWindowCommand", typeof(UserCommand));
            _deleteVacancyCommand = new RoutedUICommand("DeleteVacancy", "DeleteVacancyCommand", typeof(UserCommand));
            _undoCommand = new RoutedUICommand("Undo","UndoCommand",typeof(UserCommand));
            _redoCommand = new RoutedUICommand("Redo","RedoCommand",typeof(UserCommand));
            _setPinkThemeCommand = new RoutedUICommand("SetPinkTheme", "SetPinkThemeCommand", typeof(UserCommand));
            _setBlueThemeCommand = new RoutedUICommand("SetBlueTheme", "SetBlueThemeCommand", typeof(UserCommand));
            _addToFavoritesCommand = new RoutedUICommand("AddToFavorites", "AddToFavoritesCommand",typeof(UserCommand));
            _showFavoritesCommand = new RoutedUICommand("ShowFavorites", "ShowFavoritesCommand",typeof(UserCommand));
            _searchCommand = new RoutedUICommand("Search", "SearchCommand",typeof(UserCommand));
            _applySalaryFilterCommand = new RoutedUICommand("ApplySalaryFilter", "ApplySalaryFilterCommand",typeof(UserCommand));
            _applyJobTypeFilterCommand = new RoutedUICommand("ApplyJobTypeFilter", "ApplyJobTypeFilterCommand",typeof(UserCommand));
            _applySortCommand = new RoutedUICommand("ApplySort", "ApplySortCommand",typeof(UserCommand));
        }
        public static RoutedUICommand OpenAddVacancyWindowCommand { get => _openAddVacancyWindowCommand; }
        public static RoutedUICommand OpenEditVacancyWindowCommand { get => _openEditVacancyWindowCommand; }
        public static RoutedUICommand DeleteVacancyCommand { get => _deleteVacancyCommand; }
        public static RoutedUICommand UndoCommand { get => _undoCommand; }
        public static RoutedUICommand RedoCommand { get => _redoCommand; }
        public static RoutedUICommand SetPinkThemeCommand { get => _setPinkThemeCommand; }
        public static RoutedUICommand SetBlueThemeCommand { get => _setBlueThemeCommand; }
        public static RoutedUICommand AddToFavoritesCommand { get => _addToFavoritesCommand; }
        public static RoutedUICommand ShowFavoritesCommand { get => _showFavoritesCommand; }
        public static RoutedUICommand SearchCommand { get => _searchCommand; }
        public static RoutedUICommand ApplySalaryFilterCommand { get => _applySalaryFilterCommand; }
        public static RoutedUICommand ApplyJobTypeFilterCommand { get => _applyJobTypeFilterCommand; }
        public static RoutedUICommand ApplySortCommand { get => _applySortCommand; }
    }
}
