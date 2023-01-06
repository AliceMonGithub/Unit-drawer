using DG.Tweening;
using System;
using UnityEngine;

internal class Unit : MonoBehaviour
{
    private readonly int DieHash = Animator.StringToHash("Die");

    public event Action<Unit> OnDie;

    [SerializeField] private GameObject _gameObject;
    [SerializeField] private Transform _transform;
    [SerializeField] private Animator _animator;

    private bool _died;

    public Transform Transform => _transform;
    public Animator Animator => _animator;

    public bool Died => _died;

    public void Die()
    {
        if (_died) return;

        transform.DOKill();
        
        _animator.SetTrigger(DieHash);

        transform.SetParent(null);

        OnDie?.Invoke(this);

        Invoke(nameof(Clear), 5);

        _died = true;
    }

    public void Clear()
    {
        Destroy(gameObject);
    }

    private void OnValidate()
    {
        _transform = transform;
        _gameObject = gameObject;

        if(_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
    }
}
