using Coup.GameLogic.Enums;
using Coup.GameLogic.GameActions;
using Coup.GameLogic.GameActions.GeneralGameActions;
using System;

namespace Coup.GameLogic
{
    public class PlayerController
    {
        public Player Player { get; private set; }

        private readonly Guid _playerId;
        private readonly GameEngine _engine;

        public PlayerController(Guid playerId, GameEngine engine)
        {
            _playerId = playerId;
            _engine = engine;
            Player = _engine.GameState.GetPlayerById(playerId);
        }

        public void PickPersonalAction(PersonalGameActionType gameAction)
        {
            switch (gameAction)
            {
                case PersonalGameActionType.Income:
                    _engine.ActionPicked(new IncomeGameAction(_playerId, _engine));
                    break;
                case PersonalGameActionType.ForeignAid:
                    _engine.ActionPicked(new ForeignAidGameAction(_playerId, _engine));
                    break;
                case PersonalGameActionType.Tax:
                    _engine.ActionPicked(new TaxGameAction(_playerId, _engine));
                    break;
                case PersonalGameActionType.Exchange:
                    _engine.ActionPicked(new ExchangeGameAction(_playerId, _engine));
                    break;
            }
        }

        public void PickTargetedAction(TargetedGameActionType gameAction, Guid targetId)
        {
            switch (gameAction)
            {
                case TargetedGameActionType.Coup:
                    _engine.ActionPicked(new CoupGameAction(_playerId, _engine, targetId));
                    break;
                case TargetedGameActionType.Assassinate:
                    _engine.ActionPicked(new AssassinateGameAction(_playerId, _engine, targetId));
                    break;
                case TargetedGameActionType.Steal:
                    _engine.ActionPicked(new StealGameAction(_playerId, _engine, targetId));
                    break;
            }
        }

        public bool CanAffordTargetedAction(TargetedGameActionType gameAction)
        {
            switch (gameAction) 
            {
                case TargetedGameActionType.Coup:
                    return Player.Coins >= 7;
                case TargetedGameActionType.Assassinate:
                    return Player.Coins >= 3;
                case TargetedGameActionType.Steal:
                    return true;
                default:
                    return false;
            }
        }

        public void PayInfluence(Guid cardId)
        {
            _engine.PayInfluence(_playerId, cardId);
        }

        public void ChallengeAction()
        {
            _engine.ChallengeAction(_playerId);
        }

        public void ChallengeCounter()
        {
            _engine.ChallengeCounter(_playerId);
        }

        public void CounterAction()
        {
            _engine.CounterAction(_playerId);
        }

        public void SkipChallengeOrCounter()
        {
            _engine.SkipChallengeOrCounter(_playerId);
        }
    }
}
