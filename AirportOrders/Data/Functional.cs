using AirportOrders.ServiceReferenceSita;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using AirportOrders.Models;
using Domain;

namespace AirportOrders.Data
{
    public  class Functional
    {
        public static AMSIntegrationServiceClient proxy = staticProxy();

        private static AMSIntegrationServiceClient staticProxy()
        {
            AMSIntegrationServiceClient proxy = new AMSIntegrationServiceClient("BasicHttpBinding_IAMSIntegrationService","http://tse2-ams-apv.aia.local/SITAAMSIntegrationService/v2/SITAAMSIntegrationService/");
            //proxy.UpdateFlight( );

            return proxy;
        }
        public static ObservableCollection<Flight> ArrivalFligts;
        public static ObservableCollection<Flight> DepartureFlights;


        public static void getFlights(XElement root)
        {
            ArrivalFligts = new ObservableCollection<Flight>();
            DepartureFlights = new ObservableCollection<Flight>();

            XNamespace xmlns = "http://www.sita.aero/ams6-xml-api-webservice";
            XElement ApiResponse = root.Element(xmlns + "ApiResponse");
            XNamespace ns = "http://www.sita.aero/ams6-xml-api-datatypes";
            XElement Status = ApiResponse.Element(ns + "Status");
            if (Status.Value == "Success")
            {
                XElement Data = ApiResponse.Element(ns + "Data");
                XElement Flights = Data.Element(ns + "Flights");
                foreach (XElement Flight in Flights.Elements(ns + "Flight"))
                {
                    Flight selFlight = ParserFlight(Flight);


                    if (selFlight == null) continue;
                    //if (selFlight.Ramp_id != App.Current.Properties["Username"].ToString()) continue;
                    //string ramp_id = Xamarin.Forms.Application.Current.Properties["Username"].ToString();
                    //if (ramp_id != selFlight.Ramp_id) continue;

                    if (selFlight.FlightKind == "Arrival")
                    {
                        ArrivalFligts.Add(selFlight);
                    }
                    else if (selFlight.FlightKind == "Departure")
                    {
                        DepartureFlights.Add(selFlight);
                    }
                }



            }
            else if (Status.Value == "Failure")
            {

            }


        }

        public static ObservableCollection<Flight> getFlightsUnion(XElement root)
        {

            ObservableCollection<Flight> flights = new ObservableCollection<Flight>();

            XNamespace xmlns = "http://www.sita.aero/ams6-xml-api-webservice";
            XElement ApiResponse = root.Element(xmlns + "ApiResponse");
            XNamespace ns = "http://www.sita.aero/ams6-xml-api-datatypes";
            XElement Status = ApiResponse.Element(ns + "Status");
            if (Status.Value == "Success")
            {
                XElement Data = ApiResponse.Element(ns + "Data");
                XElement Flights = Data.Element(ns + "Flights");
                foreach (XElement Flight in Flights.Elements(ns + "Flight"))
                {
                    Flight selFlight = ParserFlight(Flight);
                    flights.Add(selFlight);
                }

            }
            else if (Status.Value == "Failure")
            {
                /*
                XElement Errors = ApiResponse.Element(ns + "Errors");
                XElement Error = Errors.Element(ns + "Error");
                XElement ErrorDescription = Error.Element(ns + "ErrorDescription");
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("Error", ErrorDescription.Value, "OK");
                });
                */

            }

            return flights;

        }

        private static Flight ParserFlight(XElement FlightsElm)
        {
            //if (!FlightsElm.Value.Contains(App.Current.Properties["Username"].ToString())) return null;

            Flight selFlight = new Flight();
            selFlight.NeedServices = new ObservableCollection<Flight.flightServiceNeed>();
            selFlight.Services = new ObservableCollection<Flight.flihghtService>();
            selFlight.Activity = new ObservableCollection<Flight.structActivity>();
            selFlight.Event = new ObservableCollection<Flight.structEvent>();

            try
            {
                XNamespace ns = "http://www.sita.aero/ams6-xml-api-datatypes";
                XElement flightid = FlightsElm.Element(ns + "FlightId");
                selFlight.FlightKind = flightid.Element(ns + "FlightKind").Value.ToString();
                selFlight.FlightNumber = flightid.Element(ns + "FlightNumber").Value.ToString();
                selFlight.ScheduledDate = DateTime.Parse(flightid.Element(ns + "ScheduledDate").Value);
                foreach (XElement AirlineDesignator in flightid.Elements(ns + "AirlineDesignator"))
                {
                    if (AirlineDesignator.Attribute("codeContext").Value.Contains("IATA"))
                        selFlight.AirlineDesignatorIATA = AirlineDesignator.Value;
                    else if (AirlineDesignator.Attribute("codeContext").Value.Contains("ICAO"))
                        selFlight.AirlineDesignatorICAO = AirlineDesignator.Value;
                }

                XElement flightState = FlightsElm.Element(ns + "FlightState");
                try
                {

                    DateTime scht;
                    DateTime.TryParse(flightState.Element(ns + "ScheduledTime").Value, out scht);
                    selFlight.ScheduledTime = scht;
                    selFlight.AircraftRegistration = flightState.Element(ns + "Aircraft").Element(ns + "AircraftId").Element(ns + "Registration").Value;
                    selFlight.AircraftType = flightState.Element(ns + "AircraftType").Element(ns + "Value").Value;

                    try
                    {
                        var linked = flightState.Element(ns + "LinkedFlight").Element(ns + "FlightId");

                        selFlight.LinkedFlight.flightKind = linked.Element(ns + "FlightKind").Value.ToString();
                        selFlight.LinkedFlight.flightNumber = linked.Element(ns + "FlightNumber").Value.ToString();
                        selFlight.LinkedFlight.scheduledDate = DateTime.Parse(linked.Element(ns + "ScheduledDate").Value);
                        foreach (XElement AirlineDesignator in linked.Elements(ns + "AirlineDesignator"))
                        {
                            if (AirlineDesignator.Attribute("codeContext").Value.Contains("IATA"))
                                selFlight.LinkedFlight.airlineDesignatorIATA = AirlineDesignator.Value;
                            else if (AirlineDesignator.Attribute("codeContext").Value.Contains("ICAO"))
                                selFlight.LinkedFlight.airlineDesignatorICAO = AirlineDesignator.Value;
                        }

                        var value = flightState.Element(ns + "LinkedFlight").Element(ns + "Value");
                        if (value.Attribute("propertyName").Value.Contains("FlightUniqueID"))
                        {
                            selFlight.LinkedFlightID = value.Value;
                        }


                    }
                    catch
                    {

                    }



                    foreach (XElement value in flightState.Elements(ns + "Value"))
                    {
                        try
                        {
                            if (value.Attribute("propertyName").Value.Contains("DataLocked"))
                            {
                                selFlight.DataLocked = bool.Parse(value.Value);
                            }
                            else if (value.Attribute("propertyName").Value.Contains("Arr_Qualifier"))
                            {
                                selFlight.Arr_Qualifier = value.Value;
                            }
                            else if (value.Attribute("propertyName").Value.Contains("arr_flight_type"))
                            {
                                selFlight.arr_flight_type = value.Value;
                            }
                            else if (value.Attribute("propertyName").Value.Contains("FlightUniqueID"))
                            {
                                selFlight.FlightUniqueID = value.Value;
                            }
                            else if (value.Attribute("propertyName").Value.Contains("ramp_id"))
                            {
                                selFlight.Ramp_id = value.Value;
                            }
                            else if (value.Attribute("propertyName").Value.Contains("AppLinkedFlightID"))
                            {
                                selFlight.AppLinkedFlightID = value.Value;
                            }
                            else if (value.Attribute("propertyName").Value.Contains("s---_ArrRouteOrigin"))
                            {
                                selFlight.filght_from = value.Value;
                            }
                            else if (value.Attribute("propertyName").Value.Contains("s---_DepRouteOrigin"))
                            {
                                selFlight.filght_to = value.Value;
                            }

                            else if (value.Attribute("propertyName").Value == "Capt")
                            {
                                selFlight.Capt = value.Value;
                            }
                            else if (value.Attribute("propertyName").Value.Contains("CrewTrip"))
                            {
                                selFlight.Crew = value.Value;
                            }
                            else if (value.Attribute("propertyName").Value.Contains("DOW"))
                            {
                                selFlight.DOW = value.Value;
                            }
                            else if (value.Attribute("propertyName").Value.Contains("RMPFUEL"))
                            {
                                selFlight.RMPFUEL = value.Value;
                            }
                            else if (value.Attribute("propertyName").Value.Contains("DOI"))
                            {
                                selFlight.DOI = value.Value;
                            }
                            else if (value.Attribute("propertyName").Value.Contains("CaptainSurname"))
                            {
                                selFlight.FaktFUEL = value.Value;
                            }
                            else if (value.Attribute("propertyName").Value.Contains("MTOWtrip"))
                            {
                                selFlight.MTOWtrip = value.Value;
                            }
                            else if (value.Attribute("propertyName").Value.Contains("MZFW"))
                            {
                                selFlight.MZFW = value.Value;
                            }
                            else if (value.Attribute("propertyName").Value.Contains("TAXI FUEL"))
                            {
                                selFlight.TAXIFUEL = value.Value;
                            }
                            else if (value.Attribute("propertyName").Value.Contains("T/O FUEL"))
                            {
                                selFlight.TOFUEL = value.Value;
                            }
                            else if (value.Attribute("propertyName").Value.Contains("MLW"))
                            {
                                selFlight.MLW = value.Value;
                            }
                            else if (value.Attribute("propertyName").Value.Contains("TRIP FUEL"))
                            {
                                selFlight.TRIPFUEL = value.Value;
                            }
                            else if (value.Attribute("propertyName").Value.Contains("EET"))
                            {
                                selFlight.EET = value.Value;
                            }
                            else if (value.Attribute("propertyName").Value == "TripInfoChanged")
                            {
                                selFlight.TripInfoChanged = Convert.ToBoolean(value.Value);
                            }
                            else if (value.Attribute("propertyName").Value == "TripInfoChangedCount")
                            {
                                selFlight.TripInfoChangedCount = Convert.ToInt16(value.Value);
                            }


                            else
                            {


                                var list = ServicesCheklist.listsets.Where(c => c.Needfname == value.Attribute("propertyName").Value).Select(c => c);
                                if (list.Count() > 0)
                                {
                                    Flight.flightServiceNeed listserv = new Flight.flightServiceNeed();
                                    listserv.name = list.First().Needfname;
                                    listserv.value = bool.Parse(value.Value);
                                    selFlight.NeedServices.Add(listserv);
                                }
                                else
                                {
                                    list = ServicesCheklist.listsets.Where(c => c.Fieldname == value.Attribute("propertyName").Value).Select(c => c);
                                    if (list.Count() > 0)
                                    {
                                        CLSet fclset = list.First();
                                        Flight.flihghtService listserv = new Flight.flihghtService();
                                        listserv.name = fclset.Fieldname;
                                        //listserv.fieldcountname = fclset.Fieldvalue;
                                        listserv.value = value.Value;
                                        selFlight.Services.Add(listserv);
                                    }
                                    else
                                    {
                                        list = ServicesCheklist.listsets.Where(c => c.Fieldvalue == value.Attribute("propertyName").Value).Select(c => c);
                                        if (list.Count() > 0)
                                        {

                                            CLSet fclset = list.First();
                                            Flight.flihghtService listserv = new Flight.flihghtService();
                                            listserv.name = fclset.Fieldvalue;
                                            //listserv.fieldcountname = fclset.Fieldvalue;
                                            listserv.value = value.Value;
                                            selFlight.Services.Add(listserv);
                                        }
                                    }
                                }



                            }

                        }
                        catch
                        {

                        }
                    }


                    foreach (XElement tablevalue in flightState.Elements(ns + "TableValue"))
                    {
                        try
                        {
                            var list = ServicesCheklist.listsets.Where(c => c.Tablename == tablevalue.Attribute("propertyName").Value).Select(c => c);
                            if (list.Count() > 0)
                            {
                                Flight.flihghtService listserv = new Flight.flihghtService();
                                listserv.name = list.First().Tablename;
                                listserv.tablevalue = new ObservableCollection<Flight.TableValue>();
                                foreach (XElement row in tablevalue.Elements(ns + "Row"))
                                {
                                    Flight.TableValue tablerow = new Flight.TableValue();
                                    foreach (XElement value in row.Elements(ns + "Value"))
                                    {
                                        if (value.Attribute("propertyName").Value.Contains("DATE_BEGIN"))
                                        {
                                            DateTime tempdate;
                                            DateTime.TryParse(value.Value, out tempdate);
                                            tablerow.date_begin = tempdate;
                                        }
                                        else if (value.Attribute("propertyName").Value.Contains("DATE_END"))
                                        {
                                            DateTime tempdate;
                                            DateTime.TryParse(value.Value, out tempdate);
                                            tablerow.date_end = tempdate;
                                        }
                                        else if (value.Attribute("propertyName").Value.Contains("DATE_ENTERED"))
                                        {
                                            DateTime tempdate;
                                            DateTime.TryParse(value.Value, out tempdate);
                                            tablerow.date_entered = tempdate;
                                        }
                                        else if (value.Attribute("propertyName").Value.Contains("TYPE_SERVICE"))
                                        {
                                            tablerow.type_service = value.Value;
                                        }
                                        else if (value.Attribute("propertyName").Value.Contains("REMARKS"))
                                        {
                                            tablerow.remarks = value.Value;
                                        }
                                        else if (value.Attribute("propertyName").Value.Contains("VALUE"))
                                        {
                                            double tempdouble;
                                            double.TryParse(value.Value, out tempdouble);
                                            tablerow.value = tempdouble;
                                        }
                                        else if (value.Attribute("propertyName").Value.Contains("DOC_NO"))
                                        {
                                            tablerow.doc_no = value.Value;
                                        }
                                        else if (value.Attribute("propertyName").Value.Contains("QUANTITY"))
                                        {
                                            tablerow.quantity = value.Value;
                                        }
                                        else if (value.Attribute("propertyName").Value.Contains("DENSITY"))
                                        {
                                            tablerow.desnity = value.Value;
                                        }
                                        else if (value.Attribute("propertyName").Value.Contains("Garage_Number"))
                                        {
                                            tablerow.refueler_num = value.Value;
                                        }

                                    }
                                    listserv.tablevalue.Add(tablerow);
                                }
                                selFlight.Services.Add(listserv);
                            }

                        }
                        catch
                        {

                        }
                    }

                    foreach (XElement activity in flightState.Elements(ns + "Activity"))
                    {
                        selFlight.ActivityEventExist = true;
                        Flight.structActivity elementActivity = new Flight.structActivity();
                        elementActivity.code = activity.Attribute("code").Value;
                        foreach (XElement value in activity.Elements(ns + "Value"))
                        {
                            if (value.Attribute("propertyName").Value.Contains("EstimatedStartTime"))
                            {
                                DateTime tempdate;
                                DateTime.TryParse(value.Value, out tempdate);
                                elementActivity.EstimatedStartTime = tempdate;
                            }
                            else if (value.Attribute("propertyName").Value.Contains("EstimatedEndTime"))
                            {
                                DateTime tempdate;
                                DateTime.TryParse(value.Value, out tempdate);
                                elementActivity.EstimatedEndTime = tempdate;
                            }
                            else if (value.Attribute("propertyName").Value.Contains("ActualStartTime"))
                            {
                                DateTime tempdate;
                                DateTime.TryParse(value.Value, out tempdate);
                                elementActivity.ActualStartTime = tempdate;
                            }
                            else if (value.Attribute("propertyName").Value.Contains("ActualEndTime"))
                            {
                                DateTime tempdate;
                                DateTime.TryParse(value.Value, out tempdate);
                                elementActivity.ActualEndTime = tempdate;
                            }
                            else if (value.Attribute("propertyName").Value.Contains("ScheduledStartTime"))
                            {
                                DateTime tempdate;
                                DateTime.TryParse(value.Value, out tempdate);
                                elementActivity.ScheduledStartTime = tempdate;
                            }
                            else if (value.Attribute("propertyName").Value.Contains("CalculatedStartTime"))
                            {
                                DateTime tempdate;
                                DateTime.TryParse(value.Value, out tempdate);
                                elementActivity.CalculatedStartTime = tempdate;
                            }
                            else if (value.Attribute("propertyName").Value.Contains("MostConfidentStartTime"))
                            {
                                DateTime tempdate;
                                DateTime.TryParse(value.Value, out tempdate);
                                elementActivity.MostConfidentStartTime = tempdate;
                            }
                            else if (value.Attribute("propertyName").Value.Contains("ScheduledEndTime"))
                            {
                                DateTime tempdate;
                                DateTime.TryParse(value.Value, out tempdate);
                                elementActivity.ScheduledEndTime = tempdate;
                            }
                            else if (value.Attribute("propertyName").Value.Contains("CalculatedEndTime"))
                            {
                                DateTime tempdate;
                                DateTime.TryParse(value.Value, out tempdate);
                                elementActivity.CalculatedEndTime = tempdate;
                            }
                            else if (value.Attribute("propertyName").Value.Contains("MostConfidentEndTime"))
                            {
                                DateTime tempdate;
                                DateTime.TryParse(value.Value, out tempdate);
                                elementActivity.MostConfidentEndTime = tempdate;
                            }
                        }
                        selFlight.Activity.Add(elementActivity);
                    }

                    foreach (XElement varevent in flightState.Elements(ns + "Event"))
                    {
                        Flight.structEvent elementEvent = new Flight.structEvent();
                        elementEvent.code = varevent.Attribute("code").Value;
                        foreach (XElement value in varevent.Elements(ns + "Value"))
                        {
                            if (value.Attribute("propertyName").Value.Contains("EstimatedTime"))
                            {
                                DateTime tempdate;
                                DateTime.TryParse(value.Value, out tempdate);
                                elementEvent.EstimatedTime = tempdate;
                            }
                            else if (value.Attribute("propertyName").Value.Contains("ActualTime"))
                            {
                                DateTime tempdate;
                                DateTime.TryParse(value.Value, out tempdate);
                                elementEvent.ActualTime = tempdate;
                            }
                            else if (value.Attribute("propertyName").Value.Contains("ScheduledTime"))
                            {
                                DateTime tempdate;
                                DateTime.TryParse(value.Value, out tempdate);
                                elementEvent.ScheduledTime = tempdate;
                            }
                            else if (value.Attribute("propertyName").Value.Contains("CalculatedTime"))
                            {
                                DateTime tempdate;
                                DateTime.TryParse(value.Value, out tempdate);
                                elementEvent.CalculatedTime = tempdate;
                            }
                            else if (value.Attribute("propertyName").Value.Contains("MostConfidentTime"))
                            {
                                DateTime tempdate;
                                DateTime.TryParse(value.Value, out tempdate);
                                elementEvent.MostConfidentTime = tempdate;
                            }
                        }
                        selFlight.Event.Add(elementEvent);
                    }
                }
                catch
                {

                }
            }
            catch
            {
                return selFlight;
            }

            return selFlight;

        }

        
    }
}
