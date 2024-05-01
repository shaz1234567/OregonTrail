﻿
using System;
using System.Text;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace OregonTrailDotNet.Window.RandomEvent
{
   
    [ParentWindow(typeof(RandomEvent))]
    public sealed class EventExecutor : InputForm<RandomEventInfo>
    {
        
        private bool _eventPlayerAcknowledge;

       
        public EventExecutor(IWindow window) : base(window)
        {
        }

       
        protected override string OnDialogPrompt()
        {
            // Create new string builder that will hold event execution data.
            var randomEventText = new StringBuilder();

            // Execute the event which should return us some text to display to user about what it did to running simulation.
            UserData.DirectorEvent.Execute(UserData);
            var eventText = UserData.DirectorEvent.Render(UserData);

            // Complain if the event text is empty.
            if (string.IsNullOrEmpty(eventText) || string.IsNullOrWhiteSpace(eventText))
                throw new InvalidOperationException(
                    $"Executed random event {UserData.DirectorEvent.Name} from director, but it returned no text data!");

            // Add the text to the user data so we can print it on another form if needed.
            UserData.EventText = eventText;

            // Allow the event to do any custom actions requiring the use of event executor form.
            if (UserData.DirectorEvent.OnPostExecute(this))
                return "Loading event...";

            // Add the text to our output about the random event.
            randomEventText.AppendLine(
                $"{Environment.NewLine}{eventText}{Environment.NewLine}");
            return randomEventText.ToString();
        }

        /// <summary>
        ///     Fired when the dialog receives favorable input and determines a response based on this. From this method it is
        ///     common to attach another state, or remove the current state based on the response.
        /// </summary>
        /// <param name="reponse">The response the dialog parsed from simulation input buffer.</param>
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            // Person has already acknowledged this event form.
            if (_eventPlayerAcknowledge)
                return;

            // Prevent multiple closures of this form, and window.
            _eventPlayerAcknowledge = true;

            // Fires off event so events can do something special when the event closes.
            UserData.DirectorEvent.OnEventClose(UserData);

            // Only remove the entire random event form if we don't have any days to skip.
            ParentWindow.RemoveWindowNextTick();
        }
    }
}