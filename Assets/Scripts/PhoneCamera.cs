using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PhoneCamera : MonoBehaviour
{
    private bool camAvailable;
    private WebCamTexture backCam;
    private Texture defaultBackground;
    [SerializeField] private RawImage background;
    [SerializeField] private AspectRatioFitter fit;
    [SerializeField] private PhotoController _photoController;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        defaultBackground = background.texture;
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            Debug.Log("No cam detected");
            camAvailable = false;
            return;
        }

        for (int i = 0; i < devices.Length; i++)
        {
            if (!devices[i].isFrontFacing)
            {
                backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
            Debug.Log(devices[i].name + i);
        }

        if (backCam == null)
        {
            Debug.Log("unable to find back cam");
            //hack pour use ma cam
            backCam = new WebCamTexture(devices[0].name, Screen.width, Screen.height);
            // return;
        }
        backCam.Play();
        background.texture = backCam;

        camAvailable = true;
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (!camAvailable) return;

        float ratio = (float)backCam.width / (float)backCam.height;
        fit.aspectRatio = ratio;

        float scaleY = backCam.videoVerticallyMirrored ? -1f : 1f;
        background.rectTransform.localScale = new Vector3(1f / ratio, scaleY, 1f / ratio);

        int orien = -backCam.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orien);
    }

    public void Photo()
    {
        // Debug.Log(Application.dataPath + "/../SaveImages/");

        Texture2D tex = new Texture2D(backCam.width, backCam.height, TextureFormat.RGB24, false);
        tex.SetPixels32(backCam.GetPixels32());

        // backCam.Stop();

        Debug.Log(tex);

        byte[] imgByte = tex.EncodeToPNG();
        _photoController.NewPicture(imgByte, backCam.width, backCam.height);// _photoController.NewPicture(tex);



        //now we must send to Firebase !!!
        // var dirPath = Application.dataPath + "/SaveImages/";
        //              if(!Directory.Exists(dirPath)) {
        //                  Directory.CreateDirectory(dirPath);
        //              }
        //              File.WriteAllBytes(dirPath + "Image" + ".png", imgByte);
        // File.WriteAllBytes(Path.Combine(Application.persistentDataPath + "/SaveImages/Exwample.JPG"), imgByte);
    }
    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        backCam.Stop();
    }

}
