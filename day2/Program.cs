using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace day2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Part one: {partOne()}");
            Console.WriteLine($"Part two: {partTwo()}");
        }
        private static int partOne()
        {
            int valid = 0;
            using (StreamReader file = new StreamReader(@"input.txt"))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    Match policy = Regex.Match(line, @"(\d+)-(\d+) ([a-z]+): (.*)");
                    int count = Regex.Matches(policy.Groups[4].Value, policy.Groups[3].Value).Count();
                    if(count >= Int32.Parse(policy.Groups[1].Value) && count <= Int32.Parse(policy.Groups[2].Value)) {
                        valid++;
                    }
                }
            }
            return valid;
        }

        private static int partTwo()
        {
            int valid = 0;
            using (StreamReader file = new StreamReader(@"input.txt"))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    Match policy = Regex.Match(line, @"(\d+)-(\d+) ([a-z]+): (.*)");
                    string pass = policy.Groups[4].Value;
                    char c = Char.Parse(policy.Groups[3].Value);
                    
                    if(
                        pass[Int32.Parse(policy.Groups[1].Value)-1] == c
                        ^ pass[Int32.Parse(policy.Groups[2].Value)-1] == c
                    ) {
                        valid++;
                    }
                }
            }
            return valid;
        }
    }
}
