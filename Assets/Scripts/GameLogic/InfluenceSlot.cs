namespace Coup.GameLogic
{
    public class InfluenceSlot
    {
        public Card Card { get; set; }
        public bool IsRevealed { get; set; }

        public bool ContainsCharacter(Character character)
        {
            return Card.Character == character;
        }
    }
}
