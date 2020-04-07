using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerChange : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            this.transform.GetComponent<SpriteRenderer>().sortingOrder = 2;
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
            this.transform.GetComponent<SpriteRenderer>().sortingOrder = 0;
    }
}
