using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TensorFlowLite;

public class CameraController : MonoBehaviour
{
    private bool cameraAvailable;
    private WebCamTexture backCamera;
    private Texture defaultBackground;

    public RawImage background;
    public AspectRatioFitter fit;

    // Start is called before the first frame update
    void Start()
    {
        defaultBackground = background.texture;
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            Debug.Log("No camera detected");
            cameraAvailable = false;
            return;
        }

        for (int i = 0; i < devices.Length; i++)
        {
			backCamera = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
        }

        if (backCamera == null)
        {
            return;
        }

        backCamera.Play();
        background.texture = backCamera;

        cameraAvailable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!cameraAvailable)
        {
            return;
        }

        float ratio = (float)backCamera.width / (float)backCamera.height;
        fit.aspectRatio = ratio;

        float scaleY = backCamera.videoVerticallyMirrored ? -1f : 1f;
        background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        int orient = -backCamera.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
    }
}
