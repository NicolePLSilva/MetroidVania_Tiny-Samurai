using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    CinemachineVirtualCamera vcam;
    CinemachineBasicMultiChannelPerlin noise;
    void Start() {
            vcam = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera> ();
            noise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin> ();

        noise.m_AmplitudeGain = 0f;
        noise.m_FrequencyGain = 0f; 
    }
 
    public IEnumerator Noise(float amplitudeGain, float frequencyGain, float duration) {
            noise.m_AmplitudeGain = amplitudeGain;
            noise.m_FrequencyGain = frequencyGain;    
            yield return new WaitForSeconds(duration);
            noise.m_AmplitudeGain = 0f;
            noise.m_FrequencyGain = 0f;  
    } 
}
