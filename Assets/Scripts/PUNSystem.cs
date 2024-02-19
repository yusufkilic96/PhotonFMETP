using System;
using System.Collections;
using System.Collections.Generic;
using FMETP;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PUNSystem : MonoBehaviour
{
    public PhotonView m_PhotonView;
    public GameViewDecoder GameViewDecoder;
    public string YourName;

    public GameObject Track;
    public Toggle WebcamToggle;
    
    private void Start()
    {
        YourName = PhotonNetwork.LocalPlayer.UserId;
    }
    
    public void WebCam()
    {
        /*
        if (WebcamToggle.isOn == true)
        {
            Track.SetActive(false);
        }
        if (WebcamToggle.isOn == false)
        {
            Track.SetActive(true);
        }
        */
    }

    public void SendMessage(byte[] _byteData, string message, string username)
    {
        m_PhotonView.RPC("RPC_SendMessage", RpcTarget.All, _byteData, message, username);
    }
    
    [PunRPC]
    private void RPC_SendMessage(byte[] _byteData, string message, string username)
    {
        if (YourName != username)
        {
            if (message.Contains("VideoShare"))
            {
                GameViewDecoder.Action_ProcessImageData(_byteData);
            }
        }
    }
}
