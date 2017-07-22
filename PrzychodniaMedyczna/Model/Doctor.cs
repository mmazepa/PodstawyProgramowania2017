using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PrzychodniaMedyczna.Database;
using PrzychodniaMedyczna.Other;

namespace PrzychodniaMedyczna.Model
{
    public class Doctor : Resource
    {
        public string Specialisation { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public int VisitsAvailable { get; set; }
        public int VisitsTaken { get; set; }

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

            Console.WriteLine("  PROSZĘ UZUPEŁNIĆ PONIŻSZE POLA:\n");

            Object[] docInfo = CheckDoctorInfo(-1);
            firstName = docInfo[0].ToString();
            lastName = docInfo[1].ToString();
            specialisation = docInfo[2].ToString();
            parsedPrice = (int)docInfo[3];
            description = docInfo[4].ToString();
            parsedVisitsAvailable = (int)docInfo[5];

            Console.WriteLine("");

            Console.WriteLine("  DODAWANIE DOKTORA ZAKOŃCZONE!");
            Console.WriteLine("  UŻYTKOWNICY ZOBACZĄ GO W NASTĘPUJĄCY SPOSÓB:\n");

            Doctor newDoctor = new Doctor()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "dr " + firstName + " " + lastName,
                Specialisation = specialisation,
                Price = parsedPrice,
                Description = description,
                VisitsAvailable = parsedVisitsAvailable,
                VisitsTaken = 0
            };

            Console.WriteLine("    IMIĘ I NAZWISKO: " + newDoctor.Name);
            Console.WriteLine("    SPECJALIZACJA:   " + newDoctor.Specialisation);
            Console.WriteLine("    CENA:            " + newDoctor.Price + " zł/wizyta");
            Console.Write("    OPIS:\n    " + newDoctor.Description + "\n");
            Console.WriteLine("    WIZYTY:          " + newDoctor.VisitsTaken + "/" + newDoctor.VisitsAvailable + "\n");

            Mock._doctors.Add(newDoctor);
            Mock._doctors.Sort((x, y) => x.Id.CompareTo(y.Id));

            MenuManager.ColorText("  SUKCES: Dodawanie lekarza zakończone pomyślnie!\n", ConsoleColor.Green);
            MenuManager.ClearScreen();
        }

        public static void EditDoctor(int doctorIndex)
        {
            string[] name = new string[2];
            string[] specialisation = new string[2];
            int[] price = new int[2];
            string[] description = new string[2];
            int[] visitsAvailable = new int[2];

            name[0] = Mock._doctors[doctorIndex].Name;
            specialisation[0] = Mock._doctors[doctorIndex].Specialisation;
            price[0] = Mock._doctors[doctorIndex].Price;
            description[0] = Mock._doctors[doctorIndex].Description;
            visitsAvailable[0] = Mock._doctors[doctorIndex].VisitsAvailable;

            Console.Clear();
            MenuManager.DisplayLogoAndMenu("adminEditDoctorMenu");

            Console.WriteLine("  STARE DANE:\n");
            DisplayDoctorInfo(doctorIndex);

            Console.WriteLine("  NOWE DANE:\n");
            Object[] docInfo = CheckDoctorInfo(doctorIndex);
            name[1] = "dr " + docInfo[0].ToString() + " " + docInfo[1].ToString();
            specialisation[1] = docInfo[2].ToString();
            price[1] = (int)docInfo[3];
            description[1] = docInfo[4].ToString();
            visitsAvailable[1] = (int)docInfo[5];

            Console.WriteLine("");

            Console.WriteLine("  EDYTOWANIE DOKTORA ZAKOŃCZONE!");
            Console.WriteLine("  UŻYTKOWNICY ZOBACZĄ GO W NASTĘPUJĄCY SPOSÓB:\n");

            Mock._doctors[doctorIndex].Name = name[1];
            Mock._doctors[doctorIndex].Specialisation = specialisation[1];
            Mock._doctors[doctorIndex].Price = price[1];
            Mock._doctors[doctorIndex].Description = description[1];
            Mock._doctors[doctorIndex].VisitsAvailable = visitsAvailable[1];
            Mock._doctors[doctorIndex].VisitsTaken = 0;

            DisplayDoctorInfo(doctorIndex);

            MenuManager.ColorText("  SUKCES: Edycja lekarza zakończona pomyślnie!\n", ConsoleColor.Green);
            MenuManager.ClearScreen();
        }

        public static void DeleteDoctor(int doctorIndex)
        {
            string doctorName = Mock._doctors[doctorIndex].Name;
            Mock._doctors.RemoveAt(doctorIndex);

            MenuManager.ColorText("  SUKCES: Usuwanie lekarza (" + doctorName  + ") zakończone pomyślnie!\n", ConsoleColor.Green);
            MenuManager.ClearScreen();
        }

        public static void DoctorChoser(string whatToDo)
        {
            string decision = string.Empty;

            Console.Write("  Którego? ");
            decision = Console.ReadLine();

            bool success = int.TryParse(decision, out int doctorIndex);

            if (success && Mock._doctors.Count >= doctorIndex && doctorIndex >= 0)
            {
                if (whatToDo == "edit") EditDoctor(doctorIndex - 1);
                else if (whatToDo == "delete")
                {
                    if (Mock._doctors[doctorIndex - 1].VisitsTaken == 0)
                    {
                        DeleteDoctor(doctorIndex - 1);
                    }
                    else
                    {
                        MenuManager.InfoAlert("  INFO: Nie możesz usunąć lekarza, który ma zarezerwowane wizyty!\n");
                    }
                }
            }
            else MenuManager.InfoAlert("  INFO: Nie ma takiego lekarza na liście!\n");
        }

        public static Object[] CheckDoctorInfo(int doctorIndex)
        {
            string firstName = string.Empty;
            string lastName = string.Empty;
            string specialisation = string.Empty;
            string price = string.Empty;
            int parsedPrice = 0;
            string description = string.Empty;
            string visitsAvailable = string.Empty;
            int parsedVisitsAvailable = 0;

            string[] nameInfo = new string[3];

            if(doctorIndex != -1)
            {
                nameInfo = Mock._doctors[doctorIndex].Name.Split(' ');
            }

            bool doctorEditor = true;
            while (doctorEditor)
            {
                Console.Write("    IMIĘ:           ");
                if(doctorIndex != -1) SendKeys.SendWait(nameInfo[1]);
                firstName = Console.ReadLine();
                if (OptionsManager.CheckString(firstName) && OptionsManager.StringValidation(firstName, 3)) doctorEditor = false;
            }

            doctorEditor = true;
            while (doctorEditor)
            {
                Console.Write("    NAZWISKO:       ");
                if (doctorIndex != -1) SendKeys.SendWait(nameInfo[2]);
                lastName = Console.ReadLine();
                if (OptionsManager.CheckString(lastName) && OptionsManager.StringValidation(lastName, 3)) doctorEditor = false;
            }

            doctorEditor = true;
            while (doctorEditor)
            {
                Console.Write("    SPECJALIZACJA:  ");
                if (doctorIndex != -1) SendKeys.SendWait(Mock._doctors[doctorIndex].Specialisation);
                specialisation = Console.ReadLine();
                if (OptionsManager.CheckString(specialisation) && OptionsManager.StringValidation(specialisation, 5)) doctorEditor = false;
            }

            doctorEditor = true;
            while (doctorEditor)
            {
                Console.Write("    CENA ZA WIZYTĘ: ");
                if (doctorIndex != -1) SendKeys.SendWait(Mock._doctors[doctorIndex].Price.ToString());
                price = Console.ReadLine();

                if (int.TryParse(price, out parsedPrice))
                {
                    if (parsedPrice > 0) doctorEditor = false;
                    else MenuManager.ColorText("    Podana liczba musi być większa od zera!\n", ConsoleColor.Red);
                }
                else
                {
                    MenuManager.ColorText("    Musisz podać liczbę całkowitą!\n", ConsoleColor.Red);
                }
            }

            doctorEditor = true;
            while (doctorEditor)
            {
                Console.Write("    OPIS:\n    ");
                if (doctorIndex != -1) SendKeys.SendWait(Mock._doctors[doctorIndex].Description);
                description = Console.ReadLine();
                if (OptionsManager.CheckString(description) && OptionsManager.StringValidation(description, 15)) doctorEditor = false;
            }

            doctorEditor = true;
            while (doctorEditor)
            {
                Console.Write("    WIZYTY:         ");
                if (doctorIndex != -1) SendKeys.SendWait(Mock._doctors[doctorIndex].VisitsAvailable.ToString());
                visitsAvailable = Console.ReadLine();

                if (int.TryParse(visitsAvailable, out parsedVisitsAvailable))
                {
                    if (doctorIndex == -1)
                    {
                        if (parsedVisitsAvailable > 0) doctorEditor = false;
                        else MenuManager.ColorText("    Podana liczba musi być większa od zera!\n", ConsoleColor.Red);
                    }
                    else if (doctorIndex != -1 && parsedVisitsAvailable >= Mock._doctors[doctorIndex].VisitsTaken) {
                        doctorEditor = false; 
                    }
                    else MenuManager.ColorText("    Podana liczba musi być większa lub równa liczbie zarezerwowanych wizyt!\n", ConsoleColor.Red);
                }
                else
                {
                    MenuManager.ColorText("    Musisz podać liczbę całkowitą!\n", ConsoleColor.Red);
                }
            }

            Object[] docInfo = {
                firstName,
                lastName,
                specialisation,
                parsedPrice,
                description,
                parsedVisitsAvailable
            };

            return docInfo;
        }

        public static void DisplayDoctorInfo(int doctorIndex)
        {
            Console.WriteLine("    IMIĘ I NAZWISKO: " + Mock._doctors[doctorIndex].Name);
            Console.WriteLine("    SPECJALIZACJA:   " + Mock._doctors[doctorIndex].Specialisation);
            Console.WriteLine("    CENA:            " + Mock._doctors[doctorIndex].Price + " zł/wizyta");
            Console.Write("    OPIS:\n    " + Mock._doctors[doctorIndex].Description + "\n");
            Console.WriteLine("    WIZYTY:          " + Mock._doctors[doctorIndex].VisitsTaken + "/" + Mock._doctors[doctorIndex].VisitsAvailable + "\n");
        }
    }
}
