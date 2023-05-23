using System;
using System.Collections.Generic;
using System.Linq;

namespace Coup.GameLogic
{
    public class Court
    {
        private List<Card> cards;

        public Court()
        {
            cards = new List<Card>();
            InitCourt();
        }

        public void ReturnCardToCourt(Card card)
        {
            cards.Add(card);
        }

        public void Reshuffle()
        {
            Random random = new Random();
            cards = cards.OrderBy(card => random.Next()).ToList();
        }

        public Card TakeCard() 
        {
            Card cardToTake = cards.Last();
            cards.Remove(cardToTake);
            return cardToTake;
        }

        private void InitCourt()
        {
            foreach(Character character in Enum.GetValues(typeof(Character)))
            {
                if (character == Character.Null) continue;
                for (int i = 0; i < 3; i++)
                {
                    cards.Add(new Card(character));
                }
            }
            Reshuffle();
        }
    }
}
