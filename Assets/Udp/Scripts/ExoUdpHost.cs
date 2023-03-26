using System;
using System.Globalization;
using Udp;
using UnityEngine.Events;

namespace Exo
{
    public class ExoUdpHost : UdpHost
    {
        public UnityEvent<ExoEntry> OnElbowValue;
        public UnityEvent<ExoEntry> OnWristValue;

        public float ElbowValue => _elbowValue;
        public float WristValue => _wristValue;

        private float _elbowValue, _wristValue;


        public override void Receive(string message)
        {
            base.Receive(message);

            var data = message.Split(',');
            var unitytime = DateTime.Now.ToString("HH:mm:ss.ffffff");

            if (data.Length == 3)
            {
                //Parse the first csv which is the elbow angle
                if (float.TryParse(data[0], NumberStyles.Any, CultureInfo.InvariantCulture, out _elbowValue))
                {
                    var entry = new ExoEntry
                    {
                        Value = _elbowValue,
                        UdpTimestamp = data[2],
                        UnityTimestamp = unitytime
                    };

                    OnElbowValue.Invoke(entry);
                }

                //Parse the second csv which is the wrist angle
                if (float.TryParse(data[1], NumberStyles.Any, CultureInfo.InvariantCulture, out _wristValue))
                {
                    var entry = new ExoEntry
                    {
                        Value = _wristValue,
                        UdpTimestamp = data[2],
                        UnityTimestamp = unitytime
                    };

                    OnWristValue.Invoke(entry);
                }
            }
        }

        /// <summary>
        /// Called to nudge the patien in either exo skeleton directions: up, down, left, right
        /// </summary>
        /// <param name="direction">The direction to nudge in. (up, down, left, right)</param>
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
        down,
        left,
        right
    }
}
