namespace QuizBattleSpel
{
    internal class TrueFalseQuestion : Question
    {
        public bool CorrectAnswer { get; set; }

        public TrueFalseQuestion(string text, string difficulty, bool correctAnswer)
            : base(text, difficulty)
        {
            CorrectAnswer = correctAnswer;
        }

        public override bool CheckAnswer(string answer)
        {
            answer = answer.Trim().ToLower();

            if (answer == "t" || answer == "true")
            {
                return CorrectAnswer == true;
            }

            if (answer == "f" || answer == "false")
            {
                return CorrectAnswer == false;
            }

            return false;
        }
    }
}

