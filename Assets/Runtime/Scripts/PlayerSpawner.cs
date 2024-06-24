using Fusion;
using UnityEngine;

namespace AceTest {
    /// <summary></summary>
    public class PlayerSpawner : SimulationBehaviour, IPlayerJoined {

        #region Instance Vars

        [SerializeField]
        private GameObject _playerPrefab;

        #endregion



        #region Event Handlers

        public void PlayerJoined(PlayerRef player) {
            if (player == Runner.LocalPlayer) {
                Runner.Spawn(_playerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
            }
        }

        #endregion
    }
}
