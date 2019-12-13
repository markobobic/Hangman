using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp10
{
    public sealed class Hangman
    {
        public Player Player { get; }
        public string Word { get; set; }
        private ImmutableList<string> _wordList { get; } = GetWordList();
        public static Random Random { get; } = new Random();
        public Outcome Outcome { get; private set; }
        public int numberOfMistakesAllowed;
        private HashSet<char> guessedAlready = new HashSet<char>();
        private int incorrectGuess = 0;
        private char[] wordToDisplay;
        public  int MistakesRemaining { get { return numberOfMistakesAllowed - incorrectGuess; } }
        public Hangman(Player player, int numberOfMistakesAllowed = 10)
        {
            Player = player ?? throw new ArgumentException(nameof(player));

            Word = ChooseWord();
            this.numberOfMistakesAllowed = numberOfMistakesAllowed > 0 ?
            numberOfMistakesAllowed : throw new ArgumentException("numbers of mistakes must be non-negative", nameof(numberOfMistakesAllowed));
            _wordList = GetWordList();
            _wordList = _wordList ?? throw new ArgumentException("word list must not be empty ", nameof(_wordList));
            Outcome = Outcome.InProgress;

            wordToDisplay = new char[Word.Length];
            for (int i = 0; i < Word.Length; i++)
            {
                wordToDisplay[i] = '_';
            }
        }


        public Hangman() : this(new Player())
        {

        }
        public void Guess(char letter)
        {
            if (Outcome != Outcome.InProgress)
            {
                throw new InvalidOperationException("Game is not in progress");
            }
            
            guessedAlready.Add(letter);

            if (!Word.Contains(letter))
            {
                IncrementGraphics();
                incorrectGuess += 1;
                if (incorrectGuess > numberOfMistakesAllowed)
                {
                    Outcome = Outcome.Lose;
                    Console.Clear();
                    FinalGraphics();
                    Console.WriteLine("You lose!");
                }
                else
                {
                    Outcome = Outcome.InProgress;
                }
               
            }
            else
            {
                FillInCorrectGuess(letter);

                Console.SetCursorPosition(15, 13); Console.Write($"Correct!\n{string.Join(" ", wordToDisplay)}");
                if (!wordToDisplay.Contains('_'))
                {
                    Outcome = Outcome.Win;
                    Console.WriteLine();
                    Console.WriteLine("You win!");
                }
            }
        }

        private string ChooseWord()
        {
            Debug.Assert(_wordList.Count() > 0);
            return _wordList[Hangman.Random.Next(_wordList.Count)];
        }
        private void FillInCorrectGuess(char letter)
        {
            for (int i = 0; i < Word.Length; i++)
            {
                if (Word[i] == letter)
                {
                    wordToDisplay[i] = letter;
                }
            }
        }
        public static ImmutableList<string> GetWordList()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream("Game.WordList.txt"))
            using (var reader = new StreamReader(stream))
            {
                string text = reader.ReadToEnd();
                string[] array = text.Split(
                  new[] { Environment.NewLine },
                  StringSplitOptions.RemoveEmptyEntries);
                return ImmutableList<string>.Empty.AddRange(array);

            }
            
             

        }
        private void FinalGraphics()
        {
            Console.WriteLine("{0}\n{1}\n{2}\n{3}\n{4}\n{5}",
            @" __________",
            @"| /      |",
            @"|/       0",
            @"|       /|\",
            @"|       / \",
            @"|_______________");
        }

        private  void IncrementGraphics()
        {
            
            switch (MistakesRemaining)
            {
                case 0: Console.CursorTop = 5; Console.Write(" _______________"); break;
                case 1: for (var i = 1; i < 6; i++) { Console.SetCursorPosition(0, i); Console.Write("|"); } break;
                case 2: Console.Write(" __________"); break;
                case 3: Console.CursorTop = 1; Console.Write("| /\n|/"); break;
                case 4: Console.SetCursorPosition(9, 1); Console.Write("|"); break;
                case 5: Console.SetCursorPosition(9, 2); Console.Write("0"); break;
                case 6: Console.SetCursorPosition(9, 3); Console.Write("|"); break;
                case 7: Console.SetCursorPosition(8, 3); Console.Write("/"); break;
                case 8: Console.SetCursorPosition(10, 3); Console.Write(@"\"); break;
                case 9: Console.SetCursorPosition(8, 4); Console.Write("/"); break;
                case 10: Console.SetCursorPosition(10, 4); Console.Write(@" \"); break;
            }
        }


    }
}
