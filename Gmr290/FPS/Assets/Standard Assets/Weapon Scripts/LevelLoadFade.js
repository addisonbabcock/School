
static function FadeAndLoadLevel (level, fadeTexture : Texture2D, fadeLength : float)
{
	if (fadeTexture == null)
		FadeAndLoadLevel (level, Color.white, fadeLength);
	
	var fade = new GameObject ("Fade");
	fade.AddComponent (LevelLoadFade);
	fade.AddComponent (GUITexture);
	fade.transform.position = Vector3 (0.5, 0.5, 1000);
	fade.guiTexture.texture = fadeTexture;
	fade.GetComponent (LevelLoadFade).DoFade (level, fadeLength, false);
}

static function FadeAndLoadLevel (level, color : Color, fadeLength : float)
{
	var fadeTexture = new Texture2D (1, 1);
	fadeTexture.SetPixel (0, 0, color);
	fadeTexture.Apply ();
	
	var fade = new GameObject ("Fade");
	fade.AddComponent (LevelLoadFade);
	fade.AddComponent (GUITexture);
	fade.transform.position = Vector3 (0.5, 0.5, 1000);
	fade.guiTexture.texture = fadeTexture;
	
	DontDestroyOnLoad (fadeTexture);
	fade.GetComponent (LevelLoadFade).DoFade (level, fadeLength, true);
}

function DoFade (level, fadeLength : float, destroyTexture : boolean)
{
	DontDestroyOnLoad (gameObject);
	
	guiTexture.color.a = 0;
	var time = 0.0;
	while (time < fadeLength)
	{
		time += Time.deltaTime;
		guiTexture.color.a = Mathf.InverseLerp (fadeLength, 0.0, time);
		yield;
	}
	guiTexture.color.a = 0;
	yield;
	
	Destroy (gameObject);
	
	if (destroyTexture)
		Destroy (guiTexture.texture);
}
