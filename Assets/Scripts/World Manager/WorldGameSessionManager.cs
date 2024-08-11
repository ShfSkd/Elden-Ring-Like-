using SKD.Character.Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SKD.World_Manager
{
    public class WorldGameSessionManager : MonoBehaviour
    {
        private static WorldGameSessionManager instace;
        public static WorldGameSessionManager Instance { get { return instace; } }

        private void Awake()
        {
            if (instace == null)
                instace = this;
            else
                Destroy(gameObject);
        }

        [Header("Active Players in Session")]
        public List<PlayerManager> _players = new List<PlayerManager>();

        public void AddPlayerToActivePlayerList(PlayerManager player)
        {
            // Check the list, if it does not already contain the player. Add them to the list 
            if (!_players.Contains(player))
                _players.Add(player);
            // Check for null slots , and remove them 
            for (int i = _players.Count - 1; i > -1; i--)
            {
                if (_players[i] == null)
                    _players.RemoveAt(i);
            }

        }
        public void RemovePlayerToActivePlayerList(PlayerManager player)
        {
            // Check the list, if it does contain the player. Remove them from the list 
            if (_players.Contains(player))
                _players.Remove(player);

            // Check for null slots , and remove them 
            for (int i = _players.Count - 1; i > -1; i--)
            {
                if (_players[i] == null)
                    _players.RemoveAt(i);
            }

        }
    }
}