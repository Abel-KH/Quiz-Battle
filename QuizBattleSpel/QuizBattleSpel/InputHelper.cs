using System;

namespace QuizBattle
{
    public static class InputHelper
    {
        // Läser in ett säkert heltal inom ett visst intervall (t.ex. menyval 1-3, eller antal spelare 2-4)
        public static int ReadIntInRange(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? "";

                if (int.TryParse(input, out int number) && number >= min && number <= max)
                {
                    return number;
                }

                Console.WriteLine($"Felaktig inmatning. Svara med en siffra mellan {min} och {max}.");
            }
        }

        // Ser till att textinmatning (som namn) inte lämnas tom
        public static string ReadNonEmptyString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = (Console.ReadLine() ?? "").Trim();

                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input;
                }

                Console.WriteLine("Inmatningen får inte vara tom. Försök igen.");
            }
        }
    }
}