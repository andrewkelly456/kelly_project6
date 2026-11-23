using System;
using System.Collections.Generic;

public class Grid
{
    public const int Size = 10;
    public Random Rng = new Random();
    public List<Animal> Animals = new List<Animal>();
    private Animal[,] cells = new Animal[Size, Size];

    public bool InBounds(int r, int c)
    {
        return r >= 0 && r < Size && c >= 0 && c < Size;
    }

    public Animal Get(int r, int c)
    {
        return cells[r, c];
    }

    public void Place(Animal a, int r, int c)
    {
        a.Row = r;
        a.Col = c;
        cells[r, c] = a;
        Animals.Add(a);
    }

    public void PlaceRandom(Animal a)
    {
        int r = Rng.Next(Size);
        int c = Rng.Next(Size);
        while (cells[r, c] != null)
        {
            r = Rng.Next(Size);
            c = Rng.Next(Size);
        }
        Place(a, r, c);
    }

    public void MoveAnimal(Animal a, int r, int c)
    {
        cells[a.Row, a.Col] = null;
        a.Row = r;
        a.Col = c;
        cells[r, c] = a;
    }

    public void Remove(Animal a)
    {
        cells[a.Row, a.Col] = null;
        Animals.Remove(a);
        a.Alive = false;
    }

    public List<(int, int)> Neighbors(int r, int c)
    {
        List<(int, int)> list = new List<(int, int)>();
        if (InBounds(r - 1, c)) list.Add((r - 1, c));
        if (InBounds(r + 1, c)) list.Add((r + 1, c));
        if (InBounds(r, c - 1)) list.Add((r, c - 1));
        if (InBounds(r, c + 1)) list.Add((r, c + 1));
        return list;
    }

    public List<(int, int)> EmptyNeighbors(int r, int c)
    {
        List<(int, int)> list = new List<(int, int)>();
        foreach ((int, int) n in Neighbors(r, c))
        {
            if (cells[n.Item1, n.Item2] == null)
            {
                list.Add(n);
            }
        }
        return list;
    }

    public void Step()
    {
        foreach (Animal a in Animals)
        {
            a.HasMoved = false;
        }

        List<Animal> turn = new List<Animal>(Animals);
        foreach (Animal a in turn)
        {
            if (!a.Alive || a.HasMoved)
            {
                continue;
            }

            a.Move(this);
            a.HasMoved = true;
            a.AfterMove(this);
            if (!a.Alive)
            {
                continue;
            }

            a.Age++;
            if (a.Age % a.BreedEvery == 0)
            {
                a.Reproduce(this);
            }
        }
    }

    public void Print(int step)
    {
        Console.Clear();
        Console.WriteLine("Time step: " + step);
        Console.WriteLine();
        for (int r = 0; r < Size; r++)
        {
            for (int c = 0; c < Size; c++)
            {
                if (cells[r, c] == null)
                {
                    Console.Write(". ");
                }
                else
                {
                    Console.Write(cells[r, c].Symbol + " ");
                }
            }
            Console.WriteLine();
        }

        int mice = 0;
        int snakes = 0;
        int hawks = 0;
        foreach (Animal a in Animals)
        {
            if (a is Mouse) mice++;
            if (a is Snake) snakes++;
            if (a is Hawk) hawks++;
        }
        Console.WriteLine();
        Console.WriteLine("Mice (M): " + mice + "  Snakes (S): " + snakes + "  Hawks (H): " + hawks);
    }
}
