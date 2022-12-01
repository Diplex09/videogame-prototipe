using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private GameObject _hitbox;
    [SerializeField] private GameObject _slash1;
    [SerializeField] private GameObject _slash2;
    [SerializeField] private GameObject _slash3;
    private int _combo;
    private bool _attacking;
    private bool _activeAttack;

    public bool Attacking {
        get => _activeAttack;
        set => _activeAttack = value;
    }

    private void Update()
    {
        Combos();
    }

    void Combos()
    {
        if (Input.GetMouseButtonDown(0) && !_attacking)
        {
            _attacking = true;
            _anim.SetTrigger("" + _combo);
        }
    }

    public void StartCombo()
    {
        _attacking = false;
        if(_combo < 3)
        {
            _combo++;
        }
    }

    public void FinishAnimation()
    {
        _attacking = false;
        _combo = 0;
        _activeAttack = false;
        _hitbox.SetActive(false);
    }

    public void EnableAttack()
    {
        _activeAttack = true;
        _hitbox.SetActive(true);
    }

    public void Slash1()
    {
        _slash1.SetActive(true);
    }

    public void Slash2()
    {
        _slash2.SetActive(true);
    }

    public void Slash3()
    {
        _slash3.SetActive(true);
    }

    public void DisableSlash()
    {
        _slash1.SetActive(false);
        _slash2.SetActive(false);
        _slash3.SetActive(false);
    }
}