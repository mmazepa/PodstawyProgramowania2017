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
        public static int count = 0;
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
                count = 0;

                while (wpis != "1")
                {
                    count = 0;

                    MenuManager.DisplayLogo();
                    MenuManager.StartScreenMenu();
                    Console.Title = "Przychodnia Medyczna: Ekran powitalny";

                    Console.Write("  Wybór: ");
                    wpis = Console.ReadLine();

                    if (wpis == "1")
                    {
                        Console.WriteLine("");
                        MenuManager.InfoAlert("  INFO: Wybrano logowanie, zapraszamy!\n");
                    }
                    else if (wpis == "exit")
                    {
                        OptionsManager.ExitConfirmation(true);
                    }
                    else
                    {
                        Console.WriteLine("");
                        OptionsManager.CommandNotFound(wpis);
                    }
                }

                do
                {
                    MenuManager.DisplayLogo();
                    MenuManager.LoggingInMenu();
                    Console.Title = "Przychodnia Medyczna: Logowanie";

                    Console.WriteLine("  [Próba " + (count+1) + "/3]");
                    Console.Write("  Login: ");
                    login = Console.ReadLine();

                    if (Mock.UserExistFinal(login))
                    {
                        count = 3;

                        Console.Write("  Hasło: ");
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
                            OptionsManager.loggedIn = true;
                            player.PlayLooping();
                        }
                        else
                        {
                            MenuManager.ClearScreen();
                            OptionsManager.loggedIn = false;
                        }

                        while (OptionsManager.loggedIn)
                        {
                            Console.Title = "Przychodnia Medyczna: Menu Główne";

                            MenuManager.DisplayLogo();
                            MenuManager.DisplayMenu();

                            Console.Write("  Wybór: ");
                            wpis = Console.ReadLine();
                            Console.WriteLine("");

                            // === OPCJE UŻYTKOWNIKA ==========================
                            if (Mock.userType == "User")
                            {
                                if (wpis == "1")
                                    OptionsManager.DoctorsList();
                                else if (wpis == "2")
                                    OptionsManager.VisitsList();
                                else if (wpis == "3")
                                    OptionsManager.AdvicesList();
                                else if (wpis == "4")
                                    OptionsManager.PharmaciesList();
                                else if (wpis == "exit")
                                    OptionsManager.ExitConfirmation(false);
                                else
                                    OptionsManager.CommandNotFound(wpis);
                            }

                            // === OPCJE ADMINISTRATORA =======================
                            else if (Mock.userType == "Administrator")
                            {
                                if (wpis == "1")
                                    OptionsManager.UsersList();
                                else if (wpis == "exit")
                                    OptionsManager.ExitConfirmation(false);
                                else
                                    OptionsManager.CommandNotFound(wpis);
                            }
                        }
                    }
                    else
                    {
                        MenuManager.ColorText("  Użytkownik nie istnieje!\n\n", ConsoleColor.Red);
                        MenuManager.ClearScreen();
                        count++;
                        if (count >= 3 && !OptionsManager.loggedIn)
                        {
                            MenuManager.DisplayLogo();
                            MenuManager.LoggingInMenu();

                            MenuManager.ColorText("  Trzy próby zakończone niepowodzeniem, do widzenia.\n", ConsoleColor.Red);
                            MenuManager.ClearScreen();

                            wpis = string.Empty;
                        }
                    }
                } while (count < 3 && OptionsManager.loggedIn);
            }
        }
    }
}
