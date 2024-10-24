using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�I�[�f�B�I�Ǘ��X�N���v�g
public class SoundManager : MonoBehaviour
{
    public  static SoundManager Instance;

    void Awake()
    {
        // ���łɃC���X�^���X�����݂���ꍇ�́A�������I��
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        // ���݂�BGM�p��SE�p��SE���[�v�Đ��p3�̃I�[�f�B�I�\�[�X����
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
        // �T�E���h�̐ݒ�
        bgmAudioSource.loop = true;
        seAudioSource.loop = true;
        seLoopAudioSource.loop = true;

        // �V���O���g���̃C���X�^���X���Z�b�g
        Instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    // �I�[�f�B�I�\�[�X
    AudioSource bgmAudioSource; //BGM�p
    AudioSource seAudioSource; //SE�p
    AudioSource seLoopAudioSource; //SE���[�v�p

    // BGM
    [SerializeField] List<AudioClip> audioClipsBGM;
    // SE
    [SerializeField] List<AudioClip> audioClipsSE;

    // BGM�Đ�
    public void PlayBGM(int index)
    {
        if (audioClipsBGM[index] != null)
        {
            bgmAudioSource.clip = audioClipsBGM[index];
            bgmAudioSource.loop = true;
            bgmAudioSource.Play();
        }
    }

    // SE�Đ�
    public void PlaySE(int index)
    {
        seAudioSource.PlayOneShot(audioClipsSE[index]);
    }

    //SE���[�v�Đ�
    public void PlaySELoop(int index)
    {
        seLoopAudioSource.loop = true;
        seLoopAudioSource.clip = audioClipsSE[index];
        seLoopAudioSource.Play();
    }
    //BGM��~
    public void StopBGM()
    {
        if (bgmAudioSource.isPlaying)
        {
            bgmAudioSource.Stop(); // BGM���~����
        }
        bgmAudioSource.loop = false; // ���[�v����������
    }
    //SE���[�v��~
    public void StopSELoop()
    {
        if (seLoopAudioSource.isPlaying)
        {
            seLoopAudioSource.loop = false;
            seLoopAudioSource.Stop();
        }
    }
    //��肭�؂�ւ��Ȃ��΍�
    public IEnumerator SwitchBGM(int newBGMIndex)
    {
        SoundManager.Instance.StopBGM();
        yield return new WaitForSeconds(0.1f); // �����ҋ@
        SoundManager.Instance.PlayBGM(newBGMIndex);
    }

    public IEnumerator StopAndSwitchSE(int newSEIndex)
    {
        // SE�̒�~����
        if (seLoopAudioSource.isPlaying)
        {
            yield return StartCoroutine(StopSELoopCor());
        }

        // BGM�̒�~����
        if (bgmAudioSource.isPlaying)
        {
            yield return StartCoroutine(StopBGMCor());
        }

        yield return new WaitForSeconds(0.2f); // �����ҋ@
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
