using System;
using System.Collections.Generic;
using System.Text;

namespace QuizBattleSpel
{
    internal class ResultManager
    {
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
    }
}
