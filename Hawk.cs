using System;
using System.Collections.Generic;

public class Hawk : Animal
{
    private int hunger = 0;

    public override char Symbol { get { return 'H'; } }
    public override int BreedEvery { get { return 5; } }

    public override Animal CreateBaby()
    {
        return new Hawk();
    }

    public override void Move(Grid grid)
    {
        List<(int, int)> options = new List<(int, int)>();
        foreach ((int, int) n in grid.Neighbors(Row, Col))
        {
            if (!(grid.Get(n.Item1, n.Item2) is Hawk))
            {
                options.Add(n);
            }
        }
        if (options.Count == 0)
        {
            return;
        }

        List<(int, int)> prey = new List<(int, int)>();
        for (int dr = -3; dr <= 3; dr++)
        {
            for (int dc = -3; dc <= 3; dc++)
            {
                if (Math.Abs(dr) + Math.Abs(dc) > 3)
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
                if (a is Mouse || a is Snake)
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
        if (hunger >= 4)
        {
            grid.Remove(this);
        }
    }
}
