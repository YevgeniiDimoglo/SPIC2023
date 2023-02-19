using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRaycast : MonoBehaviour
{
    [SerializeField] private float RayLength = 2.0f;
    [SerializeField] private GameObject Player;
    [SerializeField] private Vector3 PlayerOffset;
    private RaycastHit hit;

    [SerializeField] private float SelectRadius = 0.3f;

    private GameObject lastTarget;

    // Start is called before the first frame update
    private void Start()
    {
        lastTarget = null;
    }

    // Update is called once per frame
    private void Update()
    {
        GameObject target = null;
        if (
            (Physics.SphereCast(transform.position,
            (Player.GetComponent<MaidController>().getHolding() == null) ? SelectRadius : 0.01f,
            transform.forward, out hit, 5, LayerMask.GetMask("Holdable"))) &&
            (Vector3.Distance(hit.point, Player.transform.position + PlayerOffset) <= RayLength) &&
            (hit.collider.GetComponent<InteractiveObject>())
           )
        {
            target = hit.collider.gameObject;
        }
        else if (
            (Physics.SphereCast(transform.position,
            (Player.GetComponent<MaidController>().getHolding() == null) ? SelectRadius : 0.01f,
            transform.forward, out hit, 5, LayerMask.GetMask("Destructible"))) &&
            (Vector3.Distance(hit.point, Player.transform.position + PlayerOffset) <= RayLength) &&
            (hit.collider.GetComponent<InteractiveObject>()))
        {
            target = hit.collider.gameObject;
        }
        updateLastTarget(target);
    }

    private void OnDrawGizmos()
    {
        if (Physics.SphereCast(transform.position, SelectRadius, transform.forward, out hit))
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, transform.forward * hit.distance);
            Gizmos.DrawWireSphere(transform.position + transform.forward * hit.distance, SelectRadius);
        }
    }

    private void updateLastTarget(GameObject target)
    {
        if (target != lastTarget)
        {
            if (lastTarget != null && target != null)
            {
                // Unhover a target
                lastTarget.GetComponent<InteractiveObject>().unhover();
            }
            lastTarget = target;
            if (lastTarget != null)
            {
                // unhover a target
                lastTarget.GetComponent<InteractiveObject>().hover();
            }
        }
    }

    public GameObject CameraTarget()
    {
        return lastTarget;
    }
}