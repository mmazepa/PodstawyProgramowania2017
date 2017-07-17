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
        public static bool doctorManager = false;
        public static string wpis = string.Empty;

        public static void DoctorsList()
        {
            doctorChoosing = true;
            while (doctorChoosing)
            {
                Console.Clear();
                MenuManager.DisplayLogoAndMenu("doctorsMenu", "pay", Mock._doctors.Count);

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

                        switch (wpis)
                        {
                            case "1":
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
                                break;
                            case "2":
                                MenuManager.InfoAlert("  INFO: Transakcja anulowana na życzenie użytkownika!\n");
                                break;
                            default:
                                MenuManager.InfoAlert("  INFO: Komenda nieznana, na wszelki wypadek transakcja anulowana!\n");
                                break;
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
            visitResignation = true;
            while (visitResignation)
            {
                Console.Clear();
                MenuManager.DisplayLogoAndMenu("doctorsMenu", "resign", Mock.loggedUser.Visits.Count);

                Console.WriteLine("\n  Lista zarezerwowanych wizyt:");

                int i = 1;
                int inAll = 0;

                if (Mock.loggedUser.Visits.Count == 0)
                {
                    Console.WriteLine("\n    Brak wizyt do wyświetlenia!");
                }
                else
                {
                    foreach (Doctor doctor in Mock._doctors)
                    {
                        int visits = 0;
                        foreach (UserVisit visit in Mock.loggedUser.Visits)
                        {
                            if (visit.DoctorId == doctor.Id)
                            {
                                Doctor visitedDoctor = doctor;
                                Console.Write("\n    " + i + ") " + visitedDoctor.Price + " zł x " + visitedDoctor.VisitsTaken + " = ");
                                visits = visit.VisitsTaken;

                                int sum = doctor.Price * visits;
                                if (sum < 10) Console.Write("   " + sum + " zł");
                                else if (sum < 100) Console.Write("  " + sum + " zł");
                                else if (sum < 1000) Console.Write(" " + sum + " zł");

                                Console.Write("    << " + doctor.Name + " [" + doctor.Specialisation + "] >>");
                                i++;
                                inAll += sum;
                            }
                        }
                    }
                }
                Console.Write("\n\n");

                if (inAll < 10) Console.WriteLine("    RAZEM:             " + inAll + " zł");
                else if (inAll < 100) Console.WriteLine("    RAZEM:            " + inAll + " zł");
                else if (inAll < 1000) Console.WriteLine("    RAZEM:           " + inAll + " zł");

                Console.Write("\n\n  Wybierz wizytę: ");
                wpis = Console.ReadLine();

                bool isNumeric = int.TryParse(wpis, out int number);
                if (isNumeric && Enumerable.Range(1, Mock.loggedUser.Visits.Count).Contains(number))
                {
                    if (Mock._doctors[number - 1].VisitsTaken > 0)
                    {
                        MenuManager.ConfirmationMenu(number - 1, "resign");
                        Console.Write("  Odpowiedź: ");
                        wpis = Console.ReadLine();

                        switch (wpis)
                        {
                            case "1":
                                ChangeAmountOfMoney(Mock.loggedUser.Login, Mock._doctors[number - 1].Price);
                                Mock._doctors[number - 1].VisitsTaken--;
                                ModifyUserVisits(Mock.loggedUser.Login, Mock._doctors[number - 1].Id, -1);
                                break;
                            case "2":
                                MenuManager.InfoAlert("  INFO: Transakcja anulowana na życzenie użytkownika!\n");
                                break;
                            default:
                                MenuManager.InfoAlert("  INFO: Komenda nieznana, na wszelki wypadek transakcja anulowana!\n");
                                break;
                        }
                    }
                    else
                    {
                        MenuManager.InfoAlert("  INFO: Nie masz więcej wizyt, z których możesz zrezygnować!\n");
                    }
                }
                else if (wpis == "doc")
                {
                    string[] visits = null;
                    foreach(UserVisit visit in Mock.loggedUser.Visits)
                    {
                        //visits = 
                    }

                    string[] report = {
                        "ID:    " + Mock.loggedUser.Id,
                        "NAME:  " + Mock.loggedUser.Name,
                        "LOGIN: " + Mock.loggedUser.Login,
                        "DATA:  " + DateTime.Now

                    };
                    CreateReport(report);
                }
                else if (wpis == "back")
                {
                    MenuManager.InfoAlert("  INFO: Wybrano powrót do poprzedniego ekranu.\n");
                    visitResignation = false;
                }
                else
                {
                    MenuManager.InfoAlert("  INFO: Podanej wizyty nie ma na liście!\n");
                }
            }
        }

        public static void AdvicesList()
        {
            Console.Clear();
            MenuManager.DisplayLogoAndMenu("adviceMenu");

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
            Console.Clear();
            MenuManager.DisplayLogoAndMenu("pharmacyMenu");

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
                    Program.countLogin = 0;
                    Program.countPassw = 0;
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

        public static void AdminUsersList()
        {
            Console.Clear();
            MenuManager.DisplayLogoAndMenu("adminUsersMenu");

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

        public static void AdminDoctorsList()
        {
            doctorManager = true;
            while (doctorManager)
            {
                Console.Clear();
                MenuManager.DisplayLogoAndMenu("adminDoctorsMenu");

                Console.WriteLine("  Lista lekarzy:");
                int i = 1;
                foreach (Doctor doctor in Mock._doctors)
                {
                    Console.WriteLine("\n    " + i + ") " + doctor.Name + ", " + doctor.Specialisation + ", " + doctor.Price + " zł/wizyta");
                    Console.WriteLine("       " + doctor.Description);
                    Console.WriteLine("       " + doctor.VisitsTaken + "/" + doctor.VisitsAvailable);
                    i++;
                }

                Console.Write("\n  Odpowiedź: ");
                wpis = Console.ReadLine();

                switch (wpis)
                {
                    case "1":
                        AddDoctor();
                        break;
                    case "back":
                        MenuManager.InfoAlert("  INFO: Wybrano powrót do poprzedniego ekranu.\n");
                        doctorManager = false;
                        break;
                    default:
                        CommandNotFound(wpis);
                        break;
                }
            }
        }

        public static void AddDoctor()
        {
            string firstName = string.Empty;
            string lastName = string.Empty;
            string specialisation = string.Empty;
            string price = string.Empty;
            int parsedPrice = 0;
            string description = string.Empty;
            string visitsAvailable = string.Empty;
            int parsedVisitsAvailable = 0;

            Console.Clear();
            MenuManager.DisplayLogoAndMenu("adminAddDoctorMenu");

            bool doctorEditor = true;
            while (doctorEditor)
            {
                Console.Write("    IMIĘ:           ");
                firstName = Console.ReadLine();
                if(CheckIfNotEmpty(firstName) && StringValidation(firstName,3)) doctorEditor = false;
            }

            doctorEditor = true;
            while (doctorEditor)
            {
                Console.Write("    NAZWISKO:       ");
                lastName = Console.ReadLine();
                if (CheckIfNotEmpty(lastName) && StringValidation(lastName,3)) doctorEditor = false;
            }

            doctorEditor = true;
            while (doctorEditor)
            {
                Console.Write("    SPECJALIZACJA:  ");
                specialisation = Console.ReadLine();
                if (CheckIfNotEmpty(specialisation) && StringValidation(specialisation,5)) doctorEditor = false;
            }

            doctorEditor = true;
            while (doctorEditor)
            {
                Console.Write("    CENA ZA WIZYTĘ: ");
                price = Console.ReadLine();

                if (int.TryParse(price, out parsedPrice))
                {
                    if (parsedPrice > 0) doctorEditor = false;
                    else MenuManager.ColorText("    Podana liczba musi być większa od zera!\n", ConsoleColor.Red);
                }
                else
                {
                    MenuManager.ColorText("    Musisz podać liczbę!\n", ConsoleColor.Red);
                }
            }

            doctorEditor = true;
            while (doctorEditor)
            {
                Console.Write("    OPIS:\n    ");
                description = Console.ReadLine();
                if (CheckIfNotEmpty(description) && StringValidation(description,15)) doctorEditor = false;
            }

            doctorEditor = true;
            while (doctorEditor)
            {
                Console.Write("    WIZYTY:         ");
                visitsAvailable = Console.ReadLine();

                if (int.TryParse(visitsAvailable, out parsedVisitsAvailable))
                {
                    if (parsedVisitsAvailable > 0) doctorEditor = false;
                    else MenuManager.ColorText("    Podana liczba musi być większa od zera!\n", ConsoleColor.Red);
                }
                else
                {
                    MenuManager.ColorText("    Musisz podać liczbę!\n", ConsoleColor.Red);
                }
            }

            Console.WriteLine("");

            Console.WriteLine("  DODAWANIE DOKTORA ZAKOŃCZONE!");
            Console.WriteLine("  WSZYSTKO OKEJ?\n");

            Doctor newDoctor = new Doctor()
            {
                Id = Guid.NewGuid().ToString(),
                Name = firstName + " " + lastName,
                Specialisation = specialisation,
                Price = parsedPrice,
                Description = description,
                VisitsAvailable = parsedVisitsAvailable,
                VisitsTaken = 0
            };

            Console.WriteLine("    IMIĘ I NAZWISKO: dr " + newDoctor.Name);
            Console.WriteLine("    SPECJALIZACJA:   " + newDoctor.Specialisation);
            Console.WriteLine("    CENA:            " + newDoctor.Price + " zł/wizyta");
            Console.Write("    OPIS:\n    " + newDoctor.Description + "\n");
            Console.WriteLine("    WIZYTY:          " + newDoctor.VisitsTaken + "/" + newDoctor.VisitsAvailable + "\n");

            Mock._doctors.Add(newDoctor);

            MenuManager.ColorText("  SUKCES: Dodawanie lekarza zakończone pomyślnie!\n", ConsoleColor.Green);
            MenuManager.ClearScreen();
        }

        public static bool CheckIfNotEmpty(string stringToCheck)
        {
            if (stringToCheck == null || stringToCheck == "")
            {
                MenuManager.ColorText("    Nie możesz zostawić tego pola pustego, musisz coś wpisać!\n", ConsoleColor.Red);
                return false;
            }
            else return true;
        }

        public static bool StringValidation(string stringToCheck, int requiredLength)
        {
            if (stringToCheck.Length < requiredLength)
            {
                MenuManager.ColorText("    To pole musi być dłuższe, minimum znaków: " + requiredLength + "\n", ConsoleColor.Red);
                return false;
            }
            else return true;
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
            int count = 0;
            UserVisit visitToRemove = new UserVisit();

            foreach (UserVisit visit in user.Visits)
            {
                if (visit.DoctorId == doctorId)
                {
                    visit.VisitsTaken = visit.VisitsTaken + anotherVisit;
                }
                else count++;

                if (visit.VisitsTaken == 0) visitToRemove = visit;
            }

            //user.Visits.Remove(visitToRemove);

            if (count == user.Visits.Count)
            {
                UserVisit newVisit = new UserVisit
                {
                    DoctorId = doctorId,
                    VisitsTaken = 1
                };
                user.Visits.Add(newVisit);
            }
            Mock.loggedUser.Visits = user.Visits;
        }

        public static void CreateReport(string[] filedata)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\report.doc";

            try
            {
                File.WriteAllLines(path, filedata);
                File.Open(path, FileMode.Open);
                MenuManager.InfoAlert("  INFO: Stworzono nowy raport.\n");
            }
            catch
            {
                MenuManager.InfoAlert("  INFO: Nie udało się stworzyć raportu.\n        Upewnij się, że plik nie jest aktualnie otwarty.\n");
            }
        }
    }
}
