using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading;
using System.Collections;
using System.Collections.Specialized;
using System.IO.Ports;

public class SerialInputController : MonoBehaviour {

	public Text inputValue;
	public String SerialPort = "/dev/cu.usbmodem1411";
	public int BaudRate = 57600;

	private static SerialInputController _instance;
	private Int16 keys = 0;
	private Int16 previousKeys = 0;
	private Int16 keysLights = 0;
	private BitArray keysBits;
	private SerialPort stream;
	private Thread updateThread;

	/* serial packet building */
	private bool escByte = false;
	private int req_size = 0;
	byte[] request = new byte[256];

	byte[] first = { 0x00, 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };

	// Use this for initialization
	void Start () {
		stream = new SerialPort (SerialPort, BaudRate);
		stream.ReadTimeout = 50;
		/*answer [0] = 0xAA;
		answer [1] = 0x04;
		answer [2] = 0x02;
		stream.Open ();
		StartCoroutine (blink ());*/
		updateThread = new Thread (new ThreadStart (updateSerialThread));
		updateThread.Start ();
	}

	/*IEnumerator blink()
	{
		while (true) 
		{
			for (int i = 0; i < 9; i++) 
			{
				if (i == 0) 
				{
					answer [3] = 0x01;
				} 
				else 
				{
					answer [3] = 0x00;
				}
				answer [4] = first [i];
				stream.Write (answer, 0, 5);
				//stream.Flush ();
				yield return new WaitForSeconds (0.04f);
			}
			for (int i = 8; i >= 0; i--) 
			{
				if (i == 0) 
				{
					answer [3] = 0x01;
				} 
				else 
				{
					answer [3] = 0x00;
				}
				answer [4] = first [i];
				stream.Write (answer, 0, 5);
				//stream.Flush ();
				yield return new WaitForSeconds (0.04f);
			}
		}
	}*/

	// Update is called once per frame
	void Update () 
	{
		inputValue.text = getKey(0) + "\n" + getKey(1) + "\n" + getKey(2) + "\n" + getKey(3) + "\n" + getKey(4) + "\n" + getKey(5) + "\n" + getKey(6) + "\n" + getKey(7) + "\n" + getKey(8) + "\n" + getKey(9) + "\n" + getKey(10) + "\n" + getKey(11);
	}

	void Awake()
	{
		if(!_instance)
		{
			_instance = this;
		}
		else
		{
			Destroy(this.gameObject);
		}
		DontDestroyOnLoad(this.gameObject);
	}

	private void updateSerialThread()
	{

		stream.Open ();
		byte[] init = { 0xAA, 0x02, 0x00 };
		stream.Write (init,0,3);
		while(true)
		{
			getRequest(ref request);
			if(isRequestComplete())
			{
				byte[] answer = new byte[256];
				processRequest(ref request, ref answer);
				sendAnswer(answer);
				req_size = 0;
			}
		}
	}

	private bool isRequestComplete()
	{
		if(req_size >= 3)
		{
			if(req_size >= 3 + request[1])
			{
				return true;
			}
		}
		return false;
	}

	private void getRequest(ref byte[] request)
	{
		if(stream.BytesToRead > 0)
		{
			byte inByte = (byte)stream.ReadByte();
			if(inByte == 0xAA)
			{
				escByte = false;
				req_size = 0;
			}

			if(inByte == 0xFF)
			{
				escByte = true;
			}

			if(inByte != 0xFF && inByte != 0xAA)
			{
				if(escByte)
				{
					inByte = (byte)~inByte;
					escByte = false;
				}

				request[req_size] = inByte;
				req_size++;
			}
		}
	}

	private void sendAnswer(byte[] answer)
	{
		int index = 0;
		byte[] output = new byte[257];
		output [index] = 0xAA;
		index++;

		byte bufsize = (byte)(2 + answer [1]);
		for (int i = 0; i < bufsize; i++)
		{
			byte outByte = answer [i];
			if (outByte == 0xAA || outByte == 0xFF)
			{
				outByte = (byte)~outByte;
				output [index] = 0xFF;
				index++;
			}
			output [index] = outByte;
			index++;
		}
		stream.Write (output,0,index+1);
	}

	private void processRequest(ref byte[] request, ref byte[] answer)
	{
		/* check the packet header */
		if (request [0] == 0xAA) 
		{
			if (request [1] == 0x03)
			{
				byte[] keysBytes = { request [4], request [3] };
				keys = BitConverter.ToInt16 (keysBytes, 0);

				//create light response
				answer [0] = 0x04;
				answer [1] = 0x02;
				answer [2] = BitConverter.GetBytes (keysLights) [1];
				answer [3] = BitConverter.GetBytes (keysLights) [0];
			}

			if (request [1] == 0x05)
			{
				answer [0] = 0x02;
				answer [1] = 0x00;
			}
		}

	}

	public bool getKey(int key)
	{
		keysBits = new BitArray (new int[] { keys });
		return keysBits[key];
	}

	public bool getKeyDown(int key)
	{

		return false;
	}
}
