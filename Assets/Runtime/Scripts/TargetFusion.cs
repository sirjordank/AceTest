using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;

namespace AceTest {
    /// <summary>Decorator for a TargetSingle to add Photon Fusion compatibility.</summary>
    public class TargetFusion : NetworkBehaviour {

        #region Instance Vars

        [SerializeField, Tooltip("")]
        private TargetSingle single;

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
            single.StartAction = OnStart;
            single.HitAction = OnHit;
        }

        #endregion



        #region Event Handlers

        private void HandleChangedLastHitTime() {
            float delta = TargetSingle.ResetDelay - (Runner.SimulationTime - LastHitTime);

            if (delta > 0f) {
                single.MeshRend.material.color = LastPlayerColor;
                UniTask.Delay((int)(delta * 1000f)).ContinueWith(() => single.MeshRend.material.color = Color.white);
            }
        }

        #endregion



        /// <inheritdoc cref="TargetSingle.OnStart(TargetSingle)"/>
        private void OnStart(TargetSingle _) {
            Random.State current = Random.state;

            // pseudo-randomize the position based on the network ID
            Random.InitState((int)Object.Id.Raw);
            float x = Random.Range(TargetSingle.RangeX.x, TargetSingle.RangeX.y);
            float y = Random.Range(TargetSingle.RangeY.x, TargetSingle.RangeY.y);
            float z = Random.Range(TargetSingle.RangeZ.x, TargetSingle.RangeZ.y);
            single.transform.position = new Vector3(x, y, z);

            Random.state = current;
        }

        /// <inheritdoc cref="TargetSingle.OnHit(TargetSingle, IPlayer)"/>
        private void OnHit(TargetSingle single, IPlayer player) {
            LastPlayerColor = player.MainColor;
            LastHitTime = Runner.SimulationTime;
        }
    }
}
