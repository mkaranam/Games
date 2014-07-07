using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeeksForGeeks
{
   
        /// <summary>
        /// Main class for Tic-Tac-Toe game.
        /// </summary>
        class TicTacToe
        {
            #region Variable declarations
            /// <summary>
            /// Private local variables
            /// </summary>
            private int _moveCount;
            private enum Players { User = 1, CPU };
            private Players _lastMove;
            private int cnt;
            private StringBuilder sb;

            /// <summary>
            /// Public local variables
            /// </summary>
            public Difficulty _level;
            public bool isFinished;
            public int _size;   //Size of the board (NxN matrix)
            public int[,] _board;
            public enum Difficulty { EASY = 1, MODERATE, HARD, EXPERT };
            #endregion


            /// <summary>
            /// Default constructor, sets the board size to 3x3 and difficulty to EASY
            /// </summary>
            public TicTacToe()
            {
                //Default size is 3 and difficulty is easy
                _size = 3;
                _board = new int[3, 3];
                _level = Difficulty.EASY;
                _lastMove = Players.CPU;
                isFinished = false;
            }

            /// <summary>
            /// Constructor to set custom game settings
            /// </summary>
            /// <param name="size">Size of the board "N" for a NxN matrix</param>
            /// <param name="d">Difficulty level</param>
            public TicTacToe(int size, Difficulty d)
            {
                if (size <= 0 || size > 9) size = 3;    //Set default
                _size = size;
                _level = d;
                _board = new int[size, size];
                _lastMove = Players.CPU;
                isFinished = false;
            }

            /// <summary>
            /// Checks whether the player who made the last move won the game or not.
            /// </summary>
            /// <param name="row">row number of the last move</param>
            /// <param name="col">column number of the last move</param>
            /// <returns></returns>
            private bool hasWon(int row, int col)
            {
                int lM = (int)_lastMove;
                bool rowMatch = true;
                bool colMatch = true;
                bool diagMatch = true;
                bool crossDiagMatch = true;
                for (int i = 0; i < _size; i++)
                {
                    if (_board[row, i] != lM) rowMatch = false; //Check rows
                    if (_board[i, col] != lM) colMatch = false; //Check columns
                    if (_board[i, i] != lM) diagMatch = false;    //Check diagonal
                    if (_board[i, (_size - i - 1)] != lM) crossDiagMatch = false; //Check other diagonal
                }
                return (rowMatch || colMatch || diagMatch || crossDiagMatch);
            }

            private bool hasWon()
            {
                int lM = (int)_lastMove;
                int lCnt1, lCnt2, rCnt1, rCnt2, dCnt1, dCnt2, cdCnt1, cdCnt2;
                lCnt1 = lCnt2 = rCnt1 = rCnt2 = dCnt1 = dCnt2 = cdCnt1 = cdCnt2 = 0;
                for (int i = 0; i < _size; i++)
                {
                    for (int j = 0; j < _size; j++)
                    {
                        //Calculate row seeds
                        if (_board[i, j] == (int)Players.CPU) rCnt1++;
                        else if (_board[i, j] == (int)Players.User) rCnt2++;

                        //Calculate column seeds
                        if (_board[j, i] == (int)Players.CPU) lCnt1++;
                        else if (_board[j, i] == (int)Players.User) lCnt2++;

                        //Calculate diagonal seeds
                        if (i == j)
                        {
                            if (_board[i, j] == (int)Players.CPU) dCnt1++;
                            else if (_board[i, j] == (int)Players.User) dCnt2++;
                        }

                        //Calculate cross diagonal seeds
                        if ((i + j) == (_size - 1))
                        {
                            if (_board[i, j] == (int)Players.CPU) cdCnt1++;
                            else if (_board[i, j] == (int)Players.User) cdCnt2++;
                        }

                    }

                    if (rCnt1 == _size || rCnt2 == _size || lCnt1 == _size || lCnt2 == _size) return true;
                    rCnt1 = rCnt2 = lCnt1 = lCnt2 = 0;

                }
                if (dCnt1 == _size || dCnt2 == _size || cdCnt1 == _size || cdCnt2 == _size) return true;
                return false;
            }

            /// <summary>
            /// Checks whether the game ended in a tie or not
            /// </summary>
            /// <returns></returns>
            private bool isTie()
            {
                if (_moveCount != _size * _size) return false;
                return true;
            }

            private Players getCurrentMove()
            {
                if (_lastMove == Players.User) return Players.CPU;
                return Players.User;
            }

            /// <summary>
            /// Main tag to continously, play the game until a player wins or game ends in a draw.
            /// Gets a user/CPU move and checks the status for a player winning or game ending in a draw
            /// </summary>
            public void playGame()
            {
                int[] move;
                while (!isFinished)
                {
                    //set current move
                    _lastMove = getCurrentMove();

                    if (_lastMove == Players.User)
                    {
                        move = getUsermove();
                        _moveCount++;
                        checkStatus(move);
                    }
                    else
                    {
                        move = getCPUmove();
                        _moveCount++;
                        checkStatus(move);
                    }
                }
            }

            /// <summary>
            /// Gets the user move
            /// </summary>
            /// <returns>returns the row and column selected using an integer array</returns>
            private int[] getUsermove()
            {
                int[] move = new int[2];
                move[0] = 0;
                move[1] = 1;
                Console.WriteLine("Enter the user move: <row,column>...");
                String line = Console.ReadLine();
                move[0] = Int32.Parse(line);
                line = Console.ReadLine();
                move[1] = Int32.Parse(line);
                _board[move[0], move[1]] = (int)Players.User;
                Console.WriteLine(board());
                return move;
            }

            /// <summary>
            /// Checks the game status for a win or a draw
            /// </summary>
            /// <param name="move"></param>
            private void checkStatus(int[] move)
            {
                if (hasWon(move[0], move[1]))
                {
                    isFinished = true;
                    notifyUser(true);
                }
                else
                {
                    isFinished = isTie();
                    if (isFinished) notifyUser(false);
                }
            }


            /// <summary>
            /// Notifies the user, that the game has ended with the player making the last move winning it or the game ending in a draw
            /// </summary>
            /// <param name="won">Whether its a tie or a win</param>
            private void notifyUser(bool won)
            {
                //TO-DO
                //Need to notify user about game ending
                if (won) Console.WriteLine("Game was won by: {0}", _lastMove);
                else Console.WriteLine("Game ended in a tie: ");
            }


            #region CPU Move
            /// <summary>
            /// Calculates the CPU move based on the difficulty
            /// </summary>
            /// <returns>returns the row and column selected using an integer array</returns>
            private int[] getCPUmove()
            {
                sb = new StringBuilder();
                int[] move = minimax(2, Players.CPU);

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Mani\Desktop\C# Projects\debug.txt"))
                {
                    file.WriteLine(sb.ToString());
                }
                //Console.ReadKey();
                _board[move[1], move[2]] = (int)Players.CPU;
                Console.WriteLine("CPU played at: {0},{1}",move[1],move[2]);
                Console.WriteLine(board());
                return new int[] { move[1], move[2] };
            }

            /// <summary>
            /// Evaluates the cost of a move.
            ///     The heuristic evaluation function for the given line of 3 cells
            ///     @Return +100, +10, +1 for 3-, 2-, 1-in-a-line for computer.
            ///     -100, -10, -1 for 3-, 2-, 1-in-a-line for opponent.
            ///       0 otherwise
            /// </summary>
            /// <returns>returns the cost of the move based on heuristic evaluation</returns>
            private int evalMove()
            {
                int score = 0;
                int lM = (int)_lastMove;
                int lCnt1, lCnt2, rCnt1, rCnt2, dCnt1, dCnt2, cdCnt1, cdCnt2;
                lCnt1 = lCnt2 = rCnt1 = rCnt2 = dCnt1 = dCnt2 = cdCnt1 = cdCnt2 = 0;
                for (int i = 0; i < _size; i++)
                {
                    for (int j = 0; j < _size; j++)
                    {
                        //Calculate row seeds
                        if (_board[i, j] == (int)Players.CPU) rCnt1++;
                        else if (_board[i, j] == (int)Players.User) rCnt2++;

                        //Calculate column seeds
                        if (_board[j, i] == (int)Players.CPU) lCnt1++;
                        else if (_board[j, i] == (int)Players.User) lCnt2++;

                        //Calculate diagonal seeds
                        if (i == j)
                        {
                            if (_board[i, j] == (int)Players.CPU) dCnt1++;
                            else if (_board[i, j] == (int)Players.User) dCnt2++;
                        }

                        //Calculate cross diagonal seeds
                        if ((i + j) == (_size - 1))
                        {
                            if (_board[i, j] == (int)Players.CPU) cdCnt1++;
                            else if (_board[i, j] == (int)Players.User) cdCnt2++;
                        }

                    }
                    score += calScore(rCnt1, rCnt2);    //Update with row scores
                    score += calScore(lCnt1, lCnt2);    //Update with col scores
                    rCnt1 = rCnt2 = lCnt1 = lCnt2 = 0;
                }
                score += calScore(dCnt1, dCnt2);    //Update with diagonal scores
                score += calScore(cdCnt1, cdCnt2);  //Update with cross diagonal scores
                sb.Append(board());
                sb.Append("SCORE EVAL: " + score + "\n");
                return score;
            }

            private String board()
            {
                StringBuilder s =new StringBuilder();
                for (int i = 0; i < _size; i++)
                {
                    for (int j = 0; j < _size; j++)
                    {
                        if (_board[i, j] == (int)Players.CPU) s.Append("X ");
                        else if (_board[i, j] == (int)Players.User) s.Append("O ");
                        else s.Append("_ ");
                    }
                    s.Append("\n");
                }
                return s.ToString();
            }

            /// <summary>
            /// Calculates the cost with the given counters
            /// </summary>
            /// <param name="myCnt">number of seeds CPU already has on row/column/diagonal</param>
            /// <param name="oppCnt">number of seed user already has on the row/column/diagonal</param>
            /// <returns>cost of row/column/diagonal</returns>
            private int calScore(int cpuCnt, int usrCnt)
            {
                if (cpuCnt == 0 && usrCnt == 0) return 1;
                else if (cpuCnt > 0 && usrCnt > 0) return 0;
                else if (cpuCnt > 0) return (int)Math.Pow(10, cpuCnt-1);
                else return (-1 * (int)Math.Pow(10, usrCnt-1));
            }

            /// <summary>
            /// Generates the list of all empty cells on the board
            /// </summary>
            /// <returns>The list of all empty cells on the board</returns>
            private List<int[]> generateMoves()
            {
                List<int[]> moves = new List<int[]>();

                if (hasWon()) return moves;

                for (int i = 0; i < _size; i++)
                {
                    for (int j = 0; j < _size; j++)
                    {
                        if (_board[i, j] == 0) moves.Add(new int[] { i, j });
                    }
                }
                return moves;

            }

            /// <summary>
            /// Recursive minimax algorirthm for either minimizing or maximizing a player's scors
            /// </summary>
            /// <param name="depth">Current depth</param>
            /// <param name="player">Current minimax player</param>
            /// <returns>The best move in the form of an int array of {best score, row, column}</returns>
            private int[] minimax(int depth, Players player)
            {
                // Generate possible next moves in a List of int[2] of {row, col}.
                List<int[]> nextMoves = generateMoves();

                // mySeed is maximizing; while oppSeed is minimizing
                int bestScore = (player == Players.CPU) ? Int32.MinValue : Int32.MaxValue;
                int currentScore = 0;
                int bestRow = -1;
                int bestCol = -1;
                int pl = (int)player;
                cnt++;
                if (nextMoves.Count == 0 || depth == 0)
                {
                    // Gameover or depth reached, evaluate score
                    bestScore = evalMove();
                }
                else
                {
                    foreach (int[] move in nextMoves)
                    {
                        // Try this move for the current "player"
                        _board[move[0], move[1]] = pl;
                        if (player == Players.CPU)
                        {
                            // mySeed (computer) is maximizing player
                            currentScore = minimax(depth - 1, Players.User)[0];
                            if (currentScore > bestScore)
                            {
                                bestScore = currentScore;
                                bestRow = move[0];
                                bestCol = move[1];
                            }
                            sb.Append("Depth: " + depth + " BS: " + bestScore + " BR: " + bestRow + " BC: " + bestCol + " Player: " + player +  " CS: " + currentScore +"\n");
                        }
                        else
                        {
                            // oppSeed is minimizing player
                            currentScore = minimax(depth - 1, Players.CPU)[0];
                            if (currentScore < bestScore)
                            {
                                bestScore = currentScore;
                                bestRow = move[0];
                                bestCol = move[1];
                            }
                            sb.Append("Depth: " + depth + " BS: " + bestScore + " BR: " + bestRow + " BC: " + bestCol + " Player: " + player + " CS: " + currentScore + "\n");
                        }
                        //if (cnt == 1) Console.WriteLine("Eval {0},{1} with score {2}/{3}", move[0], move[1], currentScore, bestScore);
                        // Undo move
                        _board[move[0], move[1]] = 0;
                    }
                }
                cnt--;
                return new int[] { bestScore, bestRow, bestCol };
            }
            #endregion
        }
}
