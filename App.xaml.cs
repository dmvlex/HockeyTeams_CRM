using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Nikolay_YW.HockeyDB;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Nikolay_YW
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            DatabaseFacade facade = new DatabaseFacade(new DataContext());

            facade.EnsureCreated();
        }
    }
}
