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
        public static ConsoleKeyInfo k;
        public static bool isKeyPressed = false;
        public static bool Padaet = false;
        public static SoundPlayer splayer = new SoundPlayer();

        static void Main()
        {
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
            System.Threading.Thread.Sleep(1000);

            Console.Clear();
            Console.SetCursorPosition(5, 5);
            Console.WriteLine("Game Over \n\t Replay? (Y/N)");
            Console.SetCursorPosition(5, 7);
            Console.WriteLine("Your score: " + score);
            k = Console.ReadKey();

            if (Program.k.Key == ConsoleKey.Y)
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
        private static void Perezapysk()
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
                Random gom = new Random();
                int f = gom.Next(0, 6);
                switch (f)
                {
                    case 0: ;
                        Doings(37); break;
                    case 1: Doings(32); break;
                    case 2: Doings(39); break;
                    case 3: Doings(40); break;
                    case 4: Doings(48); break;
                    case 5: Doings(39); break;
                    default: Doings(27); break;

                }
                //if (Console.KeyAvailable)
                //{
                //    key = Console.ReadKey();
                //    isKeyPressed = true; Doings(key);
                //}
                //else
                //    isKeyPressed = false;
                //Doings();
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
        public static void Doings(int str)
        {
            if (str == 37 & !form.cheto_cleva() /*& isKeyPressed*/)
            {
                for (int i = 0; i < 4; i++)
                {
                    form.loc[i][1] -= 1;
                }
                form.Smena();
            }
            if (str == 39 & !form.cheto_cprava() /*& isKeyPressed*/)
            {
                for (int i = 0; i < 4; i++)
                {
                    form.loc[i][1] += 1;
                }
                form.Smena();
            }
            if (str == 40 /*& isKeyPressed*/)
            {
                form.Fall();

            }
            if (str == 32 /*& isKeyPressed*/)
            {
                form.Rotate();

            }
            if (str == 38 & !form.cheTo_c_nizy() /*& isKeyPressed*/)
            {
                while (form.cheTo_c_nizy() != true)
                {
                    form.Fall();
                }
            }
            if (str == 27 /*& isKeyPressed*/)
            {
                splayer.Stop();
                for (int i = 0; i < 1000; i++)
                {
                    System.Threading.Thread.Sleep(1000);
                   if (Console.KeyAvailable)
                        {
                            k = Console.ReadKey();
                        if (str == 27) {  splayer.Play(); splayer.SoundLocation = Environment.CurrentDirectory + "\\01. Main Menu.wav";
                            splayer.PlayLooping(); break; }
                        }
                }
            }

        }
    }
}
