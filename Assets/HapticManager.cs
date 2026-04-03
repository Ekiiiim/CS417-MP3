using UnityEngine;
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
    }

    private void SendHaptic(HapticImpulsePlayer controller, float amplitude, float duration)
    {
        if (controller != null)
        {
            controller.SendHapticImpulse(amplitude, duration);
        }
    }
}
