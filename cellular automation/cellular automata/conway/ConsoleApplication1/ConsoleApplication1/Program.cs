using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    //Main enterance class to program
    class Program
    {
        static void Main(string[] args)
        {
                Grid grid = new Grid(47);
                grid.run(2000);   
        }
    }

    class Grid
    {
        //board size - will be square
        int dimension;
        //boolean 2D array representing the states of the cells
        Boolean[,] board;
        //2D integer array to represent the neighbours of each cell
        int[,] neighbourCount;
        public Grid(int _x)
        {
            dimension = _x;
            board = new Boolean[dimension, dimension];
            neighbourCount = new int[dimension, dimension];
            initialStates();
            //initialStatesRandom();
            drawBoard();
        }

        //set random initial states as active
        private void initialStatesRandom()
        {
            Random r = new Random();
            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    if (r.Next(2) == 1)
                        board[i, j] = true;
                    else
                        board[i, j] = false;
                }
            }
        }

        //set up initial active cells
        private void initialStates()
        {
            //cross
            board[20, 20] = true;
            board[21, 21] = true;
            board[22, 22] = true;
            board[23, 23] = true;
            board[24, 24] = true;
            board[25, 25] = true;
            board[26, 26] = true;

            board[20, 26] = true;
            board[21, 25] = true;
            board[22, 24] = true;
            board[23, 23] = true;
            board[24, 22] = true;
            board[25, 21] = true;
            board[26, 20] = true;

            //box
            board[20, 20] = true;
            board[20, 21] = true;
            board[20, 22] = true;
            board[20, 23] = true;
            board[20, 24] = true;
            board[20, 25] = true;
            board[20, 26] = true;

            board[26, 20] = true;
            board[26, 21] = true;
            board[26, 22] = true;
            board[26, 23] = true;
            board[26, 24] = true;
            board[26, 25] = true;
            board[26, 26] = true;

            board[20, 26] = true;
            board[21, 26] = true;
            board[22, 26] = true;
            board[23, 26] = true;
            board[24, 26] = true;
            board[25, 26] = true;
            board[26, 26] = true;

            board[20, 20] = true;
            board[21, 20] = true;
            board[22, 20] = true;
            board[23, 20] = true;
            board[24, 20] = true;
            board[25, 20] = true;
            board[26, 20] = true;
            
        }

        //Gets copy of current board to work on for next generation
        private Boolean[,] getCopy()
        {
            Boolean[,] ret = new Boolean[dimension, dimension];
            Array.Copy(board, ret, dimension);
            return ret;
        }

        public void run(int generations)
        {
            int count = 0;
            drawBoard();
            //drawCount();
            Console.WriteLine("Press any key to begin");
            Console.ReadKey();
            
            while(count < generations)
            {
                getNextState();
                drawBoard();
               //drawCount();
                Console.WriteLine("Press any key to go to next state of board");
                Console.ReadKey();
                count++;
            }
        }

        //gets next solution by getting a copy of the current board and manipulating the copy based on the original board
        public void getNextState()
        {
            Boolean[,] copy = getCopy();
            for(int i = 0; i<dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    copy[i, j] = getNextCellState(i, j);
                }
            }
            board = copy;
        }

        //gets next state of cell represented at board[x,y]
        public Boolean getNextCellState(int x, int y)
        {
            int position = isEdge(x, y); 
            if(position == -1)
            {
                int liveCount = 0;
                //count active cells in 3x3 grid of current cell
                for(int i = x-1; i<=x+1; i++)
                {
                    for(int j = y-1; j<= y+1; j++)
                    {
                        if(board[i,j])
                        {
                            liveCount++;
                        }
                    }
                }
                //discount current cell if active
                if (board[x, y])
                    liveCount--;
                neighbourCount[x, y] = liveCount;
                return implementRules(x, y, liveCount);
            }
            else
            {
                return getNextCellStateSpecial(x, y, position);
            }
        }

        //the rules that determine the cell (board[x,y]) state based on the number of neighbours of the cell (count)
        public Boolean implementRules(int x, int y, int count)
        {
            Boolean status = board[x, y];
            if(status)
            {
                if (count < 2 || count > 4)
                    return false;
            }
            else
            {
                if (count >= 3 && count <= 4)
                    return true;
            }
            return status;
        }

        //gets next state of cell at board[x,y] based on an edge case represented by position
        private Boolean getNextCellStateSpecial(int x, int y, int position)
        {
            int liveCount = 0;
            switch(position)
            {
                    //top left corner
                    case 0:
                    {
                        for (int i = 0; i <= 1; i++)
                        {
                            for(int j = 0; j<=1; j++)
                            {
                                if (board[i, j])
                                    liveCount++;
                            }
                        }
                        break;
                    }
                    //top right corner
                    case 1:
                    {
                        for (int i = 0; i <= 1; i++)
                        {
                            for (int j = dimension - 2; j <= dimension - 1; j++)
                            {
                                if (board[i, j])
                                    liveCount++;
                            }
                        }
                        break;
                    }
                    //bottom right corner
                    case 2:
                    {
                        for (int i = dimension - 2; i <= dimension - 1; i++)
                        {
                            for (int j = dimension - 2; j <= dimension - 1; j++)
                            {
                                if (board[i, j])
                                    liveCount++;
                            }
                        }
                        break;
                    }
                    //bottom left corner
                    case 3:
                    {
                        for (int i = dimension - 2; i <= dimension - 1; i++)
                        {
                            for (int j = 0; j <= 1; j++)
                            {
                                if (board[i, j])
                                    liveCount++;
                            }
                        }
                        break;
                    }

                    //left edge
                    case 4:
                    {
                        for (int i = x - 1; i <= x + 1; i++)
                        {
                            for (int j = 0; j <= 1; j++)
                            {
                                if (board[i, j])
                                    liveCount++;
                            }
                        }
                        break;
                    }

                    //top edge
                    case 5:
                    {
                        for (int i = 0; i <= 1; i++)
                        {
                            for (int j = y - 1; j <= y + 1; j++)
                            {
                                if (board[i, j])
                                    liveCount++;
                            }
                        }
                        break;
                    }

                    //right edge
                    case 6:
                    {
                        for (int i = x-1; i <= x+1; i++)
                        {
                            for (int j = dimension-2; j <= dimension-1; j++)
                            {
                                if (board[i, j])
                                    liveCount++;
                            }
                        }
                        break;
                    }

                    //bottom edge
                    case 7:
                    {
                        for (int i = dimension-2; i <= dimension-1; i++)
                        {
                            for (int j = y - 1; j <= y + 1; j++)
                            {
                                if (board[i, j])
                                    liveCount++;
                            }
                        }
                        break;
                    }
            }
            if (board[x, y])
                liveCount--;
            neighbourCount[x, y] = liveCount;
            return implementRules(x, y, liveCount);
        }


        /**
         * returns an int that represents an edge case
         * case 0: top left corner x
         * case 1: top right corner x
         * case 2: bottom right corner x
         * case 3: bottom left corner x
         * case 4: left edge x
         * case 5: top edge x
         * case 6: right edge x
         * case 7: bottom edge x
         **/
        public int isEdge(int x, int y)
        {
            if (x == 0)
            {
                if (y == 0)
                    return 0;
                if (y == dimension - 1)
                    return 1;
                return 5;
            }

            if (x == dimension - 1)
            {
                if (y == 0)
                    return 3;
                if (y == dimension - 1)
                    return 2;
                return 7;
            }

            if (y == 0)
                return 4;
            if (y == dimension - 1)
                return 6;
            return -1;
        }

        //draw board with x as an active cell and a space for a deactivated cell
        public void drawBoard()
        {
            string output = "";
            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
			    {
                    if (board[i, j])
                        output += "x";
                    else
                        output += " ";
			    }
                output += "\n";
			}
            Console.Clear();
            Console.Write(output);
        }

        public void drawCount()
        {
            string output = "";
            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    output += neighbourCount[i, j];
                }
                output += "\n";
            }
            Console.Write(output);
        }
    }
}
