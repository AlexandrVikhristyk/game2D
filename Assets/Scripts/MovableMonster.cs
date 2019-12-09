using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovableMonster : Monster
{
    [SerializeField]
    private float speed = 2.0F;

    private Vector3 direction;
    

    private SpriteRenderer sprite;

    protected override void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    protected override void Start()
    {
        direction = transform.right;
    }

    protected override void Update()
    {
        Move();
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        Unit unit = collider.GetComponent<Unit>();

        if (unit && unit is Character)
        {
            if (Mathf.Abs(unit.transform.position.x - transform.position.x) < 0.3F) ReceiveDamage();
            else unit.ReceiveDamage();
        }
    }

    private void Move()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.5f + transform.right * direction.x * 0.5f, 0.1f);
        Collider2D[] collidersDown = Physics2D.OverlapCircleAll(transform.position + transform.right * 0.5f * direction.x * 0.5f, 0.1f);

       if ((colliders.Length > 0 && colliders.All(x => (!x.GetComponent<Character>() && !x.GetComponent<Bullet>()))) ||
           collidersDown.Length == 1)
       {
           sprite.flipX = direction.x < 0.0f;
           direction *= -1.0f;
       }
        
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
    }
}
