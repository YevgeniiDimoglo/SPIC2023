using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableArea : MonoBehaviour
{
    [SerializeField] private int ItemsX;
    [SerializeField] private int ItemsY;

    // Start is called before the first frame update
    private void Start()
    {
        Vector3 Size = GetComponent<Collider>().bounds.size;
        Vector3 pos = new Vector3(transform.position.x - Size.x / 2.0f, transform.position.y, transform.position.z - Size.z / 2.0f);

        for (int y = 0; y < ItemsY; y++)
        {
            for (int x = 0; x < ItemsX; x++)
            {
                GameObject obj = new GameObject("PlacePoint_" + y + "_" + x);
                obj.transform.position = new Vector3(pos.x + Size.x / ItemsX * (x + 0.5f), pos.y, pos.z + Size.z / ItemsY * (y + 0.5f));
                obj.AddComponent<PlaceablePoint>();
                obj.AddComponent<SphereCollider>().radius = 0.05f;
                obj.GetComponent<SphereCollider>().isTrigger = true;
                obj.transform.parent = gameObject.transform;
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }
}