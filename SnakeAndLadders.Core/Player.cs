namespace SnakeAndLadders.Core
{
    // Represents a player in the Snake and Ladders game
    public class Player
    {
        // Gets or sets the player's ID
        public string Id { get; set; }

        // Gets or sets the player's name
        public string Name { get; set; }

        // Gets or sets the player's position on the board
        public int Position { get; set; }

        // Gets or sets the player's token color
        public string TokenColor { get; set; }

        // Creates a new player
        public Player(string id, string name, string tokenColor)
        {
            Id = id;
            Name = name;
            Position = 0; // Starting position
            TokenColor = tokenColor;
        }
    }
}
