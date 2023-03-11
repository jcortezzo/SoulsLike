using UnityEngine;

namespace OGS
{
    public class PlayerLocomotion : MonoBehaviour
    {
        Transform cameraObject;
        InputHandler inputHandler;
        private Vector3 moveDirection;

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
            //if (inputHandler.RollFlag)
            //{
            //    return;
            //}
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

            if (inputHandler.SprintFlag)
            {
                speed = sprintSpeed;
                playerManager.IsSprinting = true;
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

        public void HandleFalling(float delta)
        {
            Vector3 moveDirection = this.moveDirection;
            playerManager.IsGrounded = false;
            RaycastHit hit;
            Vector3 origin = myTransform.position;
            origin.y += groundDetectionRayStartPoint;

            if (Physics.Raycast(origin, myTransform.forward, out hit, 0.4f))
            {
                moveDirection = Vector3.zero;
            }
            if (playerManager.IsAirborne && Mathf.Abs(Rigidbody.velocity.y) < maxFallingSpeed)
            {
                Rigidbody.AddForce(-Vector3.up * fallingSpeed * delta);
                Rigidbody.AddForce(moveDirection * fallingSpeed / 40f * delta);

                if (Mathf.Abs(Rigidbody.velocity.y) > maxFallingSpeed)
                {
                    Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, maxFallingSpeed, Rigidbody.velocity.z);
                }
            }

            Vector3 dir = moveDirection;
            dir.Normalize();
            origin += dir * groundDirectionRayDistance;

            this.targetPosition = myTransform.position;

            Debug.DrawRay(origin, -Vector3.up * minDistanceNeededToBeginFall, Color.red, 0.1f, false);

            // Land
            if (Physics.Raycast(origin, -Vector3.up, out hit, minDistanceNeededToBeginFall, ignoreForGroundCheck))
            {
                normalVector = hit.normal;
                Vector3 targetPosition = hit.point;
                playerManager.IsGrounded = true;
                this.targetPosition.y = targetPosition.y;

                if (playerManager.IsAirborne)
                {
                    if (airborneTimer > 0.5f)
                    {
                        Debug.Log($"Player airborne for {airborneTimer}");
                        animatorHandler.PlayTargetAnimation("Land", true);
                    }
                    else
                    {
                        animatorHandler.PlayTargetAnimation("Locomotion", false);
                    }
                    airborneTimer = 0;

                    playerManager.IsAirborne = false;
                }
            }
            else
            {
                playerManager.IsGrounded = false;
                if (!playerManager.IsAirborne)
                {
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

            if (playerManager.IsGrounded)
            {
                if (playerManager.IsInteracting || inputHandler.MoveAmount > 0)
                {
                    myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime);
                }
                else
                {
                    myTransform.position = targetPosition;
                }
            }
        }

        public void Roll()
        {
            if (animatorHandler.anim.GetBool("IsInteracting"))
            {
                return;
            }

            moveDirection = cameraObject.forward * inputHandler.Vertical;
            moveDirection += cameraObject.right * inputHandler.Horizontal;

            if (inputHandler.MoveAmount > 0)
            {
                animatorHandler.PlayTargetAnimation("Roll", true);
                moveDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                myTransform.rotation = rollRotation;
            }
            else
            {
                animatorHandler.PlayTargetAnimation("Backstep", true);
            }
        }
        #endregion
    }
}
