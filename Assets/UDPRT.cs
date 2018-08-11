using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Collections.Generic;

using System;
 
public class UDPRT
{
    public string ReceivedMsg;                                      // INPUT DATA
    public IPEndPoint IP;
    public bool messageSent = false;

    private UdpClient udpc;
    private object obj;
    private AsyncCallback AC;
    private byte[] DATA;
    private List<byte[]> PacketList;

   
    public UDPRT(int Port)                          // RECEVE UDP
    {
            IP = new IPEndPoint(IPAddress.Any, Port);
            udpc = new UdpClient(Port);
            AC = new AsyncCallback(ReceiveIt);
	    obj = new object();
            StartUdpReceive();
            PacketList = new List<byte[]>();
    }
 
    public UDPRT(int Port, string Host,string msg)  // SEND UDP
    {
        udpc = new UdpClient(Host,Port);
        AC = new AsyncCallback(SendIt);
        byte[] data = Encoding.UTF8.GetBytes(msg);
        udpc.BeginSend(data, data.Length, AC, obj);
    }

    public UDPRT(int Port, string Host,byte[] data)  // SEND UDP
    {
        udpc = new UdpClient(Host,Port);
        AC = new AsyncCallback(SendIt);
        udpc.BeginSend(data, data.Length, AC, obj);
    }
 
    private void ReceiveIt(IAsyncResult result)
    {
        DATA = (udpc.EndReceive(result, ref IP));
        //Debug.Log(Encoding.UTF8.GetString(DATA));
        ReceivedMsg = Encoding.UTF8.GetString(DATA);
        StartUdpReceive();
    }
 
    private void SendIt(IAsyncResult result)
    {
        udpc.EndSend(result);
	//Debug.Log("UDP message send");
	messageSent = true;
    }
 
 
    private void StartUdpReceive()
    {
        udpc.BeginReceive(AC, obj);
    }

    public void OnDestroy() {
        udpc.Close();
        Debug.Log("UDPRT was destroyed");
    }

    public void AddToPacketList(byte[] data) {
        PacketList.Add(data);
    }

    private void SendProcessor() {
        while(true){
            if(PacketList.Count > 0) {
                 AC = new AsyncCallback(SendIt);
                 udpc.BeginSend(PacketList[0], PacketList[0].Length, AC, obj);
                 PacketList.RemoveAt(0);
            }
            else {
                break;
            }
        }
    }
    /*~UDPRT() {
        udpc.Close();
        Debug.Log("UDPRT destructor");
    }*/
}
 