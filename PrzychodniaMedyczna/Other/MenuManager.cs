using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using PrzychodniaMedyczna.Database;
using PrzychodniaMedyczna.Model;

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

        public static void DisplayLogoAndMenu(string menuType)
        {
            DisplayLogo();
            switch (menuType)
            {
                case "startMenu":
                    Console.Title = "Przychodnia Medyczna: Ekran powitalny";
                    StartScreenMenu();
                    break;
                case "loginMenu":
                    Console.Title = "Przychodnia Medyczna: Logowanie";
                    LoggingInMenu();
                    break;
                case "mainMenu":
                    Console.Title = "Przychodnia Medyczna: Menu Główne";
                    DisplayMenu();
                    break;
                case "adviceMenu":
                    Console.Title = "Przychodnia Medyczna: Porada medyczna";
                    DisplayAdviceMenu();
                    break;
                case "pharmacyMenu":
                    Console.Title = "Przychodnia Medyczna: Apteki";
                    DisplayPharmacyMenu();
                    break;
                case "adminUsersMenu":
                    Console.Title = "Przychodnia Medyczna: Użytkownicy";
                    DisplayAdminUsersMenu();
                    break;
                case "adminDoctorsMenu":
                    Console.Title = "Przychodnia medyczna: Lekarze";
                    DisplayAdminDoctorsMenu();
                    break;
                case "adminAddDoctorMenu":
                    Console.Title = "Przychodnia medyczna: Dodawanie lekarza";
                    DisplayAdminAddDoctorMenu();
                    break;
                case "adminEditDoctorMenu":
                    Console.Title = "Przychodnia medyczna: Edycja lekarza";
                    DisplayAdminEditDoctorMenu();
                    break;
            }
        }

        public static void DisplayLogoAndMenu(string menuType, string transaction, int doctorsNum)
        {
            DisplayLogo();
            if (menuType == "doctorsMenu")
            {
                if (transaction == "pay") Console.Title = "Przychodnia Medyczna: Wybór lekarza";
                else if (transaction == "resign") Console.Title = "Przychodnia Medyczna: Rachunek/Rezygnacja z wizyty";
                DisplayDoctorMenu(transaction, doctorsNum);
            }
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
                Console.WriteLine("  ║    2    - Lista dostępnych lekarzy                  ║");
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

            Mock._users.Sort((x, y) => x.Name.CompareTo(y.Name));
            Mock.loggedUser.Visits.Sort((x, y) => x.DoctorId.CompareTo(y.DoctorId));

            if (transaction == "pay")
            {
                Console.WriteLine("  ║  ZAREZERWUJ WIZYTĘ:                                 ║");
                Console.WriteLine("  ║  Wyświetlono listę lekarzy, co chcesz zrobić?       ║");
                Console.WriteLine("  ╠═════════════════════════════════════════════════════╣");
                Console.WriteLine("  ║    1-" + doctorsNum + "  - Wybór konkretnego lekarza                 ║");
                Console.WriteLine("  ║    prev - Poprzednia strona                         ║");
                Console.WriteLine("  ║    next - Następna strona                           ║");
            }
            else if (transaction == "resign")
            {
                Console.WriteLine("  ║  ZREZYGNUJ Z WIZYTY:                                ║");
                Console.WriteLine("  ║  Wyświetlono listę wizyt, co chcesz zrobić?         ║");
                Console.WriteLine("  ╠═════════════════════════════════════════════════════╣");
                Console.WriteLine("  ║    1-" + doctorsNum + "  - Wybór konkretnej wizyty                   ║");
                Console.WriteLine("  ║    doc  - Utworzenie dokumentu DOC z raportem       ║");
            }

            Console.WriteLine("  ║    back - Powrót                                    ║");
            Console.WriteLine("  ╚═════════════════════════════════════════════════════╝\n");
        }

        public static void DisplayUserInfo()
        {
            if(Mock.loggedUser.IsAdmin)
                ColorText("  " + Mock.loggedUser.Name + ", " + Mock.userType + "\n", Mock.color);
            else
                ColorText("  " + Mock.loggedUser.Name + ", " + Mock.userType + " [" + Mock.loggedUser.Wallet + "zł]\n", Mock.color);
        }

        public static void ConfirmationMenu(int doctorIndex, string transaction)
        {
            string price = string.Empty;

            Console.WriteLine("  ╔═════════════════════════════════════════════════════╗");
            Console.WriteLine("  ║  POTWIERDZENIE TRANSAKCJI [" + (doctorIndex + 1) + "]:                      ║");
            Console.WriteLine("  ╠═════════════════════════════════════════════════════╣");

            if (transaction == "pay")
            {
                price = Mock._doctors[doctorIndex].Price + " zł?";

                while (price.Length <= 32) price = price + " ";
                Console.WriteLine("  ║    Czy jesteś pewien, że chcesz zapłacić za wizytę  ║");
                Console.WriteLine("  ║    u tego lekarza " + price + " ║");
            }
            else if (transaction == "resign")
            {
                foreach(Doctor doctor in Mock._doctors)
                {
                    if (doctor.Id == Mock.loggedUser.Visits[doctorIndex].DoctorId)
                    {
                        price = doctor.Price + " zł?";
                    }
                }

                while (price.Length <= 25) price = price + " ";
                Console.WriteLine("  ║    Czy jesteś pewien, że chcesz zrezygnować z       ║");
                Console.WriteLine("  ║    tej wizyty i odzyskać " + price + " ║");
            }

            Console.WriteLine("  ║      1 - tak                2 - nie                 ║");
            Console.WriteLine("  ╚═════════════════════════════════════════════════════╝\n");
        }

        public static void DisplayAdviceMenu()
        {
            DisplayUserInfo();
            Console.WriteLine("  ╔═════════════════════════════════════════════════════╗");
            Console.WriteLine("  ║  PORADA MEDYCZNA:                                   ║");
            Console.WriteLine("  ║  Losowo wybrana porada na dziś.                     ║");
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

        public static void DisplayAdminUsersMenu()
        {
            DisplayUserInfo();
            Console.WriteLine("  ╔═════════════════════════════════════════════════════╗");
            Console.WriteLine("  ║  UŻYTKOWNICY:                                       ║");
            Console.WriteLine("  ║  Wyświetlono listę zarejestrowanych użytkowników.   ║");
            Console.WriteLine("  ╚═════════════════════════════════════════════════════╝\n");
        }

        public static void DisplayAdminDoctorsMenu()
        {
            DisplayUserInfo();
            Console.WriteLine("  ╔═════════════════════════════════════════════════════╗");
            Console.WriteLine("  ║  DOSTĘPNI LEKARZE:                                  ║");
            Console.WriteLine("  ║  Wyświetlono listę dostępnych lekarzy, co chcesz    ║");
            Console.WriteLine("  ║  zrobić?                                            ║");
            Console.WriteLine("  ╠═════════════════════════════════════════════════════╣");
            Console.WriteLine("  ║    1    - Dodaj lekarza                             ║");
            Console.WriteLine("  ║    2    - Edytuj lekarza                            ║");
            Console.WriteLine("  ║    3    - Usuń lekarza                              ║");
            Console.WriteLine("  ║    back - Powrót                                    ║");
            Console.WriteLine("  ╚═════════════════════════════════════════════════════╝\n");
        }

        public static void DisplayAdminAddDoctorMenu()
        {
            DisplayUserInfo();
            Console.WriteLine("  ╔═════════════════════════════════════════════════════╗");
            Console.WriteLine("  ║  DODAWANIE LEKARZA:                                 ║");
            Console.WriteLine("  ║  Dodawanie lekarza w toku... postępuj zgodnie z     ║");
            Console.WriteLine("  ║  wyświetlanymi instrukcjami.                        ║");
            Console.WriteLine("  ╚═════════════════════════════════════════════════════╝\n");
        }

        public static void DisplayAdminEditDoctorMenu()
        {
            DisplayUserInfo();
            Console.WriteLine("  ╔═════════════════════════════════════════════════════╗");
            Console.WriteLine("  ║  EDYCJA LEKARZA:                                    ║");
            Console.WriteLine("  ║  Edycja lekarza w toku... postępuj zgodnie z        ║");
            Console.WriteLine("  ║  wyświetlanymi instrukcjami.                        ║");
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

        public static void ConfirmationVoteMenu()
        {
            Console.WriteLine("  ╔═════════════════════════════════════════════════════╗");
            Console.WriteLine("  ║  OCENA PORADY MEDYCZNEJ:                            ║");
            Console.WriteLine("  ╠═════════════════════════════════════════════════════╣");
            Console.WriteLine("  ║  Jak oceniasz przydatność powyższej porady?         ║");
            Console.WriteLine("  ║  1 - lubię to   2 - nie lubię   3 - nie mam zdania  ║"); 
            Console.WriteLine("  ╚═════════════════════════════════════════════════════╝\n");
        }

        public static void ConfirmationDoctorDeletionMenu(int doctorIndex)
        {
            Console.WriteLine("  ╔═════════════════════════════════════════════════════╗");
            Console.WriteLine("  ║  POTWIERDZENIE USUNIĘCIA [" + doctorIndex + "]:                       ║");
            Console.WriteLine("  ╠═════════════════════════════════════════════════════╣");
            Console.WriteLine("  ║  Czy na pewno chcesz usunąć tego lekarza?           ║");
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

        public static void AddSpaceBeforeNumber(int number, int limit)
        {
            if (number.ToString().Length < limit) ManyChars(' ', limit - number.ToString().Length);
            Console.Write(number + "\n");
        }
    }
}
