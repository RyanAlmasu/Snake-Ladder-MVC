using SnakeAndLadders.Core;
using System.Collections.Generic;

namespace SnakeAndLadders.Models
{
    public class GameViewModel
    {
        public string GameId { get; set; }
        public List<PlayerViewModel> Players { get; set; }
        public BoardViewModel Board { get; set; }
        public string? CurrentPlayerId { get; set; }
        public int MoveCount { get; set; }
        public int? LastRoll { get; set; }
        public bool IsGameOver { get; set; }
        public string? WinnerId { get; set; }

        public GameViewModel()
        {
            GameId = string.Empty;
            Players = new List<PlayerViewModel>();
            Board = new BoardViewModel();
        }

        public GameViewModel(string gameId, GameState gameState, bool isGameOver, string? winnerId = null)
        {
            GameId = gameId;
            Players = gameState.Players.Select(p => new PlayerViewModel(p)).ToList();
            Board = new BoardViewModel();
            CurrentPlayerId = gameState.CurrentPlayer?.Id;
            MoveCount = gameState.MoveCount;
            LastRoll = gameState.LastRoll;
            IsGameOver = isGameOver;
            WinnerId = winnerId;
        }
    }

    public class PlayerViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public string TokenColor { get; set; }

        public PlayerViewModel()
        {
            Id = string.Empty;
            Name = string.Empty;
            TokenColor = "#000000";
        }

        public PlayerViewModel(Player player)
        {
            Id = player.Id;
            Name = player.Name;
            Position = player.Position;
            TokenColor = player.TokenColor;
        }
    }

    public class BoardViewModel
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public List<SnakeViewModel> Snakes { get; set; } = new List<SnakeViewModel>();
        public List<LadderViewModel> Ladders { get; set; } = new List<LadderViewModel>();

        public BoardViewModel()
        {
            // Create a standard board to get the default configuration
            var standardBoard = new StandardBoard();
            
            // Get board dimensions from standard board
            Rows = standardBoard.Rows;
            Columns = standardBoard.Columns;
            
            // Get snakes and ladders from the standard board
            Snakes = standardBoard.GetSnakes().Select(s => new SnakeViewModel
            { 
                HeadPosition = s.Head, 
                TailPosition = s.Tail 
            }).ToList();
            
            Ladders = standardBoard.GetLadders().Select(l => new LadderViewModel
            { 
                BottomPosition = l.Bottom, 
                TopPosition = l.Top 
            }).ToList();
        }

        public BoardViewModel(Board board)
        {
            Rows = board.Rows;
            Columns = board.Columns;
            
            Snakes = board.GetSnakes().Select(s => new SnakeViewModel
            { 
                HeadPosition = s.Head, 
                TailPosition = s.Tail 
            }).ToList();
            
            Ladders = board.GetLadders().Select(l => new LadderViewModel
            { 
                BottomPosition = l.Bottom, 
                TopPosition = l.Top 
            }).ToList();
        }
    }

    public class SnakeViewModel
    {
        public int HeadPosition { get; set; }
        public int TailPosition { get; set; }
    }

    public class LadderViewModel
    {
        public int BottomPosition { get; set; }
        public int TopPosition { get; set; }
    }

    public class NewGameViewModel
    {
        public string? GameId { get; set; }
        public List<NewPlayerViewModel> Players { get; set; } = new List<NewPlayerViewModel>();
    }

    public class NewPlayerViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string TokenColor { get; set; } = "#000000";
    }
}
