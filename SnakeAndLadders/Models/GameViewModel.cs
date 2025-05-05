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
        public int Rows { get; set; } = 10;
        public int Columns { get; set; } = 10;
        public List<SnakeViewModel> Snakes { get; set; } = new List<SnakeViewModel>();
        public List<LadderViewModel> Ladders { get; set; } = new List<LadderViewModel>();

        public BoardViewModel()
        {
            // Default snakes for a standard 10x10 board
            Snakes = new List<SnakeViewModel>
            {
                // new SnakeViewModel { HeadPosition = 98, TailPosition = 78 },
                // new SnakeViewModel { HeadPosition = 95, TailPosition = 75 },
                // new SnakeViewModel { HeadPosition = 93, TailPosition = 73 },
                // new SnakeViewModel { HeadPosition = 87, TailPosition = 24 },
                new SnakeViewModel { HeadPosition = 64, TailPosition = 60 },
                new SnakeViewModel { HeadPosition = 62, TailPosition = 19 },
                new SnakeViewModel { HeadPosition = 54, TailPosition = 34 },
                new SnakeViewModel { HeadPosition = 17, TailPosition = 7 }
            };

            // Default ladders for a standard 10x10 board
            Ladders = new List<LadderViewModel>
            {
                // new LadderViewModel { BottomPosition = 1, TopPosition = 38 },
                // new LadderViewModel { BottomPosition = 4, TopPosition = 14 },
                // new LadderViewModel { BottomPosition = 9, TopPosition = 31 },
                // new LadderViewModel { BottomPosition = 21, TopPosition = 42 },
                // new LadderViewModel { BottomPosition = 28, TopPosition = 84 },
                new LadderViewModel { BottomPosition = 36, TopPosition = 44 },
                new LadderViewModel { BottomPosition = 51, TopPosition = 67 },
                new LadderViewModel { BottomPosition = 71, TopPosition = 91 },
                // new LadderViewModel { BottomPosition = 80, TopPosition = 100 }
            };
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
