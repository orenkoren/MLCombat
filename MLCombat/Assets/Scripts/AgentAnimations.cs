using StarterAssets;
using UnityEngine;

public class AgentAnimations : MonoBehaviour
{
    private Animator anim;
    private StarterAssetsInputs input;
    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDFreeFall;
    private int _animIDMotionSpeed;
    private float _animationBlend;
    private float targetSpeed = 2f;
    private float SpeedChangeRate = 10.0f;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private float RotationSmoothTime = 0.12f;

    private void Start()
    {
        anim = GetComponent<Animator>();
        input = GetComponent<StarterAssetsInputs>();
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        //var currentSpeed = anim.GetFloat("Speed");
        //if (input.move.magnitude > 0 && currentSpeed  != 2f)
        //{
        //    anim.SetFloat("Speed", 2.0f);
        //}
        //else if(currentSpeed != 0)
        //{
        //    anim.SetFloat("Speed", 0f);
        //}

        _animationBlend = Mathf.Lerp(_animationBlend, input.move.magnitude * targetSpeed, Time.deltaTime * SpeedChangeRate);

        Vector3 inputDirection = new Vector3(input.move.x, 0.0f, input.move.y).normalized;

        if (input.move != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);

            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        float inputMagnitude = input.analogMovement ? input.move.magnitude : 1f;

        anim.SetFloat(_animIDSpeed, _animationBlend);
        anim.SetFloat(_animIDMotionSpeed, inputMagnitude);
    }

}
