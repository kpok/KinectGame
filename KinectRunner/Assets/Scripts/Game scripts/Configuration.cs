using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml.Serialization;

[Serializable()]
public class Configuration {

	[XmlElement(ElementName = "speed")]
	public float speed = 7;
	
	[XmlElement(ElementName = "jumpSpeed")]
	public float jumpSpeed = 4;
	
	[XmlElement(ElementName = "gravity")]
	public float gravity = 20;
}
