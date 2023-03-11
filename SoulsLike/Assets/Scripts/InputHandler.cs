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

        PlayerManager playerManager;

        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
        }

        public void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += ctx => cameraInput = ctx.ReadValue<Vector2>();
                inputActions.PlayerMovement.Sprint.performed += ctx =>
                {
                    Debug.Log("Sprint!");
                    SprintFlag = true;
                };
                inputActions.PlayerMovement.Sprint.canceled += ctx =>
                {
                    Debug.Log("End Sprint");
                    SprintFlag = false;
                };

                inputActions.PlayerActions.Roll.performed += ctx =>
                {
                    Debug.Log("Roll!");
                    playerManager.RollEvent.Invoke();
                };

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
