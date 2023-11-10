using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public float speed = 10f; //Velocidade base de movimento do projetil
    public float damage = 10f; //Dano do tiro
    public float timer, cooldown = 0.5f, aliveTime = 10f; //Temporizador, cooldown e tempo que o objeto deve permanecer apos o tiro

}   
