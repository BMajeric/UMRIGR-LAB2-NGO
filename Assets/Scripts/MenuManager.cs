using UnityEngine;
using Unity.Netcode;

public class MenuManager : MonoBehaviour
{
    private NetworkManager networkManager;

    private void Start()
    {
        networkManager = FindObjectOfType<NetworkManager>();
    }

    // TODO
    public void StartHost()
    {
        //Start host through networkManager
        networkManager.StartHost();
    }

    // TODO
    public void StartClient()
    {
        //Start client through networkManager
        networkManager.StartClient();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
