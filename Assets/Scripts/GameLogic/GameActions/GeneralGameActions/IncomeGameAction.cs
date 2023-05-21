using System;

namespace Coup.GameLogic.GameActions.GeneralGameActions
{
    public class IncomeGameAction : GeneralGameAction
    {
        public IncomeGameAction(Guid playerTakingAction, GameEngine gameEngine) : base(playerTakingAction, gameEngine)
        {
            CoinCost = 0;
        }

        public override void ExecuteAction()
        {
            base.ExecuteAction();

            Player player = _engine.GameState.GetPlayerById(PlayerTakingActionID);
            player.GiveCoins(1);
        }
    }
}
