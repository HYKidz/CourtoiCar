using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Storage;
using UnityEngine;
using UnityEngine.UI;

public class PhotoController : MonoBehaviour
{
    //pour l'instant on prend des image deja la, mais penser a consevoir qu'on peu rajouter nombre de photo
    [SerializeField] private RawImage[] _images;
    // private Coroutine _co;
    // public Coroutine Co {get =>_co; set=> _co = value}
    private int _counter = 0;
    private Dictionary<int, byte[]> _dicpic { get; set; } = new Dictionary<int, byte[]>();
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
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

    public Dictionary<int,byte[]> GetPicture()
    {
        // StartCoroutine(UploadCoroutine());
        return _dicpic;
    }
    //may be good, but must think of how to implement photo in panel
    // private IEnumerator UploadCoroutine()
    // {
    //     var storage = FirebaseStorage.DefaultInstance;
    //             Debug.Log("you");
    //     for (int i = 0; i < _images.Length; i++)
    //     {
    //         if (!_dicpic.ContainsKey(i))
    //         {
    //             Debug.Log("are");
    //             yield break;
    //         }
    //         else
    //         {
    //             Debug.Log("hey");
    //             var ScreenshotReference = storage.GetReference($"/photo/${Guid.NewGuid()}.jpg");
    //             var metadataChange = new MetadataChange()
    //             {
    //                 ContentEncoding = "image/png",
    //                 // CustomMetadata = 
    //             };
    //             var uploadTask = ScreenshotReference.PutBytesAsync(_dicpic[i], metadataChange);
    //             yield return new WaitUntil(() => uploadTask.IsCompleted);

    //             if (uploadTask.Exception != null)
    //             {
    //                 Debug.LogError($"Failed to upload because {uploadTask.Exception}");
    //                 yield break;
    //             }
    //             var getUrlTask = ScreenshotReference.GetDownloadUrlAsync();
    //             yield return new WaitUntil(() => getUrlTask.IsCompleted);
    //             if (getUrlTask.Exception != null)
    //             {
    //                 Debug.LogError($"Failed to get a download url with {getUrlTask.Exception}");
    //                 yield break;
    //             }
    //             Debug.Log($"Download from {getUrlTask.Result}");
    //         }
    //     }
    // }
}
