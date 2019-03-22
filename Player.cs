using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public float speed      = 10f;
    public float jumpForce  = 600f;
    public float radious    = 0.2f;

    private int extraJumps = 1;
    private bool isJumping = false;
    private bool isOnFloor = false;
    private Rigidbody2D body;
    private SpriteRenderer sprite;
    private Animator anim;

    void Start()
    {
        body    = GetComponent<Rigidbody2D>();
        sprite  = GetComponent<SpriteRenderer>();
        anim    = GetComponent<Animator>();
    }


    void Update()
    {
        // Linecast recebe de parâmetros posicao 1, posicao 1, e
        // a layer do que é para ser verificado.
        // isOnFloor = Physics2D.Linecast(transform.position, groundCheck.position, whatIsGround);

        // Este segundo método troca a linha reta do Linecast por um
        // círculo, e verifica se o mesmo está em contato com o layer.
        // isOnFloor = Physics2D.OverlapCircle(groundCheck.position, radious, whatIsGround);

        // Terceiro método, utilizando IsTouchingLayers
        isOnFloor = body.IsTouchingLayers(whatIsGround);

        // Verificar se apertou o botão de pulo
        /* if (Input.GetButtonDown("Jump") && isOnFloor) {
            isJumping = true;
        } */
        
        // Nova verificação de pulo adaptado para pulo duplo
        if (Input.GetButtonDown("Jump") && extraJumps > 0) {
            isJumping = true;
            extraJumps--;
        }

        if (isOnFloor) {
            extraJumps = 1;
        }

        PlayerAnimation();
    }


    void FixedUpdate()
    {
        // Armazenar o input que vem do GetAxis em uma variável.
        // Este input é 1 quando é direita, e -1 quando esquerda.
        float move = Input.GetAxis("Horizontal");

        // Movimentação lateral do personagem
        body.velocity = new Vector2(move * speed, body.velocity.y);

        // Verificação para dar flip no sprite
        if ((move > 0 && sprite.flipX == true) ||( move < 0 && sprite.flipX == false)) {
            Flip();
        }

        // O pulo em si
        if (isJumping) {
            body.velocity = new Vector2(body.velocity.x, 0f);
            // A linha acima é somente para corrigir bug de pulo duplo.
            body.AddForce(new Vector2(0, jumpForce));
            isJumping = false;
        }

        // Efeito de pular mais alto enquanto segurar o botão de pulo
        if (body.velocity.y > -10.0f && !Input.GetButton("Jump")) {
            body.velocity += Vector2.up * -0.8f;
        }
    }


    /* Função que da flip no sprite para fazer o personagem virar */
    void Flip()
    {
        sprite.flipX = !sprite.flipX;
    }


    /* Função que faz aparecer desenhos ao redor do gizmo quando
    o player está selecionado */
    /* void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, radious);
    } */


    void PlayerAnimation()
    {
        anim.SetFloat("VelX", Mathf.Abs(body.velocity.x));
        anim.SetFloat("VelY", Mathf.Abs(body.velocity.y));

        /* if (body.velocity.x == 0 && body.velocity.y == 0) {
            anim.Play("Idle");
        } else if (body.velocity.x != 0 && body.velocity.y == 0) {
            anim.Play("Walk");
        } else if (body.velocity.y != 0) {
            anim.Play("Jump");
        } */
    }
}
