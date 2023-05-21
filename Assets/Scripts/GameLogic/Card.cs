using System;
using System.Collections;
using System.Collections.Generic;

namespace Coup.GameLogic
{
    public class Card
    {
        public Guid Id { get; private set; }
        public Character character { get; private set; }

        public Card(Character character)
        {
            Id = Guid.NewGuid();
            this.character = character;
        }
    }
}
