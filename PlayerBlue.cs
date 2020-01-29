using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Variáveis de Movimento")]
    public Transform m_groundCheck;
    public LayerMask m_groundLayer;
    public bool m_interact;
    public float m_speed = 5;
    public float m_jumpForce = 200;
    public float m_movePlayer;

    private AudioSource m_playerAudioSource;
    private bool m_isJumping = false;
    private bool m_isOnFloor = false;
    private bool m_ladoDireito = true;
    private Rigidbody2D m_body;
    private Animator m_animator;
    private SpriteRenderer m_sprite;
    private bool m_corroutineRunning;   

    [Header("Life")]
    public int m_life;
    public GameObject m_ObjSpriteLife;

    public int m_blockOfLife;
    public Image[] m_blocks;
    public Sprite m_fullBlocks;

    private Color m_lifeColor1 = new Color(1f, 1f, 1f, 0.1f);
    private float m_colorSpeed = 1f;
    private bool m_invunerable = false;
    private SpriteRenderer m_spriteLife;

    [Header("Balãozinho de fala")]
    public GameObject m_balao;

    private Blink m_Blink;

    private IdentificarControle m_IdentificarControle;

    void Start()
    {
        m_body              = GetComponent<Rigidbody2D>();
        m_animator          = GetComponent<Animator>();
        m_sprite            = GetComponent<SpriteRenderer>();
        m_spriteLife        = m_ObjSpriteLife.GetComponent<SpriteRenderer>();
        m_playerAudioSource = GetComponent<AudioSource>();
        m_IdentificarControle = FindObjectOfType(typeof(IdentificarControle)) as IdentificarControle;

        m_balao.SetActive(false);

        m_Blink = GetComponent<Blink>();

        m_life = 3;
    }

    void Update()
    {
        // Detectar se está no chão
        m_isOnFloor = Physics2D.Linecast(transform.position, m_groundCheck.position, m_groundLayer);

        // Verificar pulo
        if (m_IdentificarControle.m_Jump && m_isOnFloor) {m_isJumping = true;}

        // Animar
        PlayerAnimation();

        m_interact = m_IdentificarControle.m_Interact;

        // Efeito de recuperar as vidas:
        if (m_life < 3 && !m_corroutineRunning)
        {
            StartCoroutine(recoverLifeAfterTime());
        }

        //transição para o limbo
        if (m_life == 0)
        {
            StartCoroutine(TransictionLimbo());
        }

        //barra de vida
        for (int i = 0; i < m_blocks.Length; i++)
        {
            if(i < m_life)
            {
                m_blocks[i].enabled = true;
            } else
            {
                m_blocks[i].enabled = false;
            }
        }
    }

    void FixedUpdate()
    {
        // Direção (1 / -1)
        float m_move = m_IdentificarControle.m_Horizontal;
        m_movePlayer = m_move;

        if(m_move == 0)
        {
            m_body.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            m_body.constraints &= ~RigidbodyConstraints2D.FreezePositionX;

            //m_body.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        // Movimento
        m_body.velocity = new Vector2(m_move * m_speed, m_body.velocity.y);
        SteepSound();

        // Checar direção do personagem
        if (m_move > 0f && !m_ladoDireito || m_move < 0f && m_ladoDireito) {Flip();}

        // Pulo
        if (m_isJumping) {
            m_body.velocity = new Vector2(m_body.velocity.x, 0f);
            m_body.AddForce(new Vector2(0, m_jumpForce));
            m_isJumping = false;
        }
        

        // Efeito de pular mais alto enquanto segurar o botão de pulo
        if (m_body.velocity.y > -10.0f && !m_IdentificarControle.m_Jump) {m_body.velocity += Vector2.up * -0.4f;}
    }

    //função para virar o sprite
    void Flip()
    {
        m_sprite.flipX = !m_sprite.flipX;        
        m_ladoDireito   = !m_ladoDireito;
    }

    //função para animação de movimento
    void PlayerAnimation()
    {
        m_animator.SetFloat("mov_x", Mathf.Abs(m_body.velocity.x));
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 11) {
            m_balao.SetActive(true);
            m_animator.SetTrigger("balao");
        }

        if (col.gameObject.tag == "Enemy")
        {
            if (!m_invunerable) {
                Damage();
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == 11) {
            m_balao.SetActive(false);
        }
    }

    //recuperar vida por segundo
    IEnumerator recoverLifeAfterTime()
    {
        m_corroutineRunning = true;
        yield return new WaitForSeconds(10);
        m_life += 1;
        m_corroutineRunning = false;
    }

    //piscar sprite enquanto leva dano
    IEnumerator DamagePlayer()
    {
        for (float i = 0; i < 1; i += 0.1f)
        {
            m_sprite.enabled = false;
            yield return new WaitForSeconds(0.1f);
            m_sprite.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

        m_invunerable = false;
    }

    //função de dano e ficar invulnerável por um tempo
    void Damage()
    {
        m_invunerable = true;
        m_life -= 1;

        m_Blink.Execute();

        StartCoroutine(DamagePlayer());
    }

    //função para transição para o limbo
    IEnumerator TransictionLimbo()
    {
        for (float i = 0; i < 1f; i += 0.1f)
        {
            m_spriteLife.color += m_lifeColor1;
            yield return new WaitForSeconds(0.1f);
        }
        SceneManager.LoadScene("Limbo");
    }

    void SteepSound()
    {
        if((m_movePlayer > 0.1f || m_movePlayer < -0.1f))
        {
            m_playerAudioSource.enabled = true;
            m_playerAudioSource.loop = true;
        }
        else
        {
            m_playerAudioSource.enabled = false;
            m_playerAudioSource.loop = false;
        }
    }
}
