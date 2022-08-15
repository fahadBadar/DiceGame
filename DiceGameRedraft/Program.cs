using System;
using System.Collections.Generic;
using System.Linq;

namespace DiceGameRedraft
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var game = new Game();
            game.Play();
        }

        class Die
        {
            private readonly Random _random = new Random();
            private int _lastRoll;

            public void Roll()
            {
                _lastRoll = _random.Next(1, 7);
            }

            public int GetLastRoll()
            {
                return _lastRoll;
            }
        }

        private class Player
        {
            private readonly string _playerName;
            private int _playerScore;

            public Player(string playerName)
            {
                _playerName = playerName;
            }

            public bool HasWon()
            {
                return _playerScore >= 5;
            }

            public void DisplayScore()
            {
                Console.WriteLine("{0}: {1}",_playerName, _playerScore);
            }

            public void AnnounceIfWon()
            {
                if (HasWon())
                {
                    Console.WriteLine("{0} HAS ONE", _playerName.ToUpper());
                }
            }

            public void PromptNextRoll()
            {
                Console.WriteLine("{0} PRESS ENTER TO ROLL", _playerName.ToUpper());
                Console.ReadLine();
            }

            public void AddToScore(int score)
            {
                _playerScore += score;
            }
        }

        class Game
        {
            private readonly Player _playerOne = new Player("player one");
            private readonly Player _playerTwo = new Player("player two");
            private Player _currentPlayer;
            private bool _hasReRolled;
            private readonly List<Die> _dice = new List<Die> { new Die(), new Die(), new Die(), new Die(), new Die() };

            public Game()
            {
                _currentPlayer = _playerOne;
            }

            public void Play()
            {
                while (!_playerOne.HasWon() && !_playerTwo.HasWon())
                {
                    _hasReRolled = false;
                    _currentPlayer.PromptNextRoll();
                    TakeTurn();
                    ShowScores();
                    SwitchPlayer();
                }

                _playerOne.AnnounceIfWon();
                _playerTwo.AnnounceIfWon();
            }

            private void SwitchPlayer()
            {
                _currentPlayer = _currentPlayer == _playerOne ? _playerTwo : _playerOne;
            }

            private void ShowScores()
            {
                Console.WriteLine("YOU ROLLED THE FOLLOWING ");
                foreach (var die in _dice)
                {
                    Console.Write(die.GetLastRoll());
                }
                Console.WriteLine("\n------------------------------");
            }

            private void DisplayScores()
            {
                Console.WriteLine("SCORES");
                _playerOne.DisplayScore();
                _playerTwo.DisplayScore();
            }

            private void TakeTurn()
            {
                foreach (var die in _dice)
                {
                    die.Roll();
                }

                var rollGroup = HighestScoringGroup();
                ScorePlayer(rollGroup);
            }

            private List<Die> HighestScoringGroup()
            {
                var groupedRolls = _dice
                    .GroupBy(u => u.GetLastRoll())
                    .Select(grp => grp.ToList())
                    .OrderByDescending(grp => grp.Count)
                    .ToList().First();

                return groupedRolls;
            }

            private void ScorePlayer(List<Die> rollGroup)
            {
                switch (rollGroup.Count)
                {
                    case 2:
                        if (!_hasReRolled)
                        {
                            ShowScores();
                            ReRoll(rollGroup.First().GetLastRoll());
                            Console.WriteLine("REROLLING...");
                            rollGroup = HighestScoringGroup();
                            ScorePlayer(rollGroup);
                        }

                        break;
                    case 3:
                        AddAndDisplayScore(rollGroup, 3);
                        break;
                    case 4:
                        AddAndDisplayScore(rollGroup, 6);
                        break;
                    case 5:
                        AddAndDisplayScore(rollGroup, 12);
                        break;
                }
                
            }

            private void AddAndDisplayScore(List<Die> rollGroup, int score)
            {
                Console.WriteLine("YOU GOT {1} {0}s", rollGroup.First().GetLastRoll(), rollGroup.Count);
                Console.WriteLine("SCORE +{0}", score);
                _currentPlayer.AddToScore(score);
                DisplayScores();
            }

            private void ReRoll(int matchingPairRoll)
            {
                foreach (var die in _dice.Where(die => die.GetLastRoll() != matchingPairRoll).ToList())
                {
                    die.Roll();
                }

                _hasReRolled = true;
            }
        }
    }
}