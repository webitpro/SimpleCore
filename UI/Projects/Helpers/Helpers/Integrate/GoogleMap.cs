using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Collections;
using System.Text;
using Core.Library;

namespace Core.Helpers
{
    public partial class Integrate
    {
        public static class GoogleMap
        {
            static JavaScriptSerializer jss = new JavaScriptSerializer();

            // *******************************************
            // PROPERTIES
            // *******************************************
            public const string APIUrl = "http://maps.googleapis.com/maps/api/";

            // ********************************************
            // PUBLIC METHODS
            // ********************************************
            public static GeocodeResponse Geocode(string street, string city, string state, string zip_code, string country, bool sensor)
            {

                string url = APIUrl + "geocode/json";
                string[] parameters = 
			{
				"address", "sensor"  
			};
                string[] values = 
			{
				FormatAddress(street, city, state, zip_code, country),
				((sensor) ? "true" : "false")
			};
                ArrayList headers = new ArrayList();

                string response = Utils.Request.ReadResponse(Utils.Request.Get(url, Encoding.UTF8, parameters, values, headers), Encoding.UTF8);

                GeocodeResponse gr = jss.Deserialize<GeocodeResponse>(response);

                return gr;



            }


            public enum DistanceUnit
            {
                imperial,
                metric
            }
            public enum DistanceType
            {
                aprox,
                driving,
                walking,
                bicycling

            }
            public static DirectionsResponse Directions(double[] origin, double[] destination, DistanceType type, DistanceUnit unit, bool sensor)
            {

                string url = APIUrl + "directions/json";
                string[] parameters = { "origin", "destination", "mode", "units", "sensor" };
                string[] values = { 
										origin[0].ToString() + "," + origin[1].ToString() ,
										destination[0].ToString() + "," + destination[1].ToString(),
										type.ToString(),
										unit.ToString(),
										((sensor) ? "true" : "false")
									};
                ArrayList headers = new ArrayList();
                string response = Utils.Request.ReadResponse(Utils.Request.Get(url, Encoding.UTF8, parameters, values, headers), Encoding.UTF8);
                DirectionsResponse dr = jss.Deserialize<DirectionsResponse>(response);



                return dr;

            }

            public static double CalculateAproxDistance(double[] origin, double[] destination, DistanceUnit unit)
            {
                const double km_factor = 1.609344;
                const double m_factor = 69.09;

                double distance = Math.Sin(DegToRad(origin[0]))
                    * Math.Sin(DegToRad(destination[0]))
                    + Math.Cos(DegToRad(origin[0]))
                    * Math.Cos(DegToRad(destination[0]))
                    * Math.Cos(DegToRad(origin[1] - destination[1]));

                double degree = RadToDeg(Math.Acos(distance));
                double miles = degree * m_factor;
                double km = miles * km_factor;


                if (unit == DistanceUnit.imperial)
                {
                    return Math.Round(miles, 2);
                }
                else
                {
                    return Math.Round(km, 2);
                }
            }

            // ********************************************
            // PRIVATE METHODS
            // ********************************************
            private static string FormatAddress(string street, string city, string state, string zip_code, string country)
            {
                string address = street + " " + city + " " + state + " " + zip_code + " " + country;
                return address.Trim().Replace(" ", "+");
            }

            private static double RadToDeg(double rad)
            {
                return (180.00 / Math.PI) * rad;
            }
            private static double DegToRad(double deg)
            {
                return (Math.PI * Convert.ToDouble(deg)) / 180.00;
            }

            // ********************************************
            // MODELS
            // ********************************************

            #region GEOCODE RESPONSE MODELS
            public class GeocodeResponse
            {
                public string status { get; set; }
                public List<Results> results { get; set; }
            }
            public class Results
            {
                public string[] types { get; set; }
                public string formatted_address { get; set; }
                public AddressComponent[] address_components { get; set; }
                public Geometry geometry { get; set; }
            }
            public class AddressComponent
            {
                public string long_name { get; set; }
                public string short_name { get; set; }
                public string[] types { get; set; }
            }
            public class Geometry
            {
                public LatLng location { get; set; }
                public string location_type { get; set; }
                public Viewport viewport { get; set; }
                public Bounds bounds { get; set; }
            }
            public class LatLng
            {
                public double lat { get; set; }
                public double lng { get; set; }
            }
            public class Viewport
            {
                public LatLng southwest { get; set; }
                public LatLng northeast { get; set; }
            }
            public class Bounds
            {
                public LatLng southwest { get; set; }
                public LatLng northeast { get; set; }
            }
            #endregion

            #region DIRECTION RESPONSE MODELS
            public class DirectionsResponse
            {
                public string status { get; set; }
                public List<Route> routes { get; set; }
            }
            public class Route
            {
                public string summary { get; set; }
                public List<Leg> legs { get; set; }
                public string copyrights { get; set; }
                public Polyline overview_polyline { get; set; }
                public string[] warnings { get; set; }
                public object[] waypoint_order { get; set; }
            }

            public class Leg
            {
                public List<Step> steps { get; set; }
                public Info duration { get; set; }
                public Info distance { get; set; }
                public LatLng start_location { get; set; }
                public LatLng end_location { get; set; }
                public string start_address { get; set; }
                public string end_address { get; set; }
                public object[] via_waypoint { get; set; }
            }

            public class Step
            {
                public string travel_mode { get; set; }
                public LatLng start_location { get; set; }
                public LatLng end_location { get; set; }
                public Polyline polyline { get; set; }
                public Info duration { get; set; }
                public string html_instructions { get; set; }
                public Info distance { get; set; }
            }

            public class Info
            {
                public int value { get; set; }
                public string text { get; set; }
            }

            public class Polyline
            {
                public string points { get; set; }
                public string levels { get; set; }
            }
            #endregion

        }
    }
}