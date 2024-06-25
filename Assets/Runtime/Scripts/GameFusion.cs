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

        #endregion



        #region IGame Implementation

        public override IDispatcher<DispatchBase> Dispatcher => _dispatcherFusion;

        #endregion



        #region Base Methods

        private void OnEnable() {
            Dispatcher.Add<DispatchNetworkConnectedToServer>(HandleNetworkConnectedToServer);
            Dispatcher.Add<DispatchNetworkPlayerJoined>(HandleNetworkPlayerJoined);
        }

        private void OnDisable() {
            Dispatcher.Remove<DispatchNetworkPlayerJoined>(HandleNetworkPlayerJoined);
            Dispatcher.Remove<DispatchNetworkConnectedToServer>(HandleNetworkConnectedToServer);
        }

        private void OnDestroy() => _dispatcherFusion.Dispose();

        #endregion



        #region Event Handlers

        private void HandleNetworkConnectedToServer(DispatchNetworkConnectedToServer args) {
            _playerCam.gameObject.SetActive(true);
            _targetsRoot.gameObject.SetActive(true);
        }

        private void HandleNetworkPlayerJoined(DispatchNetworkPlayerJoined args) {
            if (args.PlayerId == _networkRunner.LocalPlayer.PlayerId.ToString()) {
                NetworkObject playerObj = _networkRunner.Spawn(_playerPrefab);
                _networkRunner.SetPlayerObject(_networkRunner.LocalPlayer, playerObj);
                _playerCam.Follow = playerObj.transform;
            }
        }

        #endregion
    }
}
