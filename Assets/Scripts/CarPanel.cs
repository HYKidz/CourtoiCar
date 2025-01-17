using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Extensions;
using Firebase.Firestore;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CarPanel : MonoBehaviour, IPointerDownHandler
{
    //a changer plus tard pour que sa sois selon le user
    [SerializeField] private string _carPath = "car_list";
    [SerializeField] private string _clientPath ="client_list";
    [SerializeField] private TMP_Text _marqueText;
    public class MyStringObjectEvent : UnityEvent<string>
    {

    }
    public MyStringObjectEvent _click = new MyStringObjectEvent();
    private string _marque;
    public string Marque
    {
        get => _marque; set
        {
            if (value == _marque) return;
            _marque = value;
            _marqueText.text = $"Marque:{value}";
        }
    }
    private bool _dispo;
    public bool Dispo
    {
        get => _dispo; set
        {
            _dispo = value;
            if (_dispo) _fond.color = Color.green;
            else
            {
                //iz not good
                CallClient();
                _fond.color = Color.red;

            }
        }
    }

    private async void CallClient()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        Query clientquery = db.Collection(_clientPath);
        await clientquery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot allClientSnapshot = task.Result;
            foreach (DocumentSnapshot item in allClientSnapshot.Documents)
            {
                Dictionary<string, object> client = item.ToDictionary();
                // Debug.Log(_ID);
                // Debug.Log(client["CarId"].ToString()==_ID);
                if (client.ContainsKey("CarId"))
                {

                    if (client["CarId"].ToString() == _ID)
                    {
                        Debug.Log("yis is it");
                        // _clientText.GetComponent<GameObject>().SetActive(true);
                        Clienttxt = client["Nom"].ToString();
                    }
                }

            }
        });
    }

    [SerializeField] private TMP_Text _plaqueText;
    private string _plaque;
    public string Plaque
    {
        get => _plaque; set
        {
            if (value == _plaque) return;
            _plaque = value;
            _plaqueText.text = $"Plaque:{value}";
        }
    }
    [SerializeField] private TMP_Text _serieText;
    private string _serie;
    public string Serie
    {
        get => _serie; set
        {
            if (value == _serie) return;
            _serie = value;
            _serieText.text = $"Serie:{value}";
        }
    }
    [SerializeField] private TMP_Text _anneText;
    private string _anne;
    public string Anne
    {
        get => _anne; set
        {
            if (value == _anne) return;
            _anne = value;
            _anneText.text = $"Anne:{value}";
        }
    }
    [SerializeField] private TMP_Text _infoText;
    private string _info;
    public string Info
    {
        get => _info; set
        {
            if (value == _info) return;
            _info = value;
            _infoText.text = $"Info:{value}";
        }
    }
    [SerializeField] private TMP_Text _clientText;
    private string _clienttxt;
    public string Clienttxt
    {
        get => _clienttxt; set
        {
            if (value == _clienttxt) return;
            _clienttxt = value;
            _clientText.text = $"Client:{value}";
        }
    }

    private string _ID;
    public string ID { get => _ID; set => _ID = value; }

    [SerializeField] private GameObject _delete;
    [SerializeField] private GameObject _edit;
    [SerializeField] private GameObject _picture;
    [Range(250, 400)][SerializeField] private int _maxX;
    [Range(250, 400)][SerializeField] private int _maxY;
    private int _minX;
    private int _minY;
    private List<GameObject> _pic = new List<GameObject>();
    private GridLayoutGroup _grid;
    private Image _fond;
    private SetClientData _clientGest;
    public SetClientData ClientGest { get => _clientGest; set => _clientGest = value; }
    private GameObject _client;
    public GameObject Client { get => _client; set => _client = value; }
    private GameObject _car;
    public GameObject Car { get => _car; set => _car = value; }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Awake()
    {
        _fond = GetComponent<Image>();
        _grid = GetComponent<GridLayoutGroup>();
        _minX = (int)_grid.cellSize.x;
        _minY = (int)_grid.cellSize.y;
    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    // void Start()
    // {
    //     _clientGest = _client.GetComponent<SetClientData>();

    // }

    public void OnPointerDown(PointerEventData data)
    {
        _click.Invoke(_ID);
        _delete.SetActive(!_delete.activeInHierarchy);
        _edit.SetActive(!_edit.activeInHierarchy);
        foreach (GameObject pic in _pic)
        {
            pic.SetActive(_delete.activeInHierarchy);
        }
        if (_delete.activeInHierarchy)
        {
            _grid.cellSize = new Vector2(_maxX, _maxY);
        }
        else
        {
            _grid.cellSize = new Vector2(_minX, _minY);
        }
    }

    public void Delete()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference carRef = db.Collection(_carPath).Document(_ID);
        carRef.DeleteAsync();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log(SceneManager.GetActiveScene().name);
    }
    public void AddPic(Texture2D pic)
    {
        GameObject aPic = Instantiate(_picture, gameObject.transform.position, quaternion.identity);
        _pic.Add(aPic);
        aPic.SetActive(false);
        aPic.transform.SetParent(gameObject.transform);
        aPic.transform.localScale = Vector3.one;
        RawImage image = aPic.GetComponent<RawImage>();
        image.texture = pic;
    }
    public void Edit()
    {
        _client.GetComponent<SetClientData>().IdCar = _ID;
        _car.SetActive(!_car.activeInHierarchy);
        _client.SetActive(!_client.activeInHierarchy);
        // SceneManager.LoadScene("Edit");
    }
    // public async void Retour()
    // {
    //     FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
    //     DocumentReference clientRef = db.Collection(_clientPath).Document(_ID);
    //     Dictionary<string, object> updateClient = new Dictionary<string, object>
    //     {
    //         // {"Hi", {}}
    //         {
    //             "Hierarchy", ($"Empreint du char{_plaque}", (_timeGet , DateTime.Now ))
    //         }
    //     };
    //     await clientRef.UpdateAsync(updateClient);
    //     //gerer retour de voiture
    // }
}
