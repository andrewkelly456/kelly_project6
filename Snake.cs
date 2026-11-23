using System;
using System.Collections.Generic;

public class Snake : Animal
{
    private int hunger = 0;

    public override char Symbol { get { return 'S'; } }
    public override int BreedEvery { get { return 4; } }

    public override Animal CreateBaby()
    {
        return new Snake();
    }

    public override void Move(Grid grid)
    {
        List<(int, int)> options = new List<(int, int)>();
        foreach ((int, int) n in grid.Neighbors(Row, Col))
        {
            Animal there = grid.Get(n.Item1, n.Item2);
            if (!(there is Snake) && !(there is Hawk))
            {
                options.Add(n);
            }
        }
        if (options.Count == 0)
        {
            return;
        }

        List<(int, int)> prey = new List<(int, int)>();
        for (int dr = -2; dr <= 2; dr++)
        {
            for (int dc = -2; dc <= 2; dc++)
            {
                if (Math.Abs(dr) + Math.Abs(dc) > 2)
                {
                    continue;
                }
                int r = Row + dr;
                int c = Col + dc;
                if (!grid.InBounds(r, c))
                {
                    continue;
                }
                Animal a = grid.Get(r, c);
                if (a is Mouse)
                {
                    prey.Add((r, c));
                }
            }
        }

        (int, int) choice = options[grid.Rng.Next(options.Count)];
        if (prey.Count > 0)
        {
            int bestDist = 1000;
            foreach ((int, int) spot in options)
            {
                foreach ((int, int) p in prey)
                {
                    int d = Math.Abs(spot.Item1 - p.Item1) + Math.Abs(spot.Item2 - p.Item2);
                    if (d < bestDist)
                    {
                        bestDist = d;
                        choice = spot;
                    }
                }
            }
        }

        Animal victim = grid.Get(choice.Item1, choice.Item2);
        if (victim != null)
        {
            grid.Remove(victim);
            hunger = -1;
        }
        grid.MoveAnimal(this, choice.Item1, choice.Item2);
    }

    public override void AfterMove(Grid grid)
    {
        hunger++;
        if (hunger >= 3)
        {
            grid.Remove(this);
        }
    }
}
