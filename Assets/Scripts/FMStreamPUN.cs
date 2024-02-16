using System;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMETP;

public class FMStreamPUN : Photon.Pun.MonoBehaviourPun, IPunObservable {
    private Queue<byte[]> appendQueueSendData = new Queue<byte[]>();
    public int appendQueueSendDataCount { get { return appendQueueSendData.Count; } }

    public UnityEventByteArray OnDataByteReadyEvent = new UnityEventByteArray();

    public GameViewDecoder _GameViewDecoder;
    
    void Start()
    {
        _GameViewDecoder = FindObjectOfType<GameViewDecoder>();
        OnDataByteReadyEvent.AddListener(_GameViewDecoder.Action_ProcessImageData);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            //Send the meta data of the byte[] queue length
            stream.SendNext(appendQueueSendDataCount);
            //Sending the queued byte[]
            while(appendQueueSendDataCount > 0) {
                byte[] sentData = appendQueueSendData.Dequeue();
                stream.SendNext(sentData);
            }
        }

        if (stream.IsReading) {
            if (!photonView.IsMine) {
                //Get the queue length
                int streamCount = (int)stream.ReceiveNext();
                for (int i = 0; i < streamCount; i++) {
                    //reading stream one by one
                    byte[] receivedData = (byte[])stream.ReceiveNext();
                    OnDataByteReadyEvent.Invoke(receivedData);
                }
            }
        }
    }

    public void Action_SendData(byte[] inputData) {
        //inputData(byte[]) is the encoded byte[] from your encoder
        //doesn't require any stream, when there is only one player in the room
        if(PhotonNetwork.CurrentRoom.PlayerCount > 1) appendQueueSendData.Enqueue(inputData);
    }
}
