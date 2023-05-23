using System;

namespace Coup.GameLogic.GameActions
{
    public class TaxGameAction : GameAction
    {
        public TaxGameAction(Guid playerTakingAction, GameEngine engine) : base(playerTakingAction, engine)
        {
            Name = "Tax";
            CharacterEnablingAction = Character.Duke;
        }

        public override void ExecuteAction()
        {
            base.ExecuteAction();

            if (!IsChallengedOrCountered) 
            {
                _engine.GameState.GetPlayerById(PlayerTakingActionID).GiveCoins(3);
            }
        }
    }
}
