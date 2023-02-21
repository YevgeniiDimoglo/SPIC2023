using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StorageBox : InteractiveObject
{
    [SerializeField] private Vector3 focusOffset;

    private List<GameObject> objects;
    private bool storeMode;
    private int selectedIndex;

    private Vector3 focusPoint;

    // Start is called before the first frame update
    private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Holdable");
        objects = new List<GameObject>();
        selectedIndex = 0;
        storeMode = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (storeMode)
        {
            focusPoint = transform.position + transform.localRotation * focusOffset;

            updateCamera();

            for (int i = 0; i < objects.Count; i++)
            {
                if (i == selectedIndex)
                {
                    objects[i].SetActive(true);
                    objects[i].transform.position = Vector3.MoveTowards(objects[i].transform.position, focusPoint, Time.deltaTime * 10.0f);
                    objects[i].transform.Rotate(Vector3.up, Time.deltaTime * 60);
                }
                else
                {
                    if (objects[i].transform.position != transform.position)
                    {
                        objects[i].transform.position = Vector3.MoveTowards(objects[i].transform.position, transform.position, Time.deltaTime * 10.0f);
                    }
                    else
                    {
                        objects[i].SetActive(false);
                    }
                }
            }

            if (InputRight())
            {
                selectedIndex++;
                if (selectedIndex >= objects.Count) selectedIndex = 0;
            }
            if (InputLeft())
            {
                selectedIndex--;
                if (selectedIndex < 0) selectedIndex = objects.Count - 1;
            }

            if (InputSelect() || InputBack())
            {
                if (InputSelect())
                {
                    player.GetComponent<MaidController>().hold(putOut(selectedIndex));
                }

                storeMode = false;
                player.SetActive(true);
                Camera.main.GetComponent<FreeFollowCamera>().startFollow();
                foreach (GameObject obj in objects)
                {
                    obj.transform.position = transform.position;
                    obj.SetActive(false);
                }
            }
        }
    }

    private void updateCamera()
    {
        Camera camera = Camera.main;

        camera.transform.position = Vector3.MoveTowards(camera.transform.position, player.transform.position + new Vector3(0, 1.5f, 0), Time.deltaTime * 10.0f);
        camera.transform.LookAt(focusPoint);
    }

    public override void hover()
    {
        if (player.GetComponent<MaidController>().getHolding() == null && objects.Count == 0) return;

        base.hover();
    }

    public override void click()
    {
        GameObject obj = player.GetComponent<MaidController>().getHolding();
        if (obj != null)
        {
            // Store
            player.GetComponent<MaidController>().release();
            Store(obj);
        }
        else if (objects.Count > 0)
        {
            // Store Mode
            selectedIndex = 0;
            storeMode = true;
            player.SetActive(false);
            Camera.main.GetComponent<FreeFollowCamera>().StopFollow();
        }
    }

    public void Store(GameObject obj)
    {
        obj.transform.position = transform.position;
        obj.transform.rotation = Quaternion.identity;
        obj.GetComponent<Collider>().enabled = false;
        obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        obj.SetActive(false);
        objects.Add(obj);
    }

    public GameObject putOut(int idx)
    {
        GameObject obj = objects[idx];
        obj.GetComponent<Collider>().enabled = true;
        obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        objects.Remove(obj);
        return obj;
    }

    private bool InputSelect()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame) return true;
        if (Gamepad.current != null)
        {
            if (Gamepad.current.buttonSouth.wasPressedThisFrame) return true;
        }
        return false;
    }

    private bool InputBack()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame) return true;
        if (Gamepad.current != null)
        {
            if (Gamepad.current.buttonEast.wasPressedThisFrame) return true;
        }
        return false;
    }

    private bool InputLeft()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame) return true;
        if (Keyboard.current.aKey.wasPressedThisFrame) return true;
        if (Gamepad.current != null)
        {
            if (
                Gamepad.current.leftShoulder.wasPressedThisFrame ||
                Gamepad.current.dpad.left.wasPressedThisFrame
                )
                return true;
        }
        return false;
    }

    private bool InputRight()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame) return true;
        if (Keyboard.current.dKey.wasPressedThisFrame) return true;
        if (Gamepad.current != null)
        {
            if (
                Gamepad.current.rightShoulder.wasPressedThisFrame ||
                Gamepad.current.dpad.right.wasPressedThisFrame
                )
                return true;
        }
        return false;
    }
}