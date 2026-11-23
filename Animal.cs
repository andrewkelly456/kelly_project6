using System.Collections.Generic;

public abstract class Animal
{
    public int Row { get; set; }
    public int Col { get; set; }
    public bool HasMoved { get; set; }
    public bool Alive { get; set; } = true;
    public int Age { get; set; }

    public abstract char Symbol { get; }
    public abstract int BreedEvery { get; }

    public abstract void Move(Grid grid);
    public abstract Animal CreateBaby();

    public virtual void AfterMove(Grid grid)
    {
    }

    public virtual void Reproduce(Grid grid)
    {
        List<(int, int)> open = grid.EmptyNeighbors(Row, Col);
        if (open.Count == 0)
        {
            return;
        }
        (int, int) spot = open[grid.Rng.Next(open.Count)];
        Animal baby = CreateBaby();
        baby.HasMoved = true;
        grid.Place(baby, spot.Item1, spot.Item2);
    }
}
