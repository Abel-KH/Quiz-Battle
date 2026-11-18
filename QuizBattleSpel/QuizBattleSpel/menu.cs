
using QuizBattleSpel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QuizBattle
{
    public class MenuManager
    {
        private QuestionManager questionManager = new QuestionManager();
        private List<Player> players = new List<Player>();
        private int rounds;
        private ResultManager resultManager = new ResultManager();

        // Huvudmeny, visar alternativ för att spela, visa topplista eller avsluta spelet
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
                        StartGame();
                        Console.WriteLine(
                            "\nTryck på valfri knapp för att återgå till menyn..."
                        );
                        Console.ReadKey();
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

        // Startar spelet, läser in frågor och hanterar spelare och rundor
        private void StartGame()
        {
            Console.Clear();
            Console.WriteLine("=== Starta spel ===");

            int playerCount = InputHelper.ReadIntInRange(
                "Antal spelare (2-4): ", 2, 4
            );

            players.Clear();

            List<string> names = GetUniquePlayerNames(playerCount);

            foreach (string name in names)
            {
                players.Add(new Player(name));
            }

            rounds = InputHelper.ReadIntInRange(
                "Antal rundor (1-10): ", 1, 10
            );

            string questionPath = Path.Combine(AppContext.BaseDirectory, "data", "questions.json");

            if (!File.Exists(questionPath))
            {
                Console.WriteLine("Kunde inte hitta questions.json.");
                return;
            }

            questionManager = new QuestionManager();
            questionManager.LoadQuestionsFromJson(questionPath);

            if (!questionManager.HasQuestionsLeft())
            {
                Console.WriteLine("Inga frågor kunde laddas.");
                return;
            }

            Console.Clear();
            Console.WriteLine("=== Spelet börjar! ===");
            Console.WriteLine($"Antal spelare: {playerCount}");
            Console.WriteLine($"Antal rundor: {rounds}");

            Console.WriteLine("\nTryck på valfri knapp för att börja...");
            Console.ReadKey();

            PlayRounds();
        }

        // Kontrollerar att spelarnamn är unika och returnerar en lista med namn
        private List<string> GetUniquePlayerNames(int playerCount)
        {
            List<string> names = new List<string>();

            for (int i = 1; i <= playerCount; i++)
            {
                while (true)
                {
                    string name = InputHelper.ReadNonEmptyString(
                        $"Namn på spelare {i}: "
                    );

                    bool nameExists = names.Exists(existing =>
                        existing.Equals(
                            name,
                            StringComparison.OrdinalIgnoreCase
                        )
                    );

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

        // Spelar rundorna, varje spelare får en tur per runda
        private void PlayRounds()
        {
            for (int currentRound = 1; currentRound <= rounds; currentRound++)
            {
                if (!questionManager.HasQuestionsLeft())
                {
                    Console.WriteLine("\nAlla frågor har använts!");
                    break;
                }

                Console.Clear();
                Console.WriteLine(
                    $"=== Runda {currentRound} av {rounds} ==="
                );

                foreach (Player player in players)
                {
                    if (!questionManager.HasQuestionsLeft())
                    {
                        Console.WriteLine("\nAlla frågor har använts!");
                        break;
                    }

                    PlayerTurn(player);
                }
            }

            ShowFinalResults();
        }

        // Spelar en tur för en spelare, väljer kategori och fråga, och hanterar svar
        private void PlayerTurn(Player player)
        {
            Console.Clear();
            Console.WriteLine($"\nDet är {player.Name}s tur!");

            Category? category = ChooseCategory();

            if (category == null)
            {
                Console.WriteLine("Ingen kategori tillgänglig.");
                return;
            }

            Question? question =
                questionManager.GetRandomQuestion(category);

            if (question == null)
            {
                Console.WriteLine("Kunde inte hitta någon fråga.");
                return;
            }

            Console.WriteLine($"\nKategori: {category.Name}");
            Console.WriteLine($"Svårighetsgrad: {question.Difficulty}");
            Console.WriteLine($"Poäng: {question.GetPoints()}");
            Console.WriteLine($"\n{question.Text}");

            if (question is MultipleChoiceQuestion mcQuestion)
            {
                for (int i = 0; i < mcQuestion.Options.Count; i++)
                {
                    Console.WriteLine(
                        $"{i + 1}. {mcQuestion.Options[i]}"
                    );
                }
            }
            else if (question is TrueFalseQuestion)
            {
                Console.WriteLine("Sant (s) eller falskt (f)?");
            }

            string answer = GetAnswer(question);

            bool isCorrect = question.CheckAnswer(answer);

            int pointsEarned = player.RegisterAnswer(
                isCorrect,
                question.Difficulty
            );

            if (isCorrect)
            {
                Console.WriteLine("\nRätt svar!");
                Console.WriteLine($"Du fick {pointsEarned} poäng.");
            }
            else
            {
                Console.WriteLine("\nFel svar!");
                Console.WriteLine(
                    $"Rätt svar: {GetCorrectAnswer(question)}"
                );
            }

            Console.WriteLine($"\nTotal poäng: {player.Score}");
            Console.WriteLine($"Antal rätt: {player.CorrectAnswers}");

            Console.WriteLine("\nTryck på valfri knapp för nästa spelare...");
            Console.ReadKey();
        }

        // välj kategori för spelaren, antingen genom att välja från listan eller slumpa
        private Category? ChooseCategory()
        {
            while (true)
            {
                List<Category> categories =
                    questionManager.GetAvailableCategories();

                if (categories.Count == 0)
                    return null;

                Console.WriteLine("\nTillgängliga kategorier:");

                for (int i = 0; i < categories.Count; i++)
                {
                    Console.WriteLine(
                        $"{i + 1}. {categories[i].Name} " +
                        $"({categories[i].Questions.Count} frågor kvar)"
                    );
                }

                Console.WriteLine($"{categories.Count + 1}. Slumpa kategori");

                int choice = InputHelper.ReadIntInRange(
                    "Välj kategori: ",
                    1,
                    categories.Count + 1
                );

                if (choice == categories.Count + 1)
                {
                    return questionManager.GetRandomCategory();
                }

                return categories[choice - 1];
            }
        }

        // Få svar från spelaren, hanterar olika frågetyper (flervalsfrågor och sant/falskt-frågor)
        private string GetAnswer(Question question)
        {
            while (true)
            {
                if (question is MultipleChoiceQuestion mcQuestion)
                {
                    int choice = InputHelper.ReadIntInRange(
                        $"Ditt svar (1-{mcQuestion.Options.Count}): ",
                        1,
                        mcQuestion.Options.Count
                    );

                    return choice.ToString();
                }

                if (question is TrueFalseQuestion)
                {
                    Console.Write("Ditt svar (s/f): ");
                    string? answer = Console.ReadLine()?.Trim().ToLower();

                    if (answer == "s")
                        return "true";

                    if (answer == "f")
                        return "false";

                    Console.WriteLine("Skriv s eller f.");
                }
                else
                {
                    return Console.ReadLine() ?? "";
                }
            }
        }

        // Få rätt svar för en fråga, används för att visa rätt svar när spelaren svarar fel
        private string GetCorrectAnswer(Question question)
        {
            if (question is MultipleChoiceQuestion mcQuestion)
            {
                return mcQuestion.Options[mcQuestion.CorrectIndex];
            }

            if (question is TrueFalseQuestion tfQuestion)
            {
                return tfQuestion.CorrectAnswer ? "Sant" : "Falskt";
            }

            return "Okänt svar";
        }

        // Resultat efter alla rundor, visar poäng och antal rätta svar för varje spelare, samt bestämmer vinnaren eller delad vinst
        private void ShowFinalResults()
        {
            Console.Clear();
            resultManager.DisplayGameResults(players, rounds);
            resultManager.UpdateHighScores(players);

        }

        // Topplista, visar sparade poäng från tidigare spel, läser från en JSON-fil och hanterar om filen inte finns eller är tom
        private void ShowHighscoreMenu()
        {
            Console.Clear();

            resultManager.DisplayHighScores();

            Console.WriteLine(
                "\nTryck på valfri knapp för att återgå till menyn..."
            );
            Console.ReadKey();
        }
    }
}
