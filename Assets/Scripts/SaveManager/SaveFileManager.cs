using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveFileManager
{
    public DirectoryInfo MainDirectory;
	public DirectoryInfo PreSaveDirectory;
    public SaveFileManager()
	{
		MainDirectory = new DirectoryInfo(Application.persistentDataPath + "\\Saves");
		if (!MainDirectory.Exists)
		{
			MainDirectory.Create();
		}
		PreSaveDirectory = new DirectoryInfo(MainDirectory.FullName + "\\PreSave");
		if (!PreSaveDirectory.Exists)
		{
			PreSaveDirectory.Create();
		}
	}
	public SaveFileManager(string path)
	{
		MainDirectory = new DirectoryInfo(path);
		PreSaveDirectory = new DirectoryInfo(MainDirectory.FullName + "\\PreSave");
	}
	public void Initializer()
	{
		if (!MainDirectory.Exists)
		{
			MainDirectory.Create();
		}
		if (!PreSaveDirectory.Exists)
		{
			PreSaveDirectory.Create();
		}
	}
}
