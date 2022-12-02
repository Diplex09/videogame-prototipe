using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Ball : NetworkBehaviour
{
    [SerializeField] private int _speed = 5;
    [SerializeField] private int _lifeTime = 2;
    [Networked] private TickTimer _life { get; set; }
    [SerializeField] private AudioClip _shootSound;
    [SerializeField] private AudioSource _audioSource;

    
    private void AwakeNetworked()
    {
        //Play shoot sound
        _audioSource.PlayOneShot(_shootSound);
    }

    public void Init() {
        _life = TickTimer.CreateFromSeconds(Runner, _lifeTime);
    }

    public override void FixedUpdateNetwork() {
        if (_life.Expired(Runner)) {
            Runner.Despawn(Object);
        } else {
            transform.position += transform.forward * _speed * Runner.DeltaTime;
        }
        // si choca con el suelo, desaparecer
        
    }

    // si choca con otro jugador, aplicarle knockback
}