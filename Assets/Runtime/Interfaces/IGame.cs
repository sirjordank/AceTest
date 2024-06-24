using System;

namespace AceTest {
    /// <summary></summary>
    public interface IGame {

        #region Class Vars

        private static IGame _instance;

        /// <summary></summary>
        public static IGame Instance {
            get => _instance;
            set {
                if (_instance != default) {
                    throw new InvalidOperationException();
                }

                _instance = value;
            }
        }

        #endregion



        #region Instance Vars

        /// <summary></summary>
        public InputDefault Input { get; }

        #endregion
    }
}
