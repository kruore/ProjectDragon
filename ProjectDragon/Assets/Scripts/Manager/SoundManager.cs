using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoSingleton<SoundManager>
{
    public AudioSource Ds_efxSource;//효과음
    public AudioSource Ds_musicSource;//배경음악
    public Slider Ds_soundslider;//옵션.사운드조절바
    public float Ds_volumeRange = 0.5f;//현재사운드 크기

    /// <summary>
    /// 초기의 배경음과 이펙스재생을 할 오디오 소스 생성
    /// </summary>
    private void Awake()
    {
        Ds_efxSource = gameObject.AddComponent<AudioSource>();
        Ds_efxSource.loop = false;
        Ds_musicSource = gameObject.AddComponent<AudioSource>();
        Ds_musicSource.loop = true;

    }
    /// <summary>
    /// 배경음 재생 SoundManager.Inst.Ds_BgmPlayer(Audio ***);
    /// </summary>
    /// <param name="clip"></param>
    public void Ds_BgmPlayer(AudioClip _clip)
    {
        Ds_musicSource.clip = _clip;
        Ds_musicSource.Play();
        Ds_musicSource.volume = Ds_volumeRange;
    }
    /// <summary>
    /// 하나의 효과음재생 SoundManager.Inst.PlaySingle(Audio ***);
    /// </summary>
    /// <param name="clip"></param>
    public void Ds_PlaySingle(AudioClip _clip)
    {
        Ds_efxSource.clip = _clip;
        Ds_efxSource.Play();
        Ds_efxSource.volume = Ds_volumeRange;
    }
    /// <summary>
    /// 여러개의 효과음중 하나만 재생하고 싶을때 SoundManager.Inst.RandomizeSfx(Audio***,*** ...);
    /// </summary>
    /// <param name="clips">랜덤하게 재생할 오디오클립의 배열</param>
    public void Ds_RandomizeSfx(params AudioClip[] _clips)
    {
        int randomIndex = Random.Range(0, _clips.Length);
        Ds_efxSource.clip = _clips[randomIndex];
        Ds_efxSource.Play();
        Ds_efxSource.volume = Ds_volumeRange;

    }
    // Use this for initialization

    public void Ds_SoundController()//슬라이더가 움직일 때마다 호출되어 사운드의 조절을 해준다.
    {
        Debug.Log(Ds_soundslider.value);
        Ds_volumeRange = Ds_soundslider.value;
        Debug.Log(Ds_volumeRange);
        Ds_musicSource.volume = Ds_volumeRange;
        Ds_efxSource.volume = Ds_volumeRange;
    }
    public void SoundInit()//설정 버튼을 눌렀을 시 설정창의 슬라이더를 현재의 볼륨과 동기화 시켜준다.
    {

        Ds_soundslider = GameObject.Find("SoundSlider").GetComponent<Slider>();
        Ds_soundslider.value = Ds_volumeRange;
        Debug.Log(Ds_soundslider);
    }
    public void WalkSound(AudioClip _clip,State state)
    {
        if(state == State.Walk)
        {
            Ds_efxSource.clip = _clip;
            Ds_efxSource.Play();
            Ds_efxSource.volume = Ds_volumeRange;
        }
        Ds_efxSource.Stop();
    }
}
