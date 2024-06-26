using UnityEngine;

namespace AceTest {
    /// <summary>Interface for each player object.</summary>
    public interface IPlayer {

        #region Instance Vars

        /// <summary>ID of the player.</summary>
        public string Id { get; }

        /// <summary>Is the player owned by the local host?</summary>
        public bool IsLocal { get; set; }

        /// <summary>Main color of the player.</summary>
        public Color MainColor { get; set; }

        #endregion
    }
}
