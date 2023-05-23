using Coup.GameLogic.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Coup.UI
{
    public class PersonalActionPanel : MonoBehaviour
    {
        [SerializeField]
        private List<PersonalActionButton> _personalActionButtons = new List<PersonalActionButton>();

        private PlayerActionPanel _playerActionPanel;


        private void Start()
        {
            for (int i = 0; i < _personalActionButtons.Count; i++)
            {
                _personalActionButtons[i].RegisterPersonalActionPanel(this);
            }
        }

        public void RegisterPlayerActionPanel(PlayerActionPanel playerActionPanel) 
        {
            _playerActionPanel = playerActionPanel;
        }

        public void SetInteractions(bool setInteractionsOn)
        {
            for(int i = 0; i < _personalActionButtons.Count; i++)
            {
                _personalActionButtons[i].SetInteractions(setInteractionsOn);
            }
        }

        public void CreateAction(PersonalGameActionType gameAction)
        {
            _playerActionPanel.CreateAction(gameAction);
        }
    }
}
