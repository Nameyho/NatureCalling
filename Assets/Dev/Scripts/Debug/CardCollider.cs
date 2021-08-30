using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCollider : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
    }
}
