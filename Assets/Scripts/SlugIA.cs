using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugIA : MonoBehaviour
{
    public  Transform       enemy;
    public  SpriteRenderer  enemySprite;
    public  Transform[]     position;
    public  float           speed;
    public  bool            isRight;


    private int             idTarget;

    // Start is called before the first frame update
    void Start()
    {
        enemySprite = enemy.gameObject.GetComponent<SpriteRenderer>();
        enemy.position = position[0].position;

        idTarget = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(enemy != null){
            enemy.position = Vector3.MoveTowards(
                enemy.position,
                position[idTarget].position,
                speed * Time.deltaTime
            );

            if(enemy.position == position[idTarget].position){
                idTarget += 1;
                if(idTarget == position.Length){
                    idTarget = 0;
                }

            }

            if(position[idTarget].position.x < enemy.position.x && isRight){
                Flip();
            }else if (position[idTarget].position.x > enemy.position.x && !isRight){
                Flip();
            }
        }
    }

    void Flip(){
        isRight = !isRight;
        enemySprite.flipX = !enemySprite.flipX;
    }
}
