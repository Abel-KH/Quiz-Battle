using System;
using System.Collections.Generic;
using System.IO;

namespace QuizBattle
{
    public class MenuManager
    {
        public void ShowMainMenu()
        {
            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine("=== Quiz Battle ===");
                Console.WriteLine("1. Spela");
                Console.WriteLine("2. Topplista");
                Console.WriteLine("3. Avsluta");

                int choice = InputHelper.ReadIntInRange("Val: ", 1, 3);

                switch (choice)
                {
                    case 1:
                        StartGameSetup();
                        break;
                    case 2:
                        ShowHighscoreMenu();
                        break;
                    case 3:
                        Console.WriteLine("Avslutar spelet. Hej då!");
                        isRunning = false;
                        break;
                }
            }
        }

        private void StartGameSetup()
        {
            Console.Clear();
            Console.WriteLine("=== Starta Spel ===");

            // 1. Inmatning av spelare och rundor
            int playerCount = InputHelper.ReadIntInRange("Antal spelare (2-4): ", 2, 4);
            List<string> playerNames = GetUniquePlayerNames(playerCount);
            int rounds = InputHelper.ReadIntInRange("Antal rundor (1-10): ", 1, 10);

            Console.Clear();
            Console.WriteLine($"Spelet startar med {playerCount} spelare i {rounds} rundor!\n");
            Console.WriteLine("Tryck på valfri knapp för att börja första rundan...");
            Console.ReadKey();

            // 2. Själva spellopen (Här rullar rundorna på!)
            for (int currentRound = 1; currentRound <= rounds; currentRound++)
            {
                Console.Clear();
                Console.WriteLine($"=== Runda {currentRound} av {rounds} ===");

                foreach (string playerName in playerNames)
                {
                    Console.WriteLine($"\nDet är {playerName}s tur!");
                    Console.WriteLine("Välj kategori:");
                    Console.WriteLine("1. Programmering");
                    Console.WriteLine("2. Geografi");
                    Console.WriteLine("3. Slumpa");

                    // Använder din säkra InputHelper här också!
                    int categoryChoice = InputHelper.ReadIntInRange("Val (1-3): ", 1, 3);

                    // Simulerar en fråga
                    Console.WriteLine("\nFråga: Vilket år släpptes C# första gången?");
                    Console.WriteLine("1. 1998");
                    Console.WriteLine("2. 2000");
                    Console.WriteLine("3. 2002");

                    int answer = InputHelper.ReadIntInRange("Ditt svar (1-3): ", 1, 3);

                    if (answer == 2)
                    {
                        Console.WriteLine("Rätt svar! Du får 20 poäng.");
                    }
                    else
                    {
                        Console.WriteLine("Fel! Rätt svar var 2000.");
                    }

                    Console.WriteLine("\nTryck på valfri knapp för nästa spelare...");
                    Console.ReadKey();
                }
            }

            // 3. När alla rundor är slut
            Console.Clear();
            Console.WriteLine("=== Slutresultat ===");
            Console.WriteLine($"Vinnare: {playerNames[0]}!");
            Console.WriteLine("\nTryck på valfri knapp för att återgå till huvudmenyn...");
            Console.ReadKey();
        }

        private void ShowHighscoreMenu()
        {
            Console.Clear();
            Console.WriteLine("=== Topplista ===");

            string path = Path.Combine("data", "highscores.json");

            if (File.Exists(path))
            {
                string content = File.ReadAllText(path);
                Console.WriteLine(content);
            }
            else
            {
                Console.WriteLine("1. Andro - 70 poäng (3 rätt)");
                Console.WriteLine("2. Max - 50 poäng (2 rätt)");
            }

            Console.WriteLine("\nTryck på valfri knapp för att återgå till menyn...");
            Console.ReadKey();
        }

        private List<string> GetUniquePlayerNames(int playerCount)
        {
            List<string> names = new List<string>();

            for (int i = 1; i <= playerCount; i++)
            {
                while (true)
                {
                    string name = InputHelper.ReadNonEmptyString($"Namn på spelare {i}: ");

                    bool nameExists = names.Exists(existing =>
                        existing.Equals(name, StringComparison.OrdinalIgnoreCase));

                    if (!nameExists)
                    {
                        names.Add(name);
                        break;
                    }

                    Console.WriteLine("Det namnet är redan taget. Välj ett annat namn.");
                }
            }

            return names;
        }
    }
}