using System;
using System.Collections.Generic;

namespace SnakeAndLadders.Core
{
    // Main game engine for Snake and Ladders
    public class GameEngine
    {
        private readonly Board _board;
        private readonly List<Player> _players;
        private readonly IDice _dice;
        private int _currentPlayerIndex;
        private bool _gameOver;
        public event EventHandler<GameStateChangedEventArgs>? GameStateChanged;
        public event EventHandler<GameOverEventArgs>? GameOver;

        // Gets the current game state
        public GameState CurrentState { get; private set; }

        // Creates a new game engine with default settings
        public GameEngine() : this(new StandardBoard(), new StandardDice()) { }

        // Creates a new game engine with custom board and dice
        public GameEngine(Board board, IDice dice)
        {
            _board = board;
            _dice = dice;
            _players = new List<Player>();
            _currentPlayerIndex = 0;
            _gameOver = false;
            CurrentState = new GameState(_players, null, 0);
        }

        // Adds a player to the game
        public void AddPlayer(Player player)
        {
            if (_gameOver || CurrentState.MoveCount > 0)
                throw new InvalidOperationException("Cannot add player after game has started");
            
            _players.Add(player);
            CurrentState = new GameState(_players, CurrentState.CurrentPlayer, CurrentState.MoveCount);
            OnGameStateChanged();
        }

        // Makes a move for the current player
        public void MakeMove()
        {
            if (_gameOver)
                throw new InvalidOperationException("Game is over");

            if (_players.Count < 2)
                throw new InvalidOperationException("Need at least 2 players to play");

            Player currentPlayer = _players[_currentPlayerIndex];
            int roll = _dice.Roll();
            
            // Move player
            int newPosition = currentPlayer.Position + roll;
            
            // Check if roll would exceed the board size
            if (newPosition > _board.Size)
            {
                // Implement bounce back mechanic
                // Calculate how much the roll exceeds 100
                int excess = newPosition - _board.Size;
                // The new position is 100 - excess
                newPosition = _board.Size - excess;
            }
            
            // Check if player won by landing exactly on the final square
            if (newPosition == _board.Size)
            {
                currentPlayer.Position = _board.Size;
                _gameOver = true;
                
                CurrentState = new GameState(
                    _players, 
                    currentPlayer, 
                    CurrentState.MoveCount + 1,
                    roll
                );
                
                OnGameStateChanged();
                OnGameOver(currentPlayer);
                return;
            }
            
            // Apply snakes and ladders
            if (_board.HasSnakeOrLadder(newPosition))
            {
                newPosition = _board.GetDestination(newPosition);
            }
            
            currentPlayer.Position = newPosition;
            
            // Next player
            _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;
            
            CurrentState = new GameState(
                _players, 
                _players[_currentPlayerIndex], 
                CurrentState.MoveCount + 1,
                roll
            );
            
            OnGameStateChanged();
        }

        // Resets the game
        public void ResetGame()
        {
            foreach (var player in _players)
            {
                player.Position = 0;
            }
            
            _currentPlayerIndex = 0;
            _gameOver = false;
            CurrentState = new GameState(_players, _players.Count > 0 ? _players[0] : null, 0);
            OnGameStateChanged();
        }

        protected virtual void OnGameStateChanged()
        {
            GameStateChanged?.Invoke(this, new GameStateChangedEventArgs(CurrentState));
        }

        protected virtual void OnGameOver(Player winner)
        {
            GameOver?.Invoke(this, new GameOverEventArgs(winner, CurrentState.MoveCount));
        }
    }

    // Event args for game state changes
    public class GameStateChangedEventArgs : EventArgs
    {
        public GameState GameState { get; }

        public GameStateChangedEventArgs(GameState gameState)
        {
            GameState = gameState;
        }
    }

    // Event args for game over events
    public class GameOverEventArgs : EventArgs
    {
        public Player Winner { get; }
        public int MoveCount { get; }

        public GameOverEventArgs(Player winner, int moveCount)
        {
            Winner = winner;
            MoveCount = moveCount;
        }
    }
}
