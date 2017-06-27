using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public static ConsoleColor infoColor = ConsoleColor.Cyan;

        public static void MainMenuOption1()
        {
            doctorChoosing = true;
            while (doctorChoosing)
            {
                Console.Clear();
                MenuManager.DisplayLogo();
                MenuManager.DisplayDoctorMenu("pay", Mock._doctors.Count);

                Console.WriteLine("\n  Lista lekarzy:");
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

                Console.Write("\n\n  Wybierz lekarza: ");
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
                            if (Mock.userWallet >= Mock._doctors[number - 1].Price)
                            {
                                Mock.userWallet -= Mock._doctors[number - 1].Price;
                                Mock._doctors[number - 1].VisitsTaken++;
                            }
                            else
                            {
                                MenuManager.ColorText("  INFO: Nie stać Cię na tę wizytę!\n", infoColor);
                                MenuManager.ClearScreen();
                            }
                        }
                        else if (wpis == "2")
                        {
                            MenuManager.ColorText("  INFO: Transakcja anulowana na życzenie użytkownika!\n", infoColor);
                            MenuManager.ClearScreen();
                        }
                        else
                        {
                            MenuManager.ColorText("  INFO: Komenda nieznana, na wszelki wypadek transakcja anulowana!\n", infoColor);
                            MenuManager.ClearScreen();
                        }
                    }
                    else
                    {
                        MenuManager.ColorText("  INFO: Ten lekarz nie ma więcej dostępnych wizyt!\n", infoColor);
                        MenuManager.ClearScreen();
                    }
                }
                else if (wpis == "back")
                {
                    MenuManager.ColorText("  INFO: Wybrano powrót do poprzedniego ekranu.\n", infoColor);
                    doctorChoosing = false;
                    MenuManager.ClearScreen();
                }
                else if (!Enumerable.Range(1, Mock._doctors.Count).Contains(number))
                {
                    MenuManager.ColorText("  INFO: Podanego lekarza nie ma na liście!\n", infoColor);
                    MenuManager.ClearScreen();
                }
            }
        }

        public static void MainMenuOption2()
        {
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
                    Console.Write("\n    " + i + ") " + doctor.Price + " zł x " + doctor.VisitsTaken + " = ");

                    int sum = doctor.Price * doctor.VisitsTaken;
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
                            Mock.userWallet += Mock._doctors[number - 1].Price;
                            Mock._doctors[number - 1].VisitsTaken--;
                        }
                        else if (wpis == "2")
                        {
                            MenuManager.ColorText("  INFO: Transakcja anulowana na życzenie użytkownika!\n", infoColor);
                            MenuManager.ClearScreen();
                        }
                        else
                        {
                            MenuManager.ColorText("  INFO: Komenda nieznana, na wszelki wypadek transakcja anulowana!\n", infoColor);
                            MenuManager.ClearScreen();
                        }
                    }
                    else
                    {
                        MenuManager.ColorText("  INFO: Nie masz więcej wizyt, z których możesz zrezygnować!\n", infoColor);
                        MenuManager.ClearScreen();
                    }
                }
                else if (wpis == "back")
                {
                    MenuManager.ColorText("  INFO: Wybrano powrót do poprzedniego ekranu.\n", infoColor);
                    visitResignation = false;
                    MenuManager.ClearScreen();
                }
                else if (!Enumerable.Range(1, Mock._doctors.Count).Contains(number))
                {
                    MenuManager.ColorText("  INFO: Podanej wizyty nie ma na liście!\n", infoColor);
                    MenuManager.ClearScreen();
                }
            }
        }

        public static void MainMenuOption3()
        {
            Console.WriteLine("  .     |___________________________________");
            Console.WriteLine("  | -----|- - -|''''|''''|''''|''''|''''|'##\\|__");
            Console.WriteLine("  | - -  |  cc 6    5    4    3    2    1 ### __]==----------------------");
            Console.WriteLine("  | -----|________________________________##/|");
            Console.WriteLine("  '     |'''''''''''''''''''''''''''''''''`'\n");

            MenuManager.ColorText("  Dupa tam, nie ma żadnej porady, szpryca w dupę i do widzenia!\n\n", ConsoleColor.Red);
            MenuManager.ClearScreen();
        }

        public static void MainMenuOption4()
        {
            Console.WriteLine("  Lista Aptek:");

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
            Console.WriteLine("");
            MenuManager.ClearScreen();
        }

        public static void MainMenuOption5(string option)
        {
            if (option == "sorry")
            {
                MenuManager.ColorText("  INFO: Na razie nic nie działa, sory, taki mamy klimat xD\n", infoColor);
                MenuManager.ClearScreen();
            }
            else if (option == "exit")
            {
                loggedIn = false;
                MenuManager.ColorText("  INFO: Dziękujemy za skorzystanie z naszych usług. Do widzenia!\n", infoColor);
                MenuManager.ClearScreen();
            }
        }

        public static void MainMenuOption5(string option, string wpis)
        {
            MenuManager.ColorText("  INFO: Komenda '" + wpis + "' nie została rozpoznana i nie może zostać zrealizowana.\n", infoColor);
            MenuManager.ColorText("        Zapoznaj się z menu wyboru i spróbuj ponownie.\n", infoColor);
            MenuManager.ClearScreen();
        }
    }
}
