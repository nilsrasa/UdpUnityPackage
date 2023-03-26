using System;
using System.Globalization;
using Udp;
using UnityEngine.Events;

namespace Exo
{
    public class ExoUdpHost : UdpHost
    {
        public UnityEvent<ExoEntry> OnElbowValue;

        public float ElbowValue => _elbowValue;

        private float _elbowValue, _wristValue;


        public override void Receive(string message)
        {
            base.Receive(message);

            var data = message.Split(',');
            var unitytime = DateTime.Now.ToString("HH:mm:ss.ffffff");

            if (data.Length == 2)
            {
                //Parse the elbow angle value
                if (float.TryParse(data[0], NumberStyles.Any, CultureInfo.InvariantCulture, out _elbowValue))
                {
                    var entry = new ExoEntry
                    {
                        Value = _elbowValue,
                        UdpTimestamp = data[1],
                        UnityTimestamp = unitytime
                    };

                    OnElbowValue.Invoke(entry);
                }
            }
        }

        /// <summary>
        /// Called to nudge the patien in either exo skeleton directions: up, down.
        /// </summary>
        /// <param name="direction">The direction to nudge in. (up, down)</param>
        public void Nudge(ExoNudgeDir direction)
        {
            Send(direction.ToString());
        }
    }

    public class ExoEntry
    {
        public float Value { get; set; }
        public string UdpTimestamp { get; set; }
        public string UnityTimestamp { get; set; }
    }

    public enum ExoNudgeDir
    {
        up,
        down
    }
}
