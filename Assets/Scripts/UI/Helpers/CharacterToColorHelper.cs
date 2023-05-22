using Coup.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Coup.UI.Helpers 
{
    [CreateAssetMenu]
    public class CharacterToColorHelper : ScriptableObject
    {
        [SerializeField]
        public List<CharacterColorPair> Colors = new List<CharacterColorPair>();

        public Color GetCharacterColor(Character character)
        {
            Color color = Color.white;
            CharacterColorPair pair = Colors.FirstOrDefault(p => p.Character == character);
            if(pair.Color != null)
            {
                color = pair.Color;
            }
            return color;
        }

        [Serializable]
        public struct CharacterColorPair 
        {
            public Character Character;
            public Color Color;
        }
    }
}
