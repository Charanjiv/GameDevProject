using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance;

    [Header("NETWORK JOIN")]
    [SerializeField] bool startGameAsClient;

    private void Awake()
    {
        //  There can only be one instance of this at any given time. If another exists, destroy it.
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (startGameAsClient)
        {
            startGameAsClient = false;
            //  First shut down the network as a host, to then start it as a client. Started as a host during the title screen.
            NetworkManager.Singleton.Shutdown();
            //  We restart the network as a client.
            NetworkManager.Singleton.StartClient();
        }
    }
}
