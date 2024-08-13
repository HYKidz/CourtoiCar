using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarStartUp : MonoBehaviour
{
    private Animator _animator;
    private ParticleSystem _particle;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _particle = transform.GetComponentInChildren<ParticleSystem>();
    }
    public void StartApp()
    {
        _animator.SetTrigger("Start");
        _particle.Play();
    }
}
