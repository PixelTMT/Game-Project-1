using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public Transform transform;
    public GameObject gameObject;
    public PlayerAnimation animation;
}

public enum PlayerAnimation
{
    idle, jumping, running, shoting
}