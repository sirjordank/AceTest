using Fusion;
using UnityEngine;

namespace AceTest {
    /// <summary>Manages all functionality related to a multiplayer game using Photon Fusion.</summary>
    public class GameFusion : GameSingle {

        #region Instance Vars

        [SerializeField, Tooltip("Instance of the runner from Photon Fusion.")]
        private NetworkRunner _networkRunner;

        [SerializeField, Tooltip("Instance of events from Photon Fusion.")]
        private NetworkEvents _networkEvents;

        [SerializeField, Tooltip("Photon Fusion implementation for the dispatcher of network events.")]
        private DispatcherFusion _dispatcherFusion;

        /// <summary>Convenience property to automatically cast the dispatcher as an interface.</summary>
        private IDispatcher<DispatchNetworkBase> DispatcherNetwork => _dispatcherFusion;

        #endregion



        #region Base Methods

        private void OnEnable() {
            DispatcherNetwork.Add<DispatchNetworkPlayerJoined>(HandleNetworkPlayerJoined);
        }

        private void OnDisable() {
            DispatcherNetwork.Remove<DispatchNetworkPlayerJoined>(HandleNetworkPlayerJoined);
        }

        private void OnDestroy() => _dispatcherFusion.Dispose();

        #endregion



        #region Event Handlers

        private void HandleNetworkPlayerJoined(DispatchNetworkPlayerJoined args) {
            if (args.PlayerId == _networkRunner.LocalPlayer.PlayerId.ToString()) {
                NetworkObject playerObj = _networkRunner.Spawn(_playerPrefab);
                _playerCam.Follow = playerObj.transform;
            }
        }

        #endregion
    }
}
