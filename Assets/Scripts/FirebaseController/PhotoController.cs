using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Storage;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class MyStringEvent : UnityEvent<string>
{

}
public class PhotoController : MonoBehaviour
{

    //pour l'instant on prend des image deja la, mais penser a consevoir qu'on peu rajouter nombre de photo
    [SerializeField] private RawImage[] _images;
    private MyStringEvent _sendPicPath;
    private int _width;
    private int _height;
    public MyStringEvent SendPicPath { get => _sendPicPath; set => _sendPicPath = value; }
    private UnityEvent _sendInfo;
    public UnityEvent SendInfo { get => _sendInfo; set => _sendInfo = value; }
    private int _counter = 0;
    private Dictionary<int, byte[]> _dicpic { get; set; } = new Dictionary<int, byte[]>();
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Awake()
    {
        _sendPicPath = new MyStringEvent();
        _sendInfo = new UnityEvent();
        // foreach (RawImage image in _images)
        // {
        //     image.GetComponent<RawImage>();
        // }
        for (int i = 0; i < _images.Length; i++)
        {
            _images[i] = _images[i].GetComponent<RawImage>();
        }
    }

    public void NewPicture(byte[] picture, int width, int height)
    {
        _width = width;
        _height = height;
        //a voir pour choisire une photo dans la gallerie
        // NativeGallery.SaveImageToGallery(picture, "no", "no");
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        ImageConversion.LoadImage(tex, picture);
        _images[_counter].texture = tex;

        if (_dicpic.ContainsKey(_counter))
        {
            _dicpic[_counter] = picture;
        }
        else _dicpic.Add(_counter, picture);
        if (_counter >= _images.Length - 1)
        {
            _counter = 0;
        }
        else _counter++;
    }

    public void GetPicture(string marque)
    {
        StartCoroutine(UploadCoroutine(marque));
    }
    //may be good, but must think of how to implement photo in panel
    private IEnumerator UploadCoroutine(string marque)
    {
        var storage = FirebaseStorage.DefaultInstance;
        for (int i = 0; i < _images.Length; i++)
        {
            if (!_dicpic.ContainsKey(i))
            {
                yield break;
            }
            else
            {
                string path = Guid.NewGuid().ToString();
                var ScreenshotReference = storage.GetReference($"/{marque}/${path}.jpg");
                var metadataChange = new MetadataChange()
                {
                    ContentEncoding = "image/jpg",
                    CustomMetadata = new Dictionary<string, string>()
                    {
                        {"Width", _width.ToString()},
                        {"Height",_height.ToString()}
                    }
                };
                var uploadTask = ScreenshotReference.PutBytesAsync(_dicpic[i], metadataChange);
                yield return new WaitUntil(() => uploadTask.IsCompleted);

                if (uploadTask.Exception != null)
                {
                    Debug.LogError($"Failed to upload because {uploadTask.Exception}");
                    yield break;
                }
                var getUrlTask = ScreenshotReference.GetDownloadUrlAsync();
                yield return new WaitUntil(() => getUrlTask.IsCompleted);
                if (getUrlTask.Exception != null)
                {
                    Debug.LogError($"Failed to get a download url with {getUrlTask.Exception}");
                    yield break;
                }
                Debug.Log($"Download from {getUrlTask.Result}");
                _sendPicPath.Invoke($"${path}");
            }
        }
        _sendInfo.Invoke();
    }
}
