using UnityEngine;
using UnityEngine.InputSystem;

namespace AceTest {
    /// <summary>Manages all functionality related to a single-player game.</summary>
    public class GameSingle : MonoBehaviour, IGame {

        #region Instance Vars

        [SerializeField, Tooltip("Reference to the player prefab.")]
        protected GameObject _playerPrefab;

        [SerializeField, Tooltip("Reference to the player actions for the new input system.")]
        protected PlayerInput _playerInput;

        /// <inheritdoc cref="IGame.Input"/>
        protected InputDefault _input;

        #endregion



        #region IGame Implementation

        public InputDefault Input => _input;

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
