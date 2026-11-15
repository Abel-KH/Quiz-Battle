using System.Text.Json;

namespace QuizBattleSpel
{
    internal class QuestionManager
    {
        private List<Category> categories = new List<Category>();
        private readonly Random random = new Random();

        public List<Category> Categories => categories;

        // Får in frågor från en JSON-fil och lägger till dem i kategorier
        public void LoadQuestionsFromJson(string filePath)
        {
            string json = File.ReadAllText(filePath);

            using JsonDocument document = JsonDocument.Parse(json);

            categories.Clear();

            JsonElement categoryElements =
                document.RootElement.GetProperty("categories");

            foreach (JsonElement categoryElement in categoryElements.EnumerateArray())
            {
                string categoryName =
                    categoryElement.GetProperty("name").GetString() ?? throw new InvalidDataException("Category name is missing.");

                List<Question> questions = new List<Question>();

                foreach (JsonElement questionElement in categoryElement.GetProperty("questions").EnumerateArray())
                {
                    string questionText = questionElement.GetProperty("text").GetString() ?? throw new InvalidDataException("Question text is missing.");

                    string difficulty = questionElement.GetProperty("difficulty").GetString() ?? throw new InvalidDataException("Difficulty is missing.");

                    string type = questionElement.GetProperty("type").GetString() ?? throw new InvalidDataException("Question type is missing.");

                    switch (type)
                    {
                        case "multipleChoice":

                            List<string> options = new List<string>();

                            foreach (JsonElement option in questionElement.GetProperty("options").EnumerateArray())
                            {
                                options.Add(option.GetString() ?? throw new InvalidDataException("Option is missing."));
                            }

                            int correctIndex = questionElement.GetProperty("correctIndex").GetInt32();

                            if (correctIndex < 0 || correctIndex >= options.Count)
                            {
                                throw new InvalidDataException("Invalid correctIndex in JSON.");
                            }

                            questions.Add(new MultipleChoiceQuestion(questionText, difficulty, options, correctIndex ));
                            break;

                        case "trueFalse":

                            bool correct = questionElement.GetProperty("correct").GetBoolean();
                            questions.Add(new TrueFalseQuestion( questionText, difficulty, correct));
                            break;

                        default:
                            throw new InvalidDataException($"Unknown question type: {type}");
                    }
                }
                categories.Add(new Category(categoryName, questions));
            }
        }

        // Skickar tillbaka en lista med kategorier som har frågor kvar
        public List<Category> GetAvailableCategories()
        {
            RemoveEmptyCategories();
            return categories;
        }

        // Väljer en kategori baserat på dess index
        public Category? SelectCategory(int index)
        {
            RemoveEmptyCategories();
            if (index < 0 || index >= categories.Count)
            {
                return null;
            }
            return categories[index];
        }

        // Väljer en slumpmässig kategori från listan med kategorier som har frågor kvar
        public Category? GetRandomCategory()
        {
            RemoveEmptyCategories();

            if (categories.Count == 0)
            {
                return null;
            }

            int index = random.Next(categories.Count);

            return categories[index];
        }

        // Får in en kategori och returnerar en slumpmässig fråga från den kategorin
        public Question? GetRandomQuestion(Category category)
        {
            if (!categories.Contains(category))
            {
                return null;
            }

            Question? question = category.GetRandomQuestion(random);

            RemoveEmptyCategories();

            return question;
        }

        // Tar bort kategorier som inte har några frågor kvar
        public void RemoveEmptyCategories()
        {
            categories.RemoveAll(category => category.Questions.Count == 0);
        }

        // Räknar ut hur många frågor som finns kvar totalt i alla kategorier
        public int GetRemainingQuestionCount()
        {
            return categories.Sum(category => category.Questions.Count);
        }

        // Kollar om det finns några frågor kvar totalt i alla kategorier
        public bool HasQuestionsLeft()
        {
            return GetRemainingQuestionCount() > 0;
        }
    }
}
