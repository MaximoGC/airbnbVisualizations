using PerkinElmer.Signals.Analytics.AppCommon;
using PerkinElmer.Signals.Analytics.Components;
using Spotfire.Dxp.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PerkinElmer.Apps.SampleApp1
{
    [Serializable]
    [PersistenceVersion(1, 0)]
    [AppMetadata("PerkinElmer.Apps.SampleApp1", "PerkinElmer.Apps.SampleApp1")]
    public partial class App : BaseComponentsApp
    {

        #region Constructors
        public App(string name, string jsonAppStore)
            : base(name, jsonAppStore)
        {

        }
        protected App(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
        #endregion

        #region User Methods

        public override void ViewController()
        {
            Scope.Watch("stepByStep.activeView", (ScopeValue oldValue, ScopeValue newValue) =>
            {
                switch (newValue.As<string>())
                {
                    case "airbnbData":
                        Scope["visualizationLayout"] = "default";
                        break;
                    case "subwayData":
                        Scope["visualizationLayout"] = "subwayDataSelector";
                        break;
                }
            });
        }

        #endregion

        [ActionDefinition("nextStep")]
        public bool ValidateStep(string view)
        {
            Audit.Log(this, "Step change", view);
            switch (view) 
            {
                case "subwayData":
                    return validateStringParameters(new List<string> { "airbnb", "airbnbLatitude", "airbnbLongitude", "airbnbMain", "airbnbNeighbourhood", "airbnbNeighbourhoodGroup", "airbnbRoomType" });
                case "subwayDataFunction":
                    return validateStringParameters(new List<string> { "subwayEntries", "subwayEntryLongitude", "subwayEntryLongitude", "subwayEntryLine" });
            }
            return true;
        }

        private bool validateStringParameters(List<string> list)
        {
            var notValidItems = list.FindAll(item => string.IsNullOrEmpty((string)GetParameterValue(item)));
            var isValid = notValidItems.Count == 0;
            if (!isValid)
            {
                NotifyUser(UserAlert.WARNING, $"Please select valid values for { string.Join(", ", notValidItems) }.");
            }
            return isValid;
        }

    }
}
