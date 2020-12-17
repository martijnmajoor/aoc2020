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
                        state.AddCube(line[x], x, y, 0);
                    }
                    y++;
                }
            }
        }
        public void Simulate(int amount, bool hyper = false)
        {
            while(amount >0) {
                state.Pad(hyper);
                state.Toggle();

                var x = state.Active();

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
        public List<Cube> Neighbors(Cube c)
        {
            return cubes.FindAll(m => Math.Abs(m.X - c.X) <2 && Math.Abs(m.Y - c.Y) <2 && Math.Abs(m.Z - c.Z) <2 && Math.Abs(m.W - c.W) <2);
        }
        public void AddCube(char state, int x, int y, int z, int w = 0)
        {
            cubes.Add(new Cube(state, x, y, z, w));
        }
        public void Pad(bool hyper)
        {
            Frame frame = Zoom(hyper);
            
            for (int w = frame.MinW -1; w <= frame.MaxW +1; w++) {
                for (int z = frame.MinZ -1; z <= frame.MaxZ +1; z++) {
                    for (int y = frame.MinY -1; y <= frame.MaxY +1; y++) {
                        for (int x = frame.MinX -1; x <= frame.MaxY +1; x++) {
                            if (!cubes.Exists(c => c.X == x && c.Y == y && c.Z == z && c.W == w)) {
                                AddCube('.', x, y, z, w);
                            }
                        }
                    }
                }
            }
        }
        public void Toggle()
        {
            cubes
                .FindAll(c => 
                    {
                        int neighbors = Neighbors(c).FindAll(cc => cc.Active == true).Count();
                        return (c.Active && (neighbors < 3 || neighbors > 4)) || (!c.Active && neighbors == 3); 
                    }
                )
                .ForEach(c => c.Toggle());
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
            return cubes.Count(c => c.Active);
        }
    }
    public class Cube
    {
        public int W { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Z { get; private set; }
        public bool Active { get; private set; }

        public Cube(char state, int x, int y, int z, int w = 0)
        {
            W = w;
            X = x;
            Y = y;
            Z = z;

            Active = state == '#' ? true : false; 
        }
        public void Toggle()
        {
            Active = !Active;
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
