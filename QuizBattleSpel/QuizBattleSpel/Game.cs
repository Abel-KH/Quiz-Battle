using System;
using System.Collections.Generic;
using System.Linq;
namespace QuizBattleSpel
{
    internal class Game
    {
        private readonly QuestionManager questionManager;
        private readonly List<Player> players;
        private readonly int rounds;
        public Game(QuestionManager questionManager, List<Player> players, int rounds)
        {
            this.players = players;
            this.rounds = rounds;
            this.questionManager = questionManager;
        }
        public void StartGame()
        {
            Console.WriteLine("Welcome to the Quiz Battle Game!");

            for (int round = 1; round <= rounds; round++)
            {
                if (!questionManager.HasQuestionsLeft())
                {
                    Console.WriteLine("No more questions available. The game ends early.");
                    break;
                }

                Console.WriteLine($"\nRound {round} of {rounds}!");

                foreach (Player player in players)
                {
                    if (!questionManager.HasQuestionsLeft())
                    {
                        Console.WriteLine("No more questions available. The game ends early.");
                        break;
                    }

                    PlayerTurn(player);
                }
            }

            ShowFinalResults();
        }


        private void PlayerTurn(Player player)
        {
            Console.WriteLine($"\n{player.Name}'s turn!");

            Category? category = ChooseCategory();

            if (category == null)
            {
                Console.WriteLine("No category available.");
                return;
            }

            Question? question = questionManager.GetRandomQuestion(category);

            if (question == null)
            {
                Console.WriteLine("No more questions in this category.");
                return;
            }

            Console.WriteLine($"\nCategory: {category.Name}");
            Console.WriteLine($"Points: {question.GetPoints()}");
            Console.WriteLine(question.Text);

            if (question is MultipleChoiceQuestion mcQuestion)
            {
                for (int i = 0; i < mcQuestion.Options.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {mcQuestion.Options[i]}");
                }
            }
            else if (question is TrueFalseQuestion)
            {
                Console.WriteLine("Answer True or False.");
            }

            string answer = GetCorrectAnswer(question);

            bool isCorrect = question.CheckAnswer(answer);

            int pointsEarned = player.RegisterAnswer(
                isCorrect,
                question.Difficulty
            );

            if (isCorrect)
            {
                int basePoints = question.GetPoints();
                int bonus = pointsEarned - basePoints;

                Console.WriteLine("\nCorrect answer!");

                if (bonus > 0)
                {
                    Console.WriteLine($"Base points: {basePoints}");
                    Console.WriteLine($"Streak bonus: {bonus}");
                }

                Console.WriteLine($"You earned {pointsEarned} points.");
            }
            else
            {
                Console.WriteLine("\nWrong answer!");
                Console.WriteLine($"Correct answer: {GetCorrectAnswer(question)}");
                Console.WriteLine("No points earned.");
            }

            Console.WriteLine($"Total points: {player.Score}");
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
        private string GetCorrectAnswer(Question question)
        {
            if (question is MultipleChoiceQuestion mcQuestion)
            {
                return mcQuestion.Options[mcQuestion.CorrectIndex];
            }
            else if (question is TrueFalseQuestion tfQuestion)
            {
                return tfQuestion.CorrectAnswer ? "True" : "False";
            }
            return "unknown answer";
        }
        private void ShowFinalResults()
        {
            Console.WriteLine("\n=== Final Results ===");

            if (players.Count == 0)
            {
                Console.WriteLine("No players participated.");
                return;
            }

            List<Player> sortedPlayers = players
                .OrderByDescending(player => player.Score)
                .ThenByDescending(player => player.CorrectAnswers)
                .ToList();

            Console.WriteLine("\nLeaderboard:");

            for (int i = 0; i < sortedPlayers.Count; i++)
            {
                Player player = sortedPlayers[i];

                Console.WriteLine(
                    $"{i + 1}. {player.Name} - {player.Score} points " +
                    $"({player.CorrectAnswers} correct answers)"
                );
            }

            Player winner = sortedPlayers[0];

            List<Player> winners = sortedPlayers
                .Where(player =>
                    player.Score == winner.Score &&
                    player.CorrectAnswers == winner.CorrectAnswers)
                .ToList();

            if (winners.Count == 1)
            {
                Console.WriteLine($"\n{winner.Name} wins!");
            }
            else
            {
                Console.WriteLine(
                    "\nShared victory between: " +
                    string.Join(", ", winners.Select(player => player.Name))
                );
            }
        }
    }
}
