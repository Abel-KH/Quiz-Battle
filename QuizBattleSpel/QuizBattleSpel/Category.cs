namespace QuizBattleSpel
{
    internal class Category
    {
        public string Name { get; set; }
        public List<Question> Questions { get; set; }
        public Category(string name, List<Question> questions)
        {
            Name = name;
            Questions = questions;
        }
        public int GetRemainingQuestionsCount()
        {
            return Questions.Count;
        }
        //hämtar en slumpmässig fråga och tar bort den från listan
        public Question GetRandomQuestion(Random random)
        {
            if (Questions.Count == 0)
            {
                return null;
            }
            int index = random.Next(Questions.Count);
            Question selectedQuestion = Questions[index];
            Questions.RemoveAt(index);
            return selectedQuestion;
        }
    }
}
