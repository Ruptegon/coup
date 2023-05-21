using System;
using System.Collections.Generic;

namespace Coup.GameLogic
{
    public class PlayerData
    {
        public Guid Id { get; private set; }
        public string PlayerName { get; private set; }
        public int Coins { get; private set; }
        public List<Card> Influence { get; private set; }

        public PlayerData(string playerName)
        {
            Id = Guid.NewGuid();
            PlayerName = playerName;
            Coins = 0;
            Influence = new List<Card>();
        }

        public void GiveCoins(int coinsToGive)
        {
            Coins += coinsToGive;
        }

        public void TakeCoins(int coinsToTake)
        {
            Coins -= coinsToTake;
        }

        public void GiveInfluence(Card influenceToGive)
        {
            Influence.Add(influenceToGive);
        }

        public void TakeInfluence(Card influenceToTake)
        {
            Influence.Remove(influenceToTake);
        }
    }
}
