using System.Collections.Generic;

namespace SnakeAndLadders.Core
{
    // Represents a game board with snakes and ladders
    public abstract class Board
    {
        // Gets the size of the board (total number of cells)
        public abstract int Size { get; }

        // Gets the number of rows on the board
        public abstract int Rows { get; }

        // Gets the number of columns on the board
        public abstract int Columns { get; }

        // Dictionary mapping positions to destinations (snakes and ladders)
        protected Dictionary<int, int> SnakesAndLadders { get; } = new Dictionary<int, int>();

        // Checks if a position has a snake or ladder
        public bool HasSnakeOrLadder(int position)
        {
            return SnakesAndLadders.ContainsKey(position);
        }

        // Gets the destination position for a snake or ladder
        public int GetDestination(int position)
        {
            return SnakesAndLadders.TryGetValue(position, out int destination) ? destination : position;
        }

        // Adds a snake to the board (moves player down)
        protected void AddSnake(int head, int tail)
        {
            if (head <= tail || head >= Size || tail <= 0)
                throw new System.ArgumentException("Snake must move downward and be within board bounds");

            SnakesAndLadders[head] = tail;
        }

        // Adds a ladder to the board (moves player up)
        protected void AddLadder(int bottom, int top)
        {
            if (bottom >= top || top > Size || bottom <= 0)
                throw new System.ArgumentException("Ladder must move upward and be within board bounds");

            SnakesAndLadders[bottom] = top;
        }

        // Returns all snake positions (starting positions)
        public List<(int Head, int Tail)> GetSnakes()
        {
            var snakes = new List<(int, int)>();
            
            foreach (var kvp in SnakesAndLadders)
            {
                if (kvp.Key > kvp.Value)
                {
                    snakes.Add((kvp.Key, kvp.Value));
                }
            }
            
            return snakes;
        }

        // Returns all ladder positions (starting positions)
        public List<(int Bottom, int Top)> GetLadders()
        {
            var ladders = new List<(int, int)>();
            
            foreach (var kvp in SnakesAndLadders)
            {
                if (kvp.Key < kvp.Value)
                {
                    ladders.Add((kvp.Key, kvp.Value));
                }
            }
            
            return ladders;
        }

        // Converts a position to its coordinates on the board (row, column)
        // Accounts for zigzag pattern of the board
        public (int Row, int Column) PositionToCoordinates(int position)
        {
            if (position <= 0 || position > Size)
                return (-1, -1);

            // Adjust position to be 0-indexed for calculation
            position--;
            
            // Calculate the row (0-indexed from the bottom)
            int row = Rows - 1 - (position / Columns);
            
            // Calculate the column based on the row (zigzag pattern)
            int column = (row % 2 == (Rows % 2)) 
                ? position % Columns 
                : Columns - 1 - (position % Columns);
            
            return (row, column);
        }
    }

    // Standard 10x10 Snake and Ladders board
    public class StandardBoard : Board
    {
        public override int Size => 100;
        public override int Rows => 10;
        public override int Columns => 10;

        public StandardBoard()
        {
            // Add standard snakes
            // AddSnake(98, 78);
            // AddSnake(95, 75);
            // AddSnake(93, 73);
            // AddSnake(87, 24);
            AddSnake(64, 60);
            AddSnake(62, 19);
            AddSnake(54, 34);
            AddSnake(17, 7);

            // Add standard ladders
            // AddLadder(1, 38);
            // AddLadder(4, 14);
            // AddLadder(9, 31);
            // AddLadder(21, 42);
            AddLadder(28, 84);
            AddLadder(36, 44);
            AddLadder(51, 67);
            AddLadder(71, 91);
            // AddLadder(80, 100);
        }
    }

    // Custom board that allows custom configuration of board size and snakes/ladders
    public class CustomBoard : Board
    {
        public override int Size { get; }
        public override int Rows { get; }
        public override int Columns { get; }

        public CustomBoard(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            Size = rows * columns;
        }

        public void ConfigureSnake(int head, int tail)
        {
            AddSnake(head, tail);
        }

        public void ConfigureLadder(int bottom, int top)
        {
            AddLadder(bottom, top);
        }
    }
}
