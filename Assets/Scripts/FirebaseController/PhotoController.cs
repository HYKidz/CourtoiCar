using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoController : MonoBehaviour
{
    [SerializeField] private RawImage[] _images; 
    private int _counter =0;
    private Dictionary<int,byte[]>_dicpic{get; set;}  = new Dictionary<int, byte[]>();
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

        if(_dicpic.ContainsKey(_counter))
        {
            _dicpic[_counter] = picture;
        }
        else _dicpic.Add(_counter, picture);
        if(_counter >=_images.Length -1)
        {
            _counter =0;
        }else _counter++;
    }
}
