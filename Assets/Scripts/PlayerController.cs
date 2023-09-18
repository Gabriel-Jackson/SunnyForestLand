using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    private Animator        _playerAnimator;
    private Rigidbody2D     _playerRigidbody2D;

    private SpriteRenderer  _playerSpriteRenderer;

    public Transform        groundCheck;
    public bool             isGround;

    public float            speed;

    public float            touchRun = 0.0f;

    public bool             facingRight = true;

    public int              qtdVida = 3;
    public bool             playerInvencivel;
    public bool             jump = false;

    public int              jumpsCount = 0;
    public int              maxJumps    = 2;
    public float            jumpForce;

    
    public AudioSource      gameFX;
    public AudioClip        puloFX;

    private GameController  _gameController;

    public Color            hitColor;

    public GameObject       playerDie;

    public ParticleSystem   poeira;

    public PlayerInput playerInput;

    // Start is called before the first frame update
    void Start()
    {
        _playerAnimator = GetComponent<Animator>();

        _playerRigidbody2D = GetComponent<Rigidbody2D>();

        _playerSpriteRenderer = GetComponent<SpriteRenderer>();

        _gameController = FindObjectOfType(typeof(GameController)) as GameController;

        _gameController.BarraVida(qtdVida);

        playerInput = GetComponent<PlayerInput>();

    }

    // Update is called once per frame
    void Update()
    {
        isGround = Physics2D.BoxCast(
            groundCheck.position, 
            groundCheck.GetComponent<BoxCollider2D>().size,
            0,
            Vector2.down, 
            0.02f,
            1 << LayerMask.NameToLayer("Ground")
        );


        touchRun = playerInput.actions["Move"].ReadValue<Vector2>().x;
        
        Debug.Log(playerInput.actions["Move"].IsInProgress());
        playerInput.actions["Jump"].performed += ctx => {
            jump = true;
        };
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
            CriarPoeira();
        }
        if(isGround || jumpsCount < maxJumps){
            _playerRigidbody2D.AddForce(new Vector2(0f, jumpForce));
            isGround = false;
            jumpsCount ++;

            gameFX.PlayOneShot(puloFX);

            CriarPoeira();
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

            CriarPoeira();
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
            case "Ground":
                isGround = true;
                break;
            case "Coletavel":
                _gameController.Pontuacao(1);
                Destroy(colisao.gameObject);
                break;
            case "Inimigo":
                GameObject tmpExplosao = Instantiate(_gameController.hitPrefab, colisao.transform.position, transform.localRotation);
                Destroy(tmpExplosao, 0.5f);

                Rigidbody2D rb = GetComponentInParent<Rigidbody2D>();
                rb.velocity = new Vector2(rb.velocity.x, 0);

                rb.AddForce(new Vector2(0, 700));

                _gameController.gameFX.PlayOneShot(_gameController.explosaoFX);
                Destroy(colisao.gameObject.transform.parent.gameObject);

                break;
            case "Damage":
                Hurt();
                break;
        }
    }

    void OnTriggerExit2D(Collider2D colisao){
        switch(colisao.gameObject.tag){
            case "Ground":
                isGround = false;
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D colisao){
        switch (colisao.gameObject.tag){
            case "Plataforma":
                this.transform.parent = colisao.transform;
                break;
            case "Inimigo":
                Hurt();
                break;
        }
    }

    void OnCollisionExit2D(Collision2D colisao){
        switch (colisao.gameObject.tag){
            case "Plataforma":
                this.transform.parent = null;
                break;
        }
    }
    void Hurt(){
        if(!playerInvencivel){
            playerInvencivel = true;

            qtdVida --;
            
            StartCoroutine("Dano");

            _gameController.BarraVida(qtdVida);

            if(qtdVida < 1){
                GameObject pDieTemp = Instantiate(playerDie, transform.position, Quaternion.identity);
                Rigidbody2D rbDie = pDieTemp.GetComponent<Rigidbody2D>();

                rbDie.AddForce(new Vector2(150f, 500f));

                _gameController.gameFX.PlayOneShot(_gameController.dieFX);

                Invoke("CarregaJogo",2.5f);
                gameObject.SetActive(false);
            }
            
            
        }
    }

    void CarregaJogo(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    IEnumerator Dano(){
        _playerSpriteRenderer.color = hitColor;
        yield return new WaitForSeconds(0.1f);

        for(float i =0;i <1; i+=0.1f){
            _playerSpriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            _playerSpriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        
        _playerSpriteRenderer.color = Color.white;
        playerInvencivel = false;

    }

    void CriarPoeira(){
        poeira.Play();
    }
}
