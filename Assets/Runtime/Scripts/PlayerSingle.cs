using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AceTest {
    /// <summary>Decorator for a PlayerSingle to add Photon Fusion compatibility.</summary>
    public class PlayerSingle : MonoBehaviour, IPlayer {

        #region Instance Vars

        [SerializeField, Tooltip("How fast the player moves in any direction.")]
        private float _playerSpeed = 5f;

        /// <summary>Reference to all of the renderer components.</summary>
        private Renderer[] Renderers => GetComponentsInChildren<Renderer>();

        /// <summary>Called when the player behaviour starts.</summary>
        public Action<PlayerSingle> StartAction { get; set; } = OnStart;

        /// <summary>Called when the player behaviour updates.</summary>
        public Action<PlayerSingle> UpdateAction { get; set; } = OnUpdate;

        #endregion



        #region IPlayer Implementation

        public string Id => ColorUtility.ToHtmlStringRGB(MainColor);

        public bool IsLocal { get; set; }

        public Color MainColor {
            get => Renderers[0].material.color;
            set {
                foreach (Renderer r in Renderers) {
                    r.material.color = value;
                }
            }
        }

        #endregion



        #region Base Methods

        private void Start() => StartAction.Invoke(this);

        private void OnEnable() => IGame.Instance.Input.Default.Fire.started += HandleFireStarted;

        private void OnDisable() => IGame.Instance.Input.Default.Fire.started -= HandleFireStarted;

        private void Update() => UpdateAction.Invoke(this);

        #endregion



        #region Event Handlers

        private void HandleFireStarted(InputAction.CallbackContext _) {
            if (!IsLocal) return;

            RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward);
            foreach (RaycastHit hit in hits) {
                if (hit.transform.TryGetComponent(out ITargetable target)) {
                    target.Hit(this);
                }
            }
        }

        #endregion



        /// <summary>Called when the player behaviour starts.</summary>
        /// <param name="single">PlayerSingle instance to start.</param>
        private static void OnStart(PlayerSingle single) => single.MainColor = Color.HSVToRGB(UnityEngine.Random.value, 1f, 1f);

        /// <summary>Called when the player behaviour updates.</summary>
        /// <param name="single">PlayerSingle instance to update.</param>
        private static void OnUpdate(PlayerSingle single) => single.ApplyTransformations(Time.deltaTime);

        /// <summary>Apply any transformations based on the given delta time, in seconds.</summary>
        /// <param name="deltaTime">Delta to apply the transformations.</param>
        public void ApplyTransformations(float deltaTime) {
            if (!IsLocal) return;

            float x = IGame.Instance.Input.Default.Horizontal.ReadValue<float>();
            float z = IGame.Instance.Input.Default.Vertical.ReadValue<float>();

            transform.Translate(_playerSpeed * deltaTime * new Vector3(x, 0f, z), Space.Self);
            transform.SetPositionAndRotation(new Vector3(transform.position.x, 0f, transform.position.z), Camera.main.transform.rotation);
        }
    }
}
