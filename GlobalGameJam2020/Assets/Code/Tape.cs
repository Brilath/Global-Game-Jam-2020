using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tape : MonoBehaviour
{
    [SerializeField, Range(0, 5)]
    private int charges;
    [SerializeField]
    private Transform spawner;
    [SerializeField, Range(0, 10)] private float flashTimer;
    [SerializeField] private SpriteRenderer flashImage;
    [SerializeField] private AudioSource audio;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(Flash(flashTimer));
    }

    private IEnumerator Flash(float seconds)
    {
        float pulseTime = seconds;
        float showTime = 0.5f;

        while (pulseTime > 0)
        {
            flashImage.enabled = false;
            pulseTime -= Time.deltaTime;

            yield return null;
        }

        while (showTime > 0)
        {
            flashImage.enabled = true;
            showTime -= Time.deltaTime;
            yield return null;
        }

        StartCoroutine(Flash(seconds));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hit something...");

        var target = collision.GetComponent<Player>();
        if (target == null) return;

        target.GetComponent<TapeHolder>().CollectTape(charges);
        audio.Play();
        Spawn();
    }

    private void Spawn()
    {
        int count = spawner.childCount;
        int random = UnityEngine.Random.Range(0, count);

        transform.position = spawner.GetChild(random).transform.position;

    }
}
