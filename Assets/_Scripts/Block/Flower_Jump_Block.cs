using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower_Jump_Block : MonoBehaviour
{
    // Start is called before the first frame update
    Animator anim;
    [SerializeField] float jumpPower = 5f;
    private bool hasPlayerJumped = false;

    void Start()
    { 
        anim = GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent<Player_Control>(out Player_Control player) && player._grounded)
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
