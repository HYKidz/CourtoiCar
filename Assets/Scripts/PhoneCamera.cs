using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneCamera : MonoBehaviour
{
    private bool camAvailable;
    private WebCamTexture backCam;
    private Texture defaultBackground;
    public RawImage background;
    public AspectRatioFitter fit;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        defaultBackground = background.texture;
        WebCamDevice[] devices = WebCamTexture.devices;

        if(devices.Length == 0)
        {
            Debug.Log("No cam detected");
            camAvailable = false;
            return;
        }

        for(int i = 0; i< devices.Length; i++)
        {
            if(!devices[i].isFrontFacing)
            {
                backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
        }

        if(backCam ==null)
        {
            Debug.Log("unable to find back cam");
            return;
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
    if(!camAvailable) return;

    float ratio = (float)backCam.width/(float) backCam.height;
    fit.aspectRatio = ratio;

    float scaleY = backCam.videoVerticallyMirrored ? -1f:1f;
    background.rectTransform.localScale = new Vector3(1f*ratio, scaleY*ratio, 1f*ratio);

    int orien = -backCam.videoRotationAngle;
    background.rectTransform.localEulerAngles = new Vector3(0,0,orien);
}
}
