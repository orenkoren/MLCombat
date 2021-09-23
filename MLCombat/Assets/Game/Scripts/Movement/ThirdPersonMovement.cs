using MiddleAges.Entities;
using MiddleAges.Global;
using UnityEngine;


namespace MiddleAges.Motion
{
    public class ThirdPersonMovement : Movement
    {
        public float turnSmoothTime = 0.1f;

        private Player player;
        private float turnSmoothVelocity;
        private Vector3 inputDirection;
        private Vector3 playerMovement;
        private float targetDirection;
        private float smoothTargetAngle;
        private float camAngle;
        private float horizontalInput;
        private float verticalInput;
        private bool isFlying;

        #region Unity Methods

        protected override void Start()
        {
            base.Start();
            player = (Player)entity;
        }

        protected override void FixedUpdate()
        {
            if (entity.IsAlive() == false) return;
            base.FixedUpdate();
            horizontalInput = Input.GetAxis("Horizontal"); // input x
            verticalInput = Input.GetAxis("Vertical"); //input z
            camAngle = (player).playerCam.eulerAngles.y;
            inputDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

            ApplyRotation();
            ApplyMovement();
            Flying();
            entity.rb.AddForce(new Vector3(0, entity.animator.GetFloat("GravityWeight") * -1 * entity.rb.mass, 0));
        }

        private void Update()
        {
            if (entity.IsAlive() == false) return;
            ApplyJumps();
        }

        #endregion Unity Methods

        #region Public Methods

        private void PlayJumpAnimations()
        {
            if (inputDirection.magnitude >= 0.1f) // currently moving
                entity.animator.SetTrigger("RunningJump");
            else
                entity.animator.SetTrigger("Jump");
        }

        #endregion Public Methods

        #region Private Methods

        private void ApplyRotation()
        {
            if (IsRotationRooted()) return;
            targetDirection = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + player.playerCam.eulerAngles.y;
            smoothTargetAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetDirection, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = CalculateRotation();
        }

        private Quaternion CalculateRotation()
        {
            return Quaternion.Euler(0f, smoothTargetAngle, 0f);
        }

        private void ApplyMovement()
        {
            moveDirection = (Quaternion.Euler(0f, targetDirection, 0f) * Vector3.forward).normalized;
            float motionValue = new Vector3(horizontalInput, 0, verticalInput).magnitude;
            if (inputDirection.magnitude >= 0.1f && IsGrounded() && IsNotRooted())
            {
                float speedValue = OriginalSpeed * (SpeedModifiers / 100);
                entity.rb.velocity = new Vector3(moveDirection.x * speedValue * Time.deltaTime, entity.rb.velocity.y,
                                                moveDirection.z * speedValue * Time.deltaTime);
            }
            else if (!isMovingExternally)
            {
                entity.rb.velocity = new Vector3(0, entity.rb.velocity.y, 0);
            }
            PlayMotionAnimations(Mathf.Clamp(motionValue - (100 - SpeedModifiers) / 100, 0, 1f));
        }

        private void ApplyJumps()
        {
            bool jump = Input.GetKeyDown(KeyCode.Space);
            if (IsGrounded() && jump && !isFlying)
            {
                PlayJumpAnimations();
            }
        }

        private void Flying()
        {
            if (isFlying && Input.GetKey(KeyCode.Space))
            {
                entity.rb.velocity = new Vector3(0, 10, 0);
            }

        }

        void OnCollisionEnter(Collision collisionInfo)
        {
            if (collisionInfo.gameObject.tag == "Transporter")
            {
                OriginalSpeed *= 20;
            }
            if (collisionInfo.gameObject.tag == "InvisWall")
            {
                ApplyRoot(gameObject, true);
            }
        }

        void OnTriggerEnter(Collider collisionInfo)
        {
            if (collisionInfo.gameObject.tag == "Elevator")
            {
                isFlying = true;
            }
        }

        void OnTriggerExit(Collider collisionInfo)
        {
            if (collisionInfo.gameObject.tag == "Elevator")
            {
                isFlying = false;
            }
        }


        void OnCollisionExit(Collision collisionInfo)
        {
            if (collisionInfo.gameObject.tag == "InvisWall")
            {
                ApplyRoot(gameObject, false);
            }
            if (collisionInfo.gameObject.tag == "Transporter")
            {
                OriginalSpeed /= 20;
            }
        }

        #endregion Private Methods

    }
}
