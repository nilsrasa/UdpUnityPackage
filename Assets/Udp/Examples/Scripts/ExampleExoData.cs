using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Exo;
using TMPro;

namespace Example
{
    public class ExampleExoData : MonoBehaviour
    {
        public ExoUdpHost exoUdpHost;
        public TMP_Text elbowValueText;

        void Start()
        {
            //Subscribe to the OnElbowValue event
            exoUdpHost.OnElbowValue.AddListener(ReceiveExoData);
        }

        public void ReceiveExoData(ExoEntry entry)
        {
            //Set the UI text to the given value
            elbowValueText.text = entry.Value.ToString("0.0000");
        }
    }
}
