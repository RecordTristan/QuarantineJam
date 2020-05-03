using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

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
    public GameObject UIPause;

    public Image[] panelImage;

    [Header("Score")]
    public GameObject endButton; 
    public GameObject endPanel;
    public GameObject winscore;
    public GameObject looseScore;
    public TextMeshProUGUI textScore;
    public Vibration vibration;

    private bool _eventActive;

    private bool _activeList = false;
    int _value;
    void Awake()
    {
        if (instance) Destroy(gameObject);
        else instance = this;

        endPanel.SetActive(false);
    }

    void Update()
    {
        if (_eventActive)
        {
            CameraManager2D.instance.ClampPositionToScreenPanel();
        }
    }

    #region ListObject
    public void ActivePause()
    {
        UIPause.SetActive(true);
    }
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
                trailListObject[i].gameObject.SetActive(true);
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

    #region EndPanel
    public void Win()
    {
        endPanel.SetActive(true);
        winscore.SetActive(true);
        looseScore.SetActive(false);
        StartCoroutine(FocusEventSystem(endButton));
        DisplayScore();
    }
    public void Loose()
    {
        endPanel.SetActive(true);
        winscore.SetActive(false);
        looseScore.SetActive(true);
        StartCoroutine(FocusEventSystem(endButton));
        DisplayScore();
    }
    public void DisplayScore()
    {
        DOTween.To(()=> _value, (x)=> {
            _value = x;
            textScore.text = _value.ToString();
        }, GameController.instance.score, 2 ).OnComplete(()=> vibration.amplify = 0);
    }
    #endregion
    #region MenuControl
    public void LoadScene(string nameScene)
    {
        SceneManager.LoadScene(nameScene);
    }
    public IEnumerator FocusEventSystem(GameObject button)
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(button);
    }
    #endregion
}
