using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AceTest {
    /// <summary>Manages all functionality related to a single-player game.</summary>
    public class GameSingle : MonoBehaviour, IGame {

        #region Instance Vars

        [SerializeField, Tooltip("Prefab to instantiate the local player.")]
        protected GameObject _playerPrefab;

        [SerializeField, Tooltip("FPS camera for the local player.")]
        protected CinemachineVirtualCamera _playerCam;

        [SerializeField, Tooltip("Root transform for all of the targets")]
        protected Transform _targetsRoot;

        [SerializeField, Tooltip("Dispatcher to manage all of the main events in the game")]
        protected MonoBehaviour _dispatcher;

        [SerializeField, Tooltip("All player actions for the new input system.")]
        protected PlayerInput _playerInput;

        /// <inheritdoc cref="IGame.Input"/>
        protected InputDefault _input;

        #endregion



        #region IGame Implementation

        public InputDefault Input => _input;

        public IDispatcher<DispatchBase> Dispatcher => _dispatcher as IDispatcher<DispatchBase>;

        #endregion



        #region Base Methods

        private void Awake() {
            _input = new();
            _playerInput.actions = _input.asset;
            IGame.Instance = this;
            Play();
        }

        private void OnDestroy() => Dispatcher.Dispose();

        #endregion



        /// <summary>Creates the local player from the assigned prefab.</summary>
        /// <returns>GameObject of the local player.</returns>
        protected virtual GameObject CreateLocalPlayerObject() => Instantiate(_playerPrefab);

        /// <summary>Sets up any additional context before allowing the game to be played.</summary>
        /// <returns>An awaitable task.</returns>
        protected virtual UniTask Play() {
            GameObject playerObj = CreateLocalPlayerObject();
            _playerCam.Follow = playerObj.transform;
            _playerCam.gameObject.SetActive(true);
            _targetsRoot.gameObject.SetActive(true);
            return UniTask.CompletedTask;
        }
    }
}
