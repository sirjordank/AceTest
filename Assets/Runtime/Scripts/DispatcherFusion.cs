using Fusion;
using System;
using System.Collections.Generic;

namespace AceTest {
    /// <summary>Dispatcher of network events for Photon Fusion.</summary>
    public class DispatcherFusion : IDispatcher<DispatchNetworkBase>, IDisposable {

        #region Instance Vars

        /// <summary>Instance of events from Photon Fusion.</summary>
        private NetworkEvents _networkEvents;

        /// <inheritdoc cref="IDispatcher{TDispatch}.EventsByType"/>
        private readonly Dictionary<Type, Action<DispatchNetworkBase>> _eventsByType = new();

        #endregion



        #region IDispatcher Implementation

        Dictionary<Type, Action<DispatchNetworkBase>> IDispatcher<DispatchNetworkBase>.EventsByType => _eventsByType;

        #endregion



        #region IDisposable Implementation

        public void Dispose() {
            // unsubscribe from all relevant events
            _networkEvents.PlayerJoined.RemoveAllListeners();
        }

        #endregion



        /// <summary>Default constructor for this class.</summary>
        /// <param name="evts">Reference to the events instance from Photon Fusion.</param>
        public DispatcherFusion(NetworkEvents evts) {
            if (evts == default) throw new ArgumentNullException();
            _networkEvents = evts;

            // subscribe to all relevant events
            evts.PlayerJoined.AddListener((_, player) => (this as IDispatcher<DispatchNetworkBase>).Invoke(new DispatchNetworkPlayerJoined() {
                Player = new FusionPlayer(player),
            }));
        }
    }
}
