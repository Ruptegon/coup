using System;

namespace Coup.GameLogic.GameActions.GeneralGameActions
{
    public class CoupGameAction : GeneralGameAction
    {
        public CoupGameAction(Guid playerTakingAction, GameEngine gameEngine, Guid targetPlayer) : base(playerTakingAction, gameEngine)
        {
            Name = "Coup";
            CoinCost = 7;
            TargetPlayerID = targetPlayer;
        }

        public override void ExecuteAction()
        {
            base.ExecuteAction();
            _engine.OrderPlayerToPayInfluence(TargetPlayerID);
        }
    }
}
