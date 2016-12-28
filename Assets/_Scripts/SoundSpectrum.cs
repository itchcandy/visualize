using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSpectrum : MonoBehaviour 
{
    public FFTWindow windowType;
    public VisualizationStyle visualizationStyle;
    public GridLayoutGroup barLayoutGroup;
    public RectTransform barRect;
    public int channel;
    int sampleCount = 256;
    Image[] bars;
	AudioSource aud;
    float[] samples;
    float min = 10000, max = 0;
    delegate void Visualize();
    Visualize visualize;

    void Start()
    {
        aud = GetComponent<AudioSource>();
        samples = new float[sampleCount];
        switch(visualizationStyle){
            case VisualizationStyle.None:
                visualize = NoAction;
                break;
            case VisualizationStyle.Bars:
                visualize = VisualizeBars;
                break;
            case VisualizationStyle.Lines:
                visualize = VisualizeLines;
                break;
        }
        InitializeBars();
    }

    void OnValidate()
    {
        switch(visualizationStyle){
            case VisualizationStyle.None:
                visualize = NoAction;
                break;
            case VisualizationStyle.Bars:
                visualize = VisualizeBars;
                break;
            case VisualizationStyle.Lines:
                visualize = VisualizeLines;
                break;
        }
    }

    void Update()
    {
		AudioListener.GetSpectrumData( samples, channel, windowType );
        VisualizeBars();
		/*for( int i = 1; i < samples.Length-1; i++ )
		{
            if(max < samples[i])
                max = samples[i];
            if(min > samples[i])
                min = samples[i];
			//Debug.DrawLine( new Vector3( i - 1, samples[i] + 10, 0 ), new Vector3( i, samples[i + 1] + 10, 0 ), Color.red );
			//Debug.DrawLine( new Vector3( i - 1, Mathf.Log( samples[i - 1] ) + 10, 2 ), new Vector3( i, Mathf.Log( samples[i] ) + 10, 2 ), Color.cyan );
			//Debug.DrawLine( new Vector3( Mathf.Log( i - 1 ), samples[i - 1] - 10, 1 ), new Vector3( Mathf.Log( i ), samples[i] - 10, 1 ), Color.green );
			//Debug.DrawLine( new Vector3( Mathf.Log( i - 1 ), Mathf.Log( samples[i - 1] ), 3 ), new Vector3( Mathf.Log( i ), Mathf.Log( samples[i] ), 3 ), Color.blue );
		}//*/
    }

    void NoAction()
    {

    }

    void InitializeBars()
    {
        bars = new Image[sampleCount];
        barLayoutGroup.cellSize = new Vector2(barRect.rect.width/sampleCount, barLayoutGroup.cellSize.y);
        bars[0] = barLayoutGroup.transform.GetChild(0).GetComponent<Image>();
        for(int i = 1; i< sampleCount; i++){
            bars[i] = Instantiate<Image>(bars[0], barLayoutGroup.transform);
        }
    }

    void VisualizeBars()
    {
        for(int i=0; i< sampleCount; i++){
            bars[i].fillAmount = samples[i];
        }
    }

    void VisualizeLines()
    {

    }
}

public enum VisualizationStyle
{
    None,
    Lines,
    Bars
};