using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UltimateClean;

public class HealthSlider : MonoBehaviour {

    private Image image;
    private SlicedFilledImage slicedImage;

    private void Start() {
        image = GetComponent<Image>();
        slicedImage = GetComponent<SlicedFilledImage>();
    }

    public void setHealthSlider(float current, float max) {
        float amountFilled = current / max;
        if (image != null)
            image.fillAmount = amountFilled;
        else if (slicedImage != null)
            slicedImage.fillAmount = amountFilled;
    }
}