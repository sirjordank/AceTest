using Fusion;
using UnityEngine;

namespace AceTest {
    /// <summary>Implementation of the player interface for Photon Fusion.</summary>
    public class PlayerFusion : NetworkBehaviour, IPlayer {

        #region Instance Vars

        [SerializeField, Tooltip("How fast the player moves in any direction.")]
        private float _playerSpeed = 5f;

        /// <summary>Reference to all of the renderer components.</summary>
        private Renderer[] Renderers => GetComponentsInChildren<Renderer>();

        [Networked, OnChangedRender(nameof(HandleChangedRender)), Tooltip("Main color of the player.")]
        public Color MainColor { get; set; }

        #endregion



        #region IPlayer Implementation

        string IPlayer.Id => ColorUtility.ToHtmlStringRGB(MainColor);

        #endregion



        #region Base Methods

        private void Start() {
            if (HasStateAuthority) {
                MainColor = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1f);
            } else {
                HandleChangedRender();
            }
        }

        public override void FixedUpdateNetwork() {
            if (!HasStateAuthority) return;

            float x = IGame.Instance.Input.Default.Horizontal.ReadValue<float>();
            float z = IGame.Instance.Input.Default.Vertical.ReadValue<float>();

            transform.Translate(_playerSpeed * Runner.DeltaTime * new Vector3(x, 0f, z), Space.Self);
            transform.SetPositionAndRotation(new Vector3(transform.position.x, 0f, transform.position.z), Camera.main.transform.rotation);
        }

        #endregion



        #region Event Handlers

        private void HandleChangedRender() {
            foreach (Renderer r in Renderers) {
                r.material.color = MainColor;
            }
        }

        #endregion
    }
}
