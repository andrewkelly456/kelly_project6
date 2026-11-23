using System;

public class Program
{
    public static void Main()
    {
        Grid grid = new Grid();

        for (int i = 0; i < 40; i++)
        {
            grid.PlaceRandom(new Mouse());
        }
        for (int i = 0; i < 10; i++)
        {
            grid.PlaceRandom(new Snake());
        }
        for (int i = 0; i < 5; i++)
        {
            grid.PlaceRandom(new Hawk());
        }

        int step = 0;
        while (true)
        {
            grid.Print(step);
            Console.WriteLine("Press any key for the next step, or Q to quit.");
            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Q)
            {
                break;
            }
            grid.Step();
            step++;
        }
    }
}
