

using System;
using System.Collections.Generic;
using System.Text;
using OregonTrailDotNet.Entity;
using WolfCurses.Window;
using WolfCurses.Window.Control;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace OregonTrailDotNet.Window.Travel.Command
{
    
    [ParentWindow(typeof(Travel))]
    public sealed class CheckSupplies : InputForm<TravelInfo>
    {
       
        public CheckSupplies(IWindow window) : base(window)
        {
        }

      
        protected override string OnDialogPrompt()
        {
            // Tick the people, but not the trail or the day.
            GameSimulationApp.Instance.TakeTurn(true);

            // Build up representation of supplies once in constructor and then reference when asked for render.
            var supplies = new StringBuilder();
            supplies.AppendLine($"{Environment.NewLine}Your Supplies{Environment.NewLine}");

            // Build up a list with tuple in it to hold our data about supplies.
            var suppliesList = new List<Tuple<string, string>>();

            // Loop through every inventory item in the vehicle.
            foreach (var item in GameSimulationApp.Instance.Vehicle.Inventory)
            {
                // Apply number formatting to quantities so they have thousand separators.
                var itemFormattedQuantity = item.Value.Quantity.ToString("N0");

                // Change up how we print out various items in the vehicle inventory.
                switch (item.Key)
                {
                    case Entities.Animal:
                        suppliesList.Add(new Tuple<string, string>("oxen", itemFormattedQuantity));
                        break;
                    case Entities.Clothes:
                        suppliesList.Add(new Tuple<string, string>("sets of clothing", itemFormattedQuantity));
                        break;
                    case Entities.Ammo:
                        suppliesList.Add(new Tuple<string, string>("bullets", itemFormattedQuantity));
                        break;
                    case Entities.Wheel:
                        suppliesList.Add(new Tuple<string, string>("wagon wheels", itemFormattedQuantity));
                        break;
                    case Entities.Axle:
                        suppliesList.Add(new Tuple<string, string>("wagon axles", itemFormattedQuantity));
                        break;
                    case Entities.Tongue:
                        suppliesList.Add(new Tuple<string, string>("wagon tongues", itemFormattedQuantity));
                        break;
                    case Entities.Food:
                        suppliesList.Add(new Tuple<string, string>("pounds of food",
                            item.Value.TotalWeight.ToString("N0")));
                        break;
                    case Entities.Cash:
                        suppliesList.Add(new Tuple<string, string>("money left", item.Value.TotalValue.ToString("C")));
                        break;
                    case Entities.Vehicle:
                    case Entities.Person:
                    case Entities.Location:
                        throw new ArgumentOutOfRangeException();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            // Generate the formatted table of supplies we will show to user.
            var supplyTable = suppliesList.ToStringTable(
                new[] {"Item Name", "Amount"},
                u => u.Item1,
                u => u.Item2);

            // Add the table to the text user interface.
            supplies.AppendLine(supplyTable);

            return supplies.ToString();
        }

       
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            // parentGameMode.State = null;
            ClearForm();
        }
    }
}