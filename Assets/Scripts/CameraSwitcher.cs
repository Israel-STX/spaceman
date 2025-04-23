/*  CameraSwitcher
 *  -------------
 *  Place this on a trigger collider that surrounds a room/section.
 *  Assign the room’s Cinemachine Virtual Camera in the Inspector.
 *  When the player enters, its Priority → 10 ; when he leaves → 0.
 */
using UnityEngine;
using Unity.Cinemachine;

[RequireComponent(typeof(Collider2D))]
public class CameraSwitcher : MonoBehaviour
{
    public CinemachineCamera vcam;
    private bool isActive;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger) isActive = true;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger) isActive = false;
    }
    private void LateUpdate()            // smoother than FixedUpdate
    {
        vcam.Priority = isActive ? 10 : 0;
    }
}
