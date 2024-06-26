using System;
using System.Collections.Generic;
using UnityEngine;

namespace AceTest {
    /// <summary>Interface to manage an ecosystem of dispatches derived from the given type.</summary>
    /// <typeparam name="TDispatchBase">Base type of dispatch to manage.</typeparam>
    public interface IDispatcher<TDispatchBase> : IDisposable where TDispatchBase : DispatchBase {

        #region Class Vars

        /// <summary>Prefix that is logged by default for each dispatch.</summary>
        private static string NoopLogPrefix => "Invoking dispatch of type:";

        #endregion



        #region Instance Vars

        /// <summary>All events indexed by their type.</summary>
        protected Dictionary<Type, object> EventsByType { get; }

        #endregion



        void IDisposable.Dispose() { }

        /// <summary>Gets the action of the given type, if any.</summary>
        /// <typeparam name="T">Type of dispatch to get.</typeparam>
        /// <returns>Action of the given type.</returns>
        protected Action<T> GetAction<T>() where T : TDispatchBase => EventsByType[typeof(T)] as Action<T>;

        /// <summary>Invokes the given dispatch for the current listeners, if any.</summary>
        /// <typeparam name="T">Type of dispatch to invoke.</typeparam>
        /// <param name="dispatch">Pre-constructed dispatch to be invoked.</param>
        public void Invoke<T>(T dispatch) where T : TDispatchBase => GetAction<T>()?.Invoke(dispatch);

        /// <summary>Adds the given handler to the dispatch type.</summary>
        /// <typeparam name="T">Type of dispatch to add to.</typeparam>
        /// <param name="handler">Handler to invoke when the event is dispatched.</param>
        public void Add<T>(Action<T> handler) where T : TDispatchBase {
            if (!EventsByType.ContainsKey(typeof(T))) {
                // add what is essentially a no-op as the base action for the type
                EventsByType.Add(typeof(T), new Action<T>(_ => Debug.Log(NoopLogPrefix + typeof(T))));
            }

            Action<T> evt = GetAction<T>();
            EventsByType[typeof(T)] = evt += handler;
        }

        /// <summary>Removes the given handler from the dispatch type.</summary>
        /// <typeparam name="T">Type of dispatch to remove from.</typeparam>
        /// <param name="handler">Handler to invoke when the event is dispatched.</param>
        public void Remove<T>(Action<T> handler) where T : TDispatchBase {
            if (EventsByType.ContainsKey(typeof(T))) {
                Action<T> evt = GetAction<T>();
                EventsByType[typeof(T)] = evt -= handler;

                if (evt.GetInvocationList().Length <= 1) {
                    // remove the type when the no-op is the only remaining action
                    EventsByType.Remove(typeof(T));
                }
            }
        }
    }
}
