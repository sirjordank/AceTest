using Fusion;
using UnityEngine;

namespace AceTest {
    public class PlayerMovement : NetworkBehaviour {

        #region Instance Vars

        public Camera Camera;

        private Vector3 _velocity;
        private bool _jumpPressed;
        private CharacterController _controller;

        public float _playerSpeed = 2f;
        public float _jumpForce = 5f;
        public float _gravityValue = -9.81f;

        private void Awake() {
            _controller = GetComponent<CharacterController>();
        }

        #endregion

        void Update() {
            if (Input.GetButtonDown("Jump")) {
                _jumpPressed = true;
            }
        }

        public override void FixedUpdateNetwork() {
            // Only move own player and not every other player. Each player controls its own player object.
            if (!HasStateAuthority) return;

            if (_controller.isGrounded) {
                _velocity = new Vector3(0, -1, 0);
            }

            Quaternion cameraRotationY = Quaternion.Euler(0, Camera.transform.rotation.eulerAngles.y, 0);
            Vector3 move = cameraRotationY * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Runner.DeltaTime * _playerSpeed;
            _velocity.y += _gravityValue * Runner.DeltaTime;

            if (_jumpPressed && _controller.isGrounded) {
                _velocity.y += _jumpForce;
            }
            _controller.Move(move + _velocity * Runner.DeltaTime);

            if (move != Vector3.zero) {
                gameObject.transform.forward = move;
            }

            _jumpPressed = false;
        }

        public override void Spawned() {
            if (HasStateAuthority) {
                Camera = Camera.main;
                Camera.GetComponent<FirstPersonCamera>().Target = transform;
            }
        }
    }
}
