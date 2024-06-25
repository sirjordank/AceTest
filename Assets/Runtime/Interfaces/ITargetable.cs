namespace AceTest {
    /// <summary>Interface for any object that can be targeted.</summary>
    public interface ITargetable {
        /// <summary>Attempt to hit the targetable object.</summary>
        /// <param name="player">Player that is hitting.</param>
        /// <returns>Did the hit successfully register?</returns>
        public bool Hit(IPlayer player);
    }
}
