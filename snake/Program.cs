using System.Xml.Linq;

namespace Pract9
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            while (true)
            {
                Snake snake = new Snake();
                snake.StartGame();
            }
        }
    }
}
internal enum pole
{
    Width = 50,
    Height = 25,
}
public class Snake
{
    private List<Position> body = new List<Position>();
    private Position fruit;
    private int napravlenie = 0;
    private bool isAlive = true;
    private bool isWin = false;
    private int maxLength;

    public Snake()
    {
        Console.Clear();
        maxLength = ((int)pole.Width - 2) * ((int)pole.Height - 2);
        int head_x = (int)pole.Width / 2;
        int head_y = (int)pole.Height / 2;
        body.Add(new Position(head_x, head_y));
        body.Add(new Position(head_x, head_y + 1));
        GenerateFruit();
        poleris();
        DrawSnake(body);
        DrawFruit();
    }

    public void StartGame()
    {
        isAlive = true;
        isWin = false;
        ConsoleKeyInfo key = Console.ReadKey();
        Thread thread = new Thread(new ThreadStart(StartDrawing));
        thread.Start();
        while (isAlive && !isWin)
        {
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    if (napravlenie != 2)
                    {
                        napravlenie = 0;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (napravlenie != 3)
                    {
                        napravlenie = 1;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (napravlenie != 0)
                    {
                        napravlenie = 2;
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    if (napravlenie != 1)
                    {
                        napravlenie = 3;
                    }
                    break;
            }

            key = Console.ReadKey();
        }
    }

    private void poleris()
    {
        for (int i = 0; i < (int)pole.Height; i++)
        {
            for (int j = 0; j < (int)pole.Width; j++)
            {
                if (i == 0 || j == 0 || i == (int)pole.Height - 1 || j == (int)pole.Width - 1)
                {
                    Console.SetCursorPosition(j, i);
                    Console.Write("_");
                }
            }
        }
    }

    private void DrawSnake(List<Position> old)
    {
        foreach (var elem in old)
        {
            Console.SetCursorPosition(elem.x, elem.y);
            Console.Write(".");
        }
        foreach (var elem in body)
        {
            Console.SetCursorPosition(elem.x, elem.y);
            Console.Write("█");
        }
    }

    private void StartDrawing()
    {
        var oldBody = CopyBody();
        while (isAlive && !isWin)
        {
            DrawSnake(oldBody);
            oldBody = CopyBody();
            Move();
            Thread.Sleep(100);
        }
    }

    private void DrawFruit()
    {
        Console.SetCursorPosition(fruit.x, fruit.y);
        Console.Write("+");
    }

    private void Move()
    {
        var head = body[0];
        Position new_head;
        switch (napravlenie)
        {
            case 0:
                new_head = new Position(head.x, head.y - 1);
                break;
            case 1:
                new_head = new Position(head.x + 1, head.y);
                break;
            case 2:
                new_head = new Position(head.x, head.y + 1);
                break;
            case 3:
                new_head = new Position(head.x - 1, head.y);
                break;
            default:
                new_head = new Position(head.x, head.y);
                break;
        }
        if (new_head.x < 1 || new_head.x > (int)pole.Width - 2 || new_head.y < 1 || new_head.y > (int)pole.Height - 2 || PositionInSnake(new_head))
        {
            isAlive = false;
            return;
        }
        body.Insert(0, new_head);
        if (new_head.Ravni(fruit))
        {
            GenerateFruit();
            DrawFruit();
            if (body.Count == maxLength)
            {
                isWin = true;
                return;
            }
        }
        else
        {
            body.RemoveAt(body.Count - 1);
        }
    }

    private void GenerateFruit()
    {
        Position fuit_position = Position.Random();
        while (PositionInSnake(fuit_position))
        {
            fuit_position = Position.Random();
        }
        fruit = fuit_position;
    }

    private bool PositionInSnake(Position position)
    {
        foreach (var elem in body)
        {
            if (elem.Ravni(position))
            {
                return true;
            }
        }

        return false;
    }

    private List<Position> CopyBody()
    {
        List<Position> copy = new List<Position>();
        foreach (var elem in body)
        {
            copy.Add(new Position(elem.x, elem.y));
        }
        return copy;
    }
}
public class Position
{
    public int x;
    public int y;

    public Position(int xParam, int yParam)
    {
        x = xParam;
        y = yParam;
    }

    public bool Ravni(Position other)
    {
        return x == other.x && y == other.y;
    }

    public static Position Random()
    {
        Random rand = new Random();
        int x = rand.Next(1, (int)pole.Width - 1);
        int y = rand.Next(1, (int)pole.Height - 1);
        return new Position(x, y);
    }
}