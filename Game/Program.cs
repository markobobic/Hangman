using ConsoleApp10;
using System;

class GFG
{
    public static void Main()
    {
        Hangman hangman = new Hangman();
        Console.SetCursorPosition(13, 11); Console.Write("Start guessing!");
        while (hangman.Outcome == Outcome.InProgress)
        {
            var keyinfo = Console.ReadKey(true);
            hangman.Guess(keyinfo.KeyChar);
            
        }
        Console.WriteLine("Correct word was : " + hangman.Word);
        Console.ReadLine();
    }


}
