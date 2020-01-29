using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour {

    [Header("Moviment Atrib")]
    public  float        m_enemyVelocity;
    public  Transform    m_positionA, m_positionB, m_positionFinal;
    private int          m_destiny;
    private float        step=0;
    private bool         m_rightVision;
    public  bool         m_flashLightAct;
    public  GameObject   m_colliderEnemy;

	// Use this for initialization
	void Start () {
        //posição inicial do inimigo
        transform.position = m_positionA.position;
        m_positionFinal.position = m_positionB.position;
        m_rightVision = true;
	}

    // Update is called once per frame
    void FixedUpdate() {
        //movimento até ponto
        if (m_flashLightAct == false)
        {
            step = m_enemyVelocity * Time.deltaTime;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, m_positionFinal.position, step);
        }

        //condição para que o inimigo olhe para o lado correto e vá para o outro ponto
        if (gameObject.transform.position == m_positionFinal.position)
        {
            if (m_destiny == 0 && m_rightVision)
            {
                Turn();
                m_positionFinal.position = m_positionA.position;
                m_destiny = 1;
            } else if (m_destiny == 1 && !m_rightVision)
            {
                Turn();
                m_destiny = 0;
                m_positionFinal.position = m_positionB.position;
            }
        }
    }

    //função para virar o sprite
    private void Turn()
    {
        m_rightVision = !m_rightVision;

        Vector3 m_theScale = transform.localScale;

        m_theScale.x *= -1;

        transform.localScale = m_theScale;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.gameObject.tag)
        {
            case "Flashlight":
                m_flashLightAct = true;
            break;
            case "Player2":
                m_flashLightAct = true;
            break;
        }
    }

   private void OnTriggerExit2D(Collider2D col)
    {
        switch (col.gameObject.tag)
        {
            case "Flashlight":
                m_flashLightAct = false;
            break;
            case "Player2":
                m_flashLightAct = false;
            break;
        }
    }
}
