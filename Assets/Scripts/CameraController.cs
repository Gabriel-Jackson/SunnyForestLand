using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    public float        offsetX = 3f;
    public float        smooth = 0.01f;

    public float        limitedUp = 2f;
    public float        limitedDown = 0f;
    public float        limitedLeft = 0f;
    public float        limitedRight = 100f;

    private Transform   _player;
    private float       _playerX;
    private float       _playerY;

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerController>().transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(_player != null){

            _playerX = Mathf.Clamp(_player.position.x  + offsetX,limitedLeft,limitedRight);
            _playerY = Mathf.Clamp(_player.position.y,limitedDown,limitedUp);

            transform.position = Vector3.Lerp(
                transform.position,
                new Vector3(_playerX,_playerY,transform.position.z),
                smooth
            );
        }
    }
}
