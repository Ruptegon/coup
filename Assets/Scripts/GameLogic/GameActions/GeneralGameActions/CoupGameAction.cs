using System;

namespace Coup.GameLogic.GameActions.GeneralGameActions
{
    public class CoupGameAction : GeneralGameAction
    {
        private Guid _targetPlayerID;

        public CoupGameAction(Guid playerTakingAction, GameEngine gameEngine, Guid targetPlayer) : base(playerTakingAction, gameEngine)
        {
            CoinCost = 7;
            _targetPlayerID = targetPlayer;
        }

        public override void ExecuteAction()
        {
            base.ExecuteAction();
            _engine.OrderPlayerToPayInfluence(_targetPlayerID);
        }
    }
}
