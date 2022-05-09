using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Shared;
using PokerKing.Utility;
using PokerKing.Gameplay;

public class PokerKing_InputHandler : MonoBehaviour
{
    [SerializeField] PokerKing_ChipController chipController;
    public Camera camera;
    private void OnMouseDown()
    {
        ProjectRay();
    }
    void ProjectRay()
    {
        Vector3 origin = camera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector3.forward * 100);
        if (hit.collider != null)
        {
            chipController.OnUserInput(hit.transform, hit.point);
        }

            // RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, Vector3.forward);
            // if( hit.collider != null )
            // {
            //     // Debug.LogError("hit collider ");
            //     chipController.OnUserInput(hit.transform, hit.point);
            // }

    }
}
