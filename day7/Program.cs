using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace day7
{
    class Program
    {
        static void Main(string[] args)
        {
            Solution solution = new Solution();

            Console.WriteLine("Part one: {0}", solution.PartOne());
            Console.WriteLine("Part two: {0}", solution.PartTwo());
        }
    }
    public class Solution
    {
        public Solution()
        {
        }
        public int PartOne()
        {
            return containsAtLeast("shiny gold", new List<string>());
        }
        public int PartTwo()
        {
            return countBags("shiny gold", 1)-1;
        }

        private int containsAtLeast(string bag, List<string> parents)
        {
            int amount = 0;

            using (StreamReader file = new StreamReader(@"input.txt"))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    string[] rule = line.Split(" bag", 2);
                    if(rule[0] != bag && !parents.Contains(rule[0]) && rule[1].Contains(bag)) {

                        parents.Add(rule[0]);
                        amount += 1+ containsAtLeast(rule[0], parents);
                    }
                }
            }
            return amount;
        }

        private int countBags(string bag, int amount)
        {
            int sum = 0;

            using (StreamReader file = new StreamReader(@"input.txt"))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    string[] rule = line.Split(" bag", 2);
                    int subSum = 0;
                    if(rule[0] == bag) {
                        foreach(Match m in Regex.Matches(rule[1], @"([\d]+) ([a-z\s]+) bag")) {
                            subSum += countBags(m.Groups[2].Value, Int32.Parse(m.Groups[1].Value));
                        }
                        sum += amount + amount * subSum;
                    }
                }
            }
            return sum;
        }
    } 
}
