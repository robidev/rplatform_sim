using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class DisplayController : EventReceiver {

	private string frame = "frame:";

	public override int parseEvent(string message, ref string response, ref UDPRT MyUdp)
	{
		int retVal = -1;
		int temp  = base.parseEvent(message, ref response, ref MyUdp);//check if message is for us

		if(temp == -1){
			return -1;
		}
		else{//remove header until first .
			message = message.Substring(temp);
		}
		//Debug.Log("DisplayController received Event:'" + message + "'");
		if(message == "identify"){
			response = "id:display:" + gameObject.name; // + ":setimage";
			retVal = 0;
		}
		else if(message.StartsWith(frame) == true){

			message = message.Substring(frame.Length);
			Debug.Log("data len '" + message.Length + "'");
                        byte[] bytes = Encoding.ASCII.GetBytes(message);

			//decode message (gzip compressed data)
			var imgdata = bytes;//GzipUtil.DecompressEncode(bytes);

			//copy array to texture
			Texture2D texture = new Texture2D(30, 30);
			texture.LoadRawTextureData(imgdata);//new Rect(0, 0, 320, 320), 0, 0);
			texture.Apply();

			//set newTex to received image data
			gameObject.GetComponent<Renderer>().material.mainTexture = texture;

			response = gameObject.name + ":OK";
			retVal = 0;
		}
		return retVal;
	}
}
