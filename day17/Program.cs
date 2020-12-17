using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace day17
{
    class Program
    {
        static void Main(string[] args)
        {
            PocketDimension solution = new PocketDimension("example.txt");
            solution.Simulate(6);
            Console.WriteLine("Part one: {0}", solution.Active());

            solution.Reset();
            solution.Simulate(6, true);
            Console.WriteLine("Part two: {0}", solution.Active());
        }
    }
    public class PocketDimension
    {
        private string source;
        private State state;
        public PocketDimension(string fn)
        {
            source = fn;
            Reset();
        }
        public void Reset()
        {
            state = new State();

            using (StreamReader file = new StreamReader(source))
            {
                int y = 0;
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    for(int x = 0; x < line.Length; x++) {
                        if (line[x] == '#') {
                            state.AddCube(x, y, 0);
                        }
                    }
                    y++;
                }
            }
        }
        public void Simulate(int amount, bool hyper = false)
        {
            while(amount >0) {
                state.Toggle(hyper);
                amount--;
            }
        }
        public int Active()
        {
            return state.Active();
        }
    }
    public class State
    {
        private List<Cube> cubes = new List<Cube>();
        
        public State()
        {
        }
        public List<Cube> Neighbors(int x, int y, int z, int w)
        {
            return cubes.FindAll(c => Math.Abs(c.X - x) <2 && Math.Abs(c.Y - y) <2 && Math.Abs(c.Z - z) <2 && Math.Abs(c.W - w) <2);
        }
        public void AddCube(int x, int y, int z, int w = 0)
        {
            cubes.Add(new Cube(x, y, z, w));
        }
        public void Toggle(bool hyper)
        {
            Frame frame = Zoom(hyper);

            List<Cube> actives = new List<Cube>();
            List<Cube> inactives = new List<Cube>();

            for (int w = frame.MinW -1; w <= frame.MaxW +1; w++) {
               for (int z = frame.MinZ -1; z <= frame.MaxZ +1; z++) {
                    for (int y = frame.MinY -1; y <= frame.MaxY +1; y++) {
                        for (int x = frame.MinX -1; x <= frame.MaxX +1; x++) {
                            Cube current = cubes.Find(c => c.X == x && c.Y == y && c.Z == z && c.W == w);
                            int neighbors = Neighbors(x, y, z, w).Count();
                            
                            if (current != null && (neighbors < 3 || neighbors > 4)) {
                                inactives.Add(current);
                            } else if (current == null && neighbors == 3) {
                                actives.Add(new Cube(x, y, z, w));
                            }    
                        }
                    }
               }
            }
            
            actives.ForEach(c => cubes.Add(c));
            inactives.ForEach(c => cubes.Remove(c));
        }
        public Frame Zoom(bool hyper)
        {
            return new Frame(
                cubes.Min(c => c.X),
                cubes.Max(c => c.X),
                cubes.Min(c => c.Y),
                cubes.Max(c => c.Y),
                cubes.Min(c => c.Z),
                cubes.Max(c => c.Z),
                hyper ? cubes.Min(c => c.W) : 1,
                hyper ? cubes.Max(c => c.W) : -1
            );
        }
        public int Active()
        {
            return cubes.Count();
        }
    }
    public class Cube
    {
        public int W { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Z { get; private set; }

        public Cube(int x, int y, int z, int w = 0)
        {
            W = w;
            X = x;
            Y = y;
            Z = z; 
        }
    }
    public class Frame
    {
        public int MinX { get; private set; }
        public int MaxX { get; private set; }
        public int MinY { get; private set; }
        public int MaxY { get; private set; }
        public int MinZ { get; private set; }
        public int MaxZ { get; private set; }
        public int MinW { get; private set; }
        public int MaxW { get; private set; }

        public Frame(int xMin, int xMax, int yMin, int yMax, int zMin, int zMax, int wMin, int wMax)
        {
            MinX = xMin;
            MaxX = xMax;
            MinY = yMin;
            MaxY = yMax;
            MinZ = zMin;
            MaxZ = zMax;
            MinW = wMin;
            MaxW = wMax;
        }
    }
}
