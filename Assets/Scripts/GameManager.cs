using Coup.GameLogic;
using UnityEngine;

namespace Coup
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private int playerCount;

        private GameEngine engine;

        void Start()
        {
            engine = new GameEngine();
            engine.SetupNewGame(playerCount);
        }
    }
}
