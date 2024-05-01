﻿

using System;

namespace OregonTrailDotNet.Module.Tombstone
{
    /// <summary>
    ///     Facilitates a tombstone base class that supports shallow copies of itself to be created.
    /// </summary>
    public sealed class Tombstone
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Tombstone" /> class.
        ///     Creates a shallow copy of the tombstone, generates a new tombstone ID in the process.
        /// </summary>
        public Tombstone()
        {
            // Loop through all the vehicle passengers and find the leader.
            foreach (var passenger in GameSimulationApp.Instance.Vehicle.Passengers)
            {
                // Skip if not the leader.
                if (!passenger.Leader)
                    continue;

                // Add the leaders name to the Tombstone header.
                PlayerName = passenger.Name;
                break;
            }

            // Grabs the current mile marker where the player died on the trail for the Tombstone to sit at.
            MileMarker = GameSimulationApp.Instance.Vehicle.Odometer;

            // Epitaph is left empty by default and ready to be filled in.
            Epitaph = string.Empty;
        }

        /// <summary>
        ///     Name of the player whom died and the Tombstone is paying respects to.
        /// </summary>
        public string PlayerName { get; }

        /// <summary>
        ///     Defines where on the trail in regards to length in miles traveled. The purpose of this is so we can check for this
        ///     Tombstone in the exact same spot where the player actually died on the trail.
        /// </summary>
        public int MileMarker { get; }

        /// <summary>
        ///     Message that can be included on the Tombstone below the players name. It can only be a few characters long but
        ///     interesting to see what people leave as warnings or just silliness for other travelers.
        /// </summary>
        public string Epitaph { get; set; }

        /// <summary>
        ///     Creates a shallow copy of our tombstone, used to add to list without having direct copy still tied to it.
        /// </summary>
        /// <returns>
        ///     The <see cref="object" />.
        /// </returns>
        public object Clone()
        {
            var clone = (Tombstone) MemberwiseClone();
            return clone;
        }

        /// <summary>
        ///     Creates a nice formatted version of the Tombstone for use in text renderer.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public override string ToString()
        {
            // Only print the name if the epitaph is empty or null.
            if (string.IsNullOrEmpty(Epitaph))
                return $"Here lies {PlayerName}{Environment.NewLine}";

            // Print the name and epitaph message the player left for others to read.
            return $"Here lies {PlayerName}{Environment.NewLine}" +
                   $"{Epitaph}{Environment.NewLine}";
        }
    }
}