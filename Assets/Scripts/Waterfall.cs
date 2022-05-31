using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waterfall : Interactable
{
    [SerializeField] private GameObject waterDrop;
    [SerializeField] private float dropLifeTime;
    [SerializeField] private int rate;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(CreateDrop), 0, 1.0f / rate);
    }

    // Update is called once per frame
    void CreateDrop()
    {
        GameObject drop = Instantiate(
            waterDrop,
            transform.position + new Vector3(Random.Range(-transform.localScale.x / 2, transform.localScale.x / 2), 0),
            transform.rotation
        );
        drop.GetComponent<Rigidbody2D>().velocity = Vector2.down * Random.Range(0, 10);
        Destroy(drop, dropLifeTime);

    }

    public override void TriggerInteraction(GameObject actor)
    {
        CancelInvoke(nameof(CreateDrop));
    }
}
