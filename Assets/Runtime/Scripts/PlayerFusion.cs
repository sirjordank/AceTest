using Fusion;
using UnityEngine;

namespace AceTest {
    /// <summary>Decorator for a PlayerSingle to add Photon Fusion compatibility.</summary>
    public class PlayerFusion : NetworkBehaviour {

        #region Instance Vars

        [SerializeField, Tooltip("Reference to the PlayerSingle to be decorated.")]
        private PlayerSingle _single;

        /// <summary>Color of the player according to the network.</summary>
        [Networked, OnChangedRender(nameof(HandleChangedMainColor))]
        public Color NetworkColor { get; set; }

        #endregion



        #region Base Methods

        private void Awake() {
            // override the start and update actions of the player
            _single.StartAction = OnStart;
            _single.UpdateAction = OnUpdate;
        }

        public override void FixedUpdateNetwork() => _single.ApplyTransformations(Runner.DeltaTime);

        #endregion



        #region Event Handlers

        private void HandleChangedMainColor() => (_single as IPlayer).MainColor = NetworkColor;

        #endregion



        /// <inheritdoc cref="PlayerSingle.OnStart(PlayerSingle)"/>
        private void OnStart(PlayerSingle _) {
            if (!HasStateAuthority) {
                HandleChangedMainColor();
            } else {
                NetworkColor = Color.HSVToRGB(Random.value, 1f, 1f);
            }
        }

        /// <inheritdoc cref="PlayerSingle.OnUpdate(PlayerSingle)"/>
        private void OnUpdate(PlayerSingle _) { }
    }
}
