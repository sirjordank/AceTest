using Cysharp.Threading.Tasks;
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

        /// <summary>Completion source for when the local player joins.</summary>
        private readonly UniTaskCompletionSource _localPlayerJoinedSource = new();

        #endregion



        #region Base Methods

        private void OnEnable() => Dispatcher.Add<DispatchNetworkPlayerJoined>(HandleNetworkPlayerJoined);

        private void OnDisable() => Dispatcher.Remove<DispatchNetworkPlayerJoined>(HandleNetworkPlayerJoined);

        protected override GameObject CreateLocalPlayerObject() {
            NetworkObject playerObj = _networkRunner.Spawn(_playerPrefab);
            _networkRunner.SetPlayerObject(_networkRunner.LocalPlayer, playerObj);
            return playerObj.gameObject;
        }

        protected override UniTask Play() => _localPlayerJoinedSource.Task.ContinueWith(() => base.Play());

        #endregion



        #region Event Handlers
        
        

        private void HandleNetworkPlayerJoined(DispatchNetworkPlayerJoined args) {
            if (_networkRunner.LocalPlayer.PlayerId.ToString() == args.PlayerId) {
                _localPlayerJoinedSource.TrySetResult();
            }
        }

        #endregion
    }
}
