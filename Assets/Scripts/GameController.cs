using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    private int             score;
    public  TMP_Text        txtScore;

    public  Sprite[]        imagensVida;
    public  Image           barraVida;
    public  GameObject      hitPrefab;
    public  AudioSource     gameFX;
    public  AudioClip       cenouraColetadaFX;
    public  AudioClip       explosaoFX;
    public  AudioClip       dieFX;
    public void Pontuacao( int qtdPontos ){
        score += qtdPontos;

        txtScore.text = score.ToString();

        gameFX.PlayOneShot(cenouraColetadaFX);
    }
    
    public void BarraVida (int vida){
        vida = Mathf.Clamp(vida, 0, imagensVida.Length);
        barraVida.sprite = imagensVida[vida];
    }

}
