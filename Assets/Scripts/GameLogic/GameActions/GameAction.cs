using System;
using System.Collections.Generic;

namespace Coup.GameLogic.GameActions
{
    public abstract class GameAction
    {
        public Guid PlayerTakingActionID { get => _playerTakingActionID; }
        public int CoinCost { get; protected set; }
        public Character? CharacterEnablingAction { get; protected set; }
        public List<Character> CharactersCounteringAction { get; protected set; }
        public bool WasChallengedOrCountered { get; set; }
        
        protected GameEngine _engine;
        private readonly Guid _playerTakingActionID;

        public GameAction (Guid playerTakingAction, GameEngine engine)
        {
            _playerTakingActionID = playerTakingAction;
            CharacterEnablingAction = null;
            CharactersCounteringAction = new List<Character>();
            WasChallengedOrCountered = false;
            _engine = engine;
        }

        public bool CanAffordAction()
        {
            return _engine.GameState.GetPlayerById(PlayerTakingActionID).Coins >= CoinCost;
        }

        public bool IsActionCounterable()
        {
            return CharactersCounteringAction.Count > 0;
        }

        public virtual void ExecuteAction()
        {
            Player player = _engine.GameState.GetPlayerById(PlayerTakingActionID);

            if (!CanAffordAction())
            {
                throw new Exception($"Player {player.PlayerName} can't afford chosen action.");
            }

            player.TakeCoins(CoinCost);
        }
    }
}