using System;
using System.IO;

namespace DeschMan
{
    internal class Program 
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.InputEncoding = System.Text.Encoding.Unicode;

            Console.Write("Введи имя карты: ");
            string mapName = Convert.ToString(Console.ReadLine());
            Console.WriteLine(mapName);
            Console.Clear();
            Console.CursorVisible = false;

            int playerDX = 0, playerDY = 0, enemyDX = 1, enemyDY = 0;
            bool isAlive = true;
            bool isPlaying = true;
            int collectedDots = 0;
            Random random = new Random();

            char[,] map = ReadMap(mapName);
            DrawMap(map, out int playerX, out int playerY, out int allDots, out int enemyX, out int enemyY);
            
            while (isPlaying)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    ChangeDirection(key, ref playerDX, ref playerDY);
                }

                if (map[playerY + playerDY, playerX + playerDX] != '#')
                {
                    Move(map, '¤', ref playerX, ref playerY, ref playerDX, ref playerDY);
                    if (map[playerY, playerX] == '·')
                        {
                            collectedDots++;
                            map[playerY, playerX] = ' ';
                        }
                }

                if (playerX == enemyX && playerY == enemyY)
                {
                    isAlive = false;
                }

                if (map[enemyY + enemyDY, enemyX + enemyDX] != '#')
                {
                    Move(map, '$', ref enemyX, ref enemyY, ref enemyDX, ref enemyDY);
                }
                else
                {
                    ChangeDirection(random, ref enemyDX, ref enemyDY);
                }

                if (enemyX == playerX && enemyY == playerY)
                {
                    isAlive = false;
                }

                Console.SetCursorPosition(0, map.GetLength(0) + 1);
                Console.WriteLine($"Собрано {collectedDots}/{allDots} ягодок");

                if (collectedDots == allDots || !isAlive)
                {
                    isPlaying = false;
                }

                if (collectedDots == allDots)
                {
                    Console.WriteLine("Уровень пройден!!!");
                }

                if (!isAlive)
                {
                    Console.SetCursorPosition(playerX, playerY);
                    Console.Write('$');
                    Console.SetCursorPosition(0, map.GetLength(0) + 2);
                    Console.WriteLine("Тебя съели :(");
                }

                System.Threading.Thread.Sleep(300);
            }
            Console.ReadKey();
        }

        static void ChangeDirection(ConsoleKeyInfo key,ref int DX, ref int DY) // изменение направления движения стрелочками
        {
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    DY = -1;
                    DX = 0;
                    break;
                case ConsoleKey.DownArrow:
                    DY = 1;
                    DX = 0;
                    break;
                case ConsoleKey.RightArrow:
                    DY = 0;
                    DX = 1;
                    break;
                case ConsoleKey.LeftArrow:
                    DY = 0;
                    DX = -1;
                    break;
            }
        }

        static void ChangeDirection(Random random, ref int DX, ref int DY) // рандомное изменение напрваления движения
        {
            int enemyDir = random.Next(1, 5);

            switch (enemyDir)
            {
                case 1:
                    DY = -1;
                    DX = 0;
                    break;
                case 2:
                    DY = 1;
                    DX = 0;
                    break;
                case 3:
                    DY = 0;
                    DX = 1;
                    break;
                case 4:
                    DY = 0;
                    DX = -1;
                    break;
            }
        }

        static void Move(char[,] map, char who, ref int X, ref int Y, ref int DX, ref int DY) // движение персонажа
        {
            Console.SetCursorPosition(X, Y);

            if (who == '¤')
            {
                Console.Write(' ');
            }
            else
            {
                Console.Write(map[Y, X]);
            }

            X += DX;
            Y += DY;

            Console.SetCursorPosition(X, Y);
            Console.Write(who);
        }

        static char[,] ReadMap(string mapName) // считывание карты из файла
        {
            string[] tempMap = File.ReadAllLines($"Maps/Map_{mapName}.txt");
            char[,] map = new char[tempMap.Length, tempMap[0].Length];

            for (int i = 0; i < tempMap.Length; i++)
            {
                for (int j = 0; j < tempMap[i].Length; j++)
                {
                    map[i, j] = tempMap[i][j];
                }
            }

            return map;
        }

        static void DrawMap(char[,] map, out int playerX, out int playerY, out int allDots, out int enemyX, out int enemyY) // вывод карты в консоль
        {
            allDots = 0;
            playerX = 0;
            playerY = 0;
            enemyX = 0;
            enemyY = 0;

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[i, j]);
                    switch (map[i,j])
                    {
                        case '¤':
                            playerX = j;
                            playerY = i;
                            map[i, j] = ' ';
                            break;
                        case '$':
                            enemyX = j;
                            enemyY = i;
                            map[i, j] = ' ';
                            break;
                        case '·':
                            allDots++;
                            break;
                    }
                }

                Console.WriteLine();
            }
        }

    }
}
