using System;

namespace Coup.GameLogic.GameActions
{
    public class StealGameAction : GameAction
    {
        public StealGameAction(Guid playerTakingAction, GameEngine engine, Guid targetPlayer) : base(playerTakingAction, engine)
        {
            Name = "Steal";
            TargetPlayerID = targetPlayer;
            CharactersCounteringAction.Add(Character.Ambassador);
            CharactersCounteringAction.Add(Character.Captain);
            CharacterEnablingAction = Character.Captain;
        }

        public override void ExecuteAction()
        {
            base.ExecuteAction();
            if (!IsChallengedOrCountered)
            {
                Player targetPlayer = _engine.GameState.GetPlayerById(TargetPlayerID);
                Player playerTakingAction = _engine.GameState.GetPlayerById(PlayerTakingActionID);
                if(targetPlayer.Coins >= 2)
                {
                    targetPlayer.TakeCoins(2);
                    playerTakingAction.GiveCoins(2);
                }
                else if(targetPlayer.Coins == 1)
                {
                    targetPlayer.TakeCoins(1);
                    playerTakingAction.GiveCoins(1);
                }
            }
        }
    }
}
