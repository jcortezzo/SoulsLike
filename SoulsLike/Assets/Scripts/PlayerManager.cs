using UnityEngine;
using UnityEngine.Events;

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
        [field: SerializeField]
        public bool IsAirborne { get; set; }
        [field: SerializeField]
        public bool IsGrounded { get; set; }

        public UnityEvent RollEvent { get; private set; }

        private void Awake()
        {
            cameraHandler = CameraHandler.Instance;
            RollEvent = new UnityEvent();
        }

        // Start is called before the first frame update
        void Start()
        {
            inputHandler = GetComponent<InputHandler>();
            anim = GetComponentInChildren<Animator>();

            playerLocomotion = GetComponent<PlayerLocomotion>();
            RollEvent.AddListener(playerLocomotion.Roll);
        }

        // Update is called once per frame
        void Update()
        {
            float delta = Time.deltaTime;

            IsInteracting = anim.GetBool("IsInteracting");

            inputHandler.TickInput(delta);
            playerLocomotion?.HandleMovement(delta);
            playerLocomotion?.HandleFalling(delta);
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;

            cameraHandler?.FollowTarget(delta);
            cameraHandler?.HandleCameraRotation(delta, inputHandler.MouseX, inputHandler.MouseY);
        }

        private void LateUpdate()
        {
            IsSprinting = inputHandler.SprintFlag;

            if (IsAirborne)
            {
                playerLocomotion.airborneTimer += Time.deltaTime;
            }
        }
    }
}
