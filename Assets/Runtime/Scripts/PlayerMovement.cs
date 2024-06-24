using Fusion;
using UnityEngine;

namespace AceTest {
    /// <summary></summary>
    public class PlayerMovement : NetworkBehaviour {

        #region Instance Vars

        public Camera Camera;

        private Vector3 _velocity;
        private CharacterController _controller;

        public float _playerSpeed = 2f;

        private void Awake() {
            _controller = GetComponent<CharacterController>();
        }

        #endregion



        public override void FixedUpdateNetwork() {
            // Only move own player and not every other player. Each player controls its own player object.
            if (!HasStateAuthority) return;

            if (_controller.isGrounded) {
                _velocity = new Vector3(0, -1, 0);
            }

            float x = IGame.Instance.Input.Default.Horizontal.ReadValue<float>();
            float z = IGame.Instance.Input.Default.Vertical.ReadValue<float>();

            Quaternion cameraRotationY = Quaternion.Euler(0, Camera.transform.rotation.eulerAngles.y, 0);
            Vector3 move = cameraRotationY * new Vector3(x, 0, z) * Runner.DeltaTime * _playerSpeed;
            _controller.Move(move + _velocity * Runner.DeltaTime);

            if (move != Vector3.zero) {
                gameObject.transform.forward = move;
            }
        }

        public override void Spawned() {
            if (HasStateAuthority) {
                Camera = Camera.main;
                //Camera.GetComponent<FirstPersonCamera>().Target = transform;
            }
        }
    }
}
