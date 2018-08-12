using System;
using System.Text;
using System.IO.Compression;
using System.IO;

class GzipUtil {
	/// <summary>
	/// Compress a JSON string with base-64 encoded gzip compressed string.
	/// </summary>
	/// <param name="json"></param>
	/// <returns>json base64 data stream</returns>
	public static byte[] CompressEncode(byte[] data) {
	    // Encode into data byte-stream with UTF8.
	    //byte[] data = UTF8Encoding.UTF8.GetBytes(json);

	    using (MemoryStream memory = new MemoryStream()) {
		using (GZipStream gzip = new GZipStream(memory, CompressionMode.Compress)) {
		    gzip.Write(data, 0, data.Length);
		}

		return memory.ToArray();//Convert.ToBase64String(memory.ToArray());
	    }
	}

	public static byte[] DecompressEncode(byte[] data) {

		using (var inStream = new MemoryStream(data))
		using (var bigStream = new GZipStream(inStream, CompressionMode.Decompress))
		using (var bigStreamOut = new MemoryStream())
		{
        	    byte[] buffer = new byte[16 * 1024];
        	    int bytesRead;
        	    while ((bytesRead = bigStream.Read(buffer, 0, buffer.Length)) > 0)
        	    {
        	        bigStreamOut.Write(buffer, 0, bytesRead);
        	    }		    
		    return bigStreamOut.ToArray();
		}
	}
}

