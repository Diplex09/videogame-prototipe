using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerMovementAdvanced : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _desiredMoveSpeed;
    [SerializeField] private float _lastDesiredMoveSpeed;
    [SerializeField] private float _walkSpeed;
    public float WalkSpeed {
        get => _walkSpeed;
        set{
            _walkSpeed = value;
            if (_walkSpeed == 10) {
                //Fly();
            }
        }
    }
    [SerializeField] private float _sprintSpeed;
    [SerializeField] private float _slideSpeed;
    [SerializeField] private float _wallrunSpeed;
    [SerializeField] private float _climbSpeed;
    [SerializeField] private float _vaultSpeed;
    [SerializeField] private float _airMinSpeed;

    [SerializeField] private float _speedIncreaseMultiplier;
    [SerializeField] private float _slopeIncreaseMultiplier;

    [SerializeField] private float _groundDrag;

    [Header("Jumping")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpCooldown;
    [SerializeField] private float _airMultiplier;
    private bool _readyToJump;

    [Header("Crouching")]
    [SerializeField] private float _crouchSpeed;
    [SerializeField] private float _crouchYScale;
    [SerializeField] private float _startYScale;

    [Header("Keybinds")]
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode _sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode _crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private bool _grounded;
    public bool Grounded {
        get => _grounded;
        set => _grounded = value;
    }

    [Header("Slope Handling")]
    [SerializeField] private float _maxSlopeAngle;
    private RaycastHit _slopeHit;
    [SerializeField] private bool _exitingSlope;

    [Header("References")]
    [SerializeField] private Climbing _climbingScript;
    [SerializeField] private ClimbingDone _climbingScriptDone;

    [SerializeField] private Transform _orientation;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 _moveDirection;

    [SerializeField] private Rigidbody _rb;

    [SerializeField] private MovementState _state;
    public enum MovementState
    {
        freeze,
        unlimited,
        walking,
        sprinting,
        wallrunning,
        climbing,
        vaulting,
        crouching,
        sliding,
        air
    }

    public bool sliding;
    public bool crouching;
    public bool wallrunning;
    public bool climbing;
    public bool vaulting;

    public bool freeze;
    public bool unlimited;

    public bool restricted;

    public TextMeshProUGUI text_speed;
    public TextMeshProUGUI text_mode;

    Vector3 direction;

    private void Start()
    {
        _rb.freezeRotation = true;
        _readyToJump = true;
        _startYScale = transform.localScale.y;
    }

    private void Update()
    {
        MyInput();
        SpeedControl();
        StateHandler();
        TextStuff();

        // handle drag
        if (_state == MovementState.walking || _state == MovementState.sprinting || _state == MovementState.crouching)
            _rb.drag = _groundDrag;
        else
            _rb.drag = 0;
    }

    private void FixedUpdate()
    {
        // ! ground check (dentro de FixedUpdate [todo lo relacionado con físicas])
        _grounded = Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f, _whatIsGround);

        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKeyDown(_jumpKey) && _readyToJump && _grounded)
        {
            _readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), _jumpCooldown);
        }

        // start crouch
        if (Input.GetKeyDown(_crouchKey) && horizontalInput == 0 && verticalInput == 0)
        {
            transform.localScale = new Vector3(transform.localScale.x, _crouchYScale, transform.localScale.z);
            // ! AddForce no se debe de ejecutar dentro de Update
            _rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

            crouching = true;
        }

        // stop crouch
        if (Input.GetKeyUp(_crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, _startYScale, transform.localScale.z);

            crouching = false;
        }
    }

    bool keepMomentum;
    private void StateHandler()
    {
        // Mode - Freeze
        
        // ! Se puede cambiar a case state para optimizar y el cambiar el estado de movimiento no debería estar en el Update
        if (freeze)
        {
            _state = MovementState.freeze;
            // ! velocity no se debe de ejecutar dentro de Update
            _rb.velocity = Vector3.zero;
            _desiredMoveSpeed = 0f;
        }

        // Mode - Unlimited
        else if (unlimited)
        {
            _state = MovementState.unlimited;
            _desiredMoveSpeed = 999f;
        }

        // Mode - Vaulting
        else if (vaulting)
        {
            _state = MovementState.vaulting;
            _desiredMoveSpeed = _vaultSpeed;
        }

        // Mode - Climbing
        else if (climbing)
        {
            _state = MovementState.climbing;
            _desiredMoveSpeed = _climbSpeed;
        }

        // Mode - Wallrunning
        else if (wallrunning)
        {
            _state = MovementState.wallrunning;
            _desiredMoveSpeed = _wallrunSpeed;
        }

        // Mode - Sliding
        else if (sliding)
        {
            _state = MovementState.sliding;
            transform.localScale = new Vector3(transform.localScale.x, _crouchYScale, transform.localScale.z);

            // increase speed by one every second
            if (OnSlope() && _rb.velocity.y < 0.1f)
            {
                _desiredMoveSpeed = _slideSpeed;
                keepMomentum = true;
            }

            else
                _desiredMoveSpeed = _sprintSpeed;
        }

        // Mode - Crouching
        else if (crouching)
        {
            _state = MovementState.crouching;
            _desiredMoveSpeed = _crouchSpeed;
        }

        // Mode - Sprinting
        else if (_grounded && Input.GetKey(_sprintKey))
        {
            _state = MovementState.sprinting;
            _desiredMoveSpeed = _sprintSpeed;
        }

        // Mode - Walking
        else if (_grounded)
        {
            _state = MovementState.walking;
            _desiredMoveSpeed = _walkSpeed;
        }

        // Mode - Air
        else
        {
            _state = MovementState.air;

            if (_moveSpeed < _airMinSpeed)
                _desiredMoveSpeed = _airMinSpeed;
        }

        bool desiredMoveSpeedHasChanged = _desiredMoveSpeed != _lastDesiredMoveSpeed;

        if (desiredMoveSpeedHasChanged)
        {
            if (keepMomentum)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothlyLerpMoveSpeed());
            }
            else
            {
                _moveSpeed = _desiredMoveSpeed;
            }
        }

        _lastDesiredMoveSpeed = _desiredMoveSpeed;

        // deactivate keepMomentum
        if (Mathf.Abs(_desiredMoveSpeed - _moveSpeed) < 0.1f) keepMomentum = false;
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        // smoothly lerp movementSpeed to desired value
        float time = 0;
        float difference = Mathf.Abs(_desiredMoveSpeed - _moveSpeed);
        float startValue = _moveSpeed;

        while (time < difference)
        {
            _moveSpeed = Mathf.Lerp(startValue, _desiredMoveSpeed, time / difference);

            if (OnSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, _slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * _speedIncreaseMultiplier * _slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else
                time += Time.deltaTime * _speedIncreaseMultiplier;

            yield return null;
        }

        _moveSpeed = _desiredMoveSpeed;
    }

// ! Almacenar los números de este métodos en variables para saber para qué sirven
    private void MovePlayer()
    {
        if (_climbingScript.exitingWall) return;
        if (_climbingScriptDone.exitingWall) return;
        if (restricted) return;

        // calculate movement direction
        _moveDirection = _orientation.forward * verticalInput + _orientation.right * horizontalInput;

        // on slope
        if (OnSlope() && !_exitingSlope)
        {
            _rb.AddForce(GetSlopeMoveDirection(_moveDirection) * _moveSpeed * 20f, ForceMode.Force);

            if (_rb.velocity.y > 0)
                _rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        else if (_grounded)
            _rb.AddForce(_moveDirection.normalized * _moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!_grounded)
        // ! Utilizar solamente _airMultipluer para la multiplicación (borrar el 10f)
            _rb.AddForce(_moveDirection.normalized * _moveSpeed * 10f * _airMultiplier, ForceMode.Force);

        // turn gravity off while on slope
        if (!wallrunning) _rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        // limiting speed on slope
        if (OnSlope() && !_exitingSlope)
        {
            if (_rb.velocity.magnitude > _moveSpeed)
                _rb.velocity = _rb.velocity.normalized * _moveSpeed;
        }

        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > _moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * _moveSpeed;
                _rb.velocity = new Vector3(limitedVel.x, _rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        _exitingSlope = true;

        // reset y velocity
        _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        _readyToJump = true;

        _exitingSlope = false;
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out _slopeHit, _playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return angle < _maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, _slopeHit.normal).normalized;
    }

    private void TextStuff()
    {
        Vector3 flatVel = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

        if (OnSlope())
            text_speed.SetText("Speed: " + Round(_rb.velocity.magnitude, 1) + " / " + Round(_moveSpeed, 1));

        else
            text_speed.SetText("Speed: " + Round(flatVel.magnitude, 1) + " / " + Round(_moveSpeed, 1));

        text_mode.SetText(_state.ToString());
    }


    // ! Esto lo podríamos convertir en una clase de C# (no de MonoBehaviour) para poder utilizarla en todos lados
    public static float Round(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }
}
