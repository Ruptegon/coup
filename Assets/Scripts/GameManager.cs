using Coup.GameLogic;
using System;
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
        }

        private void CreatePlayerControllers(int playerCount)
        {
            _playerControllers = new List<PlayerController>();
            for (int i = 0; i < playerCount; i++) 
            {
                _playerControllers.Add(new PlayerController(GameEngine.GameState.Players[i].Id, GameEngine));
            }
        }
    }
}
