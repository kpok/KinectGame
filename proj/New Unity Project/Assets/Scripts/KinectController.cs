using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Text; 
using Microsoft.Kinect;

public class KinectController : MonoBehaviour {

	private KinectSensor sensor;
	// Use this for initialization
	void Start () {
		foreach (var potentialSensor in KinectSensor.KinectSensors)
		{
			if (potentialSensor.Status == KinectStatus.Connected)
			{
				this.sensor = potentialSensor;
				break;
			}
		}
		
		if (null != this.sensor)
		{
			// Turn on the skeleton stream to receive skeleton frames
			this.sensor.SkeletonStream.Enable();
			
			// Add an event handler to be called whenever there is new color frame data
			this.sensor.SkeletonFrameReady += this.SensorSkeletonFrameReady;
			
			// Start the sensor!
			try
			{
				this.sensor.Start();
			}
			catch (IOException)
			{
				this.sensor = null;
			}
		}
		
		if (null == this.sensor)
		{
			Debug.Log ("no kinect");
		}
	}

	private void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
	{
		Skeleton[] skeletons = new Skeleton[0];
		
		using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame()) {
			if (skeletonFrame != null) {
				skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
				skeletonFrame.CopySkeletonDataTo (skeletons);
			}
		}

		for (int i=0; i< skeletons.Length; i++) {
			Debug.Log(skeletons[i].ToString());
		}
	}
	
}
