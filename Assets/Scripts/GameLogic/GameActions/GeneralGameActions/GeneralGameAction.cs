using System;

namespace Coup.GameLogic.GameActions.GeneralGameActions
{
    /// <summary>
    /// General Actions are Actions that can't be challanged, as they are always available.
    /// </summary>
    public abstract class GeneralGameAction : GameAction
    {
        protected GeneralGameAction(Guid playerTakingAction, GameEngine engine) : base(playerTakingAction, engine)
        {
        }
    }
}
