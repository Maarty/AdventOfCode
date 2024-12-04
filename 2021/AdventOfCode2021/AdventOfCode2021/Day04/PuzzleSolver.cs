using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day04
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "04";

        protected override string SolvePuzzle(string input)
        {
            var lines = GetLinesInput(input).Where(a => a.Length > 1).ToArray();

            var drawing = lines[0].Split(',').Select(int.Parse).ToList();

            var boardLines = lines[1..];

            var boards = new List<int[,]>();

            for (var i = 0; i <(boardLines.Length / 5); i++)
            {
                var board = new int[5, 5];

                for (var y = 0; y < 5; y++)
                {
                    var line = boardLines[i*5+y].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    for (var x = 0; x < 5; x++)
                    {
                        board[y, x] = int.Parse(line[x]);
                    }
                }
                
                boards.Add(board);
            }


            var result = 0;

            var winningBoards = new bool[boards.Count];


            foreach (var draw in drawing)
            {
                var updatedBoards = new List<int[,]>();
                var boardNumber = 0;
                
                foreach (var board in boards)
                {
                    var updatedBoard = SetWinningNumber(board, draw);

                    if (!winningBoards[boardNumber])
                    {
                        var checkWinning = CheckWinning(updatedBoard);

                        if (checkWinning.Item1)
                        {
                            winningBoards[boardNumber] = true;
                            if (winningBoards.All(a => a))
                            {
                                result = draw * GetBoardSum(checkWinning.Item2);
                                break;
                            }
                        }
                    }
                    
                    updatedBoards.Add(updatedBoard);

                    boardNumber++;
                }

                if (result > 0)
                {
                    break;
                }

                boards = updatedBoards.ToList();
            }

            return result.ToString();
        }

        private int[,] SetWinningNumber(int[,] board, int number)
        {
            for (var y = 0; y < 5; y++)
            {
                for (var x = 0; x < 5; x++)
                {
                    if (board[y,x] == number)
                    {
                        board[y, x] = -1 * board[y, x] -1;
                    }
                }
            }

            return board;
        }

        private (bool, int[,]) CheckWinning(int[,] board)
        {
            var winningRow = -1;
            var winningColumn = -1;
            for (var i = 0; i < 5; i++)
            {
                var isWinningColumn = true;
                var isWinningRow = true;
                
                for (var j = 0; j < 5; j++)
                {
                    if (board[i, j] <= -1) continue;
                    isWinningRow = false;
                    break;
                }

                if (isWinningRow)
                {
                    winningRow = i;
                    break;
                }
                
                for (var j = 0; j < 5; j++)
                {
                    if (board[j, i] <= -1) continue;
                    isWinningColumn = false;
                    break;
                }

                if (isWinningColumn)
                {
                    winningColumn = i;
                    break;
                }
            }

            if (winningColumn > -1)
            {
                for (var i = 0; i < 5; i++)
                {
                    board[i, winningColumn] = 0;
                }
            }
            
            if (winningRow > -1)
            {
                for (var i = 0; i < 5; i++)
                {
                    board[winningRow, i] = 0;
                }
            }

            return (winningColumn > -1 || winningRow > -1, board);
        }
        
        private int GetBoardSum(int[,] board)
        {
            var sum = 0;
            for (var y = 0; y < 5; y++)
            {
                for (var x = 0; x < 5; x++)
                {
                    if (board[y,x] > 0)
                    {
                        sum += board[y, x];
                    }
                }
            }

            return sum;
        }

        protected override string SolvePuzzleExtended(string input)
        {

            return String.Empty;
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("1924", SolvePuzzle(await LoadTestInputAsync(1))),
               // ("5", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            };
        }

        private string SolvePuzzle()
        {

            return string.Empty;
        }
    }
}