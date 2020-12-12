using System;
using System.IO;

namespace day12
{
    class Program
    {
        static void Main(string[] args)
        {
            string f = "example.txt";
            Solution solution = new Solution();

            var dest = solution.Navigate(f);

            Console.WriteLine("Part one: {0}", dest.ship);
            Console.WriteLine("Part two: {0}", dest.waypoint);
        }
    }
    public class Solution
    {
        public Solution()
        {
        }
        public (int ship, int waypoint) Navigate(string f)
        {
            var dist = (north: 0, east: 0, south: 0, west: 0);
            var waypoint = (north: 1, east: 10);
            int facing = 90;

            using (StreamReader file = new StreamReader(f))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    string direction = line.Substring(0,1);
                    int amount = Int32.Parse(line.Substring(1));

                    switch(direction) {
                        case "N":
                            dist.north -= amount;
                            waypoint.north += amount;
                        break;
                        case "S":
                            dist.north += amount;
                            waypoint.north -= amount;
                        break;
                        case "E":
                            dist.east += amount;
                            waypoint.east += amount;
                        break;
                        case "W":
                            dist.east -= amount;
                            waypoint.east -= amount;
                        break;
                        case "L":
                            facing = (360 + (facing - amount)) % 360;

                            switch(amount % 360) {
                                case 90:
                                    waypoint = (north: waypoint.east, east: -waypoint.north);
                                break;
                                case 180:
                                    waypoint = (north: -waypoint.north, east: -waypoint.east);
                                break;
                                case 270:
                                    waypoint = (north: -waypoint.east, east: waypoint.north);
                                break;
                            }
                        break;
                        case "R":
                            facing = (360 + (facing + amount)) % 360;

                            switch(amount % 360) {
                                case 90:
                                    waypoint = (north: -waypoint.east, east: waypoint.north);
                                break;
                                case 180:
                                    waypoint = (north: -waypoint.north, east: -waypoint.east);
                                break;
                                case 270:
                                    waypoint = (north: waypoint.east, east: -waypoint.north);
                                break;
                            }
                        break;
                        case "F":
                            dist.south -= waypoint.north * amount;
                            dist.west -= waypoint.east * amount;
                            
                            switch(facing) {
                                case 0:
                                    dist.north -= amount;
                                break;
                                case 90:
                                    dist.east += amount;
                                break;
                                case 180:
                                    dist.north += amount;
                                break;
                                case 270:
                                    dist.east -= amount;
                                break;
                            }
                        break;
                    }
                }
            }
 
            return (
                ship: Math.Abs(dist.east) + Math.Abs(dist.north),
                waypoint: Math.Abs(dist.west) + Math.Abs(dist.south)
            );
        }
    } 
}
