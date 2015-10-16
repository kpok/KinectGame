using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml.Serialization;

[Serializable()]
public class Score
{
    [XmlElement(ElementName = "player")]
    public List<playerScore> player;
}

[Serializable()]
public class playerScore
{
    [XmlElement(ElementName = "nick")]
    public String nick = "";

    [XmlElement(ElementName = "score")]
    public int score = 0;
}