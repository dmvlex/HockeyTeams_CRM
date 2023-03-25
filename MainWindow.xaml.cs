using Nikolay_YW.HockeyDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Nikolay_YW
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<Player> DatabasePlayers { get; private set; }
        private Dictionary<string, string> propertyDictionary = new Dictionary<string, string>();

        public MainWindow()
        {
            InitializeComponent();
            DatabaseController.Read(DatabasePlayers, PlayersList);
            DatabaseController.ReadBestPlayers(BestPlayersList);
            FillPropertyDictionary();
            ConfigureComboBoxes();
            
        }

        //Проверка форм на пустое значение. Может можно было сделать изящнее?
        private bool IsFormsEmpty()
        {
            string[] formsText = new string[] { NameBox.Text, TeamBox.Text, GoalBox.Text, MayGoalBox.Text, PenaltyTimeBox.Text };

            foreach (var item in formsText)
            {
                if (string.IsNullOrEmpty(item))
                {
                    return true;
                }
            }

            return false;
        }
        private void ConfigureComboBoxes()
        {
            ColumnBox.ItemsSource = new string[] { "ФИО", "Команда", "Забитые шайбы", "Голевые подачи", "Штрафное время" };
            DirectionBox.ItemsSource = new string[] { "по возрастанию", "по убыванию" };
        }
        private void FillPropertyDictionary()
        {
            propertyDictionary.Add("ФИО", "Name");
            propertyDictionary.Add("Команда", "Team");
            propertyDictionary.Add("Забитые шайбы", "Goals");
            propertyDictionary.Add("Голевые подачи", "MayGoals");
            propertyDictionary.Add("Штрафное время", "PenaltyTime");
        }

        //Обработчики событий для кнопок. Получают данные из форм и передают их контроллеру
        private void Read(object sender, RoutedEventArgs e)
        {
            DatabaseController.Read(DatabasePlayers, PlayersList);
            DatabaseController.ReadBestPlayers(BestPlayersList);
        }
        private void Create(object sender, RoutedEventArgs e)
        {
            if (IsFormsEmpty())
            {
                MessageBox.Show("Все поля информации о игроке должны быть заполнены!", "Ошибка!");
            }
            else
            {
                var name = NameBox.Text;
                var team = TeamBox.Text;
                var goals = int.Parse(GoalBox.Text);
                var mayGoals = int.Parse(MayGoalBox.Text);
                var penalty = double.Parse(PenaltyTimeBox.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture);

                DatabaseController.Create(name, team, goals, mayGoals, penalty);
                DatabaseController.Read(DatabasePlayers, PlayersList);
                DatabaseController.ReadBestPlayers(BestPlayersList);
            }
        }
        private void Delete(object sender, RoutedEventArgs e)
        {
            DatabaseController.Delete(PlayersList);
            DatabaseController.Read(DatabasePlayers, PlayersList);
            DatabaseController.ReadBestPlayers(BestPlayersList);
        }
        private void Update(object sender, RoutedEventArgs e)
        {
            if (IsFormsEmpty())
            {
                MessageBox.Show("Все поля информации о игроке должны быть заполнены!", "Ошибка!");
            }
            else
            {
                var name = NameBox.Text;
                var team = TeamBox.Text;
                var goals = int.Parse(GoalBox.Text);
                var mayGoals = int.Parse(MayGoalBox.Text);
                var penalty = double.Parse(PenaltyTimeBox.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture);

                DatabaseController.Update(PlayersList,name,team,goals,mayGoals,penalty);
                DatabaseController.Read(DatabasePlayers, PlayersList);
                DatabaseController.ReadBestPlayers(BestPlayersList);
            }
        }

        private void GoalBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(GoalBox.Text, "[^0-9]"))
            {
                MessageBox.Show("Пожайлуста, вводите только цифры", "Внимание!");
                GoalBox.Text = GoalBox.Text.Remove(GoalBox.Text.Length - 1);
            }
        }

        private void MayGoalBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(MayGoalBox.Text, "[^0-9]"))
            {
                MessageBox.Show("Пожайлуста, вводите только цифры", "Внимание!");
                MayGoalBox.Text = MayGoalBox.Text.Remove(MayGoalBox.Text.Length - 1);
            }
        }

        private void PenaltyTimeTextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(PenaltyTimeBox.Text, "[^0-9,.]"))
            {
                MessageBox.Show("Пожайлуста, вводите только цифры", "Внимание!");
                PenaltyTimeBox.Text = PenaltyTimeBox.Text.Remove(PenaltyTimeBox.Text.Length - 1);
            }
        }

        private void SortList()
        {
            PlayersList.Items.SortDescriptions.Clear();
            var SortProperty = propertyDictionary[ColumnBox.SelectedItem.ToString()];
            ListSortDirection SortDirection;

            if (DirectionBox.SelectedItem != null)
            {
                SortDirection = DirectionBox.SelectedItem.ToString() == "по убыванию" ? ListSortDirection.Descending : ListSortDirection.Ascending;
            }
            else
            {
                SortDirection = ListSortDirection.Ascending;
            }
            

            PlayersList.Items.SortDescriptions.Add(new SortDescription(SortProperty, SortDirection));
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SortList();
        }

        private void OpenMoreInfo(object sender, RoutedEventArgs e)
        {
            AdditionalInfoWindow additionalInfo = new AdditionalInfoWindow();
            additionalInfo.Owner = this;
            additionalInfo.Show();
        }
    }
}
