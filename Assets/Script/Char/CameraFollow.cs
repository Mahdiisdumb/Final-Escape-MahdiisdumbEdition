using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public string playerTag = "Player";        // tag to look for
    public Vector3 offset = new Vector3(0, 0, -10f); // keep Z = -10
    public float smoothSpeed = 5f;

    private Transform player;

    void LateUpdate()
    {
        // Find player if we don't have it yet
        if (player == null)
        {
            GameObject p = GameObject.FindWithTag(playerTag);
            if (p != null) player = p.transform;
        }

        // Follow player if found
        if (player != null)
        {
            Vector3 targetPos = new Vector3(player.position.x, player.position.y, 0) + offset;
            transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);
        }
        // else camera stays where it is
    }
}