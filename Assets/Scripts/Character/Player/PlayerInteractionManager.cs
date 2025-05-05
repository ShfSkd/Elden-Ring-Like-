using SKD.Interacts;
using SKD.UI.PlayerUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKD.Character.Player
{
    public class PlayerInteractionManager : MonoBehaviour
    {
        PlayerManager _player;

        private List<Interactable> _currentInteractableAction;
        private void Awake()
        {
            _player = GetComponent<PlayerManager>();
        }
        private void Start()
        {
            _currentInteractableAction = new List<Interactable>();
        }
        private void FixedUpdate()
        {
            if (!_player.IsOwner)
                return;

            // If the UI menu is not open and also the UI pop up check for interactable 
            if (!PlayerUIManger.instance._menuWindowIsOpen && !PlayerUIManger.instance._popUpWindowIsOpen)
            {
                CheackForInteractable();
            }

        }

        private void CheackForInteractable()
        {
            if (_currentInteractableAction.Count == 0)
                return;

            if (_currentInteractableAction[0] == null)
            {
                _currentInteractableAction.RemoveAt(0);
                return;
            }

            // If we have an interactable action and have not modify the player ,we do it here
            if (_currentInteractableAction[0] != null)
                PlayerUIManger.instance._playerUIPopUpManager.SendPlayerMessagePopUp(_currentInteractableAction[0]._interactableText);
        }
        private void RefreshInteractionList()
        {
            for (int i = _currentInteractableAction.Count - 1; i > -1; i--)
            {
                if (_currentInteractableAction[i] == null)
                    _currentInteractableAction.RemoveAt(i);
            }
        }
        public void Interact()
        {
            // If we press the interact button with or without an interactable, if will clear the pop up windows (item pick up, messages, etc)
            PlayerUIManger.instance._playerUIPopUpManager.CloseAllPopUpsWindows();
            
            if (_currentInteractableAction.Count == 0)
                return;

            if (_currentInteractableAction[0] != null)
            {
                _currentInteractableAction[0].Interact(_player);
                RefreshInteractionList();
            }
        }

        public void AddInteractionList(Interactable interactableObject)
        {
            RefreshInteractionList();

            if(!_currentInteractableAction.Contains(interactableObject))
                _currentInteractableAction.Add(interactableObject);
        }

        public void RemoveInteractionList(Interactable interactableObject)
        {
            if (_currentInteractableAction.Contains(interactableObject))
                _currentInteractableAction.Remove(interactableObject);

            RefreshInteractionList();
        }
    }
}