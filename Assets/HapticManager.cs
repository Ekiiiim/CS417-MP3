using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class HapticManager : MonoBehaviour
{
    public HapticImpulsePlayer leftController;
    public HapticImpulsePlayer rightController;

    public void OnCropClick()
    {
        SendHaptic(rightController, 0.5f, 0.1f);
    }

    public void OnBuildingUnlock()
    {
        SendHaptic(leftController, 0.6f, 0.1f);
    }

    public void OnPowerUp()
    {
        SendHaptic(leftController, 0.75f, 0.2f);
    }

    public void OnNewRegionUnlock()
    {
        SendHaptic(rightController, 0.8f, 0.3f);
        SendHaptic(leftController, 0.8f, 0.3f);
    }

    public void OnCoffeeShopUnlock()
    {
        SendHaptic(rightController, 0.8f, 0.3f);
    }

    public void OnSupermarketUnlock()
    {
        SendHaptic(rightController, 0.8f, 0.3f);
    }

    public void OnStadiumUnlock()
    {
        SendHaptic(rightController, 0.8f, 0.3f);
    }

    public void OnCropPowerUpActivate()
    {
        SendDoubleHaptic(0.7f, 0.1f);
    }

    private IEnumerator SendDoubleHaptic(float amplitude, float duration)
    {
        SendHaptic(rightController, amplitude, duration);
        SendHaptic(leftController, amplitude, duration);
        yield return new WaitForSeconds(0.1f);
        SendHaptic(rightController, amplitude, duration);
        SendHaptic(leftController, amplitude, duration);
    }

    private void SendHaptic(HapticImpulsePlayer controller, float amplitude, float duration)
    {
        if (controller != null)
        {
            controller.SendHapticImpulse(amplitude, duration);
        }
    }
}
