using SnakeAndLadders.Core;
using System.Collections.Concurrent;

namespace SnakeAndLadders.Services
{
    // Service for managing game instances
    public class GameService
    {
        private readonly ConcurrentDictionary<string, GameEngine> _games;

        public GameService()
        {
            _games = new ConcurrentDictionary<string, GameEngine>();
        }

        // Creates a new game with the specified ID
        public GameEngine CreateGame(string gameId, Board? board = null, IDice? dice = null)
        {
            var gameEngine = new GameEngine(
                board ?? new StandardBoard(),
                dice ?? new StandardDice()
            );

            if (!_games.TryAdd(gameId, gameEngine))
            {
                throw new InvalidOperationException($"Game with ID {gameId} already exists");
            }

            return gameEngine;
        }

        // Gets a game by ID
        public GameEngine GetGame(string gameId)
        {
            if (!_games.TryGetValue(gameId, out var game))
            {
                throw new KeyNotFoundException($"Game with ID {gameId} not found");
            }

            return game;
        }

        // Checks if a game with the specified ID exists
        public bool GameExists(string gameId)
        {
            return _games.ContainsKey(gameId);
        }

        // Removes a game with the specified ID
        public bool RemoveGame(string gameId)
        {
            return _games.TryRemove(gameId, out _);
        }
    }
}
