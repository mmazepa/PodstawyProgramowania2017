using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PrzychodniaMedyczna.Database;
using PrzychodniaMedyczna.Model;

namespace PrzychodniaMedyczna.Other
{
    public static class OptionsManager
    {
        public static bool loggedIn = false;
        public static bool doctorChoosing = false;
        public static bool visitResignation = false;
        public static string wpis = string.Empty;

        public static void DoctorsList()
        {
            Console.Title = "Przychodnia Medyczna: Wybór lekarza";
            doctorChoosing = true;
            while (doctorChoosing)
            {
                Console.Clear();
                MenuManager.DisplayLogo();
                MenuManager.DisplayDoctorMenu("pay", Mock._doctors.Count);

                Console.WriteLine("  Lista lekarzy:");
                int i = 1;
                foreach (Doctor doctor in Mock._doctors)
                {
                    Console.WriteLine("\n    " + i + ") " + doctor.Name);
                    Console.WriteLine("       SPECJALIZACJA: " + doctor.Specialisation);
                    Console.WriteLine("       CENA:          " + doctor.Price + "zł/wizyta");
                    Console.WriteLine("       OPIS:");
                    Console.WriteLine("       " + doctor.Description);
                    Console.Write("       WIZYTY:        ");
                    if (doctor.VisitsTaken == doctor.VisitsAvailable)
                    {
                        MenuManager.ColorText(doctor.VisitsTaken + "/" + doctor.VisitsAvailable + "\n", ConsoleColor.Red);
                    }
                    else if (doctor.VisitsTaken >= (float)doctor.VisitsAvailable / 2)
                    {
                        MenuManager.ColorText(doctor.VisitsTaken + "/" + doctor.VisitsAvailable + "\n", ConsoleColor.Yellow);
                    }
                    else if (doctor.VisitsTaken >= 0)
                    {
                        MenuManager.ColorText(doctor.VisitsTaken + "/" + doctor.VisitsAvailable + "\n", ConsoleColor.Green);
                    }
                    i++;
                }

                Console.Write("\n  Wybierz lekarza: ");
                wpis = Console.ReadLine();

                bool isNumeric = int.TryParse(wpis, out int number);
                if (isNumeric && Enumerable.Range(1, Mock._doctors.Count).Contains(number))
                {
                    if (Mock._doctors[number - 1].VisitsTaken < Mock._doctors[number - 1].VisitsAvailable)
                    {
                        MenuManager.ConfirmationMenu(number - 1, "pay");
                        Console.Write("  Odpowiedź: ");
                        wpis = Console.ReadLine();

                        if (wpis == "1")
                        {
                            if (Mock.loggedUser.Wallet >= Mock._doctors[number - 1].Price)
                            {
                                ChangeAmountOfMoney(Mock.loggedUser.Login, -Mock._doctors[number - 1].Price);
                                Mock._doctors[number - 1].VisitsTaken++;
                                ModifyUserVisits(Mock.loggedUser.Login, Mock._doctors[number - 1].Id, 1);
                            }
                            else
                            {
                                MenuManager.InfoAlert("  INFO: Nie stać Cię na tę wizytę!\n");
                            }
                        }
                        else if (wpis == "2")
                        {
                            MenuManager.InfoAlert("  INFO: Transakcja anulowana na życzenie użytkownika!\n");
                        }
                        else
                        {
                            MenuManager.InfoAlert("  INFO: Komenda nieznana, na wszelki wypadek transakcja anulowana!\n");
                        }
                    }
                    else
                    {
                        MenuManager.InfoAlert("  INFO: Ten lekarz nie ma więcej dostępnych wizyt!\n");
                    }
                }
                else if (wpis == "back")
                {
                    MenuManager.InfoAlert("  INFO: Wybrano powrót do poprzedniego ekranu.\n");
                    doctorChoosing = false;
                }
                else if (!Enumerable.Range(1, Mock._doctors.Count).Contains(number))
                {
                    MenuManager.InfoAlert("  INFO: Podanego lekarza nie ma na liście!\n");
                }
            }
        }

        public static void VisitsList()
        {
            Console.Title = "Przychodnia Medyczna: Rachunek/Rezygnacja z wizyty";
            visitResignation = true;
            while (visitResignation)
            {
                Console.Clear();
                MenuManager.DisplayLogo();
                MenuManager.DisplayDoctorMenu("resign", Mock._doctors.Count);

                Console.WriteLine("\n  Lista zarezerwowanych wizyt:");

                int i = 1;
                int inAll = 0;
                foreach (Doctor doctor in Mock._doctors)
                {
                    int visits = 0;
                    foreach (UserVisit visit in Mock.loggedUser.Visits)
                    {
                        Console.Write("\n    " + i + ") " + doctor.Price + " zł x " + visit.VisitsTaken + " = ");
                        visits = visit.VisitsTaken;
                    }

                    int sum = doctor.Price * visits;
                    if (sum < 10) Console.Write("   " + sum + " zł");
                    else if (sum < 100) Console.Write("  " + sum + " zł");
                    else if (sum < 1000) Console.Write(" " + sum + " zł");

                    Console.Write("    << " + doctor.Name + " [" + doctor.Specialisation + "] >>");
                    i++;
                    inAll += sum;
                }
                Console.Write("\n\n");

                if (inAll < 10) Console.WriteLine("    RAZEM:             " + inAll + " zł");
                else if (inAll < 100) Console.WriteLine("    RAZEM:            " + inAll + " zł");
                else if (inAll < 1000) Console.WriteLine("    RAZEM:           " + inAll + " zł");

                Console.Write("\n\n  Wybierz wizytę: ");
                wpis = Console.ReadLine();

                bool isNumeric = int.TryParse(wpis, out int number);
                if (isNumeric && Enumerable.Range(1, Mock._doctors.Count).Contains(number))
                {
                    if (Mock._doctors[number - 1].VisitsTaken > 0)
                    {
                        MenuManager.ConfirmationMenu(number - 1, "resign");
                        Console.Write("  Odpowiedź: ");
                        wpis = Console.ReadLine();

                        if (wpis == "1")
                        {
                            ChangeAmountOfMoney(Mock.loggedUser.Login, Mock._doctors[number - 1].Price);
                            Mock._doctors[number - 1].VisitsTaken--;
                            ModifyUserVisits(Mock.loggedUser.Login, Mock._doctors[number - 1].Id, -1);
                        }
                        else if (wpis == "2")
                        {
                            MenuManager.InfoAlert("  INFO: Transakcja anulowana na życzenie użytkownika!\n");
                        }
                        else
                        {
                            MenuManager.InfoAlert("  INFO: Komenda nieznana, na wszelki wypadek transakcja anulowana!\n");
                        }
                    }
                    else
                    {
                        MenuManager.InfoAlert("  INFO: Nie masz więcej wizyt, z których możesz zrezygnować!\n");
                    }
                }
                else if (wpis == "pdf")
                {
                    string[] report = {
                        "Plepleple...",
                        "Plepleple2..."
                    };
                    CreateReport(report);
                    MenuManager.InfoAlert("  INFO: Stworzono nowy raport.\n");
                }
                else if (wpis == "back")
                {
                    MenuManager.InfoAlert("  INFO: Wybrano powrót do poprzedniego ekranu.\n");
                    visitResignation = false;
                }
                else if (!Enumerable.Range(1, Mock._doctors.Count).Contains(number))
                {
                    MenuManager.InfoAlert("  INFO: Podanej wizyty nie ma na liście!\n");
                }
            }
        }

        public static void AdvicesList()
        {
            Console.Title = "Przychodnia Medyczna: Porada medyczna";

            Console.Clear();
            MenuManager.DisplayLogo();
            MenuManager.DisplayAdviceMenu();

            Console.WriteLine("  .     |___________________________________");
            Console.WriteLine("  | -----|- - -|''''|''''|''''|''''|''''|'##\\|__");
            Console.WriteLine("  | - -  |  cc 6    5    4    3    2    1 ### __]==----------------------");
            Console.WriteLine("  | -----|________________________________##/|");
            Console.WriteLine("  '     |'''''''''''''''''''''''''''''''''`'\n");

            MenuManager.ColorText("  Chwilowo brak porady medycznej, zapraszamy później!\n\n", ConsoleColor.Red);
            MenuManager.ClearScreen();
        }

        public static void PharmaciesList()
        {
            Console.Title = "Przychodnia Medyczna: Apteki";

            Console.Clear();
            MenuManager.DisplayLogo();
            MenuManager.DisplayPharmacyMenu();

            Console.WriteLine("\n  Lista Aptek:");

            int i = 1;
            foreach (Pharmacy pharmacy in Mock._pharmacies)
            {
                Console.WriteLine("\n    " + i + ") " + pharmacy.Name);
                Console.WriteLine("       ADRES:            " + pharmacy.Address);
                Console.WriteLine("       TELEFON:          " + pharmacy.PhoneNumber);
                Console.WriteLine("       GODZINY OTWARCIA: " + pharmacy.OpenHour + ":00-" + pharmacy.CloseHour + ":00");

                if (Enumerable.Range(pharmacy.OpenHour, pharmacy.CloseHour - pharmacy.OpenHour).Contains<int>(DateTime.Now.Hour))
                    MenuManager.ColorText("       OTWARTE!\n", ConsoleColor.Green);
                else
                    MenuManager.ColorText("       ZAMKNIĘTE!\n", ConsoleColor.Red);
                i++;
            }
            Console.Write("\n\n");
            MenuManager.ClearScreen();
        }

        public static void ExitConfirmation(bool exit)
        {
            if(!exit) MenuManager.ConfirmationExitMenu("logout");
            else if (exit) MenuManager.ConfirmationExitMenu("exit");

            Console.Write("  Wybór: ");
            wpis = Console.ReadLine();

            if (wpis == "1")
            {
                if (!exit)
                {
                    loggedIn = false;
                    MenuManager.InfoAlert("  INFO: Dziękujemy za skorzystanie z naszych usług. Do widzenia!\n");
                    Program.player.Stop();
                    Program.count = 0;
                    Program.wpis = string.Empty;
                }
                else if (exit)
                {
                    MenuManager.InfoAlert("  INFO: Wybrano wyjście, do widzenia!\n");
                    Environment.Exit(0);
                }
            }
            else if (wpis == "2")
            {
                MenuManager.InfoAlert("  INFO: Anulowano na życzenie użytkownika!\n");
            }
            else
            {
                MenuManager.InfoAlert("  INFO: Komenda nierozpoznana, na wszelki wypadek anulowano!\n");
            }
        }

        public static void CommandNotFound(string wpis)
        {
            MenuManager.InfoAlert("  INFO: Komenda '" + wpis + "' nie została rozpoznana i nie może zostać zrealizowana.\n        Zapoznaj się z menu wyboru i spróbuj ponownie.\n");
        }

        public static void UsersList()
        {
            Console.Clear();
            MenuManager.DisplayLogo();
            MenuManager.DisplayUsersMenu();

            int i = 0;
            string isAdmin = string.Empty;

            Console.WriteLine("  Lista użytkowników:\n");					

            Console.WriteLine("    ┌───────────┬───────────┬───────────┬───────────┬───────────┬───────────┐");
            Console.WriteLine("    │ L.P.      │ IMIĘ      │ LOGIN     │ HASŁO     │ ADMIN?    │ KONTO     │");
            Console.WriteLine("    ├───────────┼───────────┼───────────┼───────────┼───────────┼───────────┤");

            Mock._users.Sort((x, y) => x.Name.CompareTo(y.Name));

            foreach (User user in Mock._users)
            {
                if (!user.IsAdmin)
                {
                    isAdmin = "Nie";
                }
                else if (user.IsAdmin)
                {
                    isAdmin = "Tak";
                }

                string[] userInfo = {
                    (i+1).ToString(),
                    user.Name,
                    user.Login,
                    "*****",
                    isAdmin,
                    user.Wallet.ToString()
                };

                int.TryParse(userInfo[5], out int wynik);

                if (wynik < 10) userInfo[5] = "     " + userInfo[5] + " zł";
                else if (wynik < 100) userInfo[5] = "    " + userInfo[5] + " zł";
                else if (wynik < 1000) userInfo[5] = "   " + userInfo[5] + " zł";
                else if (wynik < 10000) userInfo[5] = "  " + userInfo[5] + " zł";
                else if (wynik < 100000) userInfo[5] = " " + userInfo[5] + " zł";

                Console.Write("    ");
                foreach (string info in userInfo)
                {
                    if (info.Length <= 10) Console.Write("│ " + info);
                    else Console.Write("│ " + info.Substring(0, 7) + "...");
                    MenuManager.ManyChars(' ', 10 - info.Length);
                }
                Console.WriteLine("│");
                i++;
            }
            Console.WriteLine("    └───────────┴───────────┴───────────┴───────────┴───────────┴───────────┘");
            Console.WriteLine("");
            MenuManager.ClearScreen();
        }

        public static void ChangeAmountOfMoney(string login, int howMany)
        {
            var user = Mock._users.FirstOrDefault(m => m.Login == login);
            user.Wallet = user.Wallet + howMany;
            Mock.loggedUser.Wallet = user.Wallet;
        }

        public static void ModifyUserVisits(string login, string doctorId, int anotherVisit)
        {
            var user = Mock._users.FirstOrDefault(m => m.Login == login);

            if (user.Visits != null) {
                foreach (UserVisit visit in user.Visits)
                {
                    if (visit.DoctorId == doctorId)
                    {
                        visit.VisitsTaken = visit.VisitsTaken + anotherVisit;
                    }
                }

                foreach (UserVisit visit in Mock.loggedUser.Visits)
                {
                    if (visit.DoctorId == doctorId)
                    {
                        visit.VisitsTaken = visit.VisitsTaken + anotherVisit;
                    }
                }
            }
            else
            {
                user.Visits.Add(new UserVisit { DoctorId = doctorId, VisitsTaken = anotherVisit });
                Mock.loggedUser.Visits.Add(new UserVisit { DoctorId = doctorId, VisitsTaken = anotherVisit });
            }
        }

        public static void CreateReport(string[] filedata)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\report.pdf";
            File.WriteAllLines(path, filedata);
            File.Open(path, FileMode.Open);
        }
    }
}
