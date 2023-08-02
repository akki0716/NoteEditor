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
    /// �`��p�̃e�N�X�`���̕�
    /// ��������B�`����image���c�ɂ�����c���ɂȂ�
    /// </summary>
    [SerializeField]
    int imageWidth;

    [SerializeField]
    float[] samples;

    Texture2D texture;
    
    /// <summary>
    /// �����̏ꍇ�ł��Œ���`�悳��镝
    /// </summary>
    float minLineSize = 0.008f;

    /// <summary>
    /// �`�悷��T���v�����O���[�g�̊Ԋu
    /// �Ⴍ����ƍׂ����Ȃ邪���Â炭�Ȃ�A��������ƍr���Ȃ邪���₷���Ȃ�͂�
    /// </summary>
    int skipSamples = 200;

    void Start()
    {
        //�g�`�`��p�̃e�N�X�`���𐶐�
        texture = new Texture2D(imageWidth, 1);
        //�쐬�����e�N�X�`���̑S�Ẵs�N�Z���𓧖��ŏ�����
        texture.SetPixels(Enumerable.Range(0, imageWidth).Select(_ => Color.clear).ToArray());
        texture.Apply();
        image.texture = texture;
    }

    void Update()
    {
        //�I�[�f�B�I�\�[�X����g�`�T���v����samples�Ɋi�[
        audioSource.clip.GetData(samples, audioSource.timeSamples);

        int textureX = 0;
        float sample = minLineSize;

        //�`�惁�C������
        for (int i = 0, l = samples.Length; i < l && textureX < imageWidth; i++)
        {
            //�g�`�̔g�̕������擾
            sample = Mathf.Max(sample, samples[i]);

            //skipSamples�œK�x�ɊԈ���
            if (i % skipSamples == 0)
            {
                texture.SetPixel(textureX, 0, new Color(sample, 0, 0));
                //Debug.Log(maxSample);
                //�ŏ��l�ŏ㏑�����邱�ƂŎ����[�v��samples��0�Ȃ�min�ɂȂ�
                sample = minLineSize;
                textureX++;
            }
        }

        texture.Apply();
    }
}
