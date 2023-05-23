using System;

namespace Coup.GameLogic.GameActions
{
    public class ExchangeGameAction : GameAction
    {
        public ExchangeGameAction(Guid playerTakingAction, GameEngine engine) : base(playerTakingAction, engine)
        {
            Name = "Exchange";
            CharacterEnablingAction = Character.Ambassador;
        }

        public override void ExecuteAction()
        {
            base.ExecuteAction();

            //Placeholder for full exchange action, allowing picking which cards to keep
            if (!IsChallengedOrCountered)
            {
                Player player = _engine.GameState.GetPlayerById(PlayerTakingActionID);

                for(int i = 0; i < player.Influence.Length; i++)
                {
                    if (!player.Influence[i].IsRevealed) 
                    {
                        _engine.GameState.Court.ReturnCardToCourt(player.Influence[i].Card);
                    }
                }
                _engine.GameState.Court.Reshuffle();
                for(int i = 0; i < player.Influence.Length; i++)
                {
                    if (!player.Influence[i].IsRevealed)
                    {
                        player.Influence[i].Card = _engine.GameState.Court.TakeCard();
                    }
                }
            }
        }
    }
}
