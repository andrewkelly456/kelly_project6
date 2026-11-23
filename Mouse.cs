using System;
using System.Collections.Generic;

public class Mouse : Animal
{
    public override char Symbol { get { return 'M'; } }
    public override int BreedEvery { get { return 2; } }

    public override Animal CreateBaby()
    {
        return new Mouse();
    }

    public override void Move(Grid grid)
    {
        List<(int, int)> open = grid.EmptyNeighbors(Row, Col);
        if (open.Count == 0)
        {
            return;
        }

        List<(int, int)> snakes = new List<(int, int)>();
        foreach ((int, int) n in grid.Neighbors(Row, Col))
        {
            if (grid.Get(n.Item1, n.Item2) is Snake)
            {
                snakes.Add(n);
            }
        }

        if (snakes.Count == 0)
        {
            (int, int) pick = open[grid.Rng.Next(open.Count)];
            grid.MoveAnimal(this, pick.Item1, pick.Item2);
            return;
        }

        List<(int, int)> best = new List<(int, int)>();
        int bestScore = -1;
        foreach ((int, int) spot in open)
        {
            int score = 0;
            foreach ((int, int) s in snakes)
            {
                score += Math.Abs(spot.Item1 - s.Item1) + Math.Abs(spot.Item2 - s.Item2);
            }
            if (score > bestScore)
            {
                bestScore = score;
                best.Clear();
            }
            if (score == bestScore)
            {
                best.Add(spot);
            }
        }
        (int, int) choice = best[grid.Rng.Next(best.Count)];
        grid.MoveAnimal(this, choice.Item1, choice.Item2);
    }
}
