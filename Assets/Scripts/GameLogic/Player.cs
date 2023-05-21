using System;
using System.Collections.Generic;
using System.Linq;

namespace Coup.GameLogic
{
    public class Player
    {
        public Guid Id { get => _id; }
        public List<Card> Influence { get => _influence; }
        public string PlayerName { get; private set; }
        public int Coins { get; private set; }

        private readonly Guid _id;
        private readonly List<Card> _influence;

        public Player(string playerName)
        {
            _id = Guid.NewGuid();
            _influence = new List<Card>();
            PlayerName = playerName;
            Coins = 0;
        }

        public bool IsInfluencingCharacter(Character character)
        {
            return Influence.Any(c => c.Character == character);
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

        public bool IsPlayerDefeated()
        {
            return Influence.Count > 0;
        }

        public Card FindCardById(Guid cardId)
        {
            return Influence.FirstOrDefault(c => c.Id == cardId);
        }
    }
}
