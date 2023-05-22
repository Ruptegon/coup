using Coup.GameLogic.Enums;
using Coup.GameLogic.GameActions.GeneralGameActions;
using System;

namespace Coup.GameLogic
{
    public class PlayerController
    {
        public Guid PlayerId { get => _playerId; }

        private readonly Guid _playerId;
        private readonly GameEngine _engine;

        public PlayerController(Guid playerId, GameEngine engine)
        {
            _playerId = playerId;
            _engine = engine;
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
            }
        }

        public void PickTargetedAction(TargetedGameActionType gameAction, Guid targetId)
        {
            switch (gameAction)
            {
                case TargetedGameActionType.Coup:
                    _engine.ActionPicked(new CoupGameAction(_playerId, _engine, targetId));
                    break;
            }
        }
    }
}
