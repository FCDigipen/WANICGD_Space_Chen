using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// credit: https://www.youtube.com/watch?v=N6vYyCahLr8
// https://www.youtube.com/watch?v=lEpigNZwM-4&t=1s

[RequireComponent(typeof(TMP_Text))]
public class LinkHandlerForTMPText : MonoBehaviour
{
    private TMP_Text _tmpTextBox;
    private Canvas _canvasToCheck;
    [SerializeField] private Camera cameraToUse;

    private void Awake()
    {
        _tmpTextBox = GetComponent<TMP_Text>();
        _canvasToCheck = GetComponentInParent<Canvas>(); // "recursively searches up object heirarchy" so should be fine?

        if (_canvasToCheck.renderMode == RenderMode.ScreenSpaceOverlay) { cameraToUse = null; }
        else { cameraToUse = _canvasToCheck.worldCamera; }
    }

    public void OnPointerClick()
    {
        Debug.Log("clicked");
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

        // check if there's actually a link below ehere we click
        int linkTaggedText = TMP_TextUtilities.FindIntersectingLink(_tmpTextBox, mousePos, cameraToUse);

        // there isn't a link
        if (linkTaggedText == -1) return;

        TMP_LinkInfo linkInfo = _tmpTextBox.textInfo.linkInfo[linkTaggedText];

        string linkId = linkInfo.GetLinkID(); // ID attached to link component
        // https://stackoverflow.com/questions/161738/what-is-the-best-regular-expression-to-check-if-a-string-is-a-valid-url
        if (Regex.IsMatch(linkId, "((([A-Za-z]{3,9}:(?:\\/\\/)?)(?:[-;:&=\\+\\$,\\w]+@)?[A-Za-z0-9.-]+|(?:www.|[-;:&=\\+\\$,\\w]+@)[A-Za-z0-9.-]+)((?:\\/[\\+~%\\/.\\w-_]*)?\\??(?:[-\\+=&;%@.\\w_]*)#?(?:[\\w]*))?)")) { Application.OpenURL(linkId); return; }
        else { throw new NotImplementedException(linkId + " is not a valid link!"); }
    }
}
