using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

public class LobbyscreenManager : UIManager 
{
    public static LobbyscreenManager singleton;

    [SerializeField] private GameObject iconPrefab;
    [SerializeField] private GameObject wheel;
    [SerializeField]private List<GameObject> iconHolders;

    private void Awake()
    {
        if(singleton != null && singleton != this)
            Destroy(this.gameObject);
        singleton = this;
    }

    private void Start() 
    {
        this.iconHolders = new List<GameObject>(Data.MAX_PLAYERS);
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

            GameObject icon = iconHolder.transform.GetChild(1).gameObject;
            icon.SetActive(false);

            Vector3 pos = MathHelper.PlaceOnCircle(wheel.transform.position, 220, maxItemSize*i);
            iconHolder.transform.localPosition = pos;
            iconHolder.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            this.iconHolders.Add(iconHolder);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            this.ShowIcon();
    }

    public void ShowIcon()
    {
        if(this.iconHolders.Count <= 0)
            return;
        
        GameObject holder = this.iconHolders[0];    
        this.iconHolders.RemoveAt(0);

        GameObject textObject = holder.transform.GetChild(0).gameObject;
        Text text = textObject.GetComponent<Text>();
        text.text = NetworkPlayer.singleton.Name;
        text.color = Color.white;
        textObject.transform.localPosition = new Vector3(0.0f, 50.0f, 0.0f);
        
        Object[] icons = Data.PLAYER_ICONS;
        Sprite icon = (Sprite)icons[0];

        GameObject iconObject = holder.transform.GetChild(1).gameObject;
        iconObject.GetComponent<Image>().sprite = icon;
        iconObject.SetActive(true);
    }

    protected override void OnScreenEnabled()
    {
        
    }

    protected override void OnScreenDisabled()
    {

    }
}
