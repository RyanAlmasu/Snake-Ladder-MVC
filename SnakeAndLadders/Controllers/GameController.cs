using Microsoft.AspNetCore.Mvc;
using SnakeAndLadders.Core;
using SnakeAndLadders.Models;
using SnakeAndLadders.Services;
using System;

namespace SnakeAndLadders.Controllers
{
    public class GameController : Controller
    {
        private readonly GameService _gameService;
        private readonly ILogger<GameController> _logger;

        public GameController(GameService gameService, ILogger<GameController> logger)
        {
            _gameService = gameService;
            _logger = logger;
        }

        // GET: Game
        public IActionResult Index()
        {
            return View();
        }

        // GET: Game/New
        public IActionResult New()
        {
            var model = new NewGameViewModel
            {
                GameId = Guid.NewGuid().ToString(),
                Players = new List<NewPlayerViewModel>
                {
                    new NewPlayerViewModel { Name = "Player 1", TokenColor = "#FF0000" },
                    new NewPlayerViewModel { Name = "Player 2", TokenColor = "#0000FF" }
                }
            };

            return View(model);
        }

        // POST: Game/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(NewGameViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("New", model);
            }

            if (model.Players.Count < 2)
            {
                ModelState.AddModelError("", "You need at least 2 players to play.");
                return View("New", model);
            }

            try
            {
                // Create a new game
                var gameId = model.GameId ?? Guid.NewGuid().ToString("N");
                var gameEngine = _gameService.CreateGame(gameId);

                // Add players to the game
                foreach (var playerModel in model.Players)
                {
                    var playerId = Guid.NewGuid().ToString("N");
                    var player = new Player(playerId, playerModel.Name, playerModel.TokenColor);
                    gameEngine.AddPlayer(player);
                }

                // Redirect to the game board
                return RedirectToAction("Play", new { id = gameId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating game");
                ModelState.AddModelError("", $"Error creating game: {ex.Message}");
                return View("New", model);
            }
        }

        // GET: Game/Play/{id}
        public IActionResult Play(string id)
        {
            try
            {
                var gameEngine = _gameService.GetGame(id);
                var gameState = gameEngine.CurrentState;
                var isGameOver = false;

                string? winnerId = null;
                if (gameState.Players.Any(p => p.Position == 100))
                {
                    isGameOver = true;
                    winnerId = gameState.Players.First(p => p.Position == 100).Id;
                }

                var model = new GameViewModel(id, gameState, isGameOver, winnerId);
                return View(model);
            }
            catch (KeyNotFoundException)
            {
                return RedirectToAction("New");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading game");
                TempData["ErrorMessage"] = $"Error loading game: {ex.Message}";
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Game/RollDice/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RollDice(string id)
        {
            try
            {
                var gameEngine = _gameService.GetGame(id);
                gameEngine.MakeMove();
                return RedirectToAction("Play", new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rolling dice");
                TempData["ErrorMessage"] = $"Error rolling dice: {ex.Message}";
                return RedirectToAction("Play", new { id });
            }
        }

        // POST: Game/Reset/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reset(string id)
        {
            try
            {
                var gameEngine = _gameService.GetGame(id);
                gameEngine.ResetGame();
                return RedirectToAction("Play", new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting game");
                TempData["ErrorMessage"] = $"Error resetting game: {ex.Message}";
                return RedirectToAction("Play", new { id });
            }
        }

        // POST: Game/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string id)
        {
            try
            {
                _gameService.RemoveGame(id);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting game");
                TempData["ErrorMessage"] = $"Error deleting game: {ex.Message}";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
