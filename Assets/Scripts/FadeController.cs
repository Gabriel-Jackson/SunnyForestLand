using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeController : MonoBehaviour
{

    public static FadeController instance;


    public Image imagemFade;
    public Color corInicial;
    public Color corFinal;
    public float duracaoFade;

    public bool isFading;
    private float _tempo;

    void Awake(){
        if(instance != this){
            if(instance != null){
                Destroy(gameObject);
            }

            instance = this;
        }
    }
    IEnumerator InicioFade(){
        isFading = true;
        _tempo = 0;

        while(_tempo <= duracaoFade){
            imagemFade.color = Color.Lerp(corInicial,corFinal, _tempo / duracaoFade);
            _tempo += Time.deltaTime;
            yield return null;
        }

        isFading = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("InicioFade");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
