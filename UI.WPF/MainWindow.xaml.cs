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

namespace Nutribuddy.UI.WPF
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
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;
        private const int STD_INPUT_HANDLE = -10;   // Standardowe wejście (np. klawiatura)
        private const int STD_OUTPUT_HANDLE = -11; // Standardowe wyjście (np. konsola)
        private const int STD_ERROR_HANDLE = -12;  // Standardowe wyjście błędów

        private bool isConsoleMode = false; // Flaga przełącznika trybu

        public MainWindow()
		{
			InitializeComponent();
		}

        // Przełącz do trybu konsolowego
        private void SwitchToConsoleMode()
        {
            if (isConsoleMode)
                return;

            isConsoleMode = true;

            if (GetConsoleWindow() == IntPtr.Zero)
            {
                if (!EnsureConsoleAllocated())
                {
                    MessageBox.Show("Nie udało się przypisać konsoli.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    Application.Current.Shutdown();
                    return;
                }
            }

            this.Close();

            ShowConsoleWindow();

            System.Console.Clear();
            System.Console.WriteLine("Przełączono do trybu konsolowego.");
            System.Console.WriteLine("Naciśnij Enter, aby wrócić do trybu WPF.");

            System.Console.ReadLine();

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
            IntPtr hWnd = GetConsoleWindow();
            if (hWnd != IntPtr.Zero)
            {
                ShowWindow(hWnd, SW_HIDE);  // Ukrywa konsolę
            }
        }
        private void ShowConsoleWindow()
        {
            IntPtr hWnd = GetConsoleWindow();
            if (hWnd != IntPtr.Zero)
            {
                ShowWindow(hWnd, SW_SHOW);  // Pokazuje konsolę
            }
        }


        private bool EnsureConsoleAllocated()
        {
            if (AllocConsole())
            {
                // Sprawdź uchwyty standardowego wejścia i wyjścia
                IntPtr stdIn = GetStdHandle(STD_INPUT_HANDLE);
                IntPtr stdOut = GetStdHandle(STD_OUTPUT_HANDLE);
                IntPtr stdErr = GetStdHandle(STD_ERROR_HANDLE);

                if (stdIn == IntPtr.Zero || stdOut == IntPtr.Zero || stdErr == IntPtr.Zero)
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
