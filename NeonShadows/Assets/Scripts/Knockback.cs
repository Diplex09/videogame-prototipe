using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] float _knockbackVel;
    [SerializeField] float _knockbackMult;

    public void ApplyKnockback(Transform t)
    {
        Vector3 dir = transform.position - t.position;
        dir.Normalize();
        dir = new Vector3(dir.x + _knockbackMult, dir.y, dir.z + _knockbackMult);
        _rigidbody.velocity = dir * _knockbackVel;
        StartCoroutine(ResetVelocity());
    }

    IEnumerator ResetVelocity()
    {
        yield return new WaitForSeconds(0.5f);
        _rigidbody.velocity = Vector3.zero;
    }
}