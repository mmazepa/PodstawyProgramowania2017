using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;
using PrzychodniaMedyczna.Database;
using PrzychodniaMedyczna.Other;

namespace PrzychodniaMedyczna
{
    class Program
    {
        public static void Main(string[] args)
        {
            int count = 0;
            string login = string.Empty;
            string passw = string.Empty;
            ConsoleKeyInfo keyInfo;
            string wpis = string.Empty;

            MenuManager.DisplayLogo();
            MenuManager.LoggingInMenu();

            SoundPlayer player = new SoundPlayer();
            player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "\\appTheme.wav";

            do
            {
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

                    if(Mock.CorrectCredentials(login, passw))
                    {
                        OptionsManager.loggedIn = true;
                        player.Play();
                    }
                    else
                    {
                        MenuManager.ClearScreen();
                        OptionsManager.loggedIn = false;
                    }

                    while (OptionsManager.loggedIn)
                    {
                        MenuManager.DisplayLogo();
                        MenuManager.DisplayMenu();
                        

                        Console.Write("  Wybór: ");
                        wpis = Console.ReadLine();
                        Console.WriteLine("");

                        if (wpis == "1" && Mock.userType == "User")
                            OptionsManager.MainMenuOption1();
                        else if (wpis == "2" && Mock.userType == "User")
                            OptionsManager.MainMenuOption2();
                        else if (wpis == "3" && Mock.userType == "User")
                            OptionsManager.MainMenuOption3();
                        else if(wpis == "4" && Mock.userType == "User")
                            OptionsManager.MainMenuOption4();
                        else if (wpis == "1" && Mock.userType != "User")
                            OptionsManager.MainMenuOption5("sorry");
                        else if (wpis == "exit")
                            OptionsManager.MainMenuOption5("exit");
                        else
                            OptionsManager.MainMenuOption5("error", wpis);
                    }
                }
                else
                {
                    MenuManager.ColorText("  Użytkownik nie istnieje!\n\n", ConsoleColor.Red);
                    count++;
                    if (count == 3 && !OptionsManager.loggedIn)
                    {
                        MenuManager.ColorText("  Trzy próby zakończone niepowodzeniem, do widzenia.\n", ConsoleColor.Red);
                        MenuManager.ClearScreen();
                    }
                }
            } while (count < 3);
        }
    }
}
