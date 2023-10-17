using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint_Block : MonoBehaviour
{
    [SerializeField] Player_Control player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player Skin").GetComponent<Player_Control>();
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player.initialPosition = transform.position + Vector3.up * 3f;
        }
    }
}
