using Nikolay_YW.HockeyDB;
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

namespace Nikolay_YW
{
    /// <summary>
    /// Логика взаимодействия для AdditionalInfo.xaml
    /// </summary>
    public partial class AdditionalInfoWindow : Window
    {
        public AdditionalInfoWindow()
        {
            InitializeComponent();
            DatabaseController.ReadTotalInfo(TotalTeamInfo);
        }

        private void GoalsTextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(DeleteGoalsBox.Text, "[^0-9]"))
            {
                MessageBox.Show("Пожайлуста, вводите только цифры", "Внимание!");
                DeleteGoalsBox.Text = DeleteGoalsBox.Text.Remove(DeleteGoalsBox.Text.Length - 1);
            }
        }

        private void DeleteOneFromEach(object sender, RoutedEventArgs e)
        {
            var numDelete = int.Parse(DeleteGoalsBox.Text);

            DatabaseController.DeleteOneFromEachInTeam(TotalTeamInfo, numDelete);
            DatabaseController.ReadTotalInfo(TotalTeamInfo);
        }
    }
}
