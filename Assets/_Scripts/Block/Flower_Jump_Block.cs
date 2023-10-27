using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower_Jump_Block : MonoBehaviour
{
    // Start is called before the first frame update
    Animator anim;
    [SerializeField] float jumpPower = 5f;

    void Start()
    { 
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") &&
            other.TryGetComponent<Player_Control>(out Player_Control player) &&
            player._grounded)
        {
            Debug.Log("Flower JUMP!");
            anim.SetTrigger("jumpTrigger");
            player.jump(jumpPower);
        }
    }
    private void OnTriggerStay(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
