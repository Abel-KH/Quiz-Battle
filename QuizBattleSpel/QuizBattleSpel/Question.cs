namespace QuizBattleSpel
{
    internal abstract class Question
    {
        public string Text { get; set; }
        public string Difficulty { get; set; }
        protected Question(string text, string difficulty)
        {
            Text = text;
            Difficulty = difficulty;
        }
        public int GetPoints()
        {
            return Difficulty.ToLower() switch
            {
                "easy" => 1,
                "medium" => 2,
                "hard" => 3,
                _ => 0
            };
        }
        public abstract bool CheckAnswer(string answer);
    }
}
