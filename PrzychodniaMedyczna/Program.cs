using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;
using PrzychodniaMedyczna.Database;
using PrzychodniaMedyczna.Other;

namespace PrzychodniaMedyczna
{
    public class Program
    {
        public static SoundPlayer player = new SoundPlayer();
        public static int countLogin = 0;
        public static int countPassw = 0;
        public static string wpis = string.Empty;

        public static void Main(string[] args)
        {    
            string login = string.Empty;
            string passw = string.Empty;
            ConsoleKeyInfo keyInfo;
            
            bool session = false;

            player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "\\appTheme.wav";
            session = true;

            while (session)
            {
                login = string.Empty;
                passw = string.Empty;
                wpis = string.Empty;
                countLogin = 0;
                countPassw = 0;

                while (wpis != "1")
                {
                    countLogin = 0;
                    countPassw = 0;

                    MenuManager.DisplayLogoAndMenu("startMenu");
                    
                    Console.Write("  Wybór: ");
                    wpis = Console.ReadLine();

                    switch (wpis)
                    {
                        case "1":
                            Console.WriteLine("");
                            MenuManager.InfoAlert("  INFO: Wybrano logowanie, zapraszamy!\n");
                            break;
                        case "exit":
                            OptionsManager.ExitConfirmation(true);
                            break;
                        default:
                            Console.WriteLine("");
                            OptionsManager.CommandNotFound(wpis);
                            break;
                    }
                }


                // === LOGOWANIE ==========================
                do
                {
                    MenuManager.DisplayLogoAndMenu("loginMenu");

                    while (countLogin < 3)
                    {
                        Console.Write("  [" + (countLogin + 1) + "/3] Login: ");
                        login = Console.ReadLine();

                        if (Mock.UserExistFinal(login))
                        {
                            countLogin = 3;
                            while (countPassw < 3)
                            {
                                Console.Write("  [" + (countPassw + 1) + "/3] Hasło: ");
                                do
                                {
                                    keyInfo = Console.ReadKey(true);
                                    if (keyInfo.Key != ConsoleKey.Backspace && keyInfo.Key != ConsoleKey.Enter)
                                    {
                                        passw += keyInfo.KeyChar;
                                        Console.Write("*");
                                    }
                                    else if (keyInfo.Key == ConsoleKey.Backspace && passw.Length > 0)
                                    {
                                        passw = passw.Substring(0, (passw.Length - 1));
                                        Console.Write("\b \b");
                                    }
                                }
                                while (keyInfo.Key != ConsoleKey.Enter);

                                if (Mock.CorrectCredentials(login, passw))
                                {
                                    countPassw = 3;
                                    OptionsManager.loggedIn = true;
                                    player.PlayLooping();
                                }
                                else
                                {
                                    MenuManager.ColorText("\n  Hasło nieprawidłowe!\n\n", ConsoleColor.Red);
                                    countPassw++;
                                    OptionsManager.loggedIn = false;
                                }
                            }
                        }
                        else
                        {
                            MenuManager.ColorText("  Login nieprawidłowy!\n\n", ConsoleColor.Red);
                            countLogin++;
                        }

                        if((countLogin >= 3 || countPassw >= 3) && !OptionsManager.loggedIn)
                        {
                            MenuManager.ColorText("  Trzy próby zakończone niepowodzeniem, do widzenia!\n", ConsoleColor.Red);
                            MenuManager.ClearScreen();
                            break;
                        }
                    }

                    // === KONIEC LOGOWANIA ====================
                    // === MENU GŁÓWNE =========================

                    while (OptionsManager.loggedIn)
                    {
                        MenuManager.DisplayLogoAndMenu("mainMenu");

                        Console.Write("  Wybór: ");
                        wpis = Console.ReadLine();
                        Console.WriteLine("");

                        // === OPCJE UŻYTKOWNIKA ==========================
                        if (Mock.userType == "User")
                        {
                            switch (wpis)
                            {
                                case "1":
                                    OptionsManager.DoctorsList();
                                    break;
                                case "2":
                                    OptionsManager.VisitsList();
                                    break;
                                case "3":
                                    OptionsManager.AdvicesList();
                                    break;
                                case "4":
                                    OptionsManager.PharmaciesList();
                                    break;
                                case "exit":
                                    OptionsManager.ExitConfirmation(false);
                                    break;
                                default:
                                    OptionsManager.CommandNotFound(wpis);
                                    break;
                            }
                        }

                        // === OPCJE ADMINISTRATORA =======================
                        else if (Mock.userType == "Administrator")
                        {
                            switch (wpis)
                            {
                                case "1":
                                    OptionsManager.AdminUsersList();
                                    break;
                                case "2":
                                    OptionsManager.AdminDoctorsList();
                                    break;
                                case "exit":
                                    OptionsManager.ExitConfirmation(false);
                                    break;
                                default:
                                    OptionsManager.CommandNotFound(wpis);
                                    break;
                            }
                        }
                    }
                } while (countLogin < 3 && countPassw < 3 && OptionsManager.loggedIn);
            }
        }
    }
}
