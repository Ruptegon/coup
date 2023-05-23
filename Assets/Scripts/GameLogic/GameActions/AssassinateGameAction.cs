using System;

namespace Coup.GameLogic.GameActions
{
    public class AssassinateGameAction : GameAction
    {
        public AssassinateGameAction(Guid playerTakingAction, GameEngine engine, Guid targetPlayer) : base(playerTakingAction, engine)
        {
            Name = "Assassinate";
            CoinCost = 3;
            TargetPlayerID = targetPlayer;
            CharactersCounteringAction.Add(Character.Contessa);
            CharacterEnablingAction = Character.Assassin;
        }

        public override void ExecuteAction()
        {
            base.ExecuteAction();
            if (!IsChallengedOrCountered)
            {
                _engine.OrderPlayerToPayInfluence(TargetPlayerID);
            }
        }
    }
}
