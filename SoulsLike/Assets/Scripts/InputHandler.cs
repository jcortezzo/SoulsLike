using UnityEngine;

namespace OGS
{
    public class InputHandler : MonoBehaviour
    {
        [field: SerializeField]
        public float Horizontal { get; private set; }
        [field: SerializeField]
        public float Vertical { get; private set; }
        [field: SerializeField]
        public float MoveAmount { get; private set; }
        [field: SerializeField]
        public float MouseX { get; private set; }
        [field: SerializeField]
        public float MouseY { get; private set; }

        PlayerControls inputActions;
        CameraHandler cameraHandler;

        Vector2 movementInput;
        Vector2 cameraInput;

        private void Awake()
        {
            cameraHandler = CameraHandler.Instance;
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;

            cameraHandler?.FollowTarget(delta);
            cameraHandler?.HandleCameraRotation(delta, MouseX, MouseY);
        }

        public void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += action => movementInput = action.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += action => cameraInput = action.ReadValue<Vector2>();
            }

            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            MoveInput(delta);
            CameraInput(delta);
        }

        private void MoveInput(float delta)
        {
            Horizontal = movementInput.x;
            Vertical = movementInput.y;
            MoveAmount = Mathf.Clamp01(Mathf.Abs(Horizontal) + Mathf.Abs(Vertical));
        }

        private void CameraInput(float delta)
        {
            MouseX = cameraInput.x;
            MouseY = cameraInput.y;
        }
    }
}
