using System;
using System.Collections.Generic;

namespace AceTest {
    /// <summary>Dispatcher of main events for single-player.</summary>
    public class DispatcherSingle : IDispatcher<DispatchBase> {

        #region Instance Vars

        /// <inheritdoc cref="IDispatcher{TDispatch}.EventsByType"/>
        private Dictionary<Type, object> _eventsByType;

        #endregion



        #region IDispatcher Implementation

        Dictionary<Type, object> IDispatcher<DispatchBase>.EventsByType => _eventsByType ??= new();

        #endregion
    }
}
