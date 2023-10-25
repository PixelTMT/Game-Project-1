using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower_Jump_Block : MonoBehaviour
{
    // Start is called before the first frame update
    Animator anim;
    [SerializeField] Player_Control player;
    [SerializeField] float jumpPower = 5f;
    private bool hasPlayerJumped = false;

    void Start()
    { 
        anim = GetComponent<Animator>();
        player = GameObject.Find("Player Skin").GetComponent<Player_Control>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && player._grounded)
        {
            Debug.Log("JUMP!");
            anim.SetBool("isPlayerOnIt", true);
            anim.SetTrigger("jumpTrigger");
            hasPlayerJumped = true;
            player.jump(jumpPower);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        anim.SetBool("isPlayerOnIt", false);
        hasPlayerJumped = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
