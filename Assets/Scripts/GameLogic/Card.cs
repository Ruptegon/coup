using System;

namespace Coup.GameLogic
{
    public class Card
    {
        public Guid Id {  get => _id; }
        public Character Character { get => _character; }

        private readonly Guid _id;
        private readonly Character _character;

        public Card(Character character)
        {
            _id = Guid.NewGuid();
            _character = character;
        }
    }
}
