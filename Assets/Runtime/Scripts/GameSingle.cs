using Cinemachine;
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

        [SerializeField, Tooltip("All player actions for the new input system.")]
        protected PlayerInput _playerInput;

        /// <inheritdoc cref="IGame.Input"/>
        protected InputDefault _input;

        #endregion



        #region IGame Implementation

        public InputDefault Input => _input;

        public virtual IDispatcher<DispatchBase> Dispatcher => default;

        #endregion



        #region Base Methods

        protected virtual void Awake() {
            IGame.Instance = this;
            _input = new();
            _playerInput.actions = _input.asset;
        }

        #endregion
    }
}
