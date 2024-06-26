namespace AceTest {
    /// <summary>Base class for all other dispatches.</summary>
    public abstract class DispatchBase { }

    /// <summary>Base class for all other network dispatches.</summary>
    public abstract class DispatchNetworkBase : DispatchBase { }

    /// <summary>Dispatched whenever a player has joined the network.</summary>
    public class DispatchNetworkPlayerJoined : DispatchNetworkBase {
        /// <summary>ID of the player that joined.</summary>
        public string PlayerId { get; set; }
    }
}
