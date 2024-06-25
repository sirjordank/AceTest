using Fusion;
using System;
using UnityEngine;

namespace AceTest {
    /// <summary>Implementation of the player interface for Photon Fusion.</summary>
    public class PlayerFusion : NetworkBehaviour, IPlayer {

        #region Instance Vars

        /// <summary>Reference to the internal player struct.</summary>
        private PlayerRef _playerRef;

        private Vector3 _velocity;
        private CharacterController _controller;

        public float _playerSpeed = 2f;

        private MeshRenderer _meshRenderer;

        [Networked, OnChangedRender(nameof(ColorChanged))]
        public Color NetworkedColor { get; set; }

        #endregion



        #region IPlayer Implementation

        string IPlayer.Id => _playerRef.PlayerId.ToString();

        #endregion



        #region Base Methods

        private void Awake() {
            _controller = GetComponent<CharacterController>();
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Start() {
            if (HasStateAuthority) {
                // Changing the material color here directly does not work since this code is only executed on the client pressing the button and not on every client.
                NetworkedColor = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1f);
            }
        }

        public override void FixedUpdateNetwork() {
            // Only move own player and not every other player. Each player controls its own player object.
            if (!HasStateAuthority) return;

            if (_controller.isGrounded) {
                _velocity = new Vector3(0, -1, 0);
            }

            float x = IGame.Instance.Input.Default.Horizontal.ReadValue<float>();
            float z = IGame.Instance.Input.Default.Vertical.ReadValue<float>();

            Quaternion cameraRotationY = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
            Vector3 move = cameraRotationY * new Vector3(x, 0, z) * Runner.DeltaTime * _playerSpeed;
            _controller.Move(move + _velocity * Runner.DeltaTime);

            if (move != Vector3.zero) {
                gameObject.transform.forward = move;
            }
        }

        #endregion



        /// <summary></summary>
        /// <param name="player"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public PlayerFusion(PlayerRef player) {
            if (player == default) throw new ArgumentNullException();
            _playerRef = player;
        }

        /// <summary></summary>
        private void ColorChanged() {
            _meshRenderer.material.color = NetworkedColor;
        }
    }
}
