using System;

namespace Coup.GameLogic.GameActions
{
    public class AssassinateGameAction : GameAction
    {
        private Guid _targetPlayerID;

        public AssassinateGameAction(Guid playerTakingAction, GameEngine engine, Guid targetPlayer) : base(playerTakingAction, engine)
        {
            Name = "Assassinate";
            CoinCost = 3;
            _targetPlayerID = targetPlayer;
            CharactersCounteringAction.Add(Character.Contessa);
            CharacterEnablingAction = Character.Assassin;
        }

        public override void ExecuteAction()
        {
            base.ExecuteAction();
            if (!IsChallengedOrCountered)
            {
                _engine.OrderPlayerToPayInfluence(_targetPlayerID);
            }
        }
    }
}
