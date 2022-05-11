using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowObjectOnTrigger : MonoBehaviour
{
    public GameObject[] triggered_by;
    public GameObject object_to_show;

    void OnTriggerEnter2D(Collider2D collider)
    {
        foreach (GameObject obj in triggered_by)
        {
            if (obj.Equals(collider.gameObject))
            {
                object_to_show.SetActive(true);
            }
        }
    }
}
