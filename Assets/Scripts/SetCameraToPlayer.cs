using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraToPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player) {transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);}
    }
}
