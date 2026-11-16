using QuizBattleSpel;
namespace QuizBattleSpel
{
    internal class Game
    {
        private readonly QuestionManager questionManager;
        private readonly List<Player> players;
        private int currentPlayerIndex = 0;
        public Game(QuestionManager questionManager, List<Player> players)
        {
            this.players = players;
            this.rounds = rounds;
            this.questionManager = questionManager;
        }
        public void StartGame()
        {
            while (true)
            {
                Console.WriteLine("Welcome to the Quiz Battle Game!");
                for (int i = 0; i < players.Count; i++)
                {
                    if (!questionManager.HasQuestionsLeft())
                    {
                        Console.WriteLine("No more questions available.");
                        break;
                    }
                    Console.WriteLine($"\n Round {rounds} av {rounds}!");

                    foreach (Player player in players)
                    {
                        if (!questionManager.HasQuestionsLeft())
                        {
                            Console.WriteLine("No more questions available.");
                            break;
                        }
                        PlayerTurn(player);


                    }
                }
                ShowFinalResults();
            }
        }
        private void PlayerTurn(Player player)
        {
            Console.WriteLine($"\n{player.Name}'s turn!");

            Category? category = ChooseCategory();

            if (category == null)
            {
                Console.WriteLine("Invalid category choice. Skipping turn.");
                return;
            }
            Question? question = questionManager.GetRandomQuestion(category);
            if (question == null)
            {
                Console.WriteLine("No more questions available in this category.");
                return;
            }
            Console.WriteLine($"Category: {category.Name}");
            Console.WriteLine($"Points: {question.GetPoints()}");

            if (question is IQuestionDisplay display)
            {
                display.DisplayQuestion();


                string answer = GetAnswer(question);

                int points = question.GetPoints();
                int totalPoints = player.AddPoints(points);
                int bonus = player.GetStreakBonus(points);

                Console.WriteLine($"You earned {points} points for this question.");
                if (bonus > 0)
                {
                    Console.WriteLine($"{player.Name} earned {points} points and a streak bonus of {bonus} points!");
                }
                else
                {
                    Console.WriteLine($"{player.Name} earned {points} points.");
                }
                Console.WriteLine($"Total points: {totalPoints}");
            }
            else
            {
                player.incorrectAnwser();
                Console.WriteLine("Wrong answer!");
                Console.WriteLine($"Right answer: {question.GetCorrectAnswer()}");
                Console.WriteLine("No points earned.");
                Console.WriteLine($"Total points: {player.Score()}");
            }
        }
        private Category? ChooseCategory()
        {
            while (true)
            {
                List<Category> availableCategories = questionManager.GetAvailableCategories();
                if (availableCategories.Count == 0)
                {
                    Console.WriteLine("No more categories available.");
                    return null;
                }
                Console.WriteLine("\nAvailable Categories:");
                for (int i = 0; i < availableCategories.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {availableCategories[i].Name}");
                }
                Console.Write("Choose a category by entering its number: ");
                for (int i = 0; i < availableCategories.Count; i++)
                {
                    Console.WriteLine(
                        $"{i + 1}. {availableCategories[i].Name} " +
                        $"({availableCategories[i].Questions.Count} questions left)");
                }

                Console.WriteLine($"{availableCategories.Count + 1}. Random category");
                Console.Write("Choice: ");
                string? input = Console.ReadLine();

                if (!int.TryParse(input, out int choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }
                if (choice == availableCategories.Count + 1)
                {
                    return questionManager.GetRandomCategory();
                }
                if (choice >= 1 && choice <= availableCategories.Count)
                {
                    return availableCategories[choice - 1];
                }
                Console.WriteLine("Invalid choice. Please try again.");
            }
        }
        private string GetAnswer(Question question)
        {
            while (true)
            {
                if (question is MultipleChoiceQuestion mcQuestion)
                {
                    Console.WriteLine("Choose the correct answer by entering the corresponding letter:");
                    foreach (var option in mcQuestion.Options)
                    {
                        Console.WriteLine($"{option.Key}: {option.Value}");
                    }
                }
                else
                {
                    Console.Write("Enter your answer: ");
                }
                string? answer = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(answer))
                {
                    return answer;
                }
                Console.WriteLine("Answer cannot be empty. Please try again.");
            }
        }
    }
}
