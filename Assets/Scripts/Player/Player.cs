using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BSLibrary;
using System.Linq;

public class Player : MonoBehaviour
{
    public Tween tween;

    public Rigidbody2D rigid;

    private float jumpKeyTime;
    private Vector3 direction;
    private const float speed = 5f;
    private const float jumpTime = 0.3f;
    private const float jumpPower = 5f;
    private int setJumpCount;

    private float positionYOrigin = 0;

    private float horizontal;
    private bool isJumping;
    private int jumpCount;

    // Start is called before the first frame update
    void Awake()
    {
        jumpKeyTime = 0;
        tween.SetTween(Tween.TweenType.InOutSine, Tween.LoopType.Restart, true, jumpTime);
        jumpCount = 1;
        isJumping = false;
        setJumpCount = 2; // get data
    }

    private void Start()
    {
        tween.motionCompleteEvent += (bool isComp) => 
        {
            Debug.Log("is comp");
            tween.IsPause = true;
            isJumping = false;
        };
        tween.IsPause = true;
        tween.Play();
    }


    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        direction = new Vector3(horizontal, 0, 0) * speed;

        if (Input.GetKeyDown(KeyCode.Z) && jumpCount > 0)
        {
            jumpKeyTime = Time.time;
            tween.ReturnValue = 0;
            tween._Time = 0;
            tween.IsPause = false;
            jumpCount -= 1;
            isJumping = true;
            positionYOrigin = transform.position.y;
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            if (Time.time - jumpKeyTime <= jumpTime)
            {
                // half jump

            }
            tween.IsPause = true;
            isJumping = false;
        }
    }

    private void FixedUpdate()
    {
        if (isJumping)
        {
            transform.position = new Vector3(transform.position.x, positionYOrigin + tween.ReturnValueToFloat * jumpPower, 0);
        }

        transform.Translate(direction * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                var a = Physics2D.RaycastAll(transform.position, Vector3.down, 1.5f).Where(x => x.collider.CompareTag("Ground"));

                if (a.Count() > 0)
                {
                    isJumping = false;
                    jumpCount = setJumpCount;
                }
            }
        }
    }

}
