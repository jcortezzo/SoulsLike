using UnityEngine;

namespace OGS
{
    public class PlayerManager : MonoBehaviour
    {
        InputHandler inputHandler;
        Animator anim;
        CameraHandler cameraHandler;
        PlayerLocomotion playerLocomotion;

        [field: SerializeField]
        public bool IsInteracting { get; set; }
        [field: SerializeField]
        public bool IsSprinting { get; set; }

        private void Awake()
        {
            cameraHandler = CameraHandler.Instance;
        }

        // Start is called before the first frame update
        void Start()
        {
            inputHandler = GetComponent<InputHandler>();
            anim = GetComponentInChildren<Animator>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
        }

        // Update is called once per frame
        void Update()
        {
            float delta = Time.deltaTime;

            IsInteracting = anim.GetBool("IsInteracting");

            inputHandler.TickInput(delta);
            playerLocomotion?.HandleMovement(delta);
            playerLocomotion?.HandleRollAndSprint(delta);
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;

            cameraHandler?.FollowTarget(delta);
            cameraHandler?.HandleCameraRotation(delta, inputHandler.MouseX, inputHandler.MouseY);
        }

        private void LateUpdate()
        {
            inputHandler.RollFlag = false;
            inputHandler.SprintFlag = false;
            IsSprinting = inputHandler.RollInput;
        }
    }
}
