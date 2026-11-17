using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace QuizBattleSpel
{
    internal class ResultManager
    {
        private const string FilePath = "data/highscores.json";
        public void DisplayGameResults(List<Player> players, int totalRounds)
        {
            List<Player> sortedPlayers = players
                .OrderByDescending(p => p.Score)
                .ThenByDescending(p => p.CorrectAnswers)
                .ToList();

            Console.WriteLine("\n=== Slutresultat ===");

            for (int i = 0; i < sortedPlayers.Count; i++)
            {
                Player p = sortedPlayers[i];

                Console.WriteLine($"{i + 1}. {p.Name} - {p.Score} poäng ({p.CorrectAnswers} av {totalRounds} rätt)");
            }

            Player topPlayer = sortedPlayers[0];

            List<Player> winners = new List<Player>();
            winners.Add(topPlayer);

            for (int i = 1; i < sortedPlayers.Count; i++)
            {
                if (sortedPlayers[i].Score == topPlayer.Score &&
                    sortedPlayers[i].CorrectAnswers == topPlayer.CorrectAnswers)
                {
                    winners.Add(sortedPlayers[i]);
                }
                else
                {
                    break;
                }
            }

            if (winners.Count == 1)
            {
                Console.WriteLine("\n" + winners[0].Name + " vinner!");
            }
            else
            {
                string winnerNames = string.Join(" och ", winners.Select(w => w.Name));

                Console.WriteLine("\nDet blev oavgjort! " + winnerNames + " delar segern!");
            }
        }

        public void UpdateHighScores(List<Player> players)
        {

            string directory = Path.GetDirectoryName(FilePath);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }


            List<HighScore> highScores = LoadHighScores();


            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");


            foreach (Player player in players)
            {
                highScores.Add(new HighScore
                {
                    Name = player.Name,
                    Score = player.Score,
                    CorrectAnswers = player.CorrectAnswers,
                    Date = currentDate
                });
            }


            highScores = highScores
                .OrderByDescending(h => h.Score)
                .ThenByDescending(h => h.CorrectAnswers)
                .Take(10)
                .ToList();


            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                string jsonString = JsonSerializer.Serialize(highScores, options);

                File.WriteAllText(FilePath, jsonString);

                Console.WriteLine("\nResultaten sparades i data/highscores.json");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kunde inte spara till topplistan: {ex.Message}");
            }


            DisplayHighScores(highScores);
        }


        public List<HighScore> LoadHighScores()
        {
            if (!File.Exists(FilePath))
            {
                return new List<HighScore>();
            }

            try
            {
                string jsonString = File.ReadAllText(FilePath);

                return JsonSerializer.Deserialize<List<HighScore>>(jsonString)
                    ?? new List<HighScore>();
            }
            catch
            {
                return new List<HighScore>();
            }
        }


        public void DisplayHighScores(List<HighScore> highScores = null)
        {
            if (highScores == null)
            {
                highScores = LoadHighScores();
            }

            Console.WriteLine("\n=== Topplista ===");

            if (highScores.Count == 0)
            {
                Console.WriteLine("Inga resultat sparade ännu.");
                return;
            }

            for (int i = 0; i < highScores.Count; i++)
            {
                HighScore h = highScores[i];

                Console.WriteLine($"{i + 1}. {h.Name} - {h.Score} poäng, {h.CorrectAnswers} rätt ({h.Date})");
            }
        }
    }
}
