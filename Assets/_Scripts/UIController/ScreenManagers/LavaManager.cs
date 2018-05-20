using UnityEngine;

public class LavaManager : MonoBehaviour 
{
	public static LavaManager singleton;
	public enum LavaScreen{Desktop = (1<<1), Mobile = (1<<2)};

	[SerializeField]private GameObject[] prefabs;
	[SerializeField]private uint maxBlobs;
	private LavaBehaviour[] pool;

	private int width;
	private int height;

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
	}

	public void SetScreen(LavaScreen screenType)
	{
		this.width = 640;
		this.height = ((screenType&LavaScreen.Desktop) != 0) ? -575 : 2045;
	}

	public void StartPlaying(Transform screen)
	{
		for(int i = 0; i < this.maxBlobs; i++)
		{
			LavaBehaviour script = this.pool[i];
			GameObject tmp = script.gameObject;
			tmp.transform.SetParent(screen);
			Vector2 pos = new Vector2(0.0f, 0.0f);
			pos.x = Random.Range(-this.width, this.width);
			pos.y = this.height;
			script.ResetPos = new Vector3(pos.x, -575.0f, -1.0f);
			tmp.transform.localScale = new Vector3(100, 100, 100);
			tmp.SetActive(true);
		}
	}

	public void Stop()
	{
		for(int i = 0; i < this.maxBlobs; i++)
		{
			LavaBehaviour script = this.pool[i];
			GameObject tmp = script.gameObject;
			tmp.SetActive(false);
		}
	}
}
