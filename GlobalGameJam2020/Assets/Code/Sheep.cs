using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Sheep hit something {collision.name}");
        if(collision.gameObject.tag == "Player")
        {

            var playerRB = collision.GetComponent<Rigidbody2D>();
            playerRB.AddForce(-playerRB.velocity * 100);
        }
    }
}
