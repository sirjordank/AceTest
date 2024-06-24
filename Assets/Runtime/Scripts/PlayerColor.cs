using Fusion;
using UnityEngine;

namespace AceTest {
    /// <summary></summary>
    public class PlayerColor : NetworkBehaviour {

        #region Instance Vars

        private MeshRenderer _meshRenderer;

        [Networked, OnChangedRender(nameof(ColorChanged))]
        public Color NetworkedColor { get; set; }

        #endregion



        private void Awake() {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Start() {
            if (HasStateAuthority) {
                // Changing the material color here directly does not work since this code is only executed on the client pressing the button and not on every client.
                NetworkedColor = new Color(Random.value, Random.value, Random.value, 1f);
            }
        }

        private void ColorChanged() {
            _meshRenderer.material.color = NetworkedColor;
        }
    }
}
