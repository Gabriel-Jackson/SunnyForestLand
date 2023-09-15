using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{

    private int score;
    public TMP_Text txtScore;

    public AudioSource gameFX;
    public AudioClip cenouraColetadaFX;
    public void Pontuacao( int qtdPontos ){
        score += qtdPontos;

        txtScore.text = score.ToString();

        gameFX.PlayOneShot(cenouraColetadaFX);
    }
    

}
