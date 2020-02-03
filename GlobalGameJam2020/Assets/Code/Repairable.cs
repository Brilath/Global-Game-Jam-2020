using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repairable : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float modifyCooldown;
    [SerializeField] private float modifyCountdown;

    public bool Repaired { get { return maxHealth == currentHealth; } }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        modifyCountdown = 0;
        maxHealth = sprites.Length - 1;

        UpdateSprite();
    }

    public bool Repair(int amount = 1)
    {
        bool repaired = false;
        if (CanModify())
        {
            currentHealth += amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            UpdateSprite();
            StartCoroutine(StartModifyCountdown());
            repaired = true;
        }

        return repaired;
    }

    public void Hurt(int amount = 1)
    {
        if (CanModify())
        {
            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            UpdateSprite();
            //StartCoroutine(StartModifyCountdown());
        }
    }

    private IEnumerator StartModifyCountdown()
    {
        modifyCountdown = modifyCooldown;
        while (modifyCountdown > 0)
        {
            modifyCountdown -= Time.deltaTime;
            yield return null;
        }
    }

    private bool CanModify()
    {
        return !Repaired;
    }

    private void UpdateSprite()
    {
        int index = Mathf.Clamp(currentHealth, 0, sprites.Length - 1);
        spriteRenderer.sprite = sprites[index];
    }
}
