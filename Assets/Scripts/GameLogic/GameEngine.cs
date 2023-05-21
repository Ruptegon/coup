using System.Collections;
using System.Collections.Generic;

namespace Coup.GameLogic 
{
    public class GameEngine
    {
        private const string DEFAULT_PLAYER_NAME = "Player";

        private Court court;
        private List<PlayerData> players;

        public void SetupGame(int playerCount)
        {
            court = new Court();
            players = new List<PlayerData>();
            for(int i = 0; i < playerCount; i++)
            {
                PlayerData player = new PlayerData(DEFAULT_PLAYER_NAME + i);
                player.GiveCoins(2);
                player.GiveInfluence(court.TakeCard());
                player.GiveInfluence(court.TakeCard());
                players.Add(player);
            }
        }
    }
}
