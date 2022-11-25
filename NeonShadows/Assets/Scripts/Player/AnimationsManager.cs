using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsManager : MonoBehaviour
{
    [SerializeField] Animator _anim;
    [SerializeField] PlayerMovementAdvanced _playerMovementAdv;
    [SerializeField] WallRunningAdvanced _wallRunning;

    private void Update()
    {
        if((_playerMovementAdv.horizontalInput != 0 || _playerMovementAdv.verticalInput != 0) && !_playerMovementAdv.crouching)
        {
            _anim.SetBool("walking", true);
        }
        else
        {
            _anim.SetBool("walking", false);
        }

        if((_playerMovementAdv._state == PlayerMovementAdvanced.MovementState.sprinting) && (_playerMovementAdv.horizontalInput != 0 || _playerMovementAdv.verticalInput != 0))
        {
            _anim.SetBool("running", true);
        }
        else
        {
            _anim.SetBool("running", false);
        }

        if ((_playerMovementAdv.horizontalInput != 0 || _playerMovementAdv.verticalInput != 0) && _playerMovementAdv.crouching)
        {
            _anim.SetBool("crouchWalk", true);
        }
        else
        {
            _anim.SetBool("crouchWalk", false);
        }

        if(_playerMovementAdv.wallrunning && _wallRunning.wallLeft)
        {
            _anim.SetBool("wallRunLeft", true);
        }
        else
        {
            _anim.SetBool("wallRunLeft", false);
        }

        if (_playerMovementAdv.wallrunning && _wallRunning.wallRight)
        {
            _anim.SetBool("wallRunRight", true);
        }
        else
        {
            _anim.SetBool("wallRunRight", false);
        }

        _anim.SetBool("grounded", _playerMovementAdv._grounded);

        _anim.SetBool("crouching", _playerMovementAdv.crouching);

        _anim.SetBool("sliding", _playerMovementAdv.sliding);

        _anim.SetBool("climbing", _playerMovementAdv.climbing);
    }
}