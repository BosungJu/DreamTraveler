using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BSLibrary;
using System.Linq;

public class Player : MonoBehaviour
{
    public Tween tween;

    private float jumpKeyTime;
    private Vector3 direction;
    private const float speed = 10f;
    private const float jumpTime = 0.5f;
    private const float jumpPower = 20f;
    private int setJumpCount;

    private bool isJumping;
    private int jumpCount;

    // Start is called before the first frame update
    void Awake()
    {
        jumpKeyTime = 0;
        tween.RepeatTime = jumpTime;
        tween.IsRepeat = false;
        tween.Ease = Tween.TweenType.OutSine;
        jumpCount = 1;
        isJumping = false;
        setJumpCount = 1; // get data
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        direction = new Vector3(horizontal, 0, 0) * speed;

        if (Input.GetKeyDown(KeyCode.Z) && jumpCount > 0)
        {
            jumpKeyTime = Time.time;
            tween.Play();
            jumpCount -= 1;
            isJumping = true;
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            if (Time.time - jumpKeyTime <= jumpTime)
            {
                // half jump
                tween.Stop();
                isJumping = false;
            }
        }

        if (isJumping)
        {
            direction += Vector3.up * tween.ReturnValueToFloat * jumpPower;
        }

        if (tween.ReturnValue == 1)
        {
            isJumping = false;
        }

        Debug.Log(direction);

        transform.Translate(direction * Time.deltaTime);

        //rigid.AddForce(direction * Time.deltaTime * speed);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                var a = Physics2D.RaycastAll(transform.position, Vector3.down, 1.5f).Where(x => x.collider.CompareTag("Ground"));

                if (a.Count() > 0)
                {
                    jumpCount = setJumpCount;
                }
            }
        }
    }

}
