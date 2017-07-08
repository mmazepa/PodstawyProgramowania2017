using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using PrzychodniaMedyczna.Database;

namespace PrzychodniaMedyczna.Other
{
    class MenuManager
    {
        public static ConsoleColor infoColor = ConsoleColor.Cyan;

        public static void DisplayLogo()
        {
            string header = "  Przychodnia Medyczna    Mariusz Mazepa    KOLOR UG 2017";
            int width = Console.WindowWidth;

            while (header.Length < width) header += " ";

            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(header);
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\n");
            Console.WriteLine("  ███╗   ███╗███████╗██████╗ ██╗ ██████╗██╗███╗   ██╗███████╗  ");
            Console.WriteLine("  ████╗ ████║██╔════╝██╔══██╗██║██╔════╝██║████╗  ██║██╔════╝  ");
            Console.WriteLine("  ██╔████╔██║█████╗  ██║  ██║██║██║     ██║██╔██╗ ██║█████╗    ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("  ██║╚██╔╝██║██╔══╝  ██║  ██║██║██║     ██║██║╚██╗██║██╔══╝    ");
            Console.WriteLine("  ██║ ╚═╝ ██║███████╗██████╔╝██║╚██████╗██║██║ ╚████║███████╗  ");
            Console.WriteLine("  ╚═╝     ╚═╝╚══════╝╚═════╝ ╚═╝ ╚═════╝╚═╝╚═╝  ╚═══╝╚══════╝  ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("                                          " + DateTime.Now);
            Console.ResetColor();
        }

        public static void StartScreenMenu()
        {
            Console.WriteLine("");
            Console.WriteLine("  ╔═════════════════════════════════════════════════════╗");
            Console.WriteLine("  ║  EKRAN POWITALNY:                                   ║");
            Console.WriteLine("  ║  Dzień dobry, co chcesz zrobić?                     ║");
            Console.WriteLine("  ╠═════════════════════════════════════════════════════╣");
            Console.WriteLine("  ║    1    - Zaloguj się                               ║");
            Console.WriteLine("  ║    exit - Wyjście z aplikacji                       ║");
            Console.WriteLine("  ╚═════════════════════════════════════════════════════╝\n");
        }

        public static void LoggingInMenu()
        {
            Console.WriteLine("");
            Console.WriteLine("                      ╔═════════════╗                    ");
            Console.WriteLine("  ╔═══════════════════╣ ZALOGUJ SIĘ ╠═══════════════════╗");
            Console.WriteLine("  ║                   ╚═════════════╝                   ║");
            Console.WriteLine("  ║                                                     ║");
            Console.WriteLine("  ║  Podaj login i hasło, aby się zalogować...          ║");
            Console.WriteLine("  ║                                                     ║");
            Console.WriteLine("  ╚═════════════════════════════════════════════════════╝\n\n");
        }

        public static void DisplayMenu()
        {   
            DisplayUserInfo();
            Console.WriteLine("  ╔═════════════════════════════════════════════════════╗");

            if (Mock.userType == "User")
            {
                Console.WriteLine("  ║  Witamy w panelu użytkownika, co chcesz zrobić?     ║");
                Console.WriteLine("  ╠═════════════════════════════════════════════════════╣");
                Console.WriteLine("  ║    1    - Lista dostępnych lekarzy                  ║");
                Console.WriteLine("  ║    2    - Twoje wizyty                              ║");
                Console.WriteLine("  ║    3    - Zasięgnij porady medycznej                ║");
                Console.WriteLine("  ║    4    - Apteki w najbliższej okolicy              ║");
            }
            else if (Mock.userType == "Administrator")
            {
                Console.WriteLine("  ║  Witamy w panelu administratora, co chcesz zrobić?  ║");
                Console.WriteLine("  ╠═════════════════════════════════════════════════════╣");
                Console.WriteLine("  ║    1    - Lista zarejestrowanych użytkowników       ║");
            }

            Console.WriteLine("  ║    exit - Wyloguj się                               ║");
            Console.WriteLine("  ╚═════════════════════════════════════════════════════╝\n");

            if (Mock.userType == "User")
            {
                Console.WriteLine("  Drogi użytkowniku,");
                Console.WriteLine("    dzięki niniejszej aplikacji możesz umówić się na wizytę");
                Console.WriteLine("    wybierając lekarza z listy lub z niej zrezygnować,");
                Console.WriteLine("    zasięgnąć porady medycznej oraz sprawdzić najbliższe apteki.");
                Console.WriteLine("    Wybierz opcję z powyższego menu i zatwierdź klawiszem ENTER.\n");
            }
            else if (Mock.userType == "Administrator")
            {
                Console.WriteLine("  Drogi administratorze,");
                Console.WriteLine("    zakładamy, że wiesz, co masz robić, więc darujemy sobie wskazówki.");
                Console.WriteLine("    Powodzenia!\n");
            }
        }

        public static void DisplayDoctorMenu(string transaction, int doctorsNum)
        {
            DisplayUserInfo();
            Console.WriteLine("  ╔═════════════════════════════════════════════════════╗");

            if (transaction == "pay")
            {
                Console.WriteLine("  ║  ZAREZERWUJ WIZYTĘ:                                 ║");
                Console.WriteLine("  ║  Wyświetlono listę lekarzy, co chcesz zrobić?       ║");
                Console.WriteLine("  ╠═════════════════════════════════════════════════════╣");
                Console.WriteLine("  ║    1-" + doctorsNum + "  - Wybór konkretnego lekarza                 ║");
            }
            else if (transaction == "resign")
            {
                Console.WriteLine("  ║  ZREZYGNUJ Z WIZYTY:                                ║");
                Console.WriteLine("  ║  Wyświetlono listę wizyt, co chcesz zrobić?         ║");
                Console.WriteLine("  ╠═════════════════════════════════════════════════════╣");
                Console.WriteLine("  ║    1-" + doctorsNum + "  - Wybór konkretnej wizyty                   ║");
                Console.WriteLine("  ║    pdf  - Utworzenie pliku PDF z raportem           ║");
            }

            Console.WriteLine("  ║    back - Powrót                                    ║");
            Console.WriteLine("  ╚═════════════════════════════════════════════════════╝\n");
        }

        public static void DisplayUserInfo()
        {
                ColorText("  " + Mock.loggedUser.Name + ", " + Mock.userType + " [" + Mock.loggedUser.Wallet + "zł]\n", Mock.color);
        }

        public static void ConfirmationMenu(int doctorIndex, string transaction)
        {
            Console.WriteLine("  ╔═════════════════════════════════════════════════════╗");
            Console.WriteLine("  ║  POTWIERDZENIE TRANSAKCJI [" + (doctorIndex + 1) + "]:                      ║");
            Console.WriteLine("  ╠═════════════════════════════════════════════════════╣");

            if (transaction == "pay")
            {
                Console.WriteLine("  ║    Czy jesteś pewien, że chcesz zapłacić za wizytę  ║");
                Console.WriteLine("  ║    u tego lekarza " + Mock._doctors[doctorIndex].Price + "zł?                            ║");
            }
            else if (transaction == "resign")
            {
                Console.WriteLine("  ║    Czy jesteś pewien, że chcesz zrezygnować z       ║");
                Console.WriteLine("  ║    tej wizyty i odzyskać " + Mock._doctors[doctorIndex].Price + "zł?                     ║");
            }

            Console.WriteLine("  ║      1 - tak                2 - nie                 ║");
            Console.WriteLine("  ╚═════════════════════════════════════════════════════╝\n");
        }

        public static void DisplayAdviceMenu()
        {
            DisplayUserInfo();
            Console.WriteLine("  ╔═════════════════════════════════════════════════════╗");
            Console.WriteLine("  ║  PORADA MEDYCZNA:                                   ║");
            Console.WriteLine("  ║  Wyświetlono listę dostępnych porad, co chcesz      ║");
            Console.WriteLine("  ║  zrobić?                                            ║");
            Console.WriteLine("  ╠═════════════════════════════════════════════════════╣");
            Console.WriteLine("  ║    1    - Porada nr 1                               ║");
            Console.WriteLine("  ║    2    - Porada nr 2                               ║");
            Console.WriteLine("  ║    3    - Porada nr 3                               ║");
            Console.WriteLine("  ║    back - Powrót                                    ║");
            Console.WriteLine("  ╚═════════════════════════════════════════════════════╝\n");
        }

        public static void DisplayPharmacyMenu()
        {
            DisplayUserInfo();
            Console.WriteLine("  ╔═════════════════════════════════════════════════════╗");
            Console.WriteLine("  ║  APTEKI:                                            ║");
            Console.WriteLine("  ║  Wyświetlono listę aptek.                           ║");
            Console.WriteLine("  ╚═════════════════════════════════════════════════════╝\n");
        }

        public static void DisplayUsersMenu()
        {
            DisplayUserInfo();
            Console.WriteLine("  ╔═════════════════════════════════════════════════════╗");
            Console.WriteLine("  ║  UŻYTKOWNICY:                                       ║");
            Console.WriteLine("  ║  Wyświetlono listę zarejestrowanych użytkowników.   ║");
            Console.WriteLine("  ╚═════════════════════════════════════════════════════╝\n");
        }

        public static void ConfirmationExitMenu(string option)
        {
            Console.WriteLine();
            Console.WriteLine("  ╔═════════════════════════════════════════════════════╗");

            if (option == "exit") Console.WriteLine("  ║  ZAMYKANIE PROGRAMU:                                ║");
            else if (option == "logout") Console.WriteLine("  ║  WYLOGOWYWANIE SIĘ:                                 ║");

            Console.WriteLine("  ╠═════════════════════════════════════════════════════╣");

            if (option == "exit") Console.WriteLine("  ║  Czy na pewno chcesz zakończyć pracę w programie?   ║");
            else if (option == "logout") Console.WriteLine("  ║  Czy na pewno chcesz się wylogować?                 ║");

            Console.WriteLine("  ║      1 - tak                2 - nie                 ║");
            Console.WriteLine("  ╚═════════════════════════════════════════════════════╝\n");
        }

        public static void ClearScreen()
        {
            Console.WriteLine("  Wciśnij ENTER, by kontynuować...");
            Console.Write("  ");
            Console.ReadLine();
            Console.Clear();
        }

        public static void ColorText(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        public static void InfoAlert(string text)
        {
            ColorText(text, infoColor);
            ClearScreen();
        }

        public static void ManyChars(char character, int times)
        {
            for (int i = 0; i < times; i++)
            {
                Console.Write(character);
            }
        }
    }
}
