using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static void Main()
    {
        while (true)
        {
            FullClear();
            string mode = ShowStartScreen();

            if (mode == "1P")
                PlaySinglePlayer();
            else if (mode == "2P_COMP")
                PlayTwoPlayer(coop: true);
            else if (mode == "2P_VS")
                PlayTwoPlayer(coop: false);
        }
    }

    static void FullClear()
    {
        Console.Clear();
        for (int i = 0; i < Console.WindowHeight; i++)
        {
            Console.SetCursorPosition(0, i);
            Console.Write(new string(' ', Console.WindowWidth));
        }
        Console.SetCursorPosition(0, 0);
    }

    static string ShowStartScreen()
    {
        Console.SetWindowSize(40, 20);
        Console.SetBufferSize(40, 20);
        Console.CursorVisible = false;

        int selected = 0;
        string[] options = { "1 Player", "2 Players" };

        while (true)
        {
            FullClear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"
   _____             _        
  / ____|           | |       
 | (___   ___   __ _| | _____ 
  \___ \ / _ \ / _` | |/ / _ \
  ____) | (_) | (_| |   <  __/
 |_____/ \___/ \__, |_|\_\___|
                 __/ |        
                |___/         
");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nWybierz tryb gry:\n");

            for (int i = 0; i < options.Length; i++)
            {
                Console.ForegroundColor = i == selected ? ConsoleColor.Cyan : ConsoleColor.Gray;
                Console.WriteLine((i == selected ? " > " : "   ") + options[i]);
            }

            ConsoleKey key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.UpArrow) selected = (selected - 1 + options.Length) % options.Length;
            if (key == ConsoleKey.DownArrow) selected = (selected + 1) % options.Length;
            if (key == ConsoleKey.Enter)
            {
                if (selected == 0) return "1P";
                if (selected == 1) return ShowTwoPlayerModeScreen();
            }
        }
    }

    static string ShowTwoPlayerModeScreen()
    {
        int selected = 0;
        string[] options = { "Kooperacja", "Rywalizacja" };

        while (true)
        {
            FullClear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nWybierz tryb 2 graczy:\n");

            for (int i = 0; i < options.Length; i++)
            {
                Console.ForegroundColor = i == selected ? ConsoleColor.Cyan : ConsoleColor.Gray;
                Console.WriteLine((i == selected ? " > " : "   ") + options[i]);
            }

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nESC - wróć");

            ConsoleKey key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.UpArrow) selected = (selected - 1 + options.Length) % options.Length;
            if (key == ConsoleKey.DownArrow) selected = (selected + 1) % options.Length;
            if (key == ConsoleKey.Enter)
                return selected == 0 ? "2P_COMP" : "2P_VS";
            if (key == ConsoleKey.Escape)
                return ShowStartScreen();
        }
    }

    static void PlaySinglePlayer()
    {
        int screenWidth = 40;
        int screenHeight = 20;
        Console.SetWindowSize(screenWidth, screenHeight);
        Console.SetBufferSize(screenWidth, screenHeight);
        Console.CursorVisible = false;

        Pixel head = new Pixel { xPos = screenWidth / 2, yPos = screenHeight / 2, schermKleur = ConsoleColor.Red };
        List<int> body = new List<int>();
        string movement = "RIGHT";
        int score = 0;
        Random random = new Random();

        int obstacleX = random.Next(1, screenWidth - 2);
        int obstacleY = random.Next(2, screenHeight - 2);

        while (true)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < screenWidth; i++)
            {
                Console.SetCursorPosition(i, 1);
                Console.Write("■");
                Console.SetCursorPosition(i, screenHeight - 1);
                Console.Write("■");
            }
            for (int i = 1; i < screenHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("■");
                Console.SetCursorPosition(screenWidth - 1, i);
                Console.Write("■");
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(0, 0);
            Console.Write($"Score: {score}");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(obstacleX, obstacleY);
            Console.Write("*");

            Console.ForegroundColor = ConsoleColor.Green;
            for (int i = 0; i < body.Count; i += 2)
            {
                Console.SetCursorPosition(body[i], body[i + 1]);
                Console.Write("■");
            }

            Console.ForegroundColor = head.schermKleur;
            Console.SetCursorPosition(head.xPos, head.yPos);
            Console.Write("■");

            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                movement = key switch
                {
                    ConsoleKey.UpArrow => "UP",
                    ConsoleKey.DownArrow => "DOWN",
                    ConsoleKey.LeftArrow => "LEFT",
                    ConsoleKey.RightArrow => "RIGHT",
                    _ => movement
                };
            }

            body.Insert(0, head.xPos);
            body.Insert(1, head.yPos);

            head = Move(head, movement);

            if (WallHit(head, screenWidth, screenHeight))
                ShowGameOver(score);

            for (int i = 0; i < body.Count; i += 2)
            {
                if (head.xPos == body[i] && head.yPos == body[i + 1])
                    ShowGameOver(score);
            }

            if (head.xPos == obstacleX && head.yPos == obstacleY)
            {
                score++;
                obstacleX = random.Next(1, screenWidth - 2);
                obstacleY = random.Next(2, screenHeight - 2);
            }
            else if (body.Count > 2)
            {
                body.RemoveAt(body.Count - 1);
                body.RemoveAt(body.Count - 1);
            }

            Thread.Sleep(100);
        }
    }

    static void PlayTwoPlayer(bool coop)
    {
        int screenWidth = 40;
        int screenHeight = 20;
        Console.SetWindowSize(screenWidth, screenHeight);
        Console.SetBufferSize(screenWidth, screenHeight);
        Console.CursorVisible = false;

        Pixel p1 = new Pixel { xPos = 10, yPos = 10, schermKleur = ConsoleColor.Red };
        Pixel p2 = new Pixel { xPos = 30, yPos = 10, schermKleur = ConsoleColor.Blue };
        List<int> b1 = new List<int>(), b2 = new List<int>();
        string m1 = "RIGHT", m2 = "LEFT";
        int s1 = 0, s2 = 0;
        Random rnd = new Random();
        int obstacleX = rnd.Next(1, screenWidth - 2), obstacleY = rnd.Next(2, screenHeight - 2);

        while (true)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < screenWidth; i++)
            {
                Console.SetCursorPosition(i, 1);
                Console.Write("■");
                Console.SetCursorPosition(i, screenHeight - 1);
                Console.Write("■");
            }
            for (int i = 1; i < screenHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("■");
                Console.SetCursorPosition(screenWidth - 1, i);
                Console.Write("■");
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(0, 0);
            Console.Write($"P1: {s1}  P2: {s2}");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(obstacleX, obstacleY);
            Console.Write("*");

            foreach (var b in new[] { (b1, ConsoleColor.Green), (b2, ConsoleColor.DarkCyan) })
            {
                Console.ForegroundColor = b.Item2;
                for (int i = 0; i < b.Item1.Count; i += 2)
                {
                    Console.SetCursorPosition(b.Item1[i], b.Item1[i + 1]);
                    Console.Write("■");
                }
            }

            Console.ForegroundColor = p1.schermKleur;
            Console.SetCursorPosition(p1.xPos, p1.yPos);
            Console.Write("■");

            Console.ForegroundColor = p2.schermKleur;
            Console.SetCursorPosition(p2.xPos, p2.yPos);
            Console.Write("■");

            if (Console.KeyAvailable)
            {
                var k = Console.ReadKey(true).Key;
                m1 = k switch
                {
                    ConsoleKey.W => "UP",
                    ConsoleKey.S => "DOWN",
                    ConsoleKey.A => "LEFT",
                    ConsoleKey.D => "RIGHT",
                    _ => m1
                };
                m2 = k switch
                {
                    ConsoleKey.UpArrow => "UP",
                    ConsoleKey.DownArrow => "DOWN",
                    ConsoleKey.LeftArrow => "LEFT",
                    ConsoleKey.RightArrow => "RIGHT",
                    _ => m2
                };
            }

            b1.Insert(0, p1.xPos); b1.Insert(1, p1.yPos);
            b2.Insert(0, p2.xPos); b2.Insert(1, p2.yPos);

            p1 = Move(p1, m1); p2 = Move(p2, m2);

            if (!coop && (p1.xPos == p2.xPos && p1.yPos == p2.yPos))
                ShowGameOver(0, twoPlayer: true);

            if (p1.xPos == obstacleX && p1.yPos == obstacleY)
            {
                s1++; obstacleX = rnd.Next(1, screenWidth - 2); obstacleY = rnd.Next(2, screenHeight - 2);
            }
            else if (b1.Count > 2) { b1.RemoveAt(b1.Count - 1); b1.RemoveAt(b1.Count - 1); }

            if (p2.xPos == obstacleX && p2.yPos == obstacleY)
            {
                s2++; if (coop) s1++; obstacleX = rnd.Next(1, screenWidth - 2); obstacleY = rnd.Next(2, screenHeight - 2);
            }
            else if (b2.Count > 2) { b2.RemoveAt(b2.Count - 1); b2.RemoveAt(b2.Count - 1); }

            if (WallHit(p1, screenWidth, screenHeight) || WallHit(p2, screenWidth, screenHeight))
                ShowGameOver(coop ? s1 : s1 + s2, twoPlayer: true);

            Thread.Sleep(100);
        }
    }

    static bool WallHit(Pixel p, int w, int h)
        => p.xPos <= 0 || p.xPos >= w - 1 || p.yPos <= 1 || p.yPos >= h - 1;

    static Pixel Move(Pixel p, string dir)
    {
        return dir switch
        {
            "UP" => new Pixel { xPos = p.xPos, yPos = p.yPos - 1, schermKleur = p.schermKleur },
            "DOWN" => new Pixel { xPos = p.xPos, yPos = p.yPos + 1, schermKleur = p.schermKleur },
            "LEFT" => new Pixel { xPos = p.xPos - 1, yPos = p.yPos, schermKleur = p.schermKleur },
            "RIGHT" => new Pixel { xPos = p.xPos + 1, yPos = p.yPos, schermKleur = p.schermKleur },
            _ => p
        };
    }

    static void ShowGameOver(int score, bool twoPlayer = false)
    {
        FullClear();
        Console.ForegroundColor = ConsoleColor.Red;

        int w = Console.WindowWidth, h = Console.WindowHeight;

        Console.SetCursorPosition(w / 5, h / 2 - 1);
        Console.WriteLine("GAME OVER");

        Console.SetCursorPosition(w / 5, h / 2);
        Console.WriteLine(twoPlayer ? "Koniec gry!" : $"Twój wynik: {score}");

        Console.SetCursorPosition(w / 5, h / 2 + 1);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("ENTER - zagraj ponownie");
        Console.SetCursorPosition(w / 5, h / 2 + 2);
        Console.WriteLine("ESC - zakończ grę");

        Console.ResetColor();

        while (true)
        {
            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.Enter) return;
            if (key == ConsoleKey.Escape) Environment.Exit(0);
        }
    }
}
