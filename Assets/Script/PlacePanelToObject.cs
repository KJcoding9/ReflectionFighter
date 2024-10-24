using UnityEngine;

public class PlacePanelToObject : MonoBehaviour
{

    public GameObject panelPosition;

    [SerializeField] Camera cam;

    void Start()
    {
        var targetScreenPos = cam.WorldToScreenPoint(panelPosition.transform.position);
        var parentUI = gameObject.transform.parent.GetComponent<RectTransform>();


        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentUI,
            targetScreenPos,
            null,
            out var uiLocalPos
            );

        gameObject.transform.localPosition = uiLocalPos;

    }
}
