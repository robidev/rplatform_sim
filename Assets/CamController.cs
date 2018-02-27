using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : EventReceiver {

	public Camera cam;
        public bool grab;
	private RenderTexture main;
	public GameObject obj;

	// Use this for initialization
	public override int parseEvent(string message, ref string response)
	{
		int retVal = -1;
		int temp  = base.parseEvent(message, ref response);//check if message is for us
		if(temp == -1){
			return -1;
		}
		else{//remove header until first .
			message = message.Substring(temp);
		}
		Debug.Log("CamController received Event:'" + message + "'");
		if(message == "identify"){
			response = "cam:" + gameObject.name; // + ":getimage";
			retVal = 0;
		}
		else if(message == "getimage"){
			Debug.Log("getimage");
			response = "OK";
			retVal = 0;
			grab = true;
		}
		return retVal;
	}
    	void OnPostRender() {
	    if (grab) {
		Texture2D image = new Texture2D(cam.pixelWidth, cam.pixelHeight);
		image.ReadPixels(new Rect(0, 0, cam.pixelWidth, cam.pixelHeight), 0, 0);
		image.Apply();

		var newTex = Instantiate (image);
		TextureScale.Bilinear (newTex, 320, 320);
		//obj.GetComponent<Renderer>().material.mainTexture = newTex;

		var zipdata = GzipUtil.CompressEncode(newTex.GetRawTextureData());
		//Debug.Log("before:" + newTex.GetRawTextureData().Length + " after:" + zipdata.Length);
		
		UDPRT send_image = new UDPRT(5002,"127.0.0.1", zipdata);
		if (!send_image.messageSent){;}
		//Debug.Log("done");
	        grab = false;
	   }
        }
}