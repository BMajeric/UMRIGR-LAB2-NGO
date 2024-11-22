using UnityEngine;
using Unity.Netcode;

public class FloorManager : NetworkBehaviour
{
    private MeshRenderer floorRenderer;
    private bool active = false;
    public string playerName;
    private Color defaultColor;

    private void Start()
    {
        floorRenderer = GetComponent<MeshRenderer>();
        defaultColor = floorRenderer.material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && active)
        {
            var playerRenderer = other.GetComponent<MeshRenderer>();
            var player = other.GetComponent<PlayerManager>().GetPlayerName();

            if (playerRenderer != null)
            {
                ColorFloorRpc(playerRenderer.material.color, player);
            }
        }
    }

    public void ActivateFloor()
    {
        active = true;
    }

    public void DeActivateFloor()
    {
        active = false;
    }

    public void ResetColor()
    {
        floorRenderer.material.color = defaultColor;
        playerName = "";
    }

    //TODO: All entities should activate this function
    [Rpc(SendTo.Everyone)]
    void ColorFloorRpc(Color color, string player)
    {
        //set floor color to the color of the player
        floorRenderer.material.color = color;
        //set playerName to the player that touched the floor
        playerName = player;
    }
}
