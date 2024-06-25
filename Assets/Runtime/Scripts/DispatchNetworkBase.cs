namespace AceTest {
    /// <summary>Base class for all other network dispatches.</summary>
    public abstract class DispatchNetworkBase : DispatchBase { }

    /// <summary>Dispatched whenever the server is connected to.</summary>
    public class DispatchNetworkConnectedToServer : DispatchNetworkBase { }

    /// <summary>Dispatched whenever a player has joined the network.</summary>
    public class DispatchNetworkPlayerJoined : DispatchNetworkBase {
        /// <summary>ID of the player that joined.</summary>
        public string PlayerId { get; set; }
    }
}
