using Unity.Netcode;
using UnityEngine;
using Unity.Collections;

public class PlayerManager : NetworkBehaviour
{
    public float moveSpeed = 5f;
    private NetworkManager networkManager;
    private GameManager gameManager;
    private Vector3 spawnPoint1 = new Vector3(11f, 1.5f, 0f);
    private Vector3 spawnPoint2 = new Vector3(0f, 1.5f, 11f);
    private bool canMove = false;
    private MeshRenderer playerMeshRenderer;
    //TODO: this network variable needs to be readable by everyone and can be written only by owner
    private NetworkVariable<FixedString512Bytes> playerName = new NetworkVariable<FixedString512Bytes>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private void Update()
    {
        if (IsOwner && canMove)
        {
            Movement();
        }
    }

    // TODO
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        networkManager = FindObjectOfType<NetworkManager>();
        gameManager = FindObjectOfType<GameManager>();

        //if you are the owner and the host, set the player to spawnPoint1 and rename the player to "Host"
        if (IsOwner && IsHost)
        {
            transform.SetPositionAndRotation(spawnPoint1, new Quaternion());
            playerName.Value = "Host";
        }
        //if you are the owner and the client, set the player to spawnPoint2 and rename the player to "Client"
        else if (IsOwner && IsClient)
        {
            transform.SetPositionAndRotation(spawnPoint2, new Quaternion());
            playerName.Value = "Client";
        }
    }

    private void Start()
    {
        playerMeshRenderer = this.GetComponent<MeshRenderer>();
    }

    void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        MovePlayer(moveDirection);
    }

    void MovePlayer(Vector3 moveDirection)
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    public void ResetPosition()
    {
        if (IsOwner && networkManager.IsHost)
        {
            transform.SetPositionAndRotation(spawnPoint1, new Quaternion());
        }
        else if (IsOwner && networkManager.IsClient)
        {
            transform.SetPositionAndRotation(spawnPoint2, new Quaternion());
        }
    }

    public void EnableMovement()
    {
        canMove = true;
    }

    public void DisableMovement()
    {
        canMove = false;
    }

    public string GetPlayerName()
    {
        return playerName.Value.ToString();
    }

    public void SetColor(Color color)
    {
        ChangeColorRpc(color);
    }

    //TODO: All entities should activate this function
    [Rpc(SendTo.Everyone)]
    private void ChangeColorRpc(Color color)
    {
        // change the color of the player's material
        playerMeshRenderer.material.color = color;
        // find playerChildManager of the child object
        var playerChildManager = GetComponentInChildren<PlayerChildManager>();
        
        if (playerChildManager != null)
        {
            // set the color of the child object
            playerChildManager.GetComponent<MeshRenderer>().material.color = color;
        }
        else
        {
            Debug.LogWarning("playerChildManager not found");
        }

        // tell the game manager that the player is ready
        gameManager.PlayerReady();
    }

}
