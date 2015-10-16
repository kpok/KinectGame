using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System;
using System.Collections.Generic;

public static class Globals
{

    public static Configuration config = null;
	public static Score score = new Score()
	{
	player = new List<playerScore>()
		{
			new playerScore(){ nick = "", score = 0}
		},
	};
    public static float GameActiveTime = 0;

    public static bool LoadConfig()
    {
        try
        {
            string path = System.IO.Directory.GetParent(Application.dataPath) + "/Config.xml";

            if (System.IO.File.Exists(path))
            {


                XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
                FileStream fs = new FileStream(path, FileMode.Open);
                XmlReader reader = XmlReader.Create(fs);

                Globals.config = (Configuration)serializer.Deserialize(reader);
                fs.Close();


                Debug.Log(Globals.config.jumpSpeed);
                return true;
            }
            else
            {
                Globals.SaveConfig();
                return Globals.LoadConfig();
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log(e.Message + "  " + e.ToString());
        }

        return false;
    }

    public static bool SaveConfig()
    {
        try
        {
            Configuration config = new Configuration();

            string path = System.IO.Directory.GetParent(Application.dataPath) + "/Config.xml";
            System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(Configuration));

            System.IO.StreamWriter file = new System.IO.StreamWriter(path);
            writer.Serialize(file, config);
            file.Close();

            return true;
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log(e.Message + "  " + e.ToString());
        }

        return false;
    }

    public static bool LoadScore()
    {
        try
        {
            string path = System.IO.Directory.GetParent(Application.dataPath) + "/Score.xml";

            if (System.IO.File.Exists(path))
            {
                Type[] extraTypes = new Type[1];
                extraTypes[0] = typeof(playerScore);

                XmlSerializer serializer = new XmlSerializer(typeof(Score), extraTypes);
                FileStream fs = new FileStream(path, FileMode.Open);
                XmlReader reader = XmlReader.Create(fs);

                Globals.score = (Score)serializer.Deserialize(reader);
                fs.Close();

                for (int i = 0; i < score.player.Capacity; i++)
                {
                    Debug.Log(score.player[i].nick);
                }
                return true;
            }
            else
            {
                Globals.SaveScore();
                return Globals.LoadConfig();
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log(e.Message + "  " + e.ToString());
        }

        return false;
    }

    public static bool SaveScore()
    {
        try
        {
            string path = System.IO.Directory.GetParent(Application.dataPath) + "/Score.xml";

            System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(Score));

            System.IO.StreamWriter file = new System.IO.StreamWriter(path);
            writer.Serialize(file, Globals.score);
            file.Close();
            Debug.Log("zapis");
            foreach (playerScore ps in Globals.score.player)
            {
                Debug.Log(ps.nick);
            }
            return true;
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log(e.Message + "  " + e.ToString());
        }

        return false;
    }

}
