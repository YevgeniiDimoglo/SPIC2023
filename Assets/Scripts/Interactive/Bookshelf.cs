using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bookshelf : InteractiveObject
{
    [SerializeField] private Vector3 BookPos = Vector3.zero;
    [SerializeField] private float cameraDistance = 1;
    [SerializeField] private int cols = 10;                                 // number of each col
    [SerializeField] private float colsHeight = 0.3f;
    private List<GameObject> Books = new List<GameObject>();
    private bool orderMode;
    private int focusIndex;
    private bool selected;

    private GameObject popupWinFocus;
    private GameObject popupWinPrev;
    private GameObject popupWinNext;

    // Start is called before the first frame update
    protected override void Start()
    {
        orderMode = false;
        gameObject.layer = LayerMask.NameToLayer("Holdable");
        selected = false;
        focusIndex = 0;

        // Popup Windows
        popupWinFocus = BookWindows("FocusBook");
        popupWinPrev = BookWindows("PrevBook");
        popupWinNext = BookWindows("NextBook");

        base.Start();
    }

    private GameObject BookWindows(string name)
    {
        GameObject obj = new GameObject(name);
        obj.transform.parent = transform;
        TextMesh text = obj.AddComponent<TextMesh>();
        text.anchor = TextAnchor.MiddleCenter;
        text.characterSize = 0.03f;

        return obj;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (Books.Count > 0)
        {
            int x = 0;
            Vector3 bookPos = Quaternion.AngleAxis(Vector3.Angle(Vector3.forward, transform.forward), Vector3.Cross(Vector3.forward, transform.forward)) * BookPos; // Start Point
            for (int i = 0; i < Books.Count; i++)
            {
                Books[i].transform.forward = transform.forward;
                Books[i].transform.position = transform.position + bookPos;
                if (selected && i == focusIndex)
                {
                    Books[i].transform.rotation = Quaternion.AngleAxis(-30.0f, transform.forward);
                    Books[i].transform.position += transform.right * 0.03f + transform.up * 0.05f;
                }

                if (i + 1 < Books.Count)
                {
                    float d = Books[i].GetComponent<Renderer>().localBounds.size.z * 0.5f +
                              Books[i + 1].GetComponent<Renderer>().localBounds.size.z * 0.5f;

                    bookPos += Books[i].transform.forward * d;
                    x++;
                    if (x >= cols)
                    {
                        // nextCol
                        bookPos.x = BookPos.x;
                        bookPos.z = BookPos.z;
                        bookPos.y -= colsHeight;
                        x = 0;
                    }
                }
            }
        }

        if (orderMode)
        {
            Camera camera = Camera.main;
            Vector3 cameraPos = Books[focusIndex].transform.position + transform.right * cameraDistance;

            clearBookWindows();
            if (Camera.main.transform.position != cameraPos)
            {
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, cameraPos, 0.3f);
                Camera.main.transform.LookAt(Books[focusIndex].transform.position);
            }
            showBookWindows();

            if (InputSelect()) selected = !selected;
            if (InputLeft() && focusIndex > 0)
            {
                if (selected) SwitchBook(focusIndex, focusIndex - 1);
                focusIndex--;
            }
            if (InputRight() && focusIndex < Books.Count - 1)
            {
                if (selected) SwitchBook(focusIndex, focusIndex + 1);
                focusIndex++;
            }

            if (InputBack())
            {
                if (selected)
                {
                    pickBook(focusIndex);
                }
                selected = false;
                focusIndex = 0;
                orderMode = false;
                clearBookWindows();
                player.SetActive(true);
                camera.GetComponent<FreeFollowCamera>().startFollow();
            }
        }
    }

    private void clearBookWindows()
    {
        popupWinFocus.GetComponent<TextMesh>().text = "";
        popupWinNext.GetComponent<TextMesh>().text = "";
        popupWinPrev.GetComponent<TextMesh>().text = "";
    }

    private void showBookWindows()
    {
        Camera camera = Camera.main;
        updateBookWindow(ref popupWinFocus, Books[focusIndex]);
        popupWinFocus.transform.position += camera.transform.up * 0.2f;

        if (focusIndex < Books.Count - 1)
        {
            updateBookWindow(ref popupWinNext, Books[focusIndex + 1]);
            popupWinNext.transform.position -= camera.transform.up * 0.1f;
            popupWinNext.transform.position += camera.transform.right * 0.4f;
        }

        if (focusIndex > 0)
        {
            updateBookWindow(ref popupWinPrev, Books[focusIndex - 1]);
            popupWinPrev.transform.position -= camera.transform.up * 0.1f;
            popupWinPrev.transform.position += camera.transform.right * -0.4f;
        }
    }

    private void updateBookWindow(ref GameObject popupWin, GameObject Book)
    {
        Camera camera = Camera.main;
        popupWin.GetComponent<TextMesh>().text = Book.GetComponent<BookObject>().PopupText();
        popupWin.transform.rotation = camera.transform.rotation;
        popupWin.transform.position = camera.transform.position + Camera.main.transform.forward * (cameraDistance * 0.7f);
    }

    private void SwitchBook(int a, int b)
    {
        GameObject temp = Books[a];
        Books[a] = Books[b];
        Books[b] = temp;
    }

    private void pickBook(int idx)
    {
        Books[idx].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        Books[idx].GetComponent<Collider>().enabled = true;
        player.GetComponent<MaidController>().hold(Books[idx]);
        Books.Remove(Books[idx]);
    }

    public override void hover()
    {
        MaidController maid = player.GetComponent<MaidController>();
        if ((maid.getHolding() == null && Books.Count > 0) || (maid.getHolding() != null && maid.getHolding().GetComponent<BookObject>()))
        {
            base.hover();
        }
    }

    public override void click()
    {
        MaidController maid = player.GetComponent<MaidController>();
        GameObject obj = maid.getHolding();

        if (obj == null && Books.Count > 0)
        {
            // Start Ordering
            orderMode = true;
            player.SetActive(false);
            Camera.main.GetComponent<FreeFollowCamera>().StopFollow();

            return;
        }
        if (obj != null && obj.GetComponent<BookObject>())
        {
            maid.drop();
            addBook(obj);
        }
    }

    public void addBook(GameObject book)
    {
        Books.Add(book);
        book.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        book.GetComponent<Collider>().enabled = false;
        book.transform.position = Vector3.zero;
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