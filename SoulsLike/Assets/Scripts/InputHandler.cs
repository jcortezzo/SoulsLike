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
        [field: SerializeField]
        public bool RollInput { get; private set; }
        [field: SerializeField]
        public bool RollFlag { get; set; }
        [field: SerializeField]
        public bool SprintFlag { get; set; }
        public float rollInputTimer;


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
            CameraInput(delta);
            HandleRollInput(delta);
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

        private void HandleRollInput(float delta)
        {
            RollInput = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Performed;
            //Debug.Log($"inputAction: {inputActions.PlayerActions.Roll.phase}");
            //Debug.Log($"UnityEngine: {UnityEngine.InputSystem.InputActionPhase.Started}");
            if (RollInput)
            {
                //Debug.Log("Pressed roll!");
                rollInputTimer += delta;
                SprintFlag = true;
            }
            else
            {
                if (rollInputTimer > 0 && rollInputTimer < 0.5f)
                {
                    SprintFlag = false;
                    RollFlag = true;
                }
                rollInputTimer = 0;
            }
        }
    }
}
