using UnityEngine;

public class LavaManager : MonoBehaviour 
{
	public static LavaManager singleton;
	public enum LavaScreen{Desktop = (1<<1), Mobile = (1<<2)};

	[SerializeField]private GameObject[] prefabs;
	[SerializeField]private GameObject bottom;
	[SerializeField]private uint maxBlobs;
	private LavaBehaviour[] pool;

	private int maxX;
	private int startY;
	private LavaScreen type;

	private void Awake()
	{
		if(singleton != null && singleton != this)
			Destroy(this);
		singleton = this;
	}

	private void Start()
	{
		this.pool = new LavaBehaviour[this.maxBlobs];
		for(int i = 0; i < this.maxBlobs; i++)
		{
			GameObject tmp = GameObject.Instantiate(this.prefabs[Random.Range(0, this.prefabs.Length)]);
			tmp.transform.position = new Vector2(0.0f, 0.0f);
			tmp.SetActive(false);
			
			LavaBehaviour script = tmp.GetComponent<LavaBehaviour>();
			this.pool[i] = script;
		}

		this.bottom.SetActive(false);
	}

	public void SetScreen(LavaScreen screenType)
	{
		this.type = screenType;
		this.maxX = ((screenType&LavaScreen.Desktop) != 0) ? 640 : 320;
		this.startY = ((screenType&LavaScreen.Desktop) != 0) ? -575 : -2045;
	}

	public void StartPlaying(Transform screen)
	{
		for(int i = 0; i < this.maxBlobs; i++)
		{
			LavaBehaviour script = this.pool[i];

			GameObject tmp = script.gameObject;
			tmp.transform.SetParent(screen);

			Vector2 pos = new Vector2(0.0f, 0.0f);
			pos.x = Random.Range(-this.maxX, this.maxX);
			pos.y = this.startY;

			script.MaxX = this.maxX;
			script.StartY = this.startY;

			tmp.transform.localScale = new Vector3(100, 100, 100);
			tmp.SetActive(true);
		}

		this.bottom.transform.SetParent(screen);
		this.bottom.transform.localPosition = ((this.type&LavaScreen.Desktop) != 0) ? new Vector3(0, -240, -1) : new Vector3(0, -800, -1);
		this.bottom.SetActive(true);
	}

	public void Stop()
	{
		if(this.pool == null)
			return;
		
		for(int i = 0; i < this.maxBlobs; i++)
		{
			LavaBehaviour script = this.pool[i];
			GameObject tmp = script.gameObject;
			tmp.SetActive(false);
		}

		this.bottom.SetActive(false);
	}
}
