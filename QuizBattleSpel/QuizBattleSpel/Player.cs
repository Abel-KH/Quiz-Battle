using System;
using System.Collections.Generic;
using System.Text;

namespace QuizBattleSpel
{
    internal class Player
    {
        public string Name { get; set; }
        public int Score { get; set; } = 0;
        public int CorrectAnswers { get; set; } = 0;

        public int CurrentStreak { get; set; } = 0;

        public Player(string name)
        {
            Name = name;

        }

        public int RegisterAnswer(bool isCorrect, string difficulty)
        {
            int pointsEarned = 0;
            if (!isCorrect)
            {
                CurrentStreak = 0;
                pointsEarned = 0;
            }
            else if (isCorrect == true && difficulty == "easy")
            {
                pointsEarned = 10;
                Score = pointsEarned + Score;
                CorrectAnswers++;
                CurrentStreak++;

            }
            else if (isCorrect == true && difficulty == "medium")
            {
                pointsEarned = 20;
                Score = pointsEarned + Score;
                CorrectAnswers++;
                CurrentStreak++;

            }
            else if (isCorrect == true && difficulty == "hard")
            {
                pointsEarned = 30;
                Score = pointsEarned + Score;
                CorrectAnswers++;
                CurrentStreak++;
            }

            return pointsEarned;
        }

    }
}
