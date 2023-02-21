using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class Tutorial : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TMP_FontAsset fontAsset;

    [Header("UI")]
    [SerializeField] private GameObject RotationTutorial;

    [SerializeField] private GameObject CameraTutorial;
    [SerializeField] private GameObject MoveTutorial;
    [SerializeField] private GameObject RunTutorial;
    [SerializeField] private GameObject ZoomTutorial;
    [SerializeField] private GameObject SelectTutorial;
    [SerializeField] private GameObject CancelTutorial;

    [Header("Gamepad Sprite")]
    [SerializeField] private Sprite RotationLImageSource;

    [SerializeField] private Sprite RotationRImageSource;
    [SerializeField] private Sprite CameraImageSource;
    [SerializeField] private Sprite MoveImageSource;
    [SerializeField] private Sprite RunImageSource;
    [SerializeField] private Sprite ZoomImageSource;
    [SerializeField] private Sprite SelectImageSource;
    [SerializeField] private Sprite CancelImageSource;

    private Color showColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    private Color hideColor = new Color(1.0f, 1.0f, 1.0f, 0);

    public enum CONTOROL
    {
        MOVE,
        RUN,
        CAMERA,
        ZOOM,
        SELECT,
        CANCEL
    }

    private struct TutorialUI
    {
        public GameObject gameObject;
        public Image image;
        public TextMeshProUGUI text;
        public bool display;

        public TutorialUI(GameObject obj)
        {
            this.gameObject = obj;
            this.image = obj.GetComponent<Image>();
            GameObject textobject = new GameObject("text");
            textobject.transform.parent = obj.transform;
            textobject.transform.position = obj.transform.position - obj.transform.up * 50.0f;
            this.text = textobject.AddComponent<TextMeshProUGUI>();
            this.text.alignment = TextAlignmentOptions.Center;
            this.display = false;
            textobject.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 50);
        }
    }

    private Dictionary<CONTOROL, TutorialUI> UI = new Dictionary<CONTOROL, TutorialUI>();

    private void Start()
    {
        UI[CONTOROL.MOVE] = new TutorialUI(MoveTutorial);
        UI[CONTOROL.RUN] = new TutorialUI(RunTutorial);
        UI[CONTOROL.CAMERA] = new TutorialUI(CameraTutorial);
        UI[CONTOROL.ZOOM] = new TutorialUI(ZoomTutorial);
        UI[CONTOROL.SELECT] = new TutorialUI(SelectTutorial);
        UI[CONTOROL.CANCEL] = new TutorialUI(CancelTutorial);

        // Gamepad
        if (Gamepad.current != null)
        {
            RotationTutorial.transform.Find("RotateL").GetComponent<Image>().sprite = RotationLImageSource;
            RotationTutorial.transform.Find("RotateR").GetComponent<Image>().sprite = RotationRImageSource;
            UI[CONTOROL.CAMERA].image.sprite = CameraImageSource;
            UI[CONTOROL.MOVE].image.sprite = MoveImageSource;
            UI[CONTOROL.RUN].image.sprite = RunImageSource;
            UI[CONTOROL.ZOOM].image.sprite = ZoomImageSource;
            UI[CONTOROL.SELECT].image.sprite = SelectImageSource;
            UI[CONTOROL.CANCEL].image.sprite = CancelImageSource;
        }

        // HIDE
        hideRotationTutorial();
        foreach (CONTOROL v in System.Enum.GetValues(typeof(CONTOROL)))
        {
            UI[v].image.color = hideColor;
            UI[v].text.color = hideColor;
            UI[v].text.font = fontAsset;
        }

        displayTutorial(CONTOROL.CAMERA, "カメラ移動");
        displayTutorial(CONTOROL.MOVE, "移動");
    }

    private void displayTutorial(CONTOROL control, string messge)
    {
        UI[control].text.text = messge;
        if (messge.Length > 10)
        {
            UI[control].text.fontSize = 24;
        }

        UpdateUIstatue(control, true);
    }

    // Update is called once per frame
    private void Update()
    {
        GetInput();
        foreach (CONTOROL v in System.Enum.GetValues(typeof(CONTOROL)))
        {
            if (UI[v].display)
            {
                if (UI[v].image.color != showColor)
                {
                    UI[v].image.color = Color.Lerp(hideColor, showColor, UI[v].image.color.a + Time.deltaTime * 10.0f);
                    UI[v].text.color = UI[v].image.color;
                }
            }
            else
            {
                if (UI[v].image.color != hideColor)
                {
                    UI[v].image.color = Color.Lerp(hideColor, showColor, UI[v].image.color.a - Time.deltaTime * 10.0f);
                    UI[v].text.color = UI[v].image.color;
                }
            }
        }
    }

    private void GetInput()
    {
        // Move
        if (UI[CONTOROL.MOVE].display && UI[CONTOROL.MOVE].image.color == showColor)
        {
            if (InputMove())
            {
                UpdateUIstatue(CONTOROL.MOVE, false);
                displayTutorial(CONTOROL.RUN, "急ぎ足モード(はしたない)");
                Destroy(UI[CONTOROL.RUN].gameObject, 5);
            }
        }

        if (UI[CONTOROL.CAMERA].display && UI[CONTOROL.CAMERA].image.color == showColor)
        {
            if (InputCamera())
            {
                UpdateUIstatue(CONTOROL.CAMERA, false);

                displayTutorial(CONTOROL.ZOOM, "ズーム");
                displayTutorial(CONTOROL.SELECT, "選択 / 拾う");
            }
        }
        if (UI[CONTOROL.ZOOM].display && UI[CONTOROL.ZOOM].image.color == showColor)
        {
            if (InputZoom())
            {
                UpdateUIstatue(CONTOROL.ZOOM, false);
            }
        }
        if (UI[CONTOROL.SELECT].display && UI[CONTOROL.SELECT].image.color == showColor)
        {
            if (InputSelect())
            {
                UpdateUIstatue(CONTOROL.SELECT, false);
                displayTutorial(CONTOROL.CANCEL, "戻る / 投げる(はしたない)");
            }
        }
        if (UI[CONTOROL.CANCEL].display && UI[CONTOROL.CANCEL].image.color == showColor)
        {
            if (InputCancel())
            {
                UpdateUIstatue(CONTOROL.CANCEL, false);
            }
        }
    }

    private bool InputMove()
    {
        return Keyboard.current.dKey.isPressed || Keyboard.current.aKey.isPressed || Keyboard.current.wKey.isPressed || Keyboard.current.sKey.isPressed ||
               (Gamepad.current != null && (Gamepad.current.leftStick.ReadValue() != Vector2.zero));
    }

    private bool InputCamera()
    {
        return Mouse.current.delta.ReadValue() != Vector2.zero ||
               (Gamepad.current != null && (Gamepad.current.rightStick.ReadValue() != Vector2.zero));
    }

    private bool InputZoom()
    {
        return Mouse.current.scroll.ReadValue().y != 0 ||
               (Gamepad.current != null && (
                    Gamepad.current.dpad.up.isPressed ||
                    Gamepad.current.dpad.down.isPressed
               ));
    }

    private bool InputSelect()
    {
        return Mouse.current.leftButton.wasPressedThisFrame ||
               (Gamepad.current != null && Gamepad.current.buttonNorth.wasPressedThisFrame);
    }

    private bool InputCancel()
    {
        return Mouse.current.rightButton.wasPressedThisFrame ||
               (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame);
    }

    private void UpdateUIstatue(CONTOROL control, bool enable)
    {
        TutorialUI ui = UI[control];
        ui.display = enable;
        UI[control] = ui;
    }

    public void updateRotationTutorialPos(Vector3 pos)
    {
        RotationTutorial.transform.position = Camera.main.WorldToScreenPoint(pos);
    }

    public void showRotationTutorial()
    {
        RotationTutorial.SetActive(true);
    }

    public void hideRotationTutorial()
    {
        RotationTutorial.SetActive(false);
    }
}