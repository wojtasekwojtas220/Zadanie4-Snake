using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

class Snake

{

    static void Main()

    {

        Console.WindowHeight = 16;

        Console.WindowWidth = 32;

        int screenwidth = Console.WindowWidth;

        int screenheight = Console.WindowHeight;

        Random randomnummer = new Random();

        pixel hoofd = new pixel();

        hoofd.xpos = screenwidth / 2;

        hoofd.ypos = screenheight / 2;

        hoofd.schermkleur = ConsoleColor.Red;

        string movement = "RIGHT";

        List<int> telje = new List<int>();

        int score = 0;

        Pixel hoofd = new Pixel();

        hoofd.xPos = screenwidth / 2;

        hoofd.yPos = screenheight / 2;

        hoofd.schermKleur = ConsoleColor.Red;



        List<int> teljePositie = new List<int>();



        teljePositie.Add(hoofd.xPos);

        teljePositie.Add(hoofd.yPos);



        DateTime tijd = DateTime.Now;

        string obstacle = "*";

        int obstacleXpos = randomnummer.Next(1, screenwidth);

        int obstacleYpos = randomnummer.Next(1, screenheight);

        while (true)

        {

            Console.Clear();

            //Draw Obstacle

            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.SetCursorPosition(obstacleXpos, obstacleYpos);

            Console.Write(obstacle);



            Console.ForegroundColor = ConsoleColor.Green;

            Console.SetCursorPosition(hoofd.xPos, hoofd.yPos);

            Console.Write("■");



            Console.ForegroundColor = ConsoleColor.White;

            for (int i = 0; i < screenwidth; i++)

            {

                Console.SetCursorPosition(i, 0);

                Console.Write("■");

            }

            for (int i = 0; i < screenwidth; i++)

            {

                Console.SetCursorPosition(i, screenheight - 1);

                Console.Write("■");

            }

            for (int i = 0; i < screenheight; i++)

            {

                Console.SetCursorPosition(0, i);

                Console.Write("■");

            }

            for (int i = 0; i < screenheight; i++)

            {

                Console.SetCursorPosition(screenwidth - 1, i);

                Console.Write("■");

            }

            Console.ForegroundColor =  /* ?? */;

            Console.WriteLine("Score: " + score);

            Console.ForegroundColor = ConsoleColor.White;

            Console.Write("H");

            for (int i = 0; i < telje.Count(); i++)

            {

                Console.SetCursorPosition(telje[i], telje[i + 1]);

                Console.Write("■");

            }

            //Draw Snake

            Console.SetCursorPosition(hoofd.xPos, hoofd.yPos);

            Console.Write("■");

            Console.SetCursorPosition(hoofd.xPos, hoofd.yPos);

            Console.Write("■");

            Console.SetCursorPosition(hoofd.xPos, hoofd.yPos);

            Console.Write("■");

            Console.SetCursorPosition(hoofd.xPos, hoofd.yPos);

            Console.Write("■");



            ConsoleKeyInfo info = Console.ReadKey();

            //Game Logic

            switch (info.Key)

            {

                case ConsoleKey.UpArrow:

                    movement = "UP";

                    break;

                case ConsoleKey.DownArrow:

                    movement = "DOWN";

                // ???

                case ConsoleKey.LeftArrow:

                    movement = "LEFT";

                    break;

                case ConsoleKey.RightArrow:

                    movement = "RIGHT";

                    break;

            }

            if (movement == "UP")

                hoofd.yPos--;

            if (movement == "DOWN")

                hoofd.yPos++;

            if (movement == "LEFT")

                hoofd.xPos--;

            if (movement == "RIGHT")

                hoofd.xPos++;

            //Hindernis treffen

            if (hoofd.xPos == obstacleXpos /* ?? */ == obstacleYpos)

            {

                score++;

                obstacleXpos = randomnummer.Next(1, screenwidth);

                obstacleYpos = randomnummer.Next(1, screenheight);

            }

            teljePositie.Insert(0, hoofd.xPos);

            teljePositie.Insert(1, hoofd.yPos);

            teljePositie.RemoveAt(teljePositie.Count - 1);

            teljePositie.RemoveAt(teljePositie.Count - 1);

            //Kollision mit Wände oder mit sich selbst

            if (hoofd.xPos == 0 || hoofd.xPos == screenwidth - 1 || hoofd.yPos == 0 || hoofd.yPos == screenheight - 1)

            {

                Console.Clear();

                Console.ForegroundColor = ConsoleColor.Red;

                Console.SetCursorPosition(screenwidth / 5, screenheight / 2);

                Console.WriteLine("Game Over");

                Console.SetCursorPosition(screenwidth / 5, screenheight / 2 + 1);

                Console.WriteLine("Dein Score ist: " + score);

                Console.SetCursorPosition(screenwidth / 5, screenheight / 2 + 2);

                Environment.Exit(0);

            }

            for (int i = 0; i < telje.Count(); i += 2)

            {

                if (hoofd.xPos == telje[i] && hoofd.yPos == telje[i + 1])

                {

                    Console.Clear();

                    Console.ForegroundColor = ConsoleColor.Red;

                    Console.SetCursorPosition(screenwidth / 5, screenheight / 2);

                    //???

                    Console.SetCursorPosition(screenwidth / 5, screenheight / 2 + 1);

                    Console.WriteLine("Dein Score ist: " + score);

                    Console.SetCursorPosition(screenwidth / 5, screenheight / 2 + 2);

                    Environment.Exit(0);

                }

            }

            Thread.Sleep(50);

        }

    }

}