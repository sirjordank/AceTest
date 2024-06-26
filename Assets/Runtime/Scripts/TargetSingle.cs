using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace AceTest {
    /// <summary>Manages all functionality related to a basic target.</summary>
    [RequireComponent(typeof(MeshRenderer))]
    public class TargetSingle : MonoBehaviour, ITargetable {

        #region Class Vars

        /// <summary>How long it takes for the target to reset, in seconds.</summary>
        public static float ResetDelay => 5f;

        /// <summary>Min and max range for the X coordinate.</summary>
        public static Vector2 RangeX => new(-50f, 50f);

        /// <summary>Min and max range for the Y coordinate.</summary>
        public static Vector2 RangeY => new(0f, 10f);

        /// <summary>Min and max range for the Z coordinate.</summary>
        public static Vector2 RangeZ => new(-50f, 50f);

        #endregion



        #region Instance Vars

        /// <summary>Reference to the required MeshRenderer component.</summary>
        public MeshRenderer MeshRend => GetComponent<MeshRenderer>();

        /// <summary>Is the target able to be hit?</summary>
        private bool IsHittable => MeshRend.material.color == Color.white;

        public Action<TargetSingle> StartAction { get; set; } = OnStart;

        public Action<TargetSingle, IPlayer> HitAction { get; set; } = OnHit;

        #endregion



        #region ITargetable Implementation

        public bool Hit(IPlayer player) {
            if (IsHittable) {
                HitAction.Invoke(this, player);
                return true;
            }

            return false;
        }

        #endregion



        #region Base Methods

        private void Start() => StartAction.Invoke(this);

        #endregion



        /// <summary>Called when the target behaviour starts.</summary>
        /// <param name="single">TargetSingle instance to start.</param>
        private static void OnStart(TargetSingle single) {
            float x = UnityEngine.Random.Range(RangeX.x, RangeX.y);
            float y = UnityEngine.Random.Range(RangeY.x, RangeY.y);
            float z = UnityEngine.Random.Range(RangeZ.x, RangeZ.y);
            single.transform.position = new Vector3(x, y, z);
        }

        /// <summary>Called when the target behaviour is hit.</summary>
        /// <param name="single">TargetSingle instance that was hit.</param>
        /// <param name="player">Player that hit the target.</param>
        private static void OnHit(TargetSingle single, IPlayer player) {
            single.MeshRend.material.color = player.MainColor;
            UniTask.Delay((int)(ResetDelay * 1000f)).ContinueWith(() => single.MeshRend.material.color = Color.white);
        }
    }
}
