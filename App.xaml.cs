using Nutribuddy;
using System;
using System.Windows;

namespace Nutribuddy
{
	public partial class App : Application
	{
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Ustawienie aplikacji na ręczne zamykanie
            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            // Otwórz główne okno WPF
            var mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}