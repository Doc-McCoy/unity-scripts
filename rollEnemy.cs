using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rollEnemy : MonoBehaviour
{
    [Header("Moviment Vars")]
    public float m_movimentVelocity;
    public float m_rotateVelocity;
    public Rigidbody2D m_enemyRB;
    public GameObject m_Player;

    private bool m_rollACtive;
    private bool m_movimentSlow;
    private Rigidbody2D m_enemyMovRB;

    private void Start()
    {
        m_enemyMovRB = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        //verificação de movimento do player
        if (m_rollACtive == true)
        {
            RollEnemy();
            MovimentEnemy();
        }
    }

    //função de rolar o inimigo
    private void RollEnemy()
    {
        if (m_Player.transform.position.x > transform.position.x)
        {
            m_enemyRB.transform.Rotate(0, 0, m_rotateVelocity * -1);
        } else if (m_Player.transform.position.x < transform.position.x)
        {
            m_enemyRB.transform.Rotate(0, 0, m_rotateVelocity * 1);
        }
    }

    //função de movimento do inimigo
    private void MovimentEnemy()
    {
        if (m_movimentSlow == false)
        {
            if (m_Player.transform.position.x < transform.position.x)
            {
                //movimento para a esquerda
                m_enemyMovRB.velocity = new Vector2(m_movimentVelocity * -1, m_enemyMovRB.velocity.y);
                m_enemyRB.velocity = new Vector2(m_movimentVelocity * -1, m_enemyMovRB.velocity.y);
            }
            else if (m_Player.transform.position.x > transform.position.x)
            {
                //movimento para a direita
                m_enemyMovRB.velocity = new Vector2(m_movimentVelocity * 1, m_enemyMovRB.velocity.y);
                m_enemyRB.velocity = new Vector2(m_movimentVelocity * 1, m_enemyMovRB.velocity.y);
            }
        }
        else
        {
            if (m_Player.transform.position.x < transform.position.x)
            {
                //movimento para a esquerda
                m_enemyMovRB.velocity = new Vector2(m_movimentVelocity * -0.25f, m_enemyMovRB.velocity.y);
                m_enemyRB.velocity = new Vector2(m_movimentVelocity * -0.25f, m_enemyMovRB.velocity.y);
            }
            else if (m_Player.transform.position.x > transform.position.x)
            {
                //movimento para a direita
                m_enemyMovRB.velocity = new Vector2(m_movimentVelocity * 0.25f, m_enemyMovRB.velocity.y);
                m_enemyRB.velocity = new Vector2(m_movimentVelocity * 0.25f, m_enemyMovRB.velocity.y);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //verificação para rolamento e movimento com velocidade normal ou com redução
        switch (col.gameObject.tag)
        {
            case "Player":
                m_rollACtive = !m_rollACtive;
                break;
            case "Player2":
                m_movimentSlow = !m_movimentSlow;
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        //verificação para rolamento e movimento com velocidade normal ou com redução
        switch (col.gameObject.tag)
        {
            case "Player":
                m_rollACtive = !m_rollACtive;

                //zera valor do movimentao de sair da colisão com o player
                m_enemyMovRB.velocity = new Vector2(0, m_enemyMovRB.velocity.y);
                m_enemyRB.velocity = new Vector2(0, m_enemyMovRB.velocity.y);
                break;
            case "Player2":
                m_movimentSlow = !m_movimentSlow;
                break;
        }
    }
}
