﻿

using System;
using System.Text;
using OregonTrailDotNet.Entity.Item;
using OregonTrailDotNet.Entity.Location;
using OregonTrailDotNet.Window.Travel.Store.Help;
using WolfCurses.Window;
using WolfCurses.Window.Form;

namespace OregonTrailDotNet.Window.Travel.Store
{
    /// <summary>
    ///     Allows the player to purchase a number of oxen to pull their vehicle.
    /// </summary>
    [ParentWindow(typeof(Travel))]
    public sealed class StorePurchase : Form<TravelInfo>
    {
        /// <summary>
        ///     Help text to ask the player a question about how many of the particular SimItem they would like to purchase.
        /// </summary>
        private StringBuilder _itemBuyText;

        /// <summary>
        ///     Reference to the SimItem the player wishes to purchase from the store, it will be added to receipt list of
        ///     it can.
        /// </summary>
        private SimItem _itemToBuy;

        /// <summary>
        ///     Reference to the total amount of items the player can purchase of SimItem of this particular type from this
        ///     store
        ///     with
        ///     the money they have.
        /// </summary>
        private int _purchaseLimit;

        /// <summary>
        ///     Initializes a new instance of the <see cref="StorePurchase" /> class.
        ///     Attaches a state that will allow the player to purchase a certain number of a particular SimItem.
        /// </summary>
        /// <param name="window">Current game Windows that requested this.</param>
        // ReSharper disable once UnusedMember.Global
        public StorePurchase(IWindow window) : base(window)
        {
        }

        /// <summary>
        ///     Fired after the state has been completely attached to the simulation letting the state know it can browse the user
        ///     data and other properties below it.
        /// </summary>
        public override void OnFormPostCreate()
        {
            base.OnFormPostCreate();

            // Figure out what we owe already from other store items, then how many of the SimItem we can afford.
            var currentBalance =
                (int) (GameSimulationApp.Instance.Vehicle.Balance - UserData.Store.TotalTransactionCost);
            _purchaseLimit = (int) (currentBalance/UserData.Store.SelectedItem.Cost);

            // Prevent negative numbers and set credit limit to zero if it drops below that.
            if (_purchaseLimit < 0)
                _purchaseLimit = 0;

            // Set the credit limit to be the carry limit if they player has lots of monies and can buy many, we must limit them!
            if (_purchaseLimit > UserData.Store.SelectedItem.MaxQuantity)
                _purchaseLimit = UserData.Store.SelectedItem.MaxQuantity;

            // Add some information about how many you can buy and total amount you can carry.
            _itemBuyText = new StringBuilder();

            // Change up question asked if plural window matches the name of the SimItem.
            var pluralMatchesName = UserData.Store.SelectedItem.PluralForm.Equals(UserData.Store.SelectedItem.Name,
                StringComparison.OrdinalIgnoreCase);

            // Print text about purchasing the selected item.
            _itemBuyText.AppendLine(pluralMatchesName
                ? $"{Environment.NewLine}You can afford {_purchaseLimit} {UserData.Store.SelectedItem.Name.ToLowerInvariant()}."
                : $"{Environment.NewLine}You can afford {_purchaseLimit} {UserData.Store.SelectedItem.PluralForm.ToLowerInvariant()} of {UserData.Store.SelectedItem.Name.ToLowerInvariant()}.");

            // Wait for user input...
            _itemBuyText.Append($"How many {UserData.Store.SelectedItem.PluralForm.ToLowerInvariant()} to buy?");

            // Set the SimItem to buy text.
            _itemToBuy = UserData.Store.SelectedItem;
        }

        /// <summary>
        ///     Returns a text only representation of the current game Windows state. Could be a statement, information, question
        ///     waiting input, etc.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public override string OnRenderForm()
        {
            return _itemBuyText.ToString();
        }

        /// <summary>Fired when the game Windows current state is not null and input buffer does not match any known command.</summary>
        /// <param name="input">Contents of the input buffer which didn't match any known command in parent game Windows.</param>
        public override void OnInputBufferReturned(string input)
        {
            // Parse the user input buffer as a unsigned int.
            if (!int.TryParse(input, out var parsedInputNumber))
                return;

            // If the number is zero remove the purchase state for this SimItem and back to store menu.
            if (parsedInputNumber <= 0)
            {
                UserData.Store.RemoveItem(_itemToBuy);
                UserData.Store.SelectedItem = null;
                SetForm(typeof(Store));
                return;
            }

            // Check that number is less than maximum quantity based on monies.
            if (parsedInputNumber > _purchaseLimit)
            {
                UserData.Store.RemoveItem(_itemToBuy);
                UserData.Store.SelectedItem = null;
                SetForm(typeof(Store));
                return;
            }

            // Check that number is less than or equal to limit that is hard-coded.
            if (parsedInputNumber > _itemToBuy.MaxQuantity)
            {
                UserData.Store.RemoveItem(_itemToBuy);
                UserData.Store.SelectedItem = null;
                SetForm(typeof(Store));
                return;
            }

            // Check that the player has enough monies to pay for the quantity of item they specified.
            if (GameSimulationApp.Instance.Vehicle.Balance < _itemToBuy.TotalValue*parsedInputNumber)
            {
                UserData.Store.RemoveItem(_itemToBuy);
                UserData.Store.SelectedItem = null;
                SetForm(typeof(Store));
                return;
            }

            // First location on the trail uses receipt to keep track of all the purchases player wants.
            UserData.Store.AddItem(_itemToBuy, parsedInputNumber);

            // If we are not on the first location we will add the item right away.
            if (GameSimulationApp.Instance.Trail.CurrentLocation?.Status == LocationStatus.Arrived)
            {
                // Normal store operation while on the trail.
                UserData.Store.PurchaseItems();
            }
            else
            {
                // Check if player can afford the items they have selected.
                var totalBill = UserData.Store.TotalTransactionCost;
                if (GameSimulationApp.Instance.Vehicle.Balance < totalBill)
                {
                    SetForm(typeof(StoreDebtWarning));
                    return;
                }
            }

            // Clear the selection for the type of item the player was purchasing.
            UserData.Store.SelectedItem = null;

            // Return to the store menu.
            SetForm(typeof(Store));
        }
    }
}