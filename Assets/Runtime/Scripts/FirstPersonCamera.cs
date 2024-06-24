using UnityEngine;

namespace AceTest {
    /// <summary></summary>
    public class FirstPersonCamera : MonoBehaviour {

        #region Instance Vars

        [SerializeField]
        private float _mouseSensitivity = 10f;

        private float _verticalRotation;
        private float _horizontalRotation;

        public Transform Target { get; set; }

        #endregion



        void LateUpdate() {
            if (Target == null) return;

            transform.position = Target.position;

            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            _verticalRotation -= mouseY * _mouseSensitivity;
            _verticalRotation = Mathf.Clamp(_verticalRotation, -70f, 70f);

            _horizontalRotation += mouseX * _mouseSensitivity;

            transform.rotation = Quaternion.Euler(_verticalRotation, _horizontalRotation, 0);
        }
    }
}
