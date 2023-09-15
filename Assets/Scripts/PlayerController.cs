using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator        _playerAnimator;
    private Rigidbody2D     _playerRigidbody2D;

    public Transform        groundCheck;
    public bool             isGround;

    public float            speed;

    public float            touchRun = 0.0f;

    public bool             facingRight = true;

    public bool             jump = false;

    public int              jumpsCount = 0;
    public int              maxJumps    = 2;
    public float            jumpForce;


    private GameController  _gameController;

    // Start is called before the first frame update
    void Start()
    {
        _playerAnimator = GetComponent<Animator>();

        _playerRigidbody2D = GetComponent<Rigidbody2D>();

        _gameController = FindObjectOfType(typeof(GameController)) as GameController;
    }

    // Update is called once per frame
    void Update()
    {
        isGround = Physics2D.Linecast(
            transform.position, 
            groundCheck.position, 
            1 << LayerMask.NameToLayer("Ground")
        );
        touchRun = Input.GetAxisRaw("Horizontal");

        if(Input.GetButtonDown("Jump")){
            jump = true;
        }
        SetMovements();
    }

    void FixedUpdate(){
        MovePlayer(touchRun);

        if(jump){
            JumpPlayer();
        }
    }

    void JumpPlayer(){
        if(isGround){
            jumpsCount = 0;
        }
        if(isGround || jumpsCount < maxJumps){
            _playerRigidbody2D.AddForce(new Vector2(0f, jumpForce));
            isGround = false;
            jumpsCount ++;

        }

        jump = false;
    }
    void MovePlayer(float hMovement){

        _playerRigidbody2D.velocity = new Vector2(
            hMovement * speed,
            _playerRigidbody2D.velocity.y
        );

        if((hMovement < 0 && facingRight) || (hMovement > 0 && !facingRight)){
            Flip();
        }
    }

    void Flip(){
            facingRight = !facingRight;

            transform.localScale = new Vector3(
                -transform.localScale.x,
                transform.localScale.y,
                transform.localScale.z
            );
    }

    void SetMovements(){
        _playerAnimator.SetBool("isGrounded", isGround);
        _playerAnimator.SetBool("Walk", _playerRigidbody2D.velocity.x != 0 && isGround);
        _playerAnimator.SetBool("Jump", !isGround);
        _playerAnimator.SetFloat("EixoY", _playerRigidbody2D.velocity.y);
    }

    void OnTriggerEnter2D(Collider2D colisao){

        switch (colisao.gameObject.tag)
        {
            case "Coletavel":
                _gameController.Pontuacao(1);
                Destroy(colisao.gameObject);
                break;
        }
    }
}
