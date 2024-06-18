using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoundOnHover : MonoBehaviour
{
    int UILayer;
    bool hovere;
    [SerializeField] AudioClip cliP;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        UILayer = LayerMask.NameToLayer("UI");
    }
    private void Update()
    {
        if (IsPointerOverUIElement())
        {
            if (!hovere)
            {
                hovere = true;
                AudioManager.Instance.UIsound(cliP);
                print("new");
            }
        }
        else
        {
            print("old");
            hovere = false;
        }
    }
    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }
    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == UILayer)
                return true;
        }
        return false;
    }
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
}
