using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// https://setchi.hatenablog.com/entry/2015/10/25/220844
/// </summary>
public class WaveRenderer : MonoBehaviour
{
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    RawImage image;

    /// <summary>
    /// 描画用のテクスチャの幅
    /// 横方向基準。描画先のimageを縦にしたら縦幅になる
    /// </summary>
    [SerializeField]
    int imageWidth;

    [SerializeField]
    float[] samples;

    Texture2D texture;
    
    /// <summary>
    /// 無音の場合でも最低限描画される幅
    /// </summary>
    float minLineSize = 0.008f;

    /// <summary>
    /// 描画するサンプリングレートの間隔
    /// 低くすると細かくなるが見づらくなり、高くすると荒くなるが見やすくなるはず
    /// </summary>
    int skipSamples = 200;

    void Start()
    {
        //波形描画用のテクスチャを生成
        texture = new Texture2D(imageWidth, 1);
        //作成したテクスチャの全てのピクセルを透明で初期化
        texture.SetPixels(Enumerable.Range(0, imageWidth).Select(_ => Color.clear).ToArray());
        texture.Apply();
        image.texture = texture;
    }

    void Update()
    {
        //オーディオソースから波形サンプルをsamplesに格納
        audioSource.clip.GetData(samples, audioSource.timeSamples);

        int textureX = 0;
        float sample = minLineSize;

        //描画メイン処理
        for (int i = 0, l = samples.Length; i < l && textureX < imageWidth; i++)
        {
            //波形の波の部分を取得
            sample = Mathf.Max(sample, samples[i]);

            //skipSamplesで適度に間引き
            if (i % skipSamples == 0)
            {
                texture.SetPixel(textureX, 0, new Color(sample, 0, 0));
                //Debug.Log(maxSample);
                //最小値で上書きすることで次ループでsamplesが0ならminになる
                sample = minLineSize;
                textureX++;
            }
        }

        texture.Apply();
    }
}
