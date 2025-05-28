using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static void Main()
    {
        int gameMode = ShowStartScreen(); 

        if (gameMode == 1) 
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Tryb 2 graczy jeszcze niezaimplementowany.");
            Console.WriteLine("Wciśnij ESC, aby wyjść...");
            while (Console.ReadKey(true).Key != ConsoleKey.Escape) ;
            Environment.Exit(0);
        }

        int screenWidth = 32;
        int screenHeight = 16;

        Console.SetWindowSize(screenWidth, screenHeight);
        Console.SetBufferSize(screenWidth, screenHeight);
        Console.CursorVisible = false;

        Random random = new Random();

        Pixel head = new Pixel
        {
            xPos = screenWidth / 2,
            yPos = screenHeight / 2,
            schermKleur = ConsoleColor.Red
        };

        string movement = "RIGHT";
        List<int> body = new List<int>();
        int score = 0;

        int obstacleX = random.Next(1, screenWidth - 1);
        int obstacleY = random.Next(1, screenHeight - 1);

        while (true)
        {
            Console.Clear();

            // Draw score (nad planszą)
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(0, 0);
            Console.Write($"Score: {score}");

            // Draw borders
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

            // Draw obstacle
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(obstacleX, obstacleY);
            Console.Write("*");

            // Draw body
            Console.ForegroundColor = ConsoleColor.Green;
            for (int i = 0; i < body.Count; i += 2)
            {
                Console.SetCursorPosition(body[i], body[i + 1]);
                Console.Write("■");
            }

            // Draw head
            Console.ForegroundColor = head.schermKleur;
            Console.SetCursorPosition(head.xPos, head.yPos);
            Console.Write("■");

            // Handle input
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

            // Move
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

            // Collision with wall
            if (head.xPos <= 0 || head.xPos >= screenWidth - 1 || head.yPos <= 1 || head.yPos >= screenHeight - 1)
                ShowGameOver(score);

            // Collision with self
            for (int i = 0; i < body.Count; i += 2)
            {
                if (head.xPos == body[i] && head.yPos == body[i + 1])
                    ShowGameOver(score);
            }

            Thread.Sleep(100);
        }
    }
    static int ShowStartScreen()
    {
        string[] options = { "1 Player", "2 Players" };
        int selected = 0;

        while (true)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;

            string title = @"
   _____             _        
  / ____|           | |       
 | (___   ___   __ _| | _____ 
  \___ \ / _ \ / _` | |/ / _ \
  ____) | (_) | (_| |   <  __/
 |_____/ \___/ \__, |_|\_\___|
                 __/ |        
                |___/         
";
            Console.WriteLine(title);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nSterowanie: Strzałki ↑ ↓  | ENTER - wybierz tryb");
            Console.WriteLine("Zbieraj * aby zdobywać punkty.\n");

            for (int i = 0; i < options.Length; i++)
            {
                if (i == selected)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(" > " + options[i]);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("   " + options[i]);
                }
            }

            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    selected = (selected - 1 + options.Length) % options.Length;
                    break;
                case ConsoleKey.DownArrow:
                    selected = (selected + 1) % options.Length;
                    break;
                case ConsoleKey.Enter:
                    return selected;
            }
        }
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
        Console.WriteLine("Twój wynik: " + score);
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
                Main(); // Restartuje grę
                return;
            }
            else if (key == ConsoleKey.Escape)
            {
                Environment.Exit(0);
            }
        }
    }
}
