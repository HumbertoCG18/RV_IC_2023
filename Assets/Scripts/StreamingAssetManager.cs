using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class StreamingAssetManager : Singleton<StreamingAssetManager>
{
    //Fonte https://discussions.unity.com/t/android-streamingassets-file-access/35531/2
    public void GetTextFile(string path, Action<string> onSuccess, Action<string> onFail)
    {
        StartCoroutine(GetTextFileCoroutine(path, onSuccess, onFail));
    }

    public IEnumerator GetTextFileCoroutine(string path, Action<string> onSuccess, Action<string> onFail)
    {
        string filePath = "jar:file://" + Application.dataPath + "!/assets/" + path;

        var www = new WWW(filePath);

        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            onFail?.Invoke($"Can't read {filePath}");
        }

        onSuccess?.Invoke(www.text);
    }

    public void GetAudioFile(List<string> files, Action<Dictionary<string, AudioClip>> onSuccess, Action<List<string>> onFail)
    {
        StartCoroutine(GetAudioFileCoroutine(files, onSuccess, onFail));
    }

    //Fonte https://discussions.unity.com/t/how-to-load-mp3-file-into-an-audioclip-and-make-it-stay-compressed-in-memory/806630
    public IEnumerator GetAudioFileCoroutine(List<string> files, Action<Dictionary<string, AudioClip>> onSuccess, Action<List<string>> onFail)
    {
        Dictionary<string, AudioClip> result = new Dictionary<string, AudioClip> ();
        List<string> erros = new List<string>();

        foreach(var file in files)
        {
            //string filePath = "jar:file://" + Application.dataPath + "!/assets/" + file;
            string filePath = Path.Combine(Application.streamingAssetsPath, file);

            var dh = new DownloadHandlerAudioClip(filePath, AudioType.MPEG);

            dh.compressed = true;

            using (UnityWebRequest wr = new UnityWebRequest(filePath, "GET", dh, null))
            {
                yield return wr.SendWebRequest();

                if (wr.responseCode == 200)
                {
                    result.Add(file, dh.audioClip);
                }
                else
                {
                    erros.Add(wr.error);
                }
            }
        }

        onSuccess?.Invoke(result);

        if (erros.Count > 0) onFail?.Invoke(erros);
    }
}
