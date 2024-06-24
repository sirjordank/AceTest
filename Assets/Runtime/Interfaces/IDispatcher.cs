using System;
using System.Collections.Generic;
using UnityEngine;

namespace AceTest {
    /// <summary>Interface to manage an ecosystem of dispatches derived from the given type.</summary>
    /// <typeparam name="TDispatch">Base type of dispatch to manage.</typeparam>
    public interface IDispatcher<TDispatch> {

        #region Class Vars

        /// <summary>Prefix that is logged by default for each dispatch.</summary>
        private static string NoopLogPrefix => "Invoking dispatch of type:";

        #endregion



        #region Instance Vars

        /// <summary>All events indexed by their type.</summary>
        protected Dictionary<Type, Action<TDispatch>> EventsByType { get; }

        #endregion



        /// <summary>Invokes the given dispatch for any current listeners, if any.</summary>
        /// <param name="dispatch">Pre-constructed dispatch to be invoked.</param>
        public void Invoke(TDispatch dispatch) => EventsByType[dispatch.GetType()]?.Invoke(dispatch);

        /// <summary>Adds the given handler to the dispatch type.</summary>
        /// <typeparam name="T">Type of dispatch to add to.</typeparam>
        /// <param name="handler">Handler to invoke when the event is dispatched.</param>
        public void Add(Action<TDispatch> handler) {
            if (!EventsByType.ContainsKey(typeof(TDispatch))) {
                // add what is essentially a no-op as the base action for the type
                EventsByType.Add(typeof(TDispatch), new Action<TDispatch>(_ => Debug.Log(NoopLogPrefix + typeof(TDispatch))));
            }

            Action<TDispatch> evt = EventsByType[typeof(TDispatch)];
            evt += handler;
        }

        /// <summary>Removes the given handler from the dispatch type.</summary>
        /// <typeparam name="T">Type of dispatch to remove from.</typeparam>
        /// <param name="handler">Handler to invoke when the event is dispatched.</param>
        public void Remove(Action<TDispatch> handler) {
            if (EventsByType.ContainsKey(typeof(TDispatch))) {
                Action<TDispatch> evt = EventsByType[typeof(TDispatch)];
                evt -= handler;

                if (evt.GetInvocationList().Length <= 1) {
                    // remove the type when the no-op is the only remaining action
                    EventsByType.Remove(typeof(TDispatch));
                }
            }
        }
    }
}
