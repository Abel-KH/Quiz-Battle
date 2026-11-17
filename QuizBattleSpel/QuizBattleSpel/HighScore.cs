using System;
using System.Collections.Generic;
using System.Text;

namespace QuizBattleSpel
{
    internal class HighScore
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public int CorrectAnswers { get; set; }
        public string Date { get; set; }
    }
}
