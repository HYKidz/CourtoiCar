using System.Collections;
using System.Collections.Generic;
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

    private string _ID;
    public string ID { get => _ID; set => _ID = value; }

    [SerializeField] private GameObject _delete;
    [SerializeField] private GameObject _edit;
    [SerializeField] private GameObject _picture;
    private List<GameObject> _pic = new List<GameObject>();

    public void OnPointerDown(PointerEventData data)
    {
        _click.Invoke(_ID);
        _delete.SetActive(!_delete.activeInHierarchy);
        _edit.SetActive(!_edit.activeInHierarchy);
        foreach (GameObject pic in _pic)
        {
            pic.SetActive(_delete.activeInHierarchy);
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
        Debug.Log("edit");
    }
}
