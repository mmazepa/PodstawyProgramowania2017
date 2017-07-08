using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrzychodniaMedyczna.Model;
using PrzychodniaMedyczna.Other;

namespace PrzychodniaMedyczna.Database
{
    public static class Mock
    {
        public static User loggedUser;
        public static string userType = string.Empty;

        public static ConsoleColor color;

        public static List<User> _users = new List<User>()
        {
            new User()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Mariusz",
                Login = "mmazepa",
                Passw = "123",
                IsAdmin = false,
                Wallet = 500,
                Visits = new List<UserVisit>()
            },

            new User()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Zygfryd",
                Login = "zygfryd",
                Passw = "123",
                IsAdmin = false,
                Wallet = 500,
                Visits = new List<UserVisit>()
            },

            new User()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Stanisław",
                Login = "stasiek",
                Passw = "123",
                IsAdmin = false,
                Wallet = 500,
                Visits = new List<UserVisit>()
            },

            new User()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Kazimierz",
                Login = "kazik",
                Passw = "123",
                IsAdmin = false,
                Wallet = 500,
                Visits = new List<UserVisit>()
            },

            new User()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Maksymilian",
                Login = "admin",
                Passw = "admin1",
                IsAdmin = true,
                Wallet = 500,
                Visits = new List<UserVisit>()
            }
        };

        public static List<Doctor> _doctors = new List<Doctor>()
        {
            new Doctor()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "dr Arnold Sercownik",
                Specialisation = "Kardiolog",
                Price = 250,
                Description = "Lekarz z 20-letnim doświadczeniem do spraw sercowych.",
                VisitsAvailable = 5,
                VisitsTaken = 0
            },

            new Doctor()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "dr Korneliusz Stopka",
                Specialisation = "Pediatra",
                Price = 100,
                Description = "Świeżo upieczony doktor, rozprawę doktorską obronił w 2015 roku.",
                VisitsAvailable = 5,
                VisitsTaken = 0
            },

            new Doctor()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "dr Stanisław Pryszczyński",
                Specialisation = "Dermatolog",
                Price = 175,
                Description = "Godny uwagi, jeśli masz problemy skórne.",
                VisitsAvailable = 5,
                VisitsTaken = 0
            }
        };

        public static List<Pharmacy> _pharmacies = new List<Pharmacy>()
        {
            new Pharmacy()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Apteka MEDIXON",
                Address = "ul. Nowogrodzka 14b, Gdańsk",
                PhoneNumber = "123-456-789",
                OpenHour = 10,
                CloseHour = 18
            },

            new Pharmacy()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Apteka DOKTOR TRAKTOR",
                Address = "ul. Specjalistyczna 24/7, Sopot",
                PhoneNumber = "987-654-321",
                OpenHour = 4,
                CloseHour = 22
            },

            new Pharmacy()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Apteka UTILIZER",
                Address = "ul. Zakończeniowa 6, Gdynia",
                PhoneNumber = "213-564-897",
                OpenHour = 0,
                CloseHour = 16
            },

            new Pharmacy()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Apteka ZWYKŁA",
                Address = "ul. Małooryginalna 7, Gdańsk",
                PhoneNumber = "321-654-987",
                OpenHour = 14,
                CloseHour = 23
            },
        };

        public static bool UserExistFinal(string login) => _users.Exists(m => m.Login == login);

        public static bool CorrectCredentials(string login, string passw)
        {
            var user = _users.FirstOrDefault(m => m.Login == login);
            if (user == null) MenuManager.ColorText("  Użytkownik nie istnieje!\n\n", ConsoleColor.Red);
            else if (user.Passw == passw)
            {
                HelloUser(user.Login);
                MenuManager.ClearScreen();
                return true;
            }
            else
            {
                MenuManager.ColorText("\n  Hasło nieprawidłowe!\n", ConsoleColor.Red);
                return false;
            }
            Program.count++;
            return false;
        }

        public static void HelloUser(string login)
        {
            var user = _users.FirstOrDefault(m => m.Login == login);

            if (!user.IsAdmin)
            {
                color = ConsoleColor.Green;
                userType = "User";
            }
            else if (user.IsAdmin)
            {
                color = ConsoleColor.Yellow;
                userType = "Administrator";
            }

            Console.Write("\n\n  Witaj, ");
            MenuManager.ColorText(user.Name, color);
            Console.Write(", zostałeś zalogowany jako ");
            MenuManager.ColorText(userType + "\n", color);

            loggedUser = user;
        }
    }
}
