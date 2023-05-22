using System;
using System.Collections.Generic;
using System.Linq;

namespace Coup.GameLogic
{
    public class Player
    {
        public Guid Id { get => _id; }
        public InfluenceSlot[] Influence { get => _influence; }
        public string PlayerName { get; private set; }
        public int Coins { get; private set; }

        private readonly Guid _id;
        private readonly InfluenceSlot[] _influence;

        public Player(string playerName)
        {
            _id = Guid.NewGuid();
            _influence = new InfluenceSlot[2];
            _influence[0] = new InfluenceSlot();
            _influence[1] = new InfluenceSlot();
            PlayerName = playerName;
            Coins = 0;
        }

        public bool IsInfluencingCharacter(Character character)
        {
            return Influence.Any(i => !i.IsRevealed && i.Card.Character == character);
        }

        public void GiveCoins(int coinsToGive)
        {
            Coins += coinsToGive;
        }

        public void TakeCoins(int coinsToTake)
        {
            Coins -= coinsToTake;
        }

        public void InitInfluence(Card firstInfluence, Card secondInfluence)
        {
            Influence[0].Card = firstInfluence;
            Influence[1].Card = secondInfluence;
        }

        public void RevealInfluence(Guid influenceToTake)
        {
            Influence.First(i => i.Card.Id == influenceToTake).IsRevealed = true;
        }

        public bool IsPlayerDefeated()
        {
            return Influence.Count(i => i.IsRevealed) == 2;
        }

        public bool InfluencesCharacter(Character? character)
        {
            return character == null || Influence.Any(i => i.ContainsCharacter((Character)character));
        }
    }
}
