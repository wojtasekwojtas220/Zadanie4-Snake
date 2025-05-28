using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    class Pixel
    {
        public int xPos;
        public int yPos;
        public ConsoleColor schermKleur;
    }

    static void Main()
    {
        int gameMode = ShowStartScreen();

        int screenWidth = 32;
        int screenHeight = 16;

        Console.SetWindowSize(screenWidth, screenHeight);
        Console.SetBufferSize(screenWidth, screenHeight);
        Console.CursorVisible = false;

        Random random = new Random();

        int obstacleX = random.Next(1, screenWidth - 2);
        int obstacleY = random.Next(2, screenHeight - 2);

        if (gameMode == 0)
        {
            // === Tryb 1 gracza ===
            Pixel head = new Pixel
            {
                xPos = screenWidth / 2,
                yPos = screenHeight / 2,
                schermKleur = ConsoleColor.Red
            };

            string movement = "RIGHT";
            List<int> body = new List<int>();
            int score = 0;

            while (true)
            {
                Console.Clear();

                // Rysuj score nad planszą
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(0, 0);
                Console.Write($"Score: {score}");

                // Borders
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

                // Obstacle
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.SetCursorPosition(obstacleX, obstacleY);
                Console.Write("*");

                // Ciało
                Console.ForegroundColor = ConsoleColor.Green;
                for (int i = 0; i < body.Count; i += 2)
                {
                    Console.SetCursorPosition(body[i], body[i + 1]);
                    Console.Write("■");
                }

                // Głowa
                Console.ForegroundColor = head.schermKleur;
                Console.SetCursorPosition(head.xPos, head.yPos);
                Console.Write("■");

                // Input
                if (Console.KeyAvailable)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;
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

                switch (movement)
                {
                    case "UP": head.yPos--; break;
                    case "DOWN": head.yPos++; break;
                    case "LEFT": head.xPos--; break;
                    case "RIGHT": head.xPos++; break;
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

                if (head.xPos <= 0 || head.xPos >= screenWidth - 1 || head.yPos <= 1 || head.yPos >= screenHeight - 1)
                    ShowGameOver(score);

                for (int i = 0; i < body.Count; i += 2)
                {
                    if (head.xPos == body[i] && head.yPos == body[i + 1])
                        ShowGameOver(score);
                }

                Thread.Sleep(100);
            }
        }
        else
        {
            // === Tryb 2 graczy ===

            Pixel head1 = new Pixel { xPos = screenWidth / 4, yPos = screenHeight / 2, schermKleur = ConsoleColor.Red };
            Pixel head2 = new Pixel { xPos = 3 * screenWidth / 4, yPos = screenHeight / 2, schermKleur = ConsoleColor.Blue };

            string movement1 = "RIGHT", movement2 = "LEFT";
            List<int> body1 = new List<int>(), body2 = new List<int>();
            int score1 = 0, score2 = 0;

            while (true)
            {
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(0, 0);
                Console.Write($"P1: {score1}   P2: {score2}");

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

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.SetCursorPosition(obstacleX, obstacleY);
                Console.Write("*");

                Console.ForegroundColor = ConsoleColor.Green;
                for (int i = 0; i < body1.Count; i += 2)
                {
                    Console.SetCursorPosition(body1[i], body1[i + 1]);
                    Console.Write("■");
                }
                Console.ForegroundColor = ConsoleColor.Magenta;
                for (int i = 0; i < body2.Count; i += 2)
                {
                    Console.SetCursorPosition(body2[i], body2[i + 1]);
                    Console.Write("■");
                }

                Console.ForegroundColor = head1.schermKleur;
                Console.SetCursorPosition(head1.xPos, head1.yPos);
                Console.Write("■");

                Console.ForegroundColor = head2.schermKleur;
                Console.SetCursorPosition(head2.xPos, head2.yPos);
                Console.Write("■");

                while (Console.KeyAvailable)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.UpArrow: movement1 = "UP"; break;
                        case ConsoleKey.DownArrow: movement1 = "DOWN"; break;
                        case ConsoleKey.LeftArrow: movement1 = "LEFT"; break;
                        case ConsoleKey.RightArrow: movement1 = "RIGHT"; break;
                        case ConsoleKey.W: movement2 = "UP"; break;
                        case ConsoleKey.S: movement2 = "DOWN"; break;
                        case ConsoleKey.A: movement2 = "LEFT"; break;
                        case ConsoleKey.D: movement2 = "RIGHT"; break;
                    }
                }

                body1.Insert(0, head1.xPos); body1.Insert(1, head1.yPos);
                body2.Insert(0, head2.xPos); body2.Insert(1, head2.yPos);

                switch (movement1)
                {
                    case "UP": head1.yPos--; break;
                    case "DOWN": head1.yPos++; break;
                    case "LEFT": head1.xPos--; break;
                    case "RIGHT": head1.xPos++; break;
                }
                switch (movement2)
                {
                    case "UP": head2.yPos--; break;
                    case "DOWN": head2.yPos++; break;
                    case "LEFT": head2.xPos--; break;
                    case "RIGHT": head2.xPos++; break;
                }

                if (head1.xPos == obstacleX && head1.yPos == obstacleY)
                {
                    score1++;
                    obstacleX = random.Next(1, screenWidth - 2);
                    obstacleY = random.Next(2, screenHeight - 2);
                }
                else if (body1.Count > 2)
                {
                    body1.RemoveAt(body1.Count - 1);
                    body1.RemoveAt(body1.Count - 1);
                }

                if (head2.xPos == obstacleX && head2.yPos == obstacleY)
                {
                    score2++;
                    obstacleX = random.Next(1, screenWidth - 2);
                    obstacleY = random.Next(2, screenHeight - 2);
                }
                else if (body2.Count > 2)
                {
                    body2.RemoveAt(body2.Count - 1);
                    body2.RemoveAt(body2.Count - 1);
                }

                bool outOfBounds1 = head1.xPos <= 0 || head1.xPos >= screenWidth - 1 || head1.yPos <= 1 || head1.yPos >= screenHeight - 1;
                bool outOfBounds2 = head2.xPos <= 0 || head2.xPos >= screenWidth - 1 || head2.yPos <= 1 || head2.yPos >= screenHeight - 1;

                if (outOfBounds1 || outOfBounds2)
                    ShowGameOver(score1 + score2);

                for (int i = 0; i < body1.Count; i += 2)
                {
                    if ((head1.xPos == body1[i] && head1.yPos == body1[i + 1]) ||
                        (head2.xPos == body1[i] && head2.yPos == body1[i + 1]))
                        ShowGameOver(score1 + score2);
                }
                for (int i = 0; i < body2.Count; i += 2)
                {
                    if ((head2.xPos == body2[i] && head2.yPos == body2[i + 1]) ||
                        (head1.xPos == body2[i] && head1.yPos == body2[i + 1]))
                        ShowGameOver(score1 + score2);
                }

                Thread.Sleep(100);
            }
        }
    }

    static int ShowStartScreen()
    {
        int selected = 0;
        ConsoleKey key;

        string[] options = { "1 Player", "2 Players" };

        do
        {
            Console.Clear();
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
            Console.WriteLine("Wybierz tryb gry (↑/↓), ENTER = Start\n");

            for (int i = 0; i < options.Length; i++)
            {
                Console.ForegroundColor = i == selected ? ConsoleColor.Cyan : ConsoleColor.Gray;
                Console.WriteLine((i == selected ? " > " : "   ") + options[i]);
            }

            key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.UpArrow) selected = (selected - 1 + options.Length) % options.Length;
            if (key == ConsoleKey.DownArrow) selected = (selected + 1) % options.Length;

        } while (key != ConsoleKey.Enter);

        return selected;
    }

    static void ShowGameOver(int score)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;

        int screenWidth = Console.WindowWidth;
        int screenHeight = Console.WindowHeight;

        Console.SetCursorPosition(screenWidth / 5, screenHeight / 2);
        Console.WriteLine("GAME OVER");
        Console.SetCursorPosition(screenWidth / 5, screenHeight / 2 + 1);
        Console.WriteLine("Łączny wynik: " + score);
        Console.SetCursorPosition(screenWidth / 5, screenHeight / 2 + 2);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("ENTER - zagraj ponownie");
        Console.SetCursorPosition(screenWidth / 5, screenHeight / 2 + 3);
        Console.WriteLine("ESC - zakończ grę");

        Console.ResetColor();

        while (true)
        {
            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.Enter)
            {
                Console.Clear();
                Main(); // Restart
                return;
            }
            else if (key == ConsoleKey.Escape)
            {
                Environment.Exit(0);
            }
        }
    }
}
