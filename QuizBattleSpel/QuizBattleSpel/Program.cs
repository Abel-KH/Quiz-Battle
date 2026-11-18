using System;

namespace QuizBattle
{
    class Program
    {
        static void Main(string[] args)
        {
            // Ställ in så att svenska tecken (å, ä, ö) fungerar i konsolen
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            MenuManager menuManager = new MenuManager();
            menuManager.ShowMainMenu();
        }
    }
}