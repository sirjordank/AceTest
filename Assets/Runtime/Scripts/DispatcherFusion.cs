using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AceTest {
    /// <summary>Dispatcher of network events for Photon Fusion.</summary>
    public class DispatcherFusion : MonoBehaviour, IDispatcher<DispatchBase>, IDisposable {

        #region Instance Vars

        [SerializeField, Tooltip("Instance of the events from Photon Fusion.")]
        private NetworkEvents _networkEvents;

        /// <inheritdoc cref="IDispatcher{TDispatch}.EventsByType"/>
        private Dictionary<Type, object> _eventsByType;

        /// <summary>Convenience property to automatically cast this dispatcher as an interface.</summary>
        private IDispatcher<DispatchBase> Dispatcher => this;

        #endregion



        #region IDispatcher Implementation

        Dictionary<Type, object> IDispatcher<DispatchBase>.EventsByType => _eventsByType ??= new();

        #endregion



        #region IDisposable Implementation

        public void Dispose() => _networkEvents.PlayerJoined.RemoveAllListeners();

        #endregion



        #region Event Handlers

        public void HandlePlayerJoined(NetworkRunner _, PlayerRef player) => Dispatcher.Invoke(new DispatchNetworkPlayerJoined() {
            PlayerId = player.PlayerId.ToString(),
        });

        #endregion
    }
}
