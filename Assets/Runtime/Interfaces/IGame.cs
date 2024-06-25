namespace AceTest {
    /// <summary>Interface to manage a game instance, which acts as a service locator.</summary>
    public interface IGame {

        #region Class Vars

        /// <summary>The only singleton instance in the game.</summary>
        public static IGame Instance { get; protected set; }

        #endregion



        #region Instance Vars

        /// <summary>Dispatcher to manage all of the main events in the game.</summary>
        public IDispatcher<DispatchBase> Dispatcher { get; }

        /// <summary>Default input containing all of the actions that a player may take.</summary>
        public InputDefault Input { get; }

        #endregion
    }
}
