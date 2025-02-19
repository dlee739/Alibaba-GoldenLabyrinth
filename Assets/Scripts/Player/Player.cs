using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Player
{ 
    public static string State = "Idle";
    public static Animator animator;
    public static float Speed = .5f;

    public static GameObject Attack;
    public static GameObject AttackExplosion;

    public static Camera Camera;
}
