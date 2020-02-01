using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tape : MonoBehaviour
{
    [SerializeField, Range(0, 5)]
    private int charges;
    [SerializeField]
    private Transform spawner;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hit something...");

        var target = collision.GetComponent<Player>();
        if (target == null) return;

        target.GetComponent<TapeHolder>().CollectTape(charges);
        Spawn();
    }

    private void Spawn()
    {
        int count = spawner.childCount;
        int random = Random.Range(0, count);

        transform.position = spawner.GetChild(random).transform.position;

    }
}
