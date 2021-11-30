using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundMgr : SingletonMono<SoundMgr>
{
    public AudioClip[] audioClips;
    //Idx:0 = Ÿ��Ʋ
    //Idx:1 = ��Ʈ��
    //Idx:2 = �������� 1, 2
    //Idx:3 = ����

    public float BgmVolume = 0.5f; //�����
    public float FxVolume = 0.5f; //ȿ����

    public string SceneName;

    bool VolumeDown = false;

    // Start is called before the first frame update
    void Start()
    {
        SceneName = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(VolumeDown == false)
            this.gameObject.GetComponent<AudioSource>().volume = BgmVolume;

        if (SceneName != SceneManager.GetActiveScene().name)
        {
            SceneName = SceneManager.GetActiveScene().name;

            if (SceneName == "Title")
            {
                VolumeDown = false;
                this.gameObject.GetComponent<AudioSource>().clip = audioClips[0];
                this.gameObject.GetComponent<AudioSource>().Play();
            }

            else if (SceneName == "Intro")
            {
                this.gameObject.GetComponent<AudioSource>().clip = audioClips[1];
                this.gameObject.GetComponent<AudioSource>().Play();
            }

            else if (SceneName == "Stage1" || SceneName == "Stage2")
            {
                VolumeDown = false;
                this.gameObject.GetComponent<AudioSource>().clip = audioClips[2];
                this.gameObject.GetComponent<AudioSource>().Play();
            }

            else if (SceneName == "Ending")
            {
                VolumeDown = false;
                this.gameObject.GetComponent<AudioSource>().clip = audioClips[3];
                this.gameObject.GetComponent<AudioSource>().Play();
            }

            //else if (SceneName == "Sungbum") �����
            //{
            //    Debug.Log("hi");
            //    this.gameObject.GetComponent<AudioSource>().clip = audioClips[3];
            //    this.gameObject.GetComponent<AudioSource>().Play();
            //}
        }

        if (SceneName == "Intro")
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Debug.Log("d");
                VolumeDown = true;
            }

            if (VolumeDown == true)
            {
                VlDown();
            }
        }
    }

    void VlDown()
    {
        if(this.gameObject.GetComponent<AudioSource>().volume >= 0)
        {
            Debug.Log("Hehe");
            this.gameObject.GetComponent<AudioSource>().volume -= Time.deltaTime * 1;
        }
    }
}
