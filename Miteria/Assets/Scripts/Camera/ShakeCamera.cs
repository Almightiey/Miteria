using System.Collections;
using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    CinemachineVirtualCamera vcam;
    CinemachineBasicMultiChannelPerlin noise;
    public float AmplitudeGain;
    public float FrequencyGain;

    private void OnEnable()
    {
        Game.shakeCamera = this;
    }

    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        noise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public IEnumerator ProcessShake(float shakeIntensity = 5f, float shakeTiming = 0.5f)
    {
        Noise(1, shakeIntensity);
        yield return new WaitForSeconds(shakeTiming);
        Noise(0, 0);
    }

    public async void Shake(float shakeIntensity = 5f, int shakeTiming = 500)
    {
        Noise(1, shakeIntensity);
        await Task.Delay(shakeTiming);
        Noise(0, 0);
    }

    public void Noise(float amplitudeGain, float frequencyGain)
    {
        noise.m_AmplitudeGain = amplitudeGain;
        noise.m_FrequencyGain = frequencyGain;
    }
}
