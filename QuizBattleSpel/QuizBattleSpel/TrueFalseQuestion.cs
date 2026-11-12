using System;
using System.Collections.Generic;
using System.Text;

namespace QuizBattleSpel
{
    internal class TrueFalseQuestion : Question
    {
        public bool CorrectAnswer { get; set; }
        public TrueFalseQuestion(string text, string difficulty, bool correctAnswer) : base(text, difficulty)
        {
            CorrectAnswer = correctAnswer;
        }
        public override bool CheckAnswer(string answer)
        {
            if (bool.TryParse(answer, out bool parsedAnswer))
            {
                return parsedAnswer == CorrectAnswer;
            }
            return false;
        }
    }
}
