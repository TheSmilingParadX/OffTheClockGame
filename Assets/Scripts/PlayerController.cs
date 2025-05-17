using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;

    public Animator animManager;

    void Start()
    {
        animManager = GetComponent<Animator>();
    }

    void Update()
    {
        Vector2 motion = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
        transform.Translate(motion * speed * Time.deltaTime);

        if (motion.x != 0)
        {
            animManager.SetBool("isMoving", true);
        }
        else
        {
            animManager.SetBool("isMoving", false);
        }

        if (motion.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (motion.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }
}
