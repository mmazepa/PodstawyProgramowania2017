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
            int pageNumber = 0;

            doctorChoosing = true;
            while (doctorChoosing)
            {
                int pages = (int)Convert.ToDecimal(Math.Ceiling((decimal)Mock._doctors.Count / 3));
                int doctorIndex = pageNumber * 3;

                Console.Clear();
                MenuManager.DisplayLogoAndMenu("doctorsMenu", "pay", Mock._doctors.Count);

                Console.WriteLine("  Lista lekarzy:                     STRONA: " + (pageNumber + 1) + "/" + pages);
                Mock._doctors.Sort((x, y) => x.Id.CompareTo(y.Id));

                if (Mock._doctors.Count > 0)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if(doctorIndex + i < Mock._doctors.Count)
                            Doctor.DisplayDoctor(doctorIndex + i);
                    }
                }
                else
                {
                    Console.WriteLine("  Brak lekarzy do wyświetlenia!");
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
                else if (wpis == "prev")
                {
                    if (pageNumber > 0)
                    {
                        pageNumber--;
                        MenuManager.InfoAlert("  INFO: Wybrano poprzednią stronę!\n");
                    }
                    else
                        MenuManager.InfoAlert("  INFO: To jest pierwsza strona, nie da się cofnąć!\n");
                }
                else if (wpis == "next")
                {
                    if (pageNumber < pages - 1)
                    {
                        pageNumber++;
                        MenuManager.InfoAlert("  INFO: Wybrano następną stronę!\n");
                    }
                    else
                        MenuManager.InfoAlert("  INFO: To jest ostatnia strona, nie da się iść dalej!\n");
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
                Mock.loggedUser.Visits.Sort((x, y) => x.DoctorId.CompareTo(y.DoctorId));

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
                        foreach (UserVisit visit in Mock.loggedUser.Visits)
                        {
                            if (visit.DoctorId == doctor.Id)
                            {
                                Doctor visitedDoctor = doctor;
                                Console.Write("\n    " + i + ") " + visitedDoctor.Price + " zł x " + visit.VisitsTaken + " = ");

                                int sum = visitedDoctor.Price * visit.VisitsTaken;
                                if (sum < 10) Console.Write("   " + sum + " zł");
                                else if (sum < 100) Console.Write("  " + sum + " zł");
                                else if (sum < 1000) Console.Write(" " + sum + " zł");

                                Console.Write("    << " + visitedDoctor.Name + " [" + visitedDoctor.Specialisation + "] >>");
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
                    if (Mock.loggedUser.Visits[number - 1].VisitsTaken > 0)
                    {
                        MenuManager.ConfirmationMenu(number - 1, "resign");
                        Console.Write("  Odpowiedź: ");
                        wpis = Console.ReadLine();

                        switch (wpis)
                        {
                            case "1":
                                foreach (Doctor doctor in Mock._doctors)
                                {
                                    if (Mock.loggedUser.Visits.Count > number - 1)
                                    {
                                        if (doctor.Id == Mock.loggedUser.Visits[number - 1].DoctorId)
                                        {
                                            ChangeAmountOfMoney(Mock.loggedUser.Login, doctor.Price);
                                            doctor.VisitsTaken--;
                                            ModifyUserVisits(Mock.loggedUser.Login, doctor.Id, -1);
                                        }
                                    }
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
                        MenuManager.InfoAlert("  INFO: Nie masz więcej wizyt, z których możesz zrezygnować!\n");
                    }
                }
                else if (wpis == "doc")
                {
                    string visits = string.Empty;
                    int sum = 0;

                    foreach(UserVisit visit in Mock.loggedUser.Visits)
                    {
                        foreach(Doctor doctor in Mock._doctors)
                        {
                            if(doctor.Id == visit.DoctorId)
                            {
                                sum = sum + (doctor.Price * visit.VisitsTaken);
                                visits = visits + doctor.Price + " zł x " + visit.VisitsTaken + " = " + (doctor.Price * visit.VisitsTaken) + " zł   << " + doctor.Name + ", " + doctor.Specialisation + " >>\n";
                            }
                        }
                    }

                    visits = visits + "RAZEM: " + sum + " zł\n\n";

                    if (Mock.loggedUser.Visits.Count > 0)
                    {
                        string[] report = {
                            "ID:    " + Mock.loggedUser.Id,
                            "NAME:  " + Mock.loggedUser.Name,
                            "LOGIN: " + Mock.loggedUser.Login,
                            "DATA:  " + DateTime.Now,
                            "",
                            "WIZYTY:",
                            "",
                            visits,
                            "Dziękujemy za skorzystanie z naszych usług.\nZałoga MEDICINE S.A."
                        };
                        CreateReport(report);
                    }
                    else
                    {
                        MenuManager.InfoAlert("  INFO: Po co tworzyć raport, kiedy brak transakcji?\n");
                    }
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
            Random random = new Random();
            int adviceNumber = random.Next(0, Mock._advices.Count);
            int sum = Mock._advices[adviceNumber].Likes
                + Mock._advices[adviceNumber].Dislikes
                + Mock._advices[adviceNumber].Other;
            int limit = 5;
            bool adviceVoted = false;

            while (!adviceVoted)
            {
                Console.Clear();
                MenuManager.DisplayLogoAndMenu("adviceMenu");

                Console.WriteLine("  " + (adviceNumber + 1) + ". " + Mock._advices[adviceNumber].Name + "\n");

                foreach (string line in Mock._advices[adviceNumber].Content)
                {
                    Console.WriteLine("     " + line);
                }

                Console.Write("\n\n");
                Console.Write("     LUBIĘ TO:       ");
                MenuManager.AddSpaceBeforeNumber(Mock._advices[adviceNumber].Likes, limit);

                Console.Write("     NIE LUBIĘ:      ");
                MenuManager.AddSpaceBeforeNumber(Mock._advices[adviceNumber].Dislikes, limit);

                Console.Write("     NIE MAM ZDANIA: ");
                MenuManager.AddSpaceBeforeNumber(Mock._advices[adviceNumber].Other, limit);

                Console.WriteLine("     ---------------------");
                Console.Write("     ŁĄCZNIE:        ");
                MenuManager.AddSpaceBeforeNumber(sum, limit);

                Console.Write("\n\n");

                MenuManager.ConfirmationVoteMenu();
                Console.Write("  Odpowiedź: ");
                wpis = Console.ReadLine();

                switch (wpis)
                {
                    case "1":
                        Mock._advices[adviceNumber].Likes += 1;
                        MenuManager.InfoAlert("  INFO: Dziękujemy za pozytywną ocenę tej porady!\n");
                        adviceVoted = true;
                        break;
                    case "2":
                        Mock._advices[adviceNumber].Dislikes += 1;
                        MenuManager.InfoAlert("  INFO: Przykro nam, że Ci się nie podoba. Może któraś z pozostałych będzie lepsza?\n");
                        adviceVoted = true;
                        break;
                    case "3":
                        Mock._advices[adviceNumber].Other += 1;
                        MenuManager.InfoAlert("  INFO: Dziękujemy za wzięcie udziału w ankiecie, niezdecydowany użytkowniku!\n");
                        adviceVoted = true;
                        break;
                    default:
                        CommandNotFound(wpis);
                        break;
                }
            }
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
                Mock._doctors.Sort((x, y) => x.Id.CompareTo(y.Id));

                if (Mock._doctors.Count > 0)
                {
                    int i = 1;
                    foreach (Doctor doctor in Mock._doctors)
                    {
                        Console.WriteLine("\n    " + i + ") " + doctor.Name + ", " + doctor.Specialisation + ", " + doctor.Price + " zł/wizyta");
                        Console.WriteLine("       " + doctor.Description);
                        Console.WriteLine("       WIZYTY: " + doctor.VisitsTaken + "/" + doctor.VisitsAvailable);
                        i++;
                    }
                }
                else
                {
                    Console.WriteLine("  Brak lekarzy do wyświetlenia!");
                }

                Console.Write("\n  Odpowiedź: ");
                wpis = Console.ReadLine();

                switch (wpis)
                {
                    case "1":
                        Doctor.AddDoctor();
                        break;
                    case "2":
                        Doctor.DoctorChoser("edit");
                        break;
                    case "3":
                        Doctor.DoctorChoser("delete");
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

        public static bool CheckIfNotEmpty(string stringToCheck)
        {
            if (stringToCheck == null || stringToCheck == "")
            {
                MenuManager.ColorText("    Nie możesz zostawić tego pola pustego, musisz coś wpisać!\n", ConsoleColor.Red);
                return false;
            }
            else return true;
        }

        public static bool CheckIfNumber(string stringToCheck)
        {
            bool isNumber = int.TryParse(stringToCheck, out int number);
            if (isNumber)
            {
                MenuManager.ColorText("    To pole nie może być liczbą!\n", ConsoleColor.Red);
                return false;
            }
            else return true;
        }

        public static bool CheckString(string stringToCheck)
        {
            if (CheckIfNotEmpty(stringToCheck) && CheckIfNumber(stringToCheck)) return true;
            else return false;
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

            foreach (UserVisit visit in user.Visits)
            {
                if (visit.DoctorId == doctorId)
                {
                    visit.VisitsTaken = visit.VisitsTaken + anotherVisit;
                }
                else count++;
            }

            if (count == user.Visits.Count)
            {
                UserVisit newVisit = new UserVisit
                {
                    DoctorId = doctorId,
                    VisitsTaken = 1
                };
                user.Visits.Add(newVisit);
            }

            int deletionIndex = -1;
            for (int i = 0; i < user.Visits.Count; i++)
            {
                if(user.Visits[i].VisitsTaken == 0)
                {
                    deletionIndex = i;
                }
            }
            if(deletionIndex != -1) user.Visits.RemoveAt(deletionIndex);

            Mock.loggedUser.Visits = user.Visits;
        }

        public static void CreateReport(string[] filedata)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\report.doc";

            try
            {
                File.WriteAllLines(path, filedata);
                File.Open(path, FileMode.Open);
                MenuManager.InfoAlert("  INFO: Stworzono nowy raport w " + path + "\n");
            }
            catch
            {
                MenuManager.InfoAlert("  INFO: Nie udało się stworzyć raportu.\n        Upewnij się, że plik nie jest aktualnie otwarty.\n");
            }
        }
    }
}
