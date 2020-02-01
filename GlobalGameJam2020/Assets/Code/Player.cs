using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField, Range (0f, 15f)]
    private float maxSpeed;
    [SerializeField]
    private int baseRepairAmount;
    [SerializeField]
    private int repairAmount;
    [SerializeField, Range(0, 3)]
    private float repairRange;
    [SerializeField]
    LayerMask repairMask;
    [SerializeField]
    private TapeHolder tapeHolder;

    private void Awake()
    {
        repairAmount = baseRepairAmount;
        tapeHolder = GetComponent<TapeHolder>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 playerInput;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");

        playerInput = Vector3.ClampMagnitude(playerInput, 1f);

        Vector3 velocity = new Vector3(playerInput.x, playerInput.y, 0.0f) * maxSpeed;

        transform.localPosition += velocity * Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Lets try to repair stuff");
            RepairObject();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Lets try to hurt stuff");
            HurtObject();
        }

    }

    private void HurtObject()
    {
        Collider2D[] repairables = Physics2D.OverlapCircleAll(transform.localPosition, repairRange, repairMask);
        Debug.Log($"Got {repairables.Length} objects to hurt");
        foreach (Collider2D repairable in repairables)
        {
            repairable.GetComponent<Repairable>().Hurt(repairAmount);
        }
    }

    private void RepairObject()
    {

        Collider2D repairable = Physics2D.OverlapCircle(transform.localPosition, repairRange, repairMask);
        if (repairable == null) return;
        Debug.Log($"Got {repairable.name} to repair");

        if(tapeHolder.Charges > 0)
        {
            var repairObject = repairable.GetComponent<Repairable>();
            bool repaired = repairObject.Repair(repairAmount);
            if (repaired)
                tapeHolder.UseTape();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.localPosition, repairRange);
    }
}
