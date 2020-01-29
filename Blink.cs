using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blink : MonoBehaviour
{
    public Color m_StartColor;
    public Color m_EndColor = Color.white;
    public float m_Time;
    public AnimationCurve m_Curve;

    public SpriteRenderer m_Image;

    public void Execute()
    {
        StopAllCoroutines();
        StartCoroutine(Blinking());
    }
    
    //função para calcular o tempo de pulsação da imagem
    private IEnumerator Blinking()
    {
        float startTime = Time.time;
        float timeRate = 0.0f;

        do
        {
            timeRate = Mathf.Clamp01((Time.time - startTime) / m_Time);
            Color color = Color.Lerp(m_StartColor, m_EndColor, m_Curve.Evaluate(timeRate));
            m_Image.color = color;
            yield return null;
        } while (timeRate < 1.0f);
    }
}
