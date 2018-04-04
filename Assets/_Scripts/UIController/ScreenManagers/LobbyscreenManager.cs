using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

public class LobbyscreenManager : UIManager 
{
    public static LobbyscreenManager singleton;

    [SerializeField] private GameObject iconPrefab;
    [SerializeField] private GameObject wheel;
    [SerializeField] private GameObject[] icons;

    private void Awake()
    {
        if(singleton != null && singleton != this)
            Destroy(this.gameObject);
        singleton = this;
    }

    private void Start() 
    {
        this.icons = new GameObject[Data.MAX_PLAYERS];
        this.CreateIconHolders();
    }

    private void CreateIconHolders()
    {
        byte len = Data.MAX_PLAYERS;
        float maxItemSize = MathHelper.GetDegreesPerSegment(len);

        for(byte i = 0; i < len; i++)
        {
            GameObject iconHolder = GameObject.Instantiate(this.iconPrefab);
            iconHolder.transform.SetParent(wheel.transform);

            Vector3 pos = MathHelper.PlaceOnCircle(wheel.transform.position, 220, maxItemSize*i);
            iconHolder.transform.localPosition = pos;
            iconHolder.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            this.icons[i] = iconHolder;
        }
    }

    protected override void OnScreenEnabled()
    {
        
    }

    protected override void OnScreenDisabled()
    {

    }
}
