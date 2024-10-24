using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//オーディオ管理スクリプト
public class SoundManager : MonoBehaviour
{
    public  static SoundManager Instance;

    void Awake()
    {
        // すでにインスタンスが存在する場合は、処理を終了
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        // 現在はBGM用とSE用とSEループ再生用3つのオーディオソース生成
        AudioSource[] audioSources = GetComponents<AudioSource>();
        if (audioSources.Length >= 3)
        {
            bgmAudioSource = audioSources[0];
            seAudioSource = audioSources[1];
            seLoopAudioSource = audioSources[2];
        }
        else
        {
            return;
        }
        // サウンドの設定
        bgmAudioSource.loop = true;
        seAudioSource.loop = true;
        seLoopAudioSource.loop = true;

        // シングルトンのインスタンスをセット
        Instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    // オーディオソース
    AudioSource bgmAudioSource; //BGM用
    AudioSource seAudioSource; //SE用
    AudioSource seLoopAudioSource; //SEループ用

    // BGM
    [SerializeField] List<AudioClip> audioClipsBGM;
    // SE
    [SerializeField] List<AudioClip> audioClipsSE;

    // BGM再生
    public void PlayBGM(int index)
    {
        if (audioClipsBGM[index] != null)
        {
            bgmAudioSource.clip = audioClipsBGM[index];
            bgmAudioSource.loop = true;
            bgmAudioSource.Play();
        }
    }

    // SE再生
    public void PlaySE(int index)
    {
        seAudioSource.PlayOneShot(audioClipsSE[index]);
    }

    //SEループ再生
    public void PlaySELoop(int index)
    {
        seLoopAudioSource.loop = true;
        seLoopAudioSource.clip = audioClipsSE[index];
        seLoopAudioSource.Play();
    }
    //BGM停止
    public void StopBGM()
    {
        if (bgmAudioSource.isPlaying)
        {
            bgmAudioSource.Stop(); // BGMを停止する
        }
        bgmAudioSource.loop = false; // ループも解除する
    }
    //SEループ停止
    public void StopSELoop()
    {
        if (seLoopAudioSource.isPlaying)
        {
            seLoopAudioSource.loop = false;
            seLoopAudioSource.Stop();
        }
    }
    //上手く切り替わらない対策
    public IEnumerator SwitchBGM(int newBGMIndex)
    {
        SoundManager.Instance.StopBGM();
        yield return new WaitForSeconds(0.1f); // 少し待機
        SoundManager.Instance.PlayBGM(newBGMIndex);
    }

    public IEnumerator StopAndSwitchSE(int newSEIndex)
    {
        // SEの停止処理
        if (seLoopAudioSource.isPlaying)
        {
            yield return StartCoroutine(StopSELoopCor());
        }

        // BGMの停止処理
        if (bgmAudioSource.isPlaying)
        {
            yield return StartCoroutine(StopBGMCor());
        }

        yield return new WaitForSeconds(0.2f); // 少し待機
        SoundManager.Instance.PlaySE(newSEIndex);
    }

    private IEnumerator StopSELoopCor()
    {
        seLoopAudioSource.loop = false;
        SoundManager.Instance.StopSELoop();
        yield return new WaitUntil(() => !seLoopAudioSource.isPlaying);
    }

    private IEnumerator StopBGMCor()
    {
        SoundManager.Instance.StopBGM();
        yield return new WaitUntil(() => !bgmAudioSource.isPlaying);
    }
}
