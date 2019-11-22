using System;
using System.Diagnostics;
using System.Media;

namespace Tetris
{
    class Program
    {
        public static Stopwatch timer = new Stopwatch();
        public static Stopwatch dropTimer = new Stopwatch();
        public static Stopwatch inputTimer = new Stopwatch();
        public static int[,] pole = new int[20, 10];
        public static string kybik = "■";
        public static int dropTime, dropRate = 300;             //for levels
        public static int linesCleared = 0, score = 0, level = 1;
        public static int NumberOfForm1, NumberOfForm;
        static BaseTetris form;
        static BaseTetris nextform;
        public static ConsoleKeyInfo key;
        public static bool isKeyPressed = false;
        public static bool Padaet = false;
        public SoundPlayer Z = new SoundPlayer(@"tetris.wav");

        static void Main()
        {
            SoundPlayer splayer = new SoundPlayer();
            splayer.SoundLocation = Environment.CurrentDirectory + "\\01. Main Menu.wav";
            splayer.PlayLooping();

            Console.CursorVisible = false;
            timer.Start();
            dropTimer.Start();
            long time = timer.ElapsedMilliseconds;
            Map();
            Console.SetCursorPosition(4, 10);
            Console.WriteLine("Press any key!");
            Console.ReadKey(true);
            Console.Clear();
            Map();
            Console.SetCursorPosition(25, 1);
            Console.WriteLine("Level " + level);
            Console.SetCursorPosition(25, 2);
            Console.WriteLine("Score " + score);
            Console.SetCursorPosition(25, 3);
            Console.WriteLine("LinesCleared " + linesCleared);

            splayer.Stop();
            splayer.SoundLocation = Environment.CurrentDirectory + "\\02. Game Theme.wav";
            splayer.Play();


            nextform = new BaseTetris();
            NumberOfForm1 = NumberOfForm;
            form = nextform;
            form.Spawn();
            nextform = new BaseTetris();
            Perezapysk();

            splayer.Stop();
            splayer.SoundLocation = Environment.CurrentDirectory + "\\05. Results.wav";
            splayer.Play();

            Console.Clear();
            Console.SetCursorPosition(5, 5);
            Console.WriteLine("Game Over \n\t Replay? (Y/N)");
            Console.SetCursorPosition(5, 7);
            Console.WriteLine("Your score: " + score);
            string input = Console.ReadLine();

            if (input == "y" || input == "Y")
            {
                pole = new int[20, 10];
                timer = new Stopwatch();
                dropTimer = new Stopwatch();
                inputTimer = new Stopwatch();
                dropRate = 300;
                Padaet = false;
                isKeyPressed = false;
                linesCleared = 0;
                score = 0;
                level = 1;
                GC.Collect();
                Console.Clear();
                Main();
            }
            else return;
        }
        public static void Map()
        {
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < 10; i++)
            {
                Console.Write("#-");
            }
            for (int i = 0; i < 19; i++)
            { Console.WriteLine("#"); }
            for (int i = 0; i < 10; i++)
            {
                Console.Write("#-");
            }
            for (int i = 1; i < 20; i++)
            {
                Console.SetCursorPosition(20, i);
                Console.WriteLine("#");
            }
        }
        public static void Paint()
        {
            for (int i = 0; i < 20; i++)
            {
                for (int f = 0; f < 10; f++)
                {
                    Console.SetCursorPosition(1 + 2 * f, i);

                    if (pole[i, f] == 1 || pole[i, f] == 2)
                    {
                        Console.SetCursorPosition(1 + 2 * f, i);
                        Console.Write(kybik);
                    }
                    else
                    {

                        Console.Write(" ");
                    }
                }
            }
        }
        static void Perezapysk()
        {
            while (true)
            {
                dropTime = (int)dropTimer.ElapsedMilliseconds;
                if (dropTime > dropRate)
                {
                    dropTime = 0;
                    dropTimer.Restart();
                    form.Fall();

                }
                if (Padaet == true)
                {
                    form = nextform;
                    NumberOfForm1 = NumberOfForm;
                    form.Spawn();
                    nextform = new BaseTetris();
                    Padaet = false;
                }
                for (int i = 0; i < 10; i++)
                {
                    if (pole[0, i] == 2)
                        return;
                }
                Doings();
                ClearBlock();
            }
        }
        private static void ClearBlock()
        {
            int combo = 0;
            for (int i = 0; i < 20; i++)
            {
                int j;
                for (j = 0; j < 10; j++)
                {
                    if (pole[i, j] != 2)
                        break;
                }
                if (j == 10)
                {

                    linesCleared++;
                    combo++;
                    for (j = 0; j < 10; j++)
                    {
                        pole[i, j] = 0;
                    }
                    int[,] newpole = new int[20, 10];
                    for (int k = 1; k < i; k++)
                    {
                        for (int l = 0; l < 10; l++)
                        {
                            if (pole[k, l] == 2)
                            { newpole[k + 1, l] = 2; }
                            else
                                newpole[k + 1, l] = 0;
                        }
                    }
                    for (int k = 1; k < i; k++)
                    {
                        for (int l = 0; l < 10; l++)
                        {
                            pole[k, l] = 0;
                        }
                    }
                    for (int k = 0; k < 20; k++)
                        for (int l = 0; l < 10; l++)
                            if (newpole[k, l] == 2)
                                pole[k, l] = 2;
                    Paint();
                }
            }
            if (combo == 1)
                score += 50 * level;
            else if (combo == 2)
                score += 100 * level;
            else if (combo == 3)
                score += 300 * level;
            else if (combo > 3)
                score += 300 * combo * level;

            if (linesCleared < 5) level = 1;
            else if (linesCleared < 10) level = 2;
            else if (linesCleared < 15) level = 3;
            else if (linesCleared < 25) level = 4;
            else if (linesCleared < 35) level = 5;
            else if (linesCleared < 50) level = 6;
            else if (linesCleared < 70) level = 7;
            else if (linesCleared < 90) level = 8;
            else if (linesCleared < 110) level = 9;
            else if (linesCleared < 150) level = 10;


            if (combo > 0)
            {
                Console.SetCursorPosition(25, 1);
                Console.WriteLine("Level " + level);
                Console.SetCursorPosition(25, 2);
                Console.WriteLine("Score " + score);
                Console.SetCursorPosition(25, 3);
                Console.WriteLine("LinesCleared " + linesCleared);
            }

            dropRate = 300 - 22 * level;

        }
        public static void Doings()
        {
            if (Console.KeyAvailable)
            {
                key = Console.ReadKey();
                isKeyPressed = true;
            }
            else
                isKeyPressed = false;

            if (Program.key.Key == ConsoleKey.LeftArrow & !form.cheto_cleva() & isKeyPressed)
            {
                for (int i = 0; i < 4; i++)
                {
                    form.loc[i][1] -= 1;
                }
                form.Smena();
            }
            if (Program.key.Key == ConsoleKey.RightArrow & !form.cheto_cprava() & isKeyPressed)
            {
                for (int i = 0; i < 4; i++)
                {
                    form.loc[i][1] += 1;
                }
                form.Smena();
            }
            if (Program.key.Key == ConsoleKey.DownArrow & isKeyPressed)
            {
                form.Fall();

            }
            if (Program.key.Key == ConsoleKey.Spacebar & isKeyPressed)
            {
                form.Rotate();

            }
            if (Program.key.Key == ConsoleKey.UpArrow & !form.cheTo_c_nizy() & isKeyPressed)
            {
                while (form.cheTo_c_nizy() != true)
                {
                    form.Fall();
                }
            }

        }
    }
}
