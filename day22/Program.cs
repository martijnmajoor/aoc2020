using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace day22
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game("example.txt");
            game.Deal();
            game.Play();
            Console.WriteLine("part one: {0}", game.Score());

            game.Deal();
            int winner = game.PlayRecursive();
            Console.WriteLine("part two: {0}", game.Score(winner));
        }
    }
    public class Game
    {
        private string source;
        private List<List<int>> decks = new List<List<int>>();
        public Game(string fn)
        {
            source = fn;
        }
        public void Deal()
        {
            decks = new List<List<int>>();

            using (StreamReader file = new StreamReader(source))
            {
                string line;
                List<int> deck = new List<int>();
                while ((line = file.ReadLine()) != null)
                {
                    if(line.StartsWith("Player")) {
                        deck = new List<int>();
                    } else if(line == "") {
                        decks.Add(deck);
                    } else {
                        deck.Add(int.Parse(line));
                    }
                }
                decks.Add(deck);
            }
        }
        public void Play()
        {
            int end = winningScore(decks);
            while(!decks.Any(deck => deck.Count() == end)) {
                if(decks[0][0] > decks[1][0]) {
                    decks[0].AddRange(new int[]{decks[0][0], decks[1][0]});
                } else {
                    decks[1].AddRange(new int[]{decks[1][0], decks[0][0]});
                }
                decks.ForEach(deck => deck.RemoveAt(0));
            }
        }
        public int PlayRecursive(List<List<int>> subdecks = null)
        {
            List<string> history = new List<string>();
            List<List<int>> playdecks = subdecks == null ? decks : subdecks;

            int end = winningScore(playdecks);
            while(!playdecks.Any(deck => deck.Count() == end)) {
                int[] cards = new int[]{playdecks[0][0], playdecks[1][0]};
                playdecks.ForEach(deck => deck.RemoveAt(0));

                if (cards[0] <= playdecks[0].Count() && cards[1] <= playdecks[1].Count()) {
                    int subwinner = PlayRecursive(
                        new List<List<int>>(){
                            playdecks[0].GetRange(0, cards[0]),
                            playdecks[1].GetRange(0, cards[1])
                        }
                    );

                    award(subwinner, playdecks, cards);
                } else if(cards[0] > cards[1]) {
                    award(0, playdecks, cards);
                } else {
                    award(1, playdecks, cards);
                }
                
                if(history.Contains(snapshot(playdecks))) {
                    return 0;
                }

                history.Add(snapshot(playdecks));
            }
            
            if(subdecks == null) {
                decks = playdecks;
            }

            return playdecks.IndexOf(playdecks.First(deck => deck.Count() == end));
        }
        public int Score(int player = -1)
        {
            List<int> winner;
            if(player == -1) {
                winner = decks.Find(deck => deck.Count() == winningScore(decks));
            } else {
                winner = decks[player];
            }
                
            
            int mul = winner.Count() +1;
            return winner.Aggregate(0, (acc, val) => {
                mul--;
                return acc + val * mul;
            });
        }
        private int winningScore(List<List<int>> decks)
        {
            return decks.Aggregate(0, (cur, val) => { return cur + val.Count(); });
        }
        private string snapshot(List<List<int>> decks)
        {
            return decks.Aggregate("", (cur, val) => {
                return $"{cur}: {string.Join(" ", val)}";
            });
        }
        private void award(int winner, List<List<int>> decks, int[] cards)
        {
            decks[winner].AddRange(new int[]{cards[winner], cards.First(c => c != cards[winner])});
        }
    }
}