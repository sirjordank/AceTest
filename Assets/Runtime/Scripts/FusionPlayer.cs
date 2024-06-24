using Fusion;
using System;

namespace AceTest {
    /// <summary>Implementation of the player interface for Photon Fusion.</summary>
    public class FusionPlayer : IPlayer {

        #region Instance Vars

        /// <summary>Reference to the internal player struct.</summary>
        private PlayerRef _playerRef;

        #endregion



        #region IPlayer Implementation

        public string Id => _playerRef.PlayerId.ToString();

        #endregion



        public FusionPlayer(PlayerRef player) {
            if (player == default) throw new ArgumentNullException();
            _playerRef = player;
        }
    }
}
