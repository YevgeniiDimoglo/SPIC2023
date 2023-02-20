using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BookObject : HoldableObject
{
    [SerializeField] private string title = "";
    [SerializeField] private string category = "";

    private TextMeshPro CoverText;

    private GameObject popupWin; // For Hover

    private void Awake()
    {
        CoverText = GetComponentInChildren<TextMeshPro>();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        popupWin = null;

        base.Start();
    }

    protected override void Update()
    {
        if (popupWin == null) return;

        updatePopup();
    }

    public void setData(string title, string category)
    {
        this.title = title;
        this.category = category;

        UpdateCover();
    }

    private void UpdateCover()
    {
        if (CoverText) CoverText.text = title;
    }

    public override void hover()
    {
        popupWin = new GameObject("Book Window");
        TextMesh popup = popupWin.AddComponent<TextMesh>();
        popup.text = PopupText();
        popup.anchor = TextAnchor.MiddleCenter;
        updatePopup();

        base.hover();
    }

    private void updatePopup()
    {
        popupWin.transform.position = transform.position + Vector3.up * 0.3f;
        popupWin.transform.rotation = Camera.main.transform.rotation;
        popupWin.GetComponent<TextMesh>().characterSize = Vector3.Distance(Camera.main.transform.position, popupWin.transform.position) * 0.05f;
    }

    public override void unhover()
    {
        if (popupWin != null)
        {
            Destroy(popupWin);
            popupWin = null;
        }

        base.unhover();
    }

    public string PopupText()
    {
        return title + "\r\n" + category;
    }

    public string Title()
    { return title; }

    public string Catgeory()
    { return category; }
}