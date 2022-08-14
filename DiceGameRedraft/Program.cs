using System;
using System.Collections.Generic;
using System.Linq;
namespace DiceGameRedraft
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var game = new Game();
            game.Play();
        }
        class Game
        {
            private readonly Random _random = new Random();
            private int _pOneScore;
            private int _pTwoScore;
            bool _isPlayerOneTurn = true;
            private bool _hasReRolled;
            
            public void Play()
            {
                while (_pTwoScore < 5 && _pOneScore < 5)
                {
                    _hasReRolled = false;
                    PromptNextPlayer();
                    List<int> roll = TakeTurn();
                    ShowScores(roll);
                    _isPlayerOneTurn = !_isPlayerOneTurn;
                }
                Console.WriteLine(!_isPlayerOneTurn ? "PLAYER ONE WINS" : "PLAYER TWO WINS");
            }
            private static void ShowScores(List<int> roll)
            {
                Console.WriteLine("YOU ROLLED THE FOLLOWING ");
                roll.ForEach(Console.Write);
                Console.WriteLine("\n------------------------------");
            }
            private void PromptNextPlayer()
            {
                if (_isPlayerOneTurn)
                {
                    Console.WriteLine("PLAYER ONE PRESS ENTER TO ROLL");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("PLAYER TWO PRESS ENTER TO ROLL");
                    Console.ReadLine();
                }
            }
            private void DisplayScores()
            {
                Console.WriteLine("SCORES: Player One: {0} Player Two {1}", _pOneScore, _pTwoScore);
            }
            private List<int> TakeTurn()
            {
                List<int> rolls = new List<int>();
                for (int i = 0; i < 5; i++)
                {
                    int roll = _random.Next(1, 7);
                    rolls.Add(roll);
                }
                List<int> rollGroup = HighestScoringGroup(rolls);
                ScorePlayer(rollGroup);
                return rolls;
            }
            private List<int> HighestScoringGroup(List<int> rolls)
            {
                var groupedRolls = rolls
                    .GroupBy(u => u)
                    .Select(grp => grp.ToList())
                    .OrderByDescending(grp => grp.Count)
                    .ToList().First();
                
                return groupedRolls;
            }
            private void ScorePlayer(List<int> rollGroup)
            {
                switch (rollGroup.Count)
                {
                    case 2:
                        if (!_hasReRolled)
                        {
                            ScorePlayer(ReRoll(rollGroup));
                        }
                        break;
                    case 3:
                        Console.WriteLine("YOU GOT {1} {0}s", rollGroup.First(), rollGroup.Count);
                        Console.WriteLine("SCORE +3");
                        if (_isPlayerOneTurn)
                        {
                            _pOneScore += 3;
                        }
                        else
                        {
                            _pTwoScore += 3;
                        }
                        DisplayScores();
                        break;
                    case 4: 
                        Console.WriteLine("YOU GOT {1} {0}s", rollGroup.First(), rollGroup.Count);
                        Console.WriteLine("SCORE +6");
                        if (_isPlayerOneTurn)
                        {
                            _pOneScore += 6;
                        }
                        else
                        {
                            _pTwoScore += 6;
                        }
                        DisplayScores();
                        break;
                    case 5:
                        Console.WriteLine("YOU GOT {1} {0}s", rollGroup.First(), rollGroup.Count);
                        Console.WriteLine("SCORE +12");
                        if (_isPlayerOneTurn)
                        {
                            _pOneScore += 12;
                        }
                        else
                        {
                            _pTwoScore += 12;
                        }
                        DisplayScores();
                        break;
                }
    
            }
            private List<int> ReRoll(List<int> rollGroup)
            {
                List<int> rolls = new List<int>();
                for (int i = 0; i < 3; i++)
                {
                    int roll = _random.Next(1, 7);
                    rolls.Add(roll);
                    if (roll == rollGroup.First())
                    {
                        rollGroup.Add(roll);
                    }
                }
                _hasReRolled = true;
                ShowScores(rolls);
                return rollGroup;
            }
        }
    }
}