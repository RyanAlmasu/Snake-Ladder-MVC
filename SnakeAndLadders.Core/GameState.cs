using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeAndLadders.Core
{
    // Represents the current state of a Snake and Ladders game
    public class GameState
    {
        // Gets a list of players in the game
        public IReadOnlyList<Player> Players { get; }

        // Gets the current player
        public Player? CurrentPlayer { get; }

        // Gets the number of moves made in the game
        public int MoveCount { get; }

        // Gets the last dice roll value
        public int? LastRoll { get; }

        // Creates a new game state
        public GameState(IEnumerable<Player> players, Player? currentPlayer, int moveCount, int? lastRoll = null)
        {
            Players = players.ToList();
            CurrentPlayer = currentPlayer;
            MoveCount = moveCount;
            LastRoll = lastRoll;
        }

        // Gets a player by ID
        public Player? GetPlayerById(string id)
        {
            return Players.FirstOrDefault(p => p.Id == id);
        }

        // Gets the leader (player with highest position)
        public Player? GetLeader()
        {
            return Players.OrderByDescending(p => p.Position).FirstOrDefault();
        }

        // Gets the player standings ordered by position
        public IEnumerable<Player> GetStandings()
        {
            return Players.OrderByDescending(p => p.Position);
        }
    }
}
