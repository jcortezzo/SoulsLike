using UnityEngine;
using UnityEngine.Events;

namespace OGS
{
    public class PlayerManager : MonoBehaviour
    {
        InputHandler inputHandler;
        Animator anim;
        private CameraHandler cameraHandler;
        private PlayerLocomotion playerLocomotion;
        private PlayerInventory playerInventory;
        private PlayerAttacker playerAttacker;

        [field: SerializeField]
        public bool IsInteracting { get; set; }
        [field: SerializeField]
        public bool IsSprinting { get; set; }
        [field: SerializeField]
        public bool IsAirborne { get; set; }
        [field: SerializeField]
        public bool IsGrounded { get; set; }

        [field: SerializeField]
        public bool IsActionable { get { return !IsInteracting && !IsAirborne; } }

        public UnityEvent RollEvent { get; private set; }
        public UnityEvent<WeaponItem> RBEvent { get; private set; }
        public UnityEvent<WeaponItem> RTEvent { get; private set; }

        private void Awake()
        {
            cameraHandler = CameraHandler.Instance;
            RollEvent = new UnityEvent();
            RBEvent = new UnityEvent<WeaponItem>();
            RTEvent = new UnityEvent<WeaponItem>();
        }

        // Start is called before the first frame update
        void Start()
        {
            inputHandler = GetComponent<InputHandler>();
            anim = GetComponentInChildren<Animator>();

            playerLocomotion = GetComponent<PlayerLocomotion>();
            RollEvent.AddListener(playerLocomotion.Roll);

            playerInventory = GetComponent<PlayerInventory>();
            playerAttacker = GetComponent<PlayerAttacker>();
            RBEvent.AddListener(playerAttacker.HandleLightAttack);
            RTEvent.AddListener(playerAttacker.HandleHeavyAttack);
        }

        // Update is called once per frame
        void Update()
        {
            float delta = Time.deltaTime;

            IsInteracting = anim.GetBool("IsInteracting");

            inputHandler.TickInput(delta);
            playerLocomotion?.HandleMovement(delta);
            playerLocomotion?.HandleFalling(delta, playerLocomotion.moveDirection);
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
