using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Threading; 

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
    private List<byte[]> ReceivedPacketList;

    public UDPRT(int Port)                          // RECEVE UDP
    {
            IP = new IPEndPoint(IPAddress.Any, Port);
            udpc = new UdpClient(Port);
            AC = new AsyncCallback(ReceiveIt);
	    obj = new object();
            StartUdpReceive();
            PacketList = new List<byte[]>();
            ReceivedPacketList = new List<byte[]>();
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
        Debug.Log("data length:" + DATA.Length);
	ReceivedPacketList.Add(DATA);
        //ReceivedMsg = Encoding.UTF8.GetString(DATA);
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

    public string ReceiveProcessor() {   
	if(ReceivedPacketList.Count > 0) {
		string msg = Encoding.ASCII.GetString(ReceivedPacketList[0]);
		ReceivedPacketList.RemoveAt(0);
		return msg;
	}
	return null;
    }
    //~UDPRT() {
    //    udpc.Close();
    //    Debug.Log("UDPRT destructor");
    //}
}

public class TCPRT 
{  	
	private TcpListener tcpListener; 
	private Thread tcpListenerThread;  	
	private TcpClient connectedTcpClient;
	public string ReceivedMsg; 	
    	private List<byte[]> PacketList;
	private volatile bool isRunning = true;

	public TCPRT () { 		
		// Start TcpServer background thread 
      		PacketList = new List<byte[]>();		
		tcpListenerThread = new Thread (new ThreadStart(ListenForIncommingRequests)); 		
		tcpListenerThread.IsBackground = true; 		
		tcpListenerThread.Start(); 	
	}  		
	
	private void ListenForIncommingRequests () { 		
		try { 			
			// Create listener on localhost port 8052. 			
			tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 4000); 			
			tcpListener.Start();              
			Debug.Log("Server is listening");              
			Byte[] bytes = new Byte[65530];  			
			while (isRunning) { 				
				using (connectedTcpClient = tcpListener.AcceptTcpClient()) { 					
					// Get a stream object for reading 					
					using (NetworkStream stream = connectedTcpClient.GetStream()) { 						
						int length; 						
						// Read incomming stream into byte arrary. 						
						while ((length = stream.Read(bytes, 0, bytes.Length)) != 0) { 							
							var incommingData = new byte[length]; 							
							Array.Copy(bytes, 0, incommingData, 0, length);  							
							// Convert byte array to string message. 							
							ReceivedMsg = Encoding.ASCII.GetString(incommingData); 												
						} 					
					} 				
				} 			
			} 		
		} 		
		catch (SocketException socketException) { 			
			Debug.Log("SocketException " + socketException.ToString()); 		
		}     
	}
  	
    	public void AddToPacketList(string msg) {
		byte[] data = Encoding.ASCII.GetBytes(msg);
        	PacketList.Add(data);
	}

    	public void AddToPacketList(byte[] data) {
        	PacketList.Add(data);
    	}

	private void SendProcessor() { 		
		if (connectedTcpClient == null) {             
			return;         
		}  		
		
		try { 			
			// Get a stream object for writing. 			
			NetworkStream stream = connectedTcpClient.GetStream(); 			
			if (stream.CanWrite) {                 
			       while(true){
				    if(PacketList.Count > 0) {
					 stream.Write(PacketList[0], 0, PacketList[0].Length);  
					 PacketList.RemoveAt(0);
				    }
				    else {
					break;
				    }
				}                      
			}       
		} 		
		catch (SocketException socketException) {             
			Debug.Log("Socket exception: " + socketException);         
		} 	
	} 

    	public void OnDestroy() {
        	isRunning = false;
		tcpListener.Stop();
		tcpListenerThread.Abort();
        	Debug.Log("TCPRT was destroyed");
    	}
}
 