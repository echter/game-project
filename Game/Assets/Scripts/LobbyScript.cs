using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyScript : NetworkBehaviour
{

    public Text text;
    public Slider slider;

    private bool run = true;
    private float timer = 0.0f;
    private float waitTime = 10.0f;
	
	// Update is called once per frame
	void Update ()
	{
	    if (run)
	    {
	        if (!isServer)
	        {
	            return;
	        }
            RpcUpdateSlider();
	        timer += Time.deltaTime;

	        if (timer >= waitTime)
	        {
	            LoadScene();
	        }
	    }
	}

    private void LoadScene()
    {
        NetworkManager.singleton.ServerChangeScene("main");
        run = false;
    }

    [ClientRpc]
    private void RpcUpdateSlider()
    {
        slider.value = (timer / waitTime);
        if ((waitTime - timer) > 0)
        {
            text.text = "Time untill next round: " + (int)(waitTime - timer);
        }
        else
        {
            text.text = "Time untill next round: " + 0;
        }
    }
}
