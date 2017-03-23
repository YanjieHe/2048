using System;
using System.Collections.Generic;

namespace Game2048
{
    public class Lattice
    {
        int[] cells;
        int nRows = 4;
        int nCols = 4;
        Random rnd = new Random();

        public Lattice()
        {
            this.cells = new int[nRows * nCols];
        }

        public int this [int row, int col]
        {
            get
            {
                return this.cells[row * nCols + col];
            }
            set
            {
                this.cells[row * nCols + col] = value;
            }
        }

        public void Reset()
        {
            for (int i = 0; i < cells.Length; i++)
            {
                cells[i] = 0;
            }
            this.RandomSet();
            this.RandomSet();
        }

        public void Display()
        {
            for (int i = 0; i < nRows; i++)
            {
                for (int j = 0; j < nCols; j++)
                {
                    Console.Write("{0}\t", this[i, j]);
                }
                Console.WriteLine();
            }
        }

        public bool TryShift(Direction direction)
        {
            if (CanShift(direction))
            {
                Shift(direction);
                while (CanShift(direction))
                {
                    Shift(direction);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        bool IsFull
        {
            get
            {
                for (int i = 0; i < this.cells.Length; i++)
                {
                    if (this.cells[i] == 0)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public bool IsGameOver
        {
            get
            {
                if (IsFull)
                {
                    if (!CanShiftUp() && !CanShiftDown() && !CanShiftLeft() && !CanShiftRight())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        int RandomSelect()
        {
            var indices = new List<int>();
            for (int i = 0; i < this.cells.Length; i++)
            {
                if (this.cells[i] == 0)
                {
                    indices.Add(i);
                }
            }
            return indices[rnd.Next(indices.Count)];
        }

        public void RandomSet()
        {
            int number = rnd.Next(2) * 2 + 2;
            this.cells[RandomSelect()] = number;
        }

        bool CanShift(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return CanShiftUp();
                case Direction.Down:
                    return CanShiftDown();
                case Direction.Left:
                    return CanShiftLeft();
                default: // Right
                    return CanShiftRight();
            }
        }

        bool CanShiftUp()
        {
            for (int i = 1; i < nRows; i++)
            {
                for (int j = 0; j < nCols; j++)
                {
                    if (this[i - 1, j] == 0 && this[i, j] != 0)
                    {
                        return true;
                    }
                    else if (this[i, j] != 0 && this[i, j] == this[i - 1, j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        bool CanShiftDown()
        {
            for (int i = nRows - 2; i >= 0; i--)
            {
                for (int j = 0; j < nCols; j++)
                {
                    if (this[i + 1, j] == 0 && this[i, j] != 0)
                    {
                        return true;
                    }
                    else if (this[i, j] != 0 && this[i, j] == this[i + 1, j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        bool CanShiftLeft()
        {
            for (int j = 1; j < nCols; j++)
            {
                for (int i = 0; i < nRows; i++)
                {
                    if (this[i, j - 1] == 0 && this[i, j] != 0)
                    {
                        return true;
                    }
                    else if (this[i, j] != 0 && this[i, j] == this[i, j - 1])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        bool CanShiftRight()
        {
            for (int j = nCols - 2; j >= 0; j--)
            {
                for (int i = 0; i < nRows; i++)
                {
                    if (this[i, j + 1] == 0 && this[i, j] != 0)
                    {
                        return true;
                    }
                    else if (this[i, j] != 0 && this[i, j] == this[i, j + 1])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        void Move(int origin_x, int origin_y, int dest_x, int dest_y)
        {
            this[dest_x, dest_y] = this[origin_x, origin_y];
            this[origin_x, origin_y] = 0;
        }

        void Merge(int origin_x, int origin_y, int dest_x, int dest_y)
        {
            this[dest_x, dest_y] += this[origin_x, origin_y];
            this[origin_x, origin_y] = 0;
        }

        void Shift(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    ShiftUp();
                    break;
                case Direction.Down:
                    ShiftDown();
                    break;
                case Direction.Left:
                    ShiftLeft();
                    break;
                default: // Right
                    ShiftRight();
                    break;
            }
        }

        void ShiftUp()
        {
            for (int i = 1; i < nRows; i++)
            {
                for (int j = 0; j < nCols; j++)
                {
                    if (this[i - 1, j] == 0 && this[i, j] != 0)
                    {
                        Move(i, j, i - 1, j);
                    }
                    else if (this[i, j] != 0 && this[i, j] == this[i - 1, j])
                    {
                        Merge(i, j, i - 1, j);
                    }
                }
            }
        }

        void ShiftDown()
        {
            for (int i = nRows - 2; i >= 0; i--)
            {
                for (int j = 0; j < nCols; j++)
                {
                    if (this[i + 1, j] == 0 && this[i, j] != 0)
                    {
                        Move(i, j, i + 1, j);
                    }
                    else if (this[i, j] != 0 && this[i, j] == this[i + 1, j])
                    {
                        Merge(i, j, i + 1, j);
                    }
                }
            }
        }

        void ShiftLeft()
        {
            for (int j = 1; j < nCols; j++)
            {
                for (int i = 0; i < nRows; i++)
                {
                    if (this[i, j - 1] == 0 && this[i, j] != 0)
                    {
                        Move(i, j, i, j - 1);
                    }
                    else if (this[i, j] != 0 && this[i, j] == this[i, j - 1])
                    {
                        Merge(i, j, i, j - 1);
                    }
                }
            }
        }

        void ShiftRight()
        {
            for (int j = nCols - 2; j >= 0; j--)
            {
                for (int i = 0; i < nRows; i++)
                {
                    if (this[i, j + 1] == 0 && this[i, j] != 0)
                    {
                        Move(i, j, i, j + 1);
                    }
                    else if (this[i, j] != 0 && this[i, j] == this[i, j + 1])
                    {
                        Merge(i, j, i, j + 1);
                    }
                }
            }
        }


    }
}

