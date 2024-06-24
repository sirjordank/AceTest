using UnityEngine;
using UnityEngine.InputSystem;

namespace AceTest {
    /// <summary></summary>
    public class GameSingle : MonoBehaviour, IGame {

        #region Instance Vars

        [SerializeField, Tooltip("")]
        private PlayerInput _playerInput;

        /// <inheritdoc cref="IGame.Input"/>
        private InputDefault _input;

        #endregion



        #region IGame Implementation

        public InputDefault Input => _input;

        #endregion



        private void Awake() {
            IGame.Instance = this;
            _input = new();
            _playerInput.actions = _input.asset;
        }
    }
}
