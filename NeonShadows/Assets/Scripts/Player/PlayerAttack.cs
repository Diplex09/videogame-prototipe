using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private float _cooldownTime = 0.8f;
    private float _nextFireTime = 0f;
    public static int _noOfClicks = 0;
    float _lastClickedTime = 0;
    float _maxComboDelay = 1;

    void Update()
    {

        if (_anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && _anim.GetCurrentAnimatorStateInfo(0).IsName("hit1"))
        {
            _anim.SetBool("hit1", false);
        }
        if (_anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && _anim.GetCurrentAnimatorStateInfo(0).IsName("hit2"))
        {
            _anim.SetBool("hit2", false);
        }
        if (_anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && _anim.GetCurrentAnimatorStateInfo(0).IsName("hit3"))
        {
            _anim.SetBool("hit3", false);
            _noOfClicks = 0;
        }


        if (Time.time - _lastClickedTime > _maxComboDelay)
        {
            _noOfClicks = 0;
        }

        //cooldown time
        if (Time.time > _nextFireTime)
        {
            // Check for mouse input
            if (Input.GetMouseButtonDown(0))
            {
                OnClick();
            }
        }
    }

    void OnClick()
    {
        //so it looks at how many clicks have been made and if one animation has finished playing starts another one.
        _lastClickedTime = Time.time;
        _noOfClicks++;
        if (_noOfClicks == 1)
        {
            _anim.SetBool("hit1", true);
        }
        _noOfClicks = Mathf.Clamp(_noOfClicks, 0, 3);

        if (_noOfClicks >= 2 && _anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && _anim.GetCurrentAnimatorStateInfo(0).IsName("hit1"))
        {
            _anim.SetBool("hit1", false);
            _anim.SetBool("hit2", true);
        }
        if (_noOfClicks >= 3 && _anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && _anim.GetCurrentAnimatorStateInfo(0).IsName("hit2"))
        {
            _anim.SetBool("hit2", false);
            _anim.SetBool("hit3", true);
        }
    }
}