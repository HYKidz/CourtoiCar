using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Extensions;
using Firebase.Firestore;
using Firebase.Storage;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class GetCarData : MonoBehaviour
{
    //a changer plus tard pour que sa sois selon le user
    [SerializeField] private string _carPath = "car_list";

    [SerializeField] private GameObject _carPanel;

    [SerializeField] private GridLayoutGroup _grid;
    private Dictionary<string, GameObject> _carActive = new Dictionary<string, GameObject>();
    [SerializeField] private float _minYCellSize;
    [SerializeField] private float _maxYCellSize;
    [SerializeField] private float _vitesseAnim;
    // [SerializeField] private GameObject _car;
    [SerializeField] private GameObject _client;
    private float _actualYCellSize;

    // private ListenerRegistration _listenerRegistration;
    void Start()
    {
        _actualYCellSize = _grid.cellSize.y;
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        Query carQuery = db.Collection(_carPath);
        carQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot allCarSnapshot = task.Result;
            Debug.Log(allCarSnapshot.Documents);
            foreach (DocumentSnapshot documentSnapshot in allCarSnapshot.Documents)
            {

                Debug.Log(String.Format("Document Data for {0} document:", documentSnapshot.Id));
                Dictionary<string, object> car = documentSnapshot.ToDictionary();
                if (_carPanel != null)
                {
                
                    GameObject GOCar = Instantiate(_carPanel, gameObject.transform.position, quaternion.identity);
                    _carActive.Add(documentSnapshot.Id, GOCar);
                    GOCar.transform.SetParent(gameObject.transform);
                    GOCar.transform.localScale = Vector3.one;
                    CarPanel panel = GOCar.GetComponent<CarPanel>();
                    panel.Marque = car["Marque"].ToString();
                    panel.Serie = car["Serie"].ToString();
                    panel.Plaque = car["Plaque"].ToString();
                    panel.Anne = car["Anne"].ToString();
                    if (car["Info"].ToString() != "") panel.Info = car["Info"].ToString();
                    else panel.Info = "Aucun information";
                    List<object> picture = car["Picture"] as List<object>;


                    // Debug.Log(picture.Count);
                    for (int i = 0; i < picture.Count; i++)
                    {
                        Trigger($"{car["Marque"]}/{picture[i]}.jpg", panel);
                        // Trigger($"{picture[i]}.jpg", panel);
                    }
                    // List<object> Boolean = car["Disponible"] as List<object>;
                    bool dispo = bool.Parse(car["Disponible"].ToString());
                    panel.Dispo = dispo;
                    panel.Car = gameObject;
                    panel.Client = _client;
                    panel.ID = documentSnapshot.Id;
                    panel._click.AddListener(Click);
                    // Debug.Log("");
                }
            }
        });
    }
    public void Trigger(string path, CarPanel panel)
    {
        // StartCoroutine(DownloadPic(path, panel));
        DownloadPic(path, panel);
    }


    // private IEnumerator DownloadPic(string path, CarPanel panel)
    private void DownloadPic(string path, CarPanel panel)
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        StorageReference screenshotReference = storage.GetReferenceFromUrl("gs://courtoicar-e37b1.appspot.com/");
      
        screenshotReference.Child(path).GetBytesAsync(long.MaxValue).ContinueWithOnMainThread((Task<byte[]> task) =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log(task.Exception.ToString());
            }
            else
            {
                var texture = new Texture2D(1024,1024);
                texture.LoadImage(task.Result);
                panel.AddPic(texture);

            }
        });

        // var downloadTask = screenshotReference.GetBytesAsync(long.MaxValue);
        // yield return new WaitUntil(()=>downloadTask.IsCompleted);

        // var texture = new Texture2D(2,2);
        // texture.LoadImage(downloadTask.Result);
        // panel.Pic.Add(texture);
    }
    void Click(string id)
    {
        foreach (KeyValuePair<string, GameObject> car in _carActive)
        {
            if (car.Key == id)
            {
                //Play anim and set info
                StartCoroutine(Anim());
                continue;
            }
            car.Value.SetActive(!car.Value.activeInHierarchy);
        }
    }
    //Meh
    private IEnumerator Anim()
    {
        float target = 0f;
        float lerp = 0f;
        if (_actualYCellSize != _minYCellSize) target = _minYCellSize;
        else target = _maxYCellSize;
        while (lerp <= 1)
        {

            lerp += Time.deltaTime / _vitesseAnim;
            // Debug.Log(lerp);
            _grid.cellSize = new Vector2(_grid.cellSize.x, (int)Mathf.Lerp(_grid.cellSize.y, target, lerp));
            _actualYCellSize = _grid.cellSize.y;
            yield return new WaitForEndOfFrame();
        }



    }

    // IEnumerator AnimSelect()
    // {
    //     float lerp = Time.deltaTime * _vitesseAnim;
    // _grid.cellSize.y = in

    // }


}
