using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LinesDrawer : MonoBehaviour {

	public GameObject linePrefab;
	public LayerMask cantDrawOverLayer;
	int cantDrawOverLayerIndex;

	[Space ( 30f )]
	public Gradient lineColor;
	public float linePointsMinDistance;
	public float lineWidth;

	Line currentLine;
	Camera cam;

	public static bool alreadyDraw;
	public static Action sub;
	AudioManager audioManagerInstance;
	static int mm;

	void Start ( ) {
		cam = Camera.main;
		cantDrawOverLayerIndex = LayerMask.NameToLayer ( "CantDrawOver" );
		alreadyDraw = false;
	}

	void Update ( ) {
		if ( Input.GetMouseButtonDown ( 0 ) && !alreadyDraw)
			BeginDraw ( );

		if ( currentLine != null )
			Draw ( );

		if ( Input.GetMouseButtonUp ( 0 ) )
			EndDraw ( );
		//HalpMe.halpwant2 = false;
	}

	// Begin Draw ----------------------------------------------
	void BeginDraw ( ) {
		currentLine = Instantiate ( linePrefab, this.transform ).GetComponent <Line> ( );

		//Set line properties
		currentLine.UsePhysics ( false );
		currentLine.SetLineColor ( lineColor );
		currentLine.SetPointsMinDistance ( linePointsMinDistance );
		currentLine.SetLineWidth ( lineWidth );
		if (audioManagerInstance != null)
		{
			FindObjectOfType<AudioManager>().Play("Click");
		}	

	}
	// Draw ----------------------------------------------------
	void Draw ( ) {
		Vector2 mousePosition = cam.ScreenToWorldPoint ( Input.mousePosition );

		//Check if mousePos hits any collider with layer "CantDrawOver", if true cut the line by calling EndDraw( )
		RaycastHit2D hit = Physics2D.CircleCast ( mousePosition, lineWidth / 3f, Vector2.zero, 1f, cantDrawOverLayer );

		if ( hit )
			
		EndDraw ( );
		else
			currentLine.AddPoint ( mousePosition );
	}
	// End Draw ------------------------------------------------
	void EndDraw ( )
	{
		if ( currentLine != null) {
			if ( currentLine.pointsCount < 2 ) {
				//If line has one point
				Destroy ( currentLine.gameObject );
			} else {
				//Add the line to "CantDrawOver" layer
				currentLine.gameObject.layer = cantDrawOverLayerIndex;

				//Activate Physics on the line
				currentLine.UsePhysics ( true );

				currentLine = null;
				alreadyDraw = true;
				if (audioManagerInstance != null)
				{
					FindObjectOfType<AudioManager>().Play("Click");
				}
				sub?.Invoke();
				HalpMe.halpwant2 = false;
			}
		}
	}
}
