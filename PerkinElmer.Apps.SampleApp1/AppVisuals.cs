using PerkinElmer.Signals.Analytics.Components;
using Spotfire.Dxp.Application;
using Spotfire.Dxp.Application.Visuals;
using Spotfire.Dxp.Data;
using Spotfire.Dxp.Application.Visuals.ConditionalColoring;
using Spotfire.Dxp.Application.Visuals.Maps;
using System;
using System.Drawing;

namespace PerkinElmer.Apps.SampleApp1
{
    public partial class App : BaseComponentsApp
    {

        [ActionDefinition("refreshBarChartAirbnbTable")]
        public void refreshBarBarChartAirbnbTable(Visual visual)
        {
            DataTable table = GetTableFromParameter("airbnb");
            if (table != null)
            {
                visual.As<BarChart>().Data.DataTableReference = table;
            }
        }

        [ActionDefinition("mapDistance")]
        public Visual createMap()
        {
            DataTable airbnbTable = GetTableFromParameter("airbnb");
            DataTable subwayEntriesTable = GetTableFromParameter("subwayEntries");
          
            var map = Page.Visuals.AddNew<Spotfire.Dxp.Application.Visuals.Maps.MapChart>();
            //map.AutoConfigure();

            var tmsLayer = map.Layers.AddNewTmsLayer();
            tmsLayer.UrlTemplate = new Uri("http://a.basemaps.cartocdn.com/light_all/{z}/{x}/{y}.png");
            tmsLayer.CopyrightLink = new Uri("http://www.openstreetmap.org/copyright");
            tmsLayer.CopyrightText = "Map tiles by CartoDB, under CC BY 3.0. Data by OpenStreetMap, under ODbL";

            MarkerLayerVisualization airbnbMarkerLayerVisualization;
            map.Layers.AddNewMarkerLayer(airbnbTable, out airbnbMarkerLayerVisualization);
            airbnbMarkerLayerVisualization.PositioningMethod = PositioningMethod.LongitudeLatitude;
            airbnbMarkerLayerVisualization.XAxis.Expression = GetExpression("$esc(${airbnbLongitude})");
            airbnbMarkerLayerVisualization.YAxis.Expression = GetExpression("$esc(${airbnbLatitude})");
            airbnbMarkerLayerVisualization.ColorAxis.Expression = GetExpression("[distanceToSubway] > ${maxDistance}");
            airbnbMarkerLayerVisualization.MarkerSize = 1;
            airbnbMarkerLayerVisualization.ShapeAxis.DefaultShape = new MarkerShape(MarkerType.Circle);


            MarkerLayerVisualization subwayEntriesMarkerLayerVisualization;
            map.Layers.AddNewMarkerLayer(subwayEntriesTable, out subwayEntriesMarkerLayerVisualization);
            subwayEntriesMarkerLayerVisualization.PositioningMethod = PositioningMethod.LongitudeLatitude;
            subwayEntriesMarkerLayerVisualization.XAxis.Expression = GetExpression("$esc(${subwayEntryLongitude})");
            subwayEntriesMarkerLayerVisualization.YAxis.Expression = GetExpression("$esc(${subwayEntryLatitude})");
            subwayEntriesMarkerLayerVisualization.ColorAxis.Expression = GetExpression("$esc(${subwayEntryLine})");
            subwayEntriesMarkerLayerVisualization.MarkerSize = 1.2f;
            subwayEntriesMarkerLayerVisualization.ShapeAxis.DefaultShape = new MarkerShape(MarkerType.Square);

            return map.Visual;
        }

        [ActionDefinition("neihbourhoodGroup")]
        public Visual CreateNeihbourhoodGroupChart()
        {
            var barChart = Page.Visuals.AddNew<BarChart>();
            barChart.Data.MarkingReference = App.Document.ActiveMarkingSelectionReference;
            barChart.XAxis.Expression = GetExpression("<$esc(${airbnbNeighbourhoodGroup})>");
            barChart.YAxis.Expression = GetExpression("Avg($esc(${airbnbMain}))");
            barChart.ColorAxis.Expression = GetExpression("$esc(${airbnbNeighbourhoodGroup})");
            barChart.Data.DataTableReference = GetTableFromParameter("airbnb");
            barChart.BarWidth = 80;
            barChart.SortedBars = true;
            barChart.Orientation = BarChartOrientation.Horizontal;

            return barChart.Visual;
        }

        [ActionDefinition("neihbourhood")]
        public Visual CreateNeihbourhoodChart()
        { 
            var barChart = Page.Visuals.AddNew<BarChart>();
            barChart.Data.MarkingReference = App.Document.ActiveMarkingSelectionReference;
            barChart.XAxis.Expression = GetExpression("<$esc(${airbnbNeighbourhood})>");
            barChart.YAxis.Expression = GetExpression("Avg($esc(${airbnbMain}))");
            barChart.ColorAxis.Expression = GetExpression("$esc(${airbnbNeighbourhoodGroup})");
            barChart.Data.DataTableReference = GetTableFromParameter("airbnb");
            barChart.BarWidth = 80;
            barChart.SortedBars = true;
            barChart.Orientation = BarChartOrientation.Horizontal;
            barChart.Data.LimitingMarkingsEmptyBehavior = LimitingMarkingsEmptyBehavior.ShowAll;
            barChart.Data.Filterings.Add(GetMarking("airbnbMarking"));

            return barChart.Visual;
        }

        [ActionDefinition("map")]
        public Visual CreateMapChart()
        {
            var map = Page.Visuals.AddNew<Spotfire.Dxp.Application.Visuals.Maps.MapChart>();
            clearAndMapAirbndLayers(map);
            return map.Visual;
        }

        [ActionDefinition("refreshMapChartAirbnbTable")]
        public void refreshMapChartAirbnbTable(Visual visual)
        {
            var map = visual.As<Spotfire.Dxp.Application.Visuals.Maps.MapChart>();
            clearAndMapAirbndLayers(map);
        }


        private void clearAndMapAirbndLayers(Spotfire.Dxp.Application.Visuals.Maps.MapChart map) 
        {
            map.Layers.Clear();

            DataTable airbnbTable = GetTableFromParameter("airbnb");

            var tmsLayer = map.Layers.AddNewTmsLayer();
            tmsLayer.UrlTemplate = new Uri("http://a.basemaps.cartocdn.com/light_all/{z}/{x}/{y}.png");
            tmsLayer.CopyrightLink = new Uri("http://www.openstreetmap.org/copyright");
            tmsLayer.CopyrightText = "Map tiles by CartoDB, under CC BY 3.0. Data by OpenStreetMap, under ODbL";

            MarkerLayerVisualization airbnbMarkerLayerVisualization;
            map.Layers.AddNewMarkerLayer(airbnbTable, out airbnbMarkerLayerVisualization);
            airbnbMarkerLayerVisualization.PositioningMethod = PositioningMethod.LongitudeLatitude;
            airbnbMarkerLayerVisualization.XAxis.Expression = GetExpression("$esc(${airbnbLongitude})");
            airbnbMarkerLayerVisualization.YAxis.Expression = GetExpression("$esc(${airbnbLatitude})");
            airbnbMarkerLayerVisualization.ColorAxis.Expression = GetExpression("$esc(${airbnbMain})");
            airbnbMarkerLayerVisualization.MarkerSize = 2;
            airbnbMarkerLayerVisualization.ShapeAxis.DefaultShape = new MarkerShape(MarkerType.Circle);
            airbnbMarkerLayerVisualization.ColorAxis.Coloring.Clear();
            var colorRule = airbnbMarkerLayerVisualization.ColorAxis.Coloring.AddContinuousColorRule();
            colorRule.Breakpoints.Add(ConditionValue.MinValue, Color.White);
            colorRule.Breakpoints.Add(ConditionValue.CreateExpression("Q3([Axis.Color])"), Color.Blue);
            colorRule.Breakpoints.Add(ConditionValue.MaxValue, Color.Red);
        }

        [ActionDefinition("roomType")]
        public Visual CreateRoomTypeChart()
        {
            var barChart = Page.Visuals.AddNew<BarChart>();
            barChart.Data.MarkingReference = App.Document.ActiveMarkingSelectionReference;
            barChart.XAxis.Expression = GetExpression("<$esc(${airbnbNeighbourhoodGroup})>");
            barChart.YAxis.Expression = GetExpression("Avg($esc(${airbnbMain}))");
            barChart.ColorAxis.Expression = GetExpression("<$esc(${airbnbRoomType})>");
            barChart.Data.DataTableReference = GetTableFromParameter("airbnb");
            barChart.BarWidth = 80;
            barChart.SortedBars = true;
            barChart.Orientation = BarChartOrientation.Horizontal;
            barChart.StackMode = StackMode.None;
            return barChart.Visual;
        }

        [ActionDefinition("mapSubway")]
        public Visual CreateMapSubwayChart()
        {
            var map = Page.Visuals.AddNew<Spotfire.Dxp.Application.Visuals.Maps.MapChart>();
            clearAndMapSubwayLayers(map);
            return map.Visual;
        }

        [ActionDefinition("refreshMapSubway")]
        public void RefreshMapSubwayChart(Visual visual)
        {
            var map = visual.As<Spotfire.Dxp.Application.Visuals.Maps.MapChart>();
            clearAndMapSubwayLayers(map);
        }

        private void clearAndMapSubwayLayers(Spotfire.Dxp.Application.Visuals.Maps.MapChart map)
        {
            map.Layers.Clear();

            DataTable airbnbTable = GetTableFromParameter("subwayEntries");

            var tmsLayer = map.Layers.AddNewTmsLayer();
            tmsLayer.UrlTemplate = new Uri("http://a.basemaps.cartocdn.com/light_all/{z}/{x}/{y}.png");
            tmsLayer.CopyrightLink = new Uri("http://www.openstreetmap.org/copyright");
            tmsLayer.CopyrightText = "Map tiles by CartoDB, under CC BY 3.0. Data by OpenStreetMap, under ODbL";

            MarkerLayerVisualization airbnbMarkerLayerVisualization;
            map.Layers.AddNewMarkerLayer(airbnbTable, out airbnbMarkerLayerVisualization);
            airbnbMarkerLayerVisualization.PositioningMethod = PositioningMethod.LongitudeLatitude;
            airbnbMarkerLayerVisualization.XAxis.Expression = GetExpression("$esc(${subwayEntryLongitude})");
            airbnbMarkerLayerVisualization.YAxis.Expression = GetExpression("$esc(${subwayEntryLatitude})");
            airbnbMarkerLayerVisualization.ColorAxis.Expression = GetExpression("$esc(${subwayEntryLine})");
            airbnbMarkerLayerVisualization.MarkerSize = 2;
            airbnbMarkerLayerVisualization.ShapeAxis.DefaultShape = new MarkerShape(MarkerType.Circle);
        }
    }
}
