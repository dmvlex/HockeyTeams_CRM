using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using System.Windows.Controls.Primitives;
using System.Xml.Linq;
using System.Collections;

namespace Nikolay_YW.HockeyDB
{
    public class PlayersComparer : IComparer<Player>
    {
        public int Compare(Player? x, Player? y)
        {
            var xPlayerPoints = x.MayGoals + x.Goals;
            var yPlayerPoints = y.MayGoals + y.Goals;

            if (xPlayerPoints > yPlayerPoints) return -1;
            if (xPlayerPoints < yPlayerPoints) return 1;
            if (xPlayerPoints == yPlayerPoints) return 0;

            return 0;
        }
    }


    static class DatabaseController
    {

        public static void DeleteOneFromEachInTeam(ListView totalTeamsInfo, int numToDelete)
        {
            using (DataContext context = new DataContext())
            {
                TotalTeamsInfo selectedPlayer = totalTeamsInfo.SelectedItem as TotalTeamsInfo;

                if (selectedPlayer != null)
                {
                    foreach (var player in context.Players)
                    {
                        if (player.Team == selectedPlayer.Team)
                        {
                            if (player.Goals != 0)
                            {
                                if (player.Goals - numToDelete < 0)
                                {
                                    player.Goals = 0;
                                }
                                else
                                {
                                    player.Goals -= numToDelete;
                                }
                            }
                        }
                    }
                    context.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Выберите команду", "Ошибка!");
                }
            }
        }
        public static void ReadTotalInfo(ListView totalTeamsInfo)
        {
            using (DataContext context = new DataContext())
            {
                var allPlayers = context.Players.ToList();
                allPlayers.Sort(new PlayersComparer());

                // На часах 1:26, я не буду комментировать код, что идет дальше.
                // Поверь, оно тебе не надо
                List<TotalTeamsInfo> teams = new List<TotalTeamsInfo>();
                foreach (var item in allPlayers)
                {
                if (teams.Count != 0)
                {
                    bool isFind = false;

                    for (int i = 0; i < teams.Count; i++)
                    {
                        if (teams[i].Team == item.Team)
                        {
                            teams[i].Penalty += Math.Round(item.PenaltyTime,2);
                            teams[i].Points += item.MayGoals + item.Goals;
                            isFind = true;
                        }
                    }
                    if (!isFind)
                    {
                            teams.Add(new TotalTeamsInfo()
                            {
                                Team = item.Team,
                                Penalty = Math.Round(item.PenaltyTime, 2),
                                Points = item.MayGoals + item.Goals
                            }); ;
                    }
                }
                else
                {
                        teams.Add(new TotalTeamsInfo()
                        {
                            Team = item.Team,
                            Penalty = Math.Round(item.PenaltyTime, 2),
                            Points = item.MayGoals + item.Goals
                        }) ;

                    }
                }

                foreach (var item in teams)
                {
                    item.Penalty = Math.Round(item.Penalty, 2);
                }
                totalTeamsInfo.ItemsSource = teams;
            }
        }
        /// <summary>
        /// Выводит лучших игроков
        /// </summary>
        public static void ReadBestPlayers(ListView bestPlayersList)
        {
            using (DataContext context = new DataContext())
            {
                var allPlayers = context.Players.ToList();
                allPlayers.Sort(new PlayersComparer());

                List<BestPlayers> bests = new List<BestPlayers>();

                for (int i = 0; i < 6; i++)
                {
                    bests.Add(new BestPlayers()
                    {
                        Name = allPlayers[i].Name,
                        Team = allPlayers[i].Team,
                        Points = allPlayers[i].MayGoals + allPlayers[i].Goals
                    });
                }

                bestPlayersList.ItemsSource = bests;
            }
        }
        /// <summary>
        /// Считывает всю инфу с базы данных и выводит в ListView
        /// </summary>
        public static void Read(List<Player> databasePlayers, ListView playersList)
        {
            using (DataContext context = new DataContext())
            {
                databasePlayers = context.Players.ToList();
                playersList.ItemsSource = databasePlayers;
            }
        }

        /// <summary>
        /// Добавляет нового игрока в базу даннных
        /// </summary>
        public static void Create(string name, string team, int goals, int mayGoals, double penalty)
        {
            using (DataContext context = new DataContext())
            {
                    context.Players.Add(new Player()
                    {
                        Name = name,
                        Team = team,
                        Goals = goals,
                        MayGoals = mayGoals,
                        PenaltyTime = penalty
                    });
                    context.SaveChanges();
            }
        }

        /// <summary>
        /// Удаляет выбранного игрока 
        /// </summary>
        public static void Delete(ListView playersList)
        {
            using (DataContext context = new DataContext())
            {
                Player selectedPlayer = playersList.SelectedItem as Player;

                if (selectedPlayer != null)
                {
                    Player player = context.Players.Single(x => x.Id == selectedPlayer.Id);

                    context.Remove(player);
                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Редактирует выбранного игрока
        /// </summary>
        public static void Update(ListView playersList, string name, string team, int goals, int mayGoals, double penalty)
        {
            using (DataContext context = new DataContext())
            {
                Player selectedPlayer = playersList.SelectedItem as Player;

                if (selectedPlayer != null)
                {
                    Player player = context.Players.Find(selectedPlayer.Id);

                    player.Name = name;
                    player.Team = team;
                    player.Goals = goals;
                    player.MayGoals = mayGoals;
                    player.PenaltyTime = penalty;

                    context.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Не выбрана ни одна строка для изменения!","Ошибка!");
                }
            }
        }


    }
}
