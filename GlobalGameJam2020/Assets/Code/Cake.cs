using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cake : MonoBehaviour
{
    public static event Action<Cake> OnNewCake = delegate { };
    [SerializeField] private float lifeSpawn;

    private void Awake()
    {
        Destroy(this.gameObject, lifeSpawn);
        OnNewCake(this);
    }
}
