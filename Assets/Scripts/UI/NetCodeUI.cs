using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class NetCodeUI : MonoBehaviour
{
    public Button startHostButton;
    public Button startClientButton;
    public Button startGameButton;
    private void Awake() {
        startGameButton.gameObject.SetActive(false);
        startHostButton.onClick.AddListener(()=>{
            Debug.Log("Host");
            NetworkManager.Singleton.StartHost();
            startHostButton.gameObject.SetActive(false);
            startClientButton.gameObject.SetActive(false);
            startGameButton.gameObject.SetActive(true);
        });
        startClientButton.onClick.AddListener(()=>{
            Debug.Log("Client");
            NetworkManager.Singleton.StartClient();
            startHostButton.gameObject.SetActive(false);
            startClientButton.gameObject.SetActive(false);
        });
        startGameButton.onClick.AddListener(()=>{
            Debug.Log("StartGame");
            GameObject.Find("GameManager").GetComponent<GameManager>().StartGameBtn();
            startGameButton.gameObject.SetActive(false);
        });
    }
}
