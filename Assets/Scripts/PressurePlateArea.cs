using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class PressurePlateArea : PressurePlate
{
    private Animator _animator;
    private Material _materialDeactivated;
    [SerializeField] private Material _materialActivated;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _materialDeactivated = _spriteRenderer.material;
        _animator = GetComponent<Animator>();
    }

    protected override void Activate()
    {
        _spriteRenderer.material = _materialActivated;
        _animator.SetBool("b_active", true);
    }

    protected override void Deactivate()
    {
        _spriteRenderer.material = _materialDeactivated;
        _animator.SetBool("b_active", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AddNumberOfActivatingPlayers(1);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AddNumberOfActivatingPlayers(-1);
        }
    }
}
