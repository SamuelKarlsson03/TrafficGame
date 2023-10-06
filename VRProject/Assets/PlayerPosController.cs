using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosController : MonoBehaviour
{
    [SerializeField] GameObject player;

    void Start()
    {
        player.transform.position = transform.position;
    }

    
}
