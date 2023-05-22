using Coup.GameLogic;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Coup
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private int _playerCount;

        public GameEngine GameEngine { get; private set; }

        private List<PlayerController> _playerControllers;
        private List<AIPlayer> _aiPlayers;

        private void Awake()
        {
            GameEngine = new GameEngine();
            SetupNewGame(_playerCount);
        }

        public PlayerController GetHumanPlayerController()
        {
            return _playerControllers.First();
        }

        private void SetupNewGame(int playerCount)
        {
            GameEngine.SetupNewGame(playerCount);
            CreatePlayerControllers(playerCount);
            CreateAIPlayers(playerCount);
        }

        private void CreatePlayerControllers(int playerCount)
        {
            _playerControllers = new List<PlayerController>();
            for (int i = 0; i < playerCount; i++) 
            {
                _playerControllers.Add(new PlayerController(GameEngine.GameState.Players[i].Id, GameEngine));
            }
        }

        /// <summary>
        /// Create AI for each player except the first.
        /// </summary>
        private void CreateAIPlayers(int playerCount)
        {
            _aiPlayers = new List<AIPlayer>();
            for(int i = 1; i < playerCount; i++)
            {
                _aiPlayers.Add(new AIPlayer(_playerControllers[i], GameEngine));
            }
        }
    }
}
