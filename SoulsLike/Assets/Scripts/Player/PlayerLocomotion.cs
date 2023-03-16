using UnityEngine;

namespace OGS
{
    public class PlayerLocomotion : MonoBehaviour
    {
        Transform cameraObject;
        InputHandler inputHandler;
        public Vector3 moveDirection;

        [HideInInspector]
        public Transform myTransform;
        [HideInInspector]
        public AnimatorHandler animatorHandler;

        public Rigidbody Rigidbody { get; private set; }
        public GameObject normalCamera;

        private PlayerManager playerManager;

        [Header("Ground & Air Detection Stats")]
        [SerializeField]
        private float groundDetectionRayStartPoint = 0.5f;
        [SerializeField]
        private float minDistanceNeededToBeginFall = 1f;
        [SerializeField]
        private float groundDirectionRayDistance = 0.2f;
        private LayerMask ignoreForGroundCheck;
        public float airborneTimer;

        [Header("Movement Stats")]
        [SerializeField]
        private float movementSpeed = 5;
        [SerializeField]
        private float sprintSpeed = 7;
        [SerializeField]
        private float rotationSpeed = 10;
        [SerializeField]
        private float fallingSpeed = 45;
        [SerializeField]
        private float maxFallingSpeed = 45;
        [SerializeField]
        private float bumpOffLedgeSpeed = 10f;

        void Start()
        {
            playerManager = GetComponent<PlayerManager>();
            Rigidbody = GetComponent<Rigidbody>();
            inputHandler = GetComponent<InputHandler>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            cameraObject = Camera.main.transform;
            myTransform = transform;
            animatorHandler.Initialize();

            playerManager.IsGrounded = true;
            ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
        }

        #region Movement
        Vector3 normalVector;
        Vector3 targetPosition;

        public void HandleMovement(float delta)
        {
            if (playerManager.IsInteracting)
            {
                return;
            }
            HandleWalking();
            animatorHandler.UpdateAnimatorValues(inputHandler.MoveAmount, 0, playerManager.IsSprinting);
            if (animatorHandler.canRotate)
            {
                HandleRotation(delta);
            }
        }

        private void HandleWalking()
        {
            moveDirection = cameraObject.forward * inputHandler.Vertical;
            moveDirection += cameraObject.right * inputHandler.Horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            float speed = movementSpeed;

            if (inputHandler.SprintFlag && inputHandler.MoveAmount > 0.5)
            {
                speed = sprintSpeed;
                playerManager.IsSprinting = true;
            }
            else
            {
                playerManager.IsSprinting = false;
            }
            moveDirection *= speed;

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            Rigidbody.velocity = projectedVelocity;
        }

        private void HandleRotation(float delta)
        {
            Vector3 targetDir;
            float moveOverride = inputHandler.MoveAmount;

            targetDir = cameraObject.forward * inputHandler.Vertical;
            targetDir += cameraObject.right * inputHandler.Horizontal;

            targetDir.Normalize();
            targetDir.y = 0;

            if (targetDir == Vector3.zero)
            {
                targetDir = myTransform.forward;
            }

            float rs = rotationSpeed;
            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

            myTransform.rotation = targetRotation;
        }

        public void HandleFalling(float delta, Vector3 moveDirection)
        {
            RaycastHit hit;
            Vector3 origin = myTransform.position + new Vector3(0, groundDetectionRayStartPoint, 0);

            if (Physics.Raycast(origin, myTransform.forward, out hit, 0.4f))
            {
                moveDirection = Vector3.zero;
            }
            if (playerManager.IsAirborne)
            {
                Rigidbody.AddForce(Vector3.down * fallingSpeed * delta);
                Rigidbody.AddForce(moveDirection * fallingSpeed / bumpOffLedgeSpeed);
            }

            Vector3 dir = moveDirection;
            dir.Normalize();
            origin += dir * groundDirectionRayDistance;

            this.targetPosition = myTransform.position;

            Debug.DrawRay(origin, Vector3.down * minDistanceNeededToBeginFall, Color.green, 0.1f, false);

            // Land
            if (Physics.Raycast(origin, Vector3.down, out hit, minDistanceNeededToBeginFall, ignoreForGroundCheck))
            {
                normalVector = hit.normal;
                Vector3 tp = hit.point;
                playerManager.IsGrounded = true;
                targetPosition.y = tp.y;

                if (playerManager.IsAirborne)
                {
                    if (airborneTimer > 0.5f)
                    {
                        Debug.Log($"Player airborne for {airborneTimer}");
                        animatorHandler.PlayTargetAnimation("Land", true);
                    }
                    else
                    {
                        animatorHandler.PlayTargetAnimation("Empty", false);
                    }
                    airborneTimer = 0;

                    playerManager.IsAirborne = false;
                }
            }
            // Airborne
            else
            {
                playerManager.IsGrounded = false;
                if (!playerManager.IsAirborne)
                {
                    //Rigidbody.AddForce(moveDirection * bumpOffLedgeSpeed);
                    if (!playerManager.IsInteracting)
                    {
                        animatorHandler.PlayTargetAnimation("Fall", true);
                    }

                    Vector3 vel = Rigidbody.velocity;
                    vel.Normalize();
                    Rigidbody.velocity = vel * (movementSpeed / 2);
                    playerManager.IsAirborne = true;
                }
            }

            if (playerManager.IsInteracting || inputHandler.MoveAmount > 0)
            {
                myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else
            {
                myTransform.position = targetPosition;
            }
        }
        #endregion

        public void Roll()
        {
            if (animatorHandler.anim.GetBool("IsInteracting") || playerManager.IsAirborne)
            {
                return;
            }

            moveDirection = cameraObject.forward * inputHandler.Vertical;
            moveDirection += cameraObject.right * inputHandler.Horizontal;

            if (inputHandler.MoveAmount > 0)
            {
                animatorHandler.PlayTargetAnimation("NewRoll", true);
                moveDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                myTransform.rotation = rollRotation;
            }
            else
            {
                animatorHandler.PlayTargetAnimation("Backstep", true);
            }
        }
    }
}
