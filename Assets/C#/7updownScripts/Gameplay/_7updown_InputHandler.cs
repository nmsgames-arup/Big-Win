using Shared;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Updown7.Utility;
using Updown7.Gameplay;
public class _7updown_InputHandler : MonoBehaviour
{
    public Spot spot;
    [SerializeField] _7updown_ChipController chipController;
    bool _clicked;
    // public void OnPointerClick(Vector3 target)
    // {
    //     switch (spot)
    //     {
    //         case Spot.left:
    //             PlayAreaScript.Instance.DragonBet(target);                
    //             break;
    //         case Spot.middle:
    //             PlayAreaScript.Instance.TieBet(target);
    //             break;
    //         case Spot.right:
    //             PlayAreaScript.Instance.TigerBet(target);
    //             break;
    //      }
    // }

    // private void Update()
    // {
    //     // if (isTimeUp) return;
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         // Debug.LogError("Pressed the mouse btn  " + this.gameObject.name);
    //         _clicked = true;
    //         ProjectRay();
    //     }
    // }

    public LayerMask bettingPlaceLM; 
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
