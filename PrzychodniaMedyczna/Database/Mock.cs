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
                VisitsAvailable = 7,
                VisitsTaken = 0
            },

            new Doctor()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "dr Stanisław Pryszczyński",
                Specialisation = "Dermatolog",
                Price = 175,
                Description = "Godny uwagi, jeśli masz problemy skórne.",
                VisitsAvailable = 12,
                VisitsTaken = 0
            },

            new Doctor()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "dr Karol Twarzyczka",
                Specialisation = "Chirurg Plastyczny",
                Price = 300,
                Description = "Nie podoba Ci się Twój wygląd? Zapisz się już teraz!",
                VisitsAvailable = 10,
                VisitsTaken = 0
            },

            new Doctor()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "dr Mieczysław Ząbek",
                Specialisation = "Stomatolog",
                Price = 50,
                Description = "Dbanie o ludzki uśmiech to jego specjalność.",
                VisitsAvailable = 25,
                VisitsTaken = 0
            }
        };

        public static List<Advice> _advices = new List<Advice>()
        {
            new Advice()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Nie śpiesz się!",
                Content = new string[]
                {
                    "Istotą zmiany Twoich nawyków jest konsekwencja",
                    "i systematyczność. Lepiej powoli modyfikować kolejne",
                    "elementy diety niż gwałtownie zmienić całą, a po",
                    "tygodniu zniechęcony wrócić do pierwotnej. Lepiej",
                    "powoli zacząć ćwiczyć niż rzucić się na głęboką wodę",
                    "i mieć bolesne zakwasy przez pół tygodnia. Zastosuj",
                    "metodę małych kroków – ewolucja, nie rewolucja!"
                },
                Likes = 0,
                Dislikes = 0,
                Other = 0
            },

            new Advice()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Zacznij ćwiczyć!",
                Content = new string[]
                {
                    "Zanim jednak to zrobisz, uczciwie spisz ilość godzin,",
                    "które przeznaczasz na aktywność fizyczną w ciągu tygodnia.",
                    "Gdy stwierdzisz, że jest ona zbyt mała, to postaraj się",
                    "o towarzystwo mobilizujące Cię do wspólnych ćwiczeń.",
                    "Dowiedziono, że trening grupowy poprawia konsekwentność",
                    "w uprawianiu sportu. Oprócz większej motywacji, dzięki",
                    "której poprawi się Twoja kondycja fizyczna, stanowi",
                    "również okazję do nawiązania nowych kontaktów i poznania",
                    "ciekawych ludzi."
                },
                Likes = 0,
                Dislikes = 0,
                Other = 0
            },

            new Advice()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Zadbaj o odnowę swoich sił!",
                Content = new string[]
                {
                    "Nie ma prostszej rzeczy na naszej liście, bo zadbanie o dobry,",
                    "zdrowy sen nic nie kosztuje, a przynosi zbawienne efekty",
                    "dla naszego organizmu. Gdy jesteś wyspany, to masz większą",
                    "odporność na stres, jesteś bardziej opanowany, kreatywny",
                    "i masz lepszy refleks. Nie tylko masz wrażenie bycia zdrowym,",
                    "a po prostu taki jesteś."
                },
                Likes = 0,
                Dislikes = 0,
                Other = 0
            },

            new Advice()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Wypisz okoliczności złych nawyków!",
                Content = new string[]
                {
                    "Nic tak nie pomaga we wdrożeniu zdrowego trybu życia, jak",
                    "unikanie sytuacji, którym zazwyczaj towarzyszyły nieprawidłowe",
                    "zachowania zdrowotne. Zastanów się, kiedy zjadasz o jedno",
                    "lub dwa ciastka za dużo, w jakich okolicznościach wypijasz",
                    "o jedno lub dwa piwa więcej, niż powinieneś? Unikaj sytuacji",
                    "wystawiających Cię na pokusy, a doprowadzi Cię to do osiągnięcia",
                    "celu w walce z niezdrowym trybem życia."
                },
                Likes = 0,
                Dislikes = 0,
                Other = 0
            },

            new Advice()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Ogranicz nałogi!",
                Content = new string[]
                {
                    "Palenie papierosów i picie dużych ilości wysokoprocentowych",
                    "alkoholi to znane nie od dziś najgroźniejsze czynniki ryzyka",
                    "chorób cywilizacyjnych. Ilość ich negatywnych działań na zdrowie",
                    "jest ogromna! Ogranicz nałogi już dziś! Jak to zrobić?",
                    "Kieruj się naszymi poradami i bądź konsekwentny. A jeśli",
                    "ograniczenie nałogu sprawia Ci trudności mimo szczerych chęci",
                    "i podejmowanych wysiłków, to skontaktuj się z terapeutą."
                },
                Likes = 0,
                Dislikes = 0,
                Other = 0
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

            if (user == null)
            {
                MenuManager.ColorText("  Użytkownik nie istnieje!\n\n", ConsoleColor.Red);
                Program.countLogin++;
                return false;
            }
            else if (user.Passw == passw)
            {
                HelloUser(user.Login);
                MenuManager.ClearScreen();
                return true;
            }
            else
            {
                return false;
            }
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
