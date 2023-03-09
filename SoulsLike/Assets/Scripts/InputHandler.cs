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

        Vector2 movementInput;
        Vector2 cameraInput;

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
        }

        private void MoveInput(float delta)
        {
            Horizontal = movementInput.x;
            Vertical = movementInput.y;
            MoveAmount = Mathf.Clamp01(Mathf.Abs(Horizontal) + Mathf.Abs(Vertical));
            MouseX = cameraInput.x;
            MouseY = cameraInput.y;
        }
    }
}
