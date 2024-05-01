﻿

using WolfCurses;
using WolfCurses.Window;

namespace OregonTrailDotNet.Window.Graveyard
{
    /// <summary>
    ///     Displays the name of a previous player whom traveled the trail and died at a given mile marker. There is also an
    ///     optional epitaph that can be displayed. These tombstones are saved per trail, and can be reset from main menu.
    /// </summary>
    public sealed class Graveyard : Window<TombstoneCommands, TombstoneInfo>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Window{TCommands,TData}" /> class.
        /// </summary>
        /// <param name="simUnit">Core simulation which is controlling the form factory.</param>
        // ReSharper disable once UnusedMember.Global
        public Graveyard(SimulationApp simUnit) : base(simUnit)
        {
        }

        /// <summary>
        ///     Called after the Windows has been added to list of modes and made active.
        /// </summary>
        public override void OnWindowPostCreate()
        {
            base.OnWindowPostCreate();

            // Depending on the living status of passengers in current player vehicle we will attach a different form.
            SetForm(GameSimulationApp.Instance.Vehicle.PassengerLivingCount <= 0
                ? typeof(EpitaphQuestion)
                : typeof(TombstoneView));
        }
    }
}