using System;
using System.Collections.Generic;
using System.Linq;

namespace Tetris
{
    public class BaseTetris
    {
        public static int[,] A = new int[4, 4] { {0, 0, 0, 0 },
                                                 {1, 1, 1, 1 },
                                                 {0, 0, 0, 0 },
                                                 {0, 0, 0, 0 }};
        public static int[,] B = new int[2, 2] { {1, 1 },
                                                 {1, 1 }};
        public static int[,] C = new int[3, 3] { {0, 1, 0 },
                                                 {1, 1, 1 },
                                                 {0, 0, 0 }};
        public static int[,] D = new int[3, 3] { {0, 1, 1 },
                                                 {1, 1, 0 },
                                                 {0, 0, 0 }};
        public static int[,] F = new int[3, 3] { {1, 1, 0 },
                                                 {0, 1, 1 },
                                                 {0, 0, 0 }};
        public static int[,] E = new int[3, 3] { {1, 0, 0 },
                                                 {1, 1, 1 },
                                                 {0, 0, 0 }};
        public static int[,] K = new int[3, 3] { {0, 0, 1 },
                                                 {1, 1, 1 },
                                                 {0, 0, 0 }};
        public static List<int[,]> tetrisbase = new List<int[,]>() { A, B, C, D, F, E, K };
        public List<int[]> loc = new List<int[]>();
        static int[,] forma;
        private bool isErect = false;

        public BaseTetris()
        {
            Random rnd = new Random();
            Program.NumberOfForm = rnd.Next(0, 7);
            forma = tetrisbase[Program.NumberOfForm];
            for (int i = 20; i < 35; ++i)
            {
                for (int j = 4; j < 10; ++j)
                {
                    Console.SetCursorPosition(i, j);
                    Console.Write("  ");
                }
            }
            Program.Map();

            for (int i = 0; i < forma.GetLength(0); i++)
            {
                for (int f = 0; f < forma.GetLength(1); f++)
                {
                    if (forma[i, f] == 1)
                    {

                        Console.SetCursorPosition(((10 - forma.GetLength(1)) / 2 + f) * 2 + 20, i + 5);
                        Console.Write(Program.kybik);
                    }
                }
            }
        }
        public void Smena()
        {
            for (int i = 0; i < 20; i++)
            {
                for (int f = 0; f < 10; f++)
                {
                    if (Program.pole[i, f] != 2 && Program.pole[i, f] != -1) Program.pole[i, f] = 0;//тут поменял
                }
            }
            for (int i = 0; i < 4; i++)
            {
                Program.pole[loc[i][0], loc[i][1]] = 1;
            }
            Program.Paint();
        }
        public void Fall()
        {

            if (cheTo_c_nizy())
            {
                for (int i = 0; i < 4; i++)
                {
                    Program.pole[loc[i][0], loc[i][1]] = 2;
                }
                Program.Padaet = true;
            }

            else
            {
                for (int i = 0; i < 4; i++)
                {
                    loc[i][0] += 1;
                }
                Smena();
            }
        }
        public void Spawn()
        {
            for (int i = 0; i < forma.GetLength(0); i++)
            {
                for (int f = 0; f < forma.GetLength(1); f++)
                {
                    if (forma[i, f] == 1)
                    {
                        loc.Add(new int[] { i, (10 - forma.GetLength(1)) / 2 + f });
                    }
                }
            }
            Smena();
        }
        public void Rotate()
        {
            List<int[]> tetloc = new List<int[]>();
            for (int i = 0; i < forma.GetLength(0); i++)
            {
                for (int j = 0; j < forma.GetLength(1); j++)
                {
                    if (forma[i, j] == 1)
                    {
                        tetloc.Add(new int[] { i, (10 - forma.GetLength(1)) / 2 + j });
                    }
                }
            }

            if (Program.NumberOfForm1 == 0)
            {
                if (isErect == false)
                {
                    for (int i = 0; i < loc.Count; i++)
                    {
                        tetloc[i] = TransformMatrix(loc[i], loc[2], "Clockwise");
                    }
                }
                else
                {
                    for (int i = 0; i < loc.Count; i++)
                    {
                        tetloc[i] = TransformMatrix(loc[i], loc[2], "Counterclockwise");
                    }
                }
            }

            else if (Program.NumberOfForm1 == 3)
            {
                for (int i = 0; i < loc.Count; i++)
                {
                    tetloc[i] = TransformMatrix(loc[i], loc[3], "Clockwise");
                }
            }

            else if (Program.NumberOfForm1 == 1) return;
            else
            {
                for (int i = 0; i < loc.Count; i++)
                {
                    tetloc[i] = TransformMatrix(loc[i], loc[2], "Clockwise");
                }
            }


            for (int count = 0; Meshaet_cleva(tetloc) != false | Meshaet_cprava(tetloc) != false | Meshaet_cverxy(tetloc) != false; count++)
            {
                if (Meshaet_cleva(tetloc) == true)
                {
                    for (int i = 0; i < loc.Count; i++)
                    {
                        tetloc[i][1] += 1;
                    }
                }

                if (Meshaet_cprava(tetloc) == true)
                {
                    for (int i = 0; i < loc.Count; i++)
                    {
                        tetloc[i][1] -= 1;
                    }
                }
                if (Meshaet_cverxy(tetloc) == true)
                {
                    for (int i = 0; i < loc.Count; i++)
                    {
                        tetloc[i][0] -= 1;
                    }
                }
                if (count == 3)
                {
                    return;
                }
            }

            loc = tetloc;
        }
        public int[] TransformMatrix(int[] coord, int[] axis, string direction)
        {
            int[] pcoord = { coord[0] - axis[0], coord[1] - axis[1] };
            if (direction == "Counterclockwise")
            {
                pcoord = new int[] { -pcoord[1], pcoord[0] };
            }
            else if (direction == "Clockwise")
            {
                pcoord = new int[] { pcoord[1], -pcoord[0] };
            }

            return new int[] { pcoord[0] + axis[0], pcoord[1] + axis[1] };
        }
        public bool? Meshaet_cverxy(List<int[]> loc)
        {
            List<int> cordinataY = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                cordinataY.Add(loc[i][0]);
                if (loc[i][0] >= 19)
                    return true;
                if (loc[i][0] < 0 || loc[i][1] < 0 || loc[i][1] > 9)
                    return null;
            }
            for (int i = 0; i < 4; i++)
            {
                if (cordinataY.Max() - cordinataY.Min() == 3)
                {
                    if (cordinataY.Max() == loc[i][0] || cordinataY.Max() - 1 == loc[i][0])
                    {
                        if (Program.pole[loc[i][0], loc[i][1]] == 2)
                        {
                            return true;
                        }
                    }

                }
                else
                {
                    if (cordinataY.Max() == loc[i][0])
                    {
                        if (Program.pole[loc[i][0], loc[i][1]] == 2)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
        public bool? Meshaet_cleva(List<int[]> loc)
        {
            List<int> cordinataX = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                cordinataX.Add(loc[i][1]);
                if (loc[i][1] < 0)
                {
                    return true;
                }
                if (loc[i][1] > 9)
                {
                    return false;
                }
                if (loc[i][0] >= 19 || loc[i][0] < 0)
                    return null;
            }
            for (int i = 0; i < 4; i++)
            {
                if (cordinataX.Max() - cordinataX.Min() == 3)
                {
                    if (cordinataX.Min() == loc[i][1] | cordinataX.Min() + 1 == loc[i][1])
                    {
                        if (Program.pole[loc[i][0], loc[i][1]] == 2)
                        {
                            return true;
                        }
                    }

                }
                else
                {
                    if (cordinataX.Min() == loc[i][1])
                    {
                        if (Program.pole[loc[i][0], loc[i][1]] == 2)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool? Meshaet_cprava(List<int[]> loc)
        {
            List<int> cordinataX = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                cordinataX.Add(loc[i][1]);
                if (loc[i][1] > 9)
                {
                    return true;
                }
                if (loc[i][1] < 0)
                {
                    return false;
                }
                if (loc[i][0] >= 19 || loc[i][0] < 0)
                    return null;
            }
            for (int i = 0; i < 4; i++)
            {
                if (cordinataX.Max() - cordinataX.Min() == 3)
                {
                    if (cordinataX.Max() == loc[i][1] | cordinataX.Max() - 1 == loc[i][1])
                    {
                        if (Program.pole[loc[i][0], loc[i][1]] == 2)
                        {
                            return true;
                        }
                    }

                }
                else
                {
                    if (cordinataX.Max() == loc[i][1])
                    {
                        if (Program.pole[loc[i][0], loc[i][1]] == 2)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool cheTo_c_nizy()
        {
            for (int i = 0; i < 4; i++)
            {
                if (loc[i][0] + 1 >= 19)
                {
                    return true;
                }

                if (loc[i][0] + 1 < 19)
                {
                    if (Program.pole[loc[i][0] + 1, loc[i][1]] == 2)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool cheto_cprava()
        {
            for (int i = 0; i < 4; i++)
            {
                if (loc[i][1] == 9)
                {
                    return true;
                }
                else if (Program.pole[loc[i][0], loc[i][1] + 1] == 2)
                {
                    return true;
                }
            }
            return false;
        }
        public bool cheto_cleva()
        {
            for (int i = 0; i < 4; i++)
            {
                if (loc[i][1] == 0)
                {
                    return true;
                }
                else if (Program.pole[loc[i][0], loc[i][1] - 1] == 2)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

