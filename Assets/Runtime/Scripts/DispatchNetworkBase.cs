namespace AceTest {
    /// <summary>Base class for each type of network event.</summary>
    public abstract class DispatchNetworkBase { }

    /// <summary>Dispatched whenever a player has joined the network.</summary>
    public class DispatchNetworkPlayerJoined : DispatchNetworkBase {
        /// <summary>ID of the player that joined.</summary>
        public string PlayerId { get; set; }
    }
}
