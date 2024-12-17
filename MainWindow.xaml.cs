using Nutribuddy.Core.Controllers;
using Nutribuddy.UI;
using Nutribuddy.UI.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace Nutribuddy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Import funkcji WinAPI
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();

        [DllImport("kernel32.dll")]
        private static extern nint GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(nint hWnd, int nCmdShow);

        [DllImport("kernel32.dll")]
        private static extern nint GetStdHandle(int nStdHandle);

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;
        private const int STD_INPUT_HANDLE = -10;   // Standardowe wejście (np. klawiatura)
        private const int STD_OUTPUT_HANDLE = -11; // Standardowe wyjście (np. konsola)
        private const int STD_ERROR_HANDLE = -12;  // Standardowe wyjście błędów

        private bool isConsoleMode = false; // Flaga przełącznika trybu
        private UserController userController;
        private FoodController foodController;
        private DishController dishController;
        private EatHistoryController eatHistoryController;
        private ViewManager viewManager;

        public MainWindow()
        {
            InitializeComponent();
            if (userController == null)
            {
                userController = new UserController();
                foodController = new FoodController("C:\\Users\\kszym\\source\\repos\\Nutribuddy\\Data\\FoodData.json");
                dishController = new DishController("C:\\Users\\kszym\\source\\repos\\Nutribuddy\\Data\\DishData.json");
                eatHistoryController = new EatHistoryController("C:\\Users\\kszym\\source\\repos\\Nutribuddy\\Data\\FoodHistory.json", "C:\\Users\\kszym\\source\\repos\\Nutribuddy\\Data\\DishHistory.json");
                viewManager = new ViewManager();
                viewManager.RegisterView("IntroSequence", new IntroSequenceView(() => viewManager.ShowView("MainMenu")));
                viewManager.RegisterView("MainMenu", new MainMenuView(
                    () => viewManager.ShowView("UserDetails"),
                    () => viewManager.ShowView("Food"),
                    () => viewManager.ShowView("Dish"),
                    () => viewManager.ShowView("Calendar")));
                viewManager.RegisterView("UserDetails", new UserDetailsView(
                    eatHistoryController,
                    userController,
                    dishController,
                    () => viewManager.ShowView("MainMenu"),
                    () => viewManager.ShowView("UserConfig")));
                viewManager.RegisterView("UserConfig", new UserConfigView(userController, () => viewManager.ShowView("UserDetails")));
                viewManager.RegisterView("Food", new FoodView(eatHistoryController, foodController, () => viewManager.ShowView("MainMenu")));
                viewManager.RegisterView("Dish", new DishView(eatHistoryController, foodController, dishController, () => viewManager.ShowView("MainMenu")));
                viewManager.RegisterView("Calendar", new CalendarView(eatHistoryController, () => viewManager.ShowView("MainMenu")));
            }
        }

        // Przełącz do trybu konsolowego
        private void SwitchToConsoleMode()
        {
            if (isConsoleMode)
                return;

            isConsoleMode = true;

            if (GetConsoleWindow() == nint.Zero)
            {
                if (!EnsureConsoleAllocated())
                {
                    MessageBox.Show("Nie udało się przypisać konsoli.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    Application.Current.Shutdown();
                    return;
                }
            }

            Close();

            ShowConsoleWindow();

            Console.Clear();

            viewManager.ShowView("MainMenu");

            HideConsoleWindow();

            Application.Current.Dispatcher.Invoke(() =>
            {
                var newWindow = new MainWindow();
                newWindow.Show();
            });

            isConsoleMode = false;
        }


        private void HideConsoleWindow()
        {
            nint hWnd = GetConsoleWindow();
            if (hWnd != nint.Zero)
            {
                ShowWindow(hWnd, SW_HIDE);  // Ukrywa konsolę
            }
        }
        private void ShowConsoleWindow()
        {
            nint hWnd = GetConsoleWindow();
            if (hWnd != nint.Zero)
            {
                ShowWindow(hWnd, SW_SHOW);  // Pokazuje konsolę
            }
        }


        private bool EnsureConsoleAllocated()
        {
            if (AllocConsole())
            {
                // Sprawdź uchwyty standardowego wejścia i wyjścia
                nint stdIn = GetStdHandle(STD_INPUT_HANDLE);
                nint stdOut = GetStdHandle(STD_OUTPUT_HANDLE);
                nint stdErr = GetStdHandle(STD_ERROR_HANDLE);

                if (stdIn == nint.Zero || stdOut == nint.Zero || stdErr == nint.Zero)
                {
                    FreeConsole(); // Zwolnij konsolę, jeśli uchwyty są błędne
                    return false;
                }

                return true;
            }

            return false;
        }


        // Obsługa kliknięcia przycisku przejścia do trybu konsolowego
        private void ButtonSwitchToConsole_Click(object sender, RoutedEventArgs e)
        {
            SwitchToConsoleMode();
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Zakończ aplikację, jeśli okno główne jest zamknięte
            if (!isConsoleMode)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
