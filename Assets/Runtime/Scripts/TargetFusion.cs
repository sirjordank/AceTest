using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;

namespace AceTest {
    /// <summary>Manages all functionality related to a basic target.</summary>
    [RequireComponent(typeof(MeshRenderer))]
    public class TargetFusion : NetworkBehaviour, ITargetable {

        #region Class Vars

        /// <summary>How long it takes for the target to reset, in seconds.</summary>
        private static float ResetDelay => 5f;

        /// <summary>Min and max range for the X coordinate.</summary>
        private static Vector2 RangeX => new(-50f, 50f);

        /// <summary>Min and max range for the Y coordinate.</summary>
        private static Vector2 RangeY => new(0f, 10f);

        /// <summary>Min and max range for the Z coordinate.</summary>
        private static Vector2 RangeZ => new(-50f, 50f);

        #endregion



        #region Instance Vars

        /// <summary>Reference to the required MeshRenderer component.</summary>
        private MeshRenderer MeshRend => GetComponent<MeshRenderer>();

        /// <summary>Is the target able to be hit?</summary>
        private bool IsHittable => MeshRend.material.color == Color.white;

        /// <summary>Color of the player that last hit the target.</summary>
        [Networked]
        public Color LastPlayerColor { get; set; }

        /// <summary>Time that the target was last hit, in seconds.</summary>
        [Networked, OnChangedRender(nameof(HandleChangedLastHitTime))]
        public float LastHitTime { get; set; }
        

        #endregion



        #region ITargetable Implementation

        public bool Hit(IPlayer player) {
            if (IsHittable) {
                LastPlayerColor = player.MainColor;
                LastHitTime = Runner.SimulationTime;
                return true;
            }

            return false;
        }

        #endregion



        #region Base Methods

        private void Start() {
            Random.State current = Random.state;

            // pseudo-randomize the position based on the network ID
            Random.InitState((int)Object.Id.Raw);
            transform.position = new Vector3(Random.Range(RangeX.x, RangeX.y), Random.Range(RangeY.x, RangeY.y), Random.Range(RangeZ.x, RangeZ.y));

            Random.state = current;
        }

        #endregion



        #region Event Handlers

        private void HandleChangedLastHitTime() {
            float delta = ResetDelay - (Runner.SimulationTime - LastHitTime);

            if (delta > 0f) {
                MeshRend.material.color = LastPlayerColor;
                UniTask.Delay((int)(delta * 1000f)).ContinueWith(() => MeshRend.material.color = Color.white);
            }
        }

        #endregion
    }
}
