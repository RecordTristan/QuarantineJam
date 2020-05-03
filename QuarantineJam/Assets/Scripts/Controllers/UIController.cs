using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public TextMeshProUGUI[] listObject;
    public Image[] trailListObject;

    public RectTransform listPanel;
    public GameObject topListPanel;
    public GameObject botListPanel;

    [Header("PanelDevil")]
    public GameObject tlPanel;
    public GameObject trPanel;
    public GameObject rPanel;
    public GameObject brPanel;
    public GameObject blPanel;
    public GameObject lPanel;

    public Image[] panelImage;
    private bool _eventActive;

    private bool _activeList = false;

    void Awake()
    {
        if (instance) Destroy(gameObject);
        else instance = this;
    }

    void Update()
    {
        if (_eventActive)
        {
            CameraManager2D.instance.ClampPositionToScreenPanel();
        }
    }

    #region ListObject
    public void ActiveList()
    {
        if (_activeList)
        {
            listPanel.DOMoveY(botListPanel.transform.position.y, 1.5f).SetEase(Ease.OutExpo);
            _activeList = false;
        }
        else
        {
            _activeList = true;
            listPanel.DOMoveY(topListPanel.transform.position.y, 1.5f).SetEase(Ease.OutExpo);
        }
    }

    public void SetList(List<GrabObject> selecterItems)
    {
        for (int i = 0; i < listObject.Length; i++)
        {
            trailListObject[i].fillAmount = 0;
            if (i < selecterItems.Count)
            {
                listObject[i].text = selecterItems[i].nameGrabObject;
            }
            else
            {
                listObject[i].text = "";
            }
        }
    }
    public void ValidateObject(string nameObject)
    {
        for (int i = 0; i < listObject.Length; i++)
        {
            if (listObject[i].text == nameObject)
            {
                trailListObject[i].DOFillAmount(1, 0.5f);
            }
        }
    }

    public void SetPanel(Sprite sprite)
    {
        for (int i = 0; i < panelImage.Length; i++)
        {
            panelImage[i].sprite = sprite;
        }
        _eventActive = true;
    }
    public void EndEvent()
    {
        _eventActive = false;
    }
    #endregion
}
