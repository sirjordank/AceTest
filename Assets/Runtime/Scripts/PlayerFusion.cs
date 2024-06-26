using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AceTest {
    /// <summary>Decorator for a PlayerSingle to add Photon Fusion compatibility.</summary>
    public class PlayerFusion : NetworkBehaviour, IPlayer {

        #region Instance Vars

        [SerializeField, Tooltip("How fast the player moves in any direction.")]
        private float _playerSpeed = 5f;

        /// <summary>Reference to all of the renderer components.</summary>
        private Renderer[] Renderers => GetComponentsInChildren<Renderer>();

        #endregion



        #region IPlayer Implementation

        [Networked, OnChangedRender(nameof(HandleChangedMainColor))]
        public Color MainColor { get; set; }

        string IPlayer.Id => ColorUtility.ToHtmlStringRGB(MainColor);

        #endregion



        #region Base Methods

        private void Start() {
            if (!HasStateAuthority) {
                HandleChangedMainColor();
            } else {
                MainColor = Color.HSVToRGB(Random.value, 1f, 1f);
            }
        }

        private void OnEnable() {
            IGame.Instance.Input.Default.Fire.started += HandleFireStarted;
        }

        private void OnDisable() {
            IGame.Instance.Input.Default.Fire.started -= HandleFireStarted;
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

        private void HandleChangedMainColor() {
            foreach (Renderer r in Renderers) {
                r.material.color = MainColor;
            }
        }

        private void HandleFireStarted(InputAction.CallbackContext _) {
            if (!HasStateAuthority) return;

            RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward);
            foreach (RaycastHit hit in hits) {
                if (hit.transform.TryGetComponent(out ITargetable target)) {
                    target.Hit(this);
                }
            }
        }

        #endregion
    }
}
