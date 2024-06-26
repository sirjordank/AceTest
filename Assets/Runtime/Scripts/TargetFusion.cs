using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;

namespace AceTest {
    /// <summary>Decorator for a TargetSingle to add Photon Fusion compatibility.</summary>
    public class TargetFusion : NetworkBehaviour {

        #region Instance Vars

        [SerializeField, Tooltip("Reference to the TargetSingle to be decorated.")]
        private TargetSingle _single;

        /// <summary>Color of the player that last hit the target.</summary>
        [Networked]
        public Color LastPlayerColor { get; set; }

        /// <summary>Time that the target was last hit, in seconds.</summary>
        [Networked, OnChangedRender(nameof(HandleChangedLastHitTime))]
        public float LastHitTime { get; set; }
        

        #endregion



        #region Base Methods

        private void Awake() {
            // override the start and hit actions of the target
            _single.StartAction = OnStart;
            _single.HitAction = OnHit;
        }

        #endregion



        #region Event Handlers

        private void HandleChangedLastHitTime() {
            float delta = TargetSingle.ResetDelay - (Runner.SimulationTime - LastHitTime);

            if (delta > 0f) {
                _single.MeshRend.material.color = LastPlayerColor;
                UniTask.Delay((int)(delta * 1000f)).ContinueWith(() => {
                    _single.MeshRend.material.color = Color.white;
                    Object.ReleaseStateAuthority();
                });
            }
        }

        #endregion



        /// <inheritdoc cref="TargetSingle.OnStart(TargetSingle)"/>
        private void OnStart(TargetSingle _) {
            UniTask.WaitUntil(() => Id.Object.IsValid).ContinueWith(() => {
                Random.State current = Random.state;

                // pseudo-randomize the position based on the network ID
                Random.InitState((int)Id.Object.Raw);
                float x = Random.Range(TargetSingle.RangeX.x, TargetSingle.RangeX.y);
                float y = Random.Range(TargetSingle.RangeY.x, TargetSingle.RangeY.y);
                float z = Random.Range(TargetSingle.RangeZ.x, TargetSingle.RangeZ.y);
                _single.transform.position = new Vector3(x, y, z);

                Random.state = current;
            });
        }

        /// <inheritdoc cref="TargetSingle.OnHit(TargetSingle, IPlayer)"/>
        private void OnHit(TargetSingle _, IPlayer player) {
            Object.RequestStateAuthority();
            UniTask.WaitUntil(() => Object.HasStateAuthority).ContinueWith(() => {
                LastPlayerColor = player.MainColor;
                LastHitTime = Runner.SimulationTime;
            });
        }
    }
}
