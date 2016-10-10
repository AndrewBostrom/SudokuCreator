using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SolvedSudokuCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int[,]> boardList = new List<int[,]>();
            int[,] gameBoard = new int[9, 9];
            int[,] blankBoard = new int[9, 9];

            while(true)
            {
                gameBoard = FillBoard(gameBoard, blankBoard);
                if (IsSolved(gameBoard))
                {
                    boardList.Add(gameBoard);
                    PrintBoard(gameBoard);
                    gameBoard = blankBoard;
                    Console.WriteLine("Board added");
                }

                if(boardList.Count() == 2)
                {
                    break;
                }
            }
            PrintBoardCode(boardList);
        }

        /*
        * Starts entering random ints 0<i<10 into the board array
        * If generated random int is already in the same row, collumn, or 3x3 grid increment int
        * If no int can be placed in slot, leave as 0 and stop generating rest of board
        */
        static int[,] FillBoard(int[,] gameBoard, int[,] blankBoard)
        {
            Random rnd = new Random();

            for(int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    int boardInt = rnd.Next(1, 10);
                    int[] blockInts = GetBlockInts(gameBoard, i, j);
                    int[] rowInts = GetRowInts(gameBoard, i);
                    int[] colInts = GetColInts(gameBoard, j);

                    for (int k = boardInt; k < boardInt + 9; k++)
                    {
                        if (k % 10 == 0)
                        {
                            continue;
                        }
                        if (blockInts.Contains(k % 10) || rowInts.Contains(k % 10) || colInts.Contains(k % 10))
                        {
                            continue;
                        }

                        gameBoard[i, j] = k % 10;

                        if (gameBoard[i, j] == 0)
                        {
                            return blankBoard;
                        }
                    }
                    if (gameBoard[i, j] == 0)
                    {
                        return blankBoard;
                    }
                }
            }
            return gameBoard;
        }

        // Gets all numbers in same 3x3 grid as current int to be added
        // Input i and j are current row and collumn respectively
        static int[] GetBlockInts(int[,] gameBoard, int i, int j)
        {
            int[] blockInts = new int[9];
            int blockRow = i - (i % 3);
            int blockCol = j - (j % 3);

            for(int ii = blockRow; ii < (blockRow+3); ii++)
            {
                for(int jj = blockCol; jj < (blockCol+3); jj++)
                {
                    for(int k = 0; k < 9; k++)
                    {
                        if(blockInts[k] == 0)
                        {
                            blockInts[k] = gameBoard[ii, jj];
                            break;
                        }
                    }
                }
            }

            return blockInts;
        }

        // Gets all numbers in same row as current int to be added
        // Input i is current row
        static int[] GetRowInts(int[,] gameBoard, int i)
        {
            int[] rowInts = new int[9];

            for(int j = 0; j < 9; j++)
            {
                rowInts[j] = gameBoard[i, j];
            }
            return rowInts;
        }

        // Gets all numbers in same collumn as current int to be added
        // Input j is current collumn
        static int[] GetColInts(int[,] gameBoard, int j)
        {
            int[] colInts = new int[9];

            for (int i = 0; i < 9; i++)
            {
                colInts[i] = gameBoard[i, j];
            }

            return colInts;
        }

        // All boards containing 0s are not solved
        static bool IsSolved(int[,] gameBoard)
        {
            /*}
            for(int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    if(gameBoard[i, j] == 0)
                    {
                        return false;
                    }
                }
            }

            return true;*/
            foreach(int num in gameBoard)
            {
                if (num == 0)
                {
                    return false;
                }
            }
            return true;
        }

        static void PrintBoard(int[,] gameBoard)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if ((j + 1) % 3 == 0)
                    {
                        Console.Write("{0} ", gameBoard[i, j]);
                    }

                    else
                    {
                        Console.Write(gameBoard[i, j]);
                    }
                }

                if((i+1) % 3 == 0)
                {
                    Console.WriteLine();
                }

                Console.WriteLine();
            }
        }

        static void PrintBoardCode(List<int[,]> boardList)
        {
            if(!File.Exists(@"C:\Users\Andrew\code.txt"))
            {
                File.Create(@"C:\Users\Andrew\code.txt");
            }

            StreamWriter file = new StreamWriter(@"C:\Users\Andrew\code.txt");
            file.WriteLine(string.Format("List<int[,]> boardList = new List<int[,]>;\n"));


            for (int k = 0; k < 2; k++)
            {
                file.WriteLine(string.Format("int[,] a{0} = new int[9, 9];\n", k));

                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        file.WriteLine(string.Format("a[{0}, {1}] = {2};\n", i, j, boardList[k][i, j]));
                    }
                }

                file.WriteLine(string.Format("boardList.Add(a{0});\n", k));
            }
            Console.ReadLine();
        }
    }
}
