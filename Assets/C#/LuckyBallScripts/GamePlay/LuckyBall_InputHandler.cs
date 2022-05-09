using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using LuckyBall.Utility;
using LuckyBall.Gameplay;
using Shared;

public class LuckyBall_InputHandler : MonoBehaviour
{
    [SerializeField] LuckyBall_ChipController chipController;
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
    }
}
