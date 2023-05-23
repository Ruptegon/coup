using System;

namespace Coup.GameLogic.GameActions.GeneralGameActions
{
    public class ForeignAidGameAction : GeneralGameAction
    {
        public ForeignAidGameAction(Guid playerTakingAction, GameEngine gameEngine) : base(playerTakingAction, gameEngine)
        {
            Name = "Foreign Aid";
            CoinCost = 0;
            CharactersCounteringAction.Add(Character.Duke);
        }

        public override void ExecuteAction()
        {
            base.ExecuteAction();

            if (!IsChallengedOrCountered)
            {
                Player player = _engine.GameState.GetPlayerById(PlayerTakingActionID);
                player.GiveCoins(2);
            }
        }
    }
}
