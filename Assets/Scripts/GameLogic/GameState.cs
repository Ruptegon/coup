using System;
using System.Collections.Generic;
using System.Linq;

namespace Coup.GameLogic 
{
    public class GameState
    {
        public List<Player> Players { get => _players; }
        public Court Court { get => _court; }

        private List<Player> _players;
        private Court _court;

        public GameState()
        {
            _players = new List<Player>();
            _court = new Court();
        }

        public Player GetPlayerById(Guid playerId)
        {
            return Players.FirstOrDefault(p => p.Id == playerId);
        }
    }
}
