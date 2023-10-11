using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase.Storage;
using System;

public class SetCarData : MonoBehaviour
{
    [SerializeField] private string _carPath ="car_list";
    [SerializeField] private TMP_InputField _marqueField;
    [SerializeField] private TMP_InputField _plaqueField;
    [SerializeField] private TMP_InputField _serieField;

    [SerializeField] private TMP_InputField _anneField;
    [SerializeField] private TMP_InputField _infoField;

    [SerializeField] private Button _submitButton;
    [SerializeField] private PhotoController _photo;
    [SerializeField] private GameObject _cam;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        _submitButton.onClick.AddListener(async ()=>
        {
            Dictionary<string, object> car = new Dictionary<string, object>
            {
            {"Marque", _marqueField.text},
            {"Plaque", _plaqueField.text},
            {"Serie", _serieField.text},
            {"Anne", int.Parse(_anneField.text)},
            {"Info", _infoField.text},

            };
            //a voir cause fucked
           Dictionary<int, byte[]> dicpic=_photo.GetPicture();
           StartCoroutine(UploadCoroutine(dicpic));
            DocumentReference AddedDocRef = await db.Collection(_carPath).AddAsync(car);
        //    var carData = new CarData
        //    {
        //    };
        //    var firestore = FirebaseFirestore.DefaultInstance;
        //    firestore.Document(_carPath).SetAsync(carData);

           //pas la meilleur fason de l'appeler, mais work for now
        //    SceneManager.LoadScene("List");
        });
    }
    //might get scrapped
     private IEnumerator UploadCoroutine(Dictionary<int, byte[]> pic)
    {
        var storage = FirebaseStorage.DefaultInstance;
                Debug.Log("you");
        for (int i = 0; i < 3; i++)
        {
            if (!pic.ContainsKey(i))
            {
                Debug.Log("are");
                yield break;
            }
            else
            {
                // Debug.Log(Guid);
                var ScreenshotReference = storage.GetReference($"/photo/${Guid.NewGuid()}.jpg");
                var metadataChange = new MetadataChange()
                {
                    ContentEncoding = "image/png",
                    // CustomMetadata = 
                };
                var uploadTask = ScreenshotReference.PutBytesAsync(pic[i], metadataChange);
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
            }
        }
    }
    public void HandlePicture()
    {
        _cam.SetActive(!_cam.activeInHierarchy);
    }
}
