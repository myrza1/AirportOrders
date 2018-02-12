using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Flight
    {
        
        //FlightID
        private flightID IDFlight;

        //FLight State

        private DateTime scheduledTime;
        private string aircraftRegistration;
        private string _arr_flight_type;
        private string arr_Qualifier;
        private bool dataLocked;
        private string flightUniqueID;
        private string linkedFlightID;
        public flightID LinkedFlight;
        private string appLinkedFlightID;
        private string ramp_id;
        public ObservableCollection<structActivity> Activity;
        public ObservableCollection<structEvent> Event;
        private bool activityEventExist = false;


        //Сервисные услуги
        private ObservableCollection<flihghtService> services;
        private ObservableCollection<flightServiceNeed> needServices;


        public string AircraftType
        {
            get
            {
                return IDFlight.AircraftType;
            }

            set
            {
                IDFlight.AircraftType = value;
            }
        }

        public string filght_to
        {
            get
            {
                return IDFlight.flight_to;
            }
            set
            {
                IDFlight.flight_to = value;
            }
        }

        public string Capt
        {
            get
            {
                return IDFlight.Capt;
            }
            set
            {
                IDFlight.Capt = value;
            }
        }



        public string Crew
        {
            get
            {
                return IDFlight.Crew;
            }
            set
            {
                IDFlight.Crew = value;
            }
        }

        public string DOW
        {
            get
            {
                return IDFlight.DOW;
            }
            set
            {
                IDFlight.DOW = value;
            }
        }
        public string RMPFUEL
        {
            get
            {
                return IDFlight.RMPFUEL;
            }
            set
            {
                IDFlight.RMPFUEL = value;
            }
        }
        public string DOI
        {
            get
            {
                return IDFlight.DOI;
            }
            set
            {
                IDFlight.DOI = value;
            }
        }
        public string FaktFUEL
        {
            get
            {
                return IDFlight.FaktFUEL;
            }
            set
            {
                IDFlight.FaktFUEL = value;
            }
        }

        public string MTOWtrip
        {
            get
            {
                return IDFlight.MTOWtrip;
            }
            set
            {
                IDFlight.MTOWtrip = value;
            }
        }
        public string MZFW
        {
            get
            {
                return IDFlight.MZFW;
            }
            set
            {
                IDFlight.MZFW = value;
            }
        }
        public string TAXIFUEL
        {
            get
            {
                return IDFlight.TAXIFUEL;
            }
            set
            {
                IDFlight.TAXIFUEL = value;
            }
        }
        public string TOFUEL
        {
            get
            {
                return IDFlight.TOFUEL;
            }
            set
            {
                IDFlight.TOFUEL = value;
            }
        }
        public string MLW
        {
            get
            {
                return IDFlight.MLW;
            }
            set
            {
                IDFlight.MLW = value;
            }
        }
        public string TRIPFUEL
        {
            get
            {
                return IDFlight.TRIPFUEL;
            }
            set
            {
                IDFlight.TRIPFUEL = value;
            }
        }
        public string EET
        {
            get
            {
                return IDFlight.EET;
            }
            set
            {
                IDFlight.EET = value;
            }
        }

        public bool TripInfoChanged
        {
            get
            {
                return IDFlight.TripInfoChanged;
            }
            set
            {
                IDFlight.TripInfoChanged = value;
            }
        }

        public int TripInfoChangedCount
        {
            get
            {
                return IDFlight.TripInfoChangedCount;
            }
            set
            {
                IDFlight.TripInfoChangedCount = value;
            }
        }

        public string filght_from
        {
            get
            {
                return IDFlight.filght_from;
            }
            set
            {
                IDFlight.filght_from = value;
            }
        }


        public string FlightKind
        {
            get
            {
                return IDFlight.flightKind;
            }

            set
            {
                IDFlight.flightKind = value;
            }
        }
        public string AirlineDesignatorIATA
        {
            get
            {
                return IDFlight.airlineDesignatorIATA;
            }

            set
            {
                IDFlight.airlineDesignatorIATA = value;
            }
        }
        public string AirlineDesignatorICAO
        {
            get
            {
                return IDFlight.airlineDesignatorICAO;
            }

            set
            {
                IDFlight.airlineDesignatorICAO = value;
            }
        }
        public string FlightNumber
        {
            get
            {
                return IDFlight.flightNumber;
            }

            set
            {
                IDFlight.flightNumber = value;
            }
        }
        public DateTime ScheduledDate
        {
            get
            {
                return IDFlight.scheduledDate;
            }

            set
            {
                IDFlight.scheduledDate = value;
            }
        }

        public DateTime ScheduledTime
        {
            get
            {
                return scheduledTime;
            }

            set
            {
                scheduledTime = value;
            }
        }
        public string AircraftRegistration
        {
            get
            {
                return aircraftRegistration;
            }

            set
            {
                aircraftRegistration = value;
            }
        }
        public string arr_flight_type
        {
            get
            {
                return _arr_flight_type;
            }

            set
            {
                _arr_flight_type = value;
            }
        }
        public string Arr_Qualifier
        {
            get
            {
                return arr_Qualifier;
            }

            set
            {
                arr_Qualifier = value;
            }
        }
        public bool DataLocked
        {
            get
            {
                return dataLocked;
            }

            set
            {
                dataLocked = value;
            }
        }

        public ObservableCollection<flightServiceNeed> NeedServices
        {
            get
            {
                return needServices;
            }

            set
            {
                needServices = value;
            }
        }

        public ObservableCollection<flihghtService> Services
        {
            get
            {
                return services;
            }

            set
            {
                services = value;
            }
        }

        public static string AirportCodeIATA
        {
            get
            {
                return flightID.airportCodeIATA;
            }
        }

        public static string AirportCodeICAO
        {
            get
            {
                return flightID.airportCodeICAO;
            }
        }

        public string FlightUniqueID
        {
            get
            {
                return flightUniqueID;
            }

            set
            {
                flightUniqueID = value;
            }
        }

        public string LinkedFlightID
        {
            get
            {
                return linkedFlightID;
            }

            set
            {
                linkedFlightID = value;
            }
        }

        public string Ramp_id
        {
            get
            {
                return ramp_id;
            }

            set
            {
                ramp_id = value;
            }
        }

        public string AppLinkedFlightID
        {
            get
            {
                return appLinkedFlightID;
            }

            set
            {
                appLinkedFlightID = value;
            }
        }

        public bool ActivityEventExist
        {
            get
            {
                return activityEventExist;
            }

            set
            {
                activityEventExist = value;
            }
        }

        public override string ToString()
        {

            return IDFlight.flightNumber + " Date: " + IDFlight.scheduledDate.ToString();
            //FlightNumber + " reg: " + Registration + " Board Airport: " + Registration + " flightID: " + Convert.ToString(FlightSTA) +  " TypeB Address: " + TypeBADDRESS + " qual: " + Qual + " flightSTO: " + FlightSTO;
        }

        public string fullname
        {
            get
            {
                return AirlineDesignatorIATA + FlightNumber;
            }
        }
        public struct flihghtService
        {
            //public 
            public string name;
            public string value;
            //public string fieldcountname;
            //public string valuecount;

            public ObservableCollection<TableValue> tablevalue;
        }

        public struct TableValue
        {
            public string type_service;
            public double value;
            public DateTime date_entered;
            public DateTime date_begin;
            public DateTime date_end;
            public string doc_no;
            public string remarks;
            public string refueler_num;
            public string quantity;
            public string desnity;
        }

        public struct flightServiceNeed
        {
            public string name;
            public bool value;
        }

        public struct flightID
        {
            public string flightKind;
            public string airlineDesignatorIATA;
            public string airlineDesignatorICAO;
            public string flightNumber;
            public string filght_from;
            public string flight_to;

            public string Capt;
            public string Crew;
            public string DOW;
            public string RMPFUEL;
            public string DOI;
            public string FaktFUEL;
            public string MTOWtrip;
            public string MZFW;
            public string TAXIFUEL;
            public string TOFUEL;
            public string MLW;
            public string TRIPFUEL;
            public string EET;
            public bool TripInfoChanged;
            public int TripInfoChangedCount;



            public string AircraftType;
            public DateTime scheduledDate;
            public const string airportCodeIATA = "TSE";
            public const string airportCodeICAO = "UACC";
            public string fullname
            {
                get
                {
                    return flightNumber;

                }

            }
        }

        public struct structActivity
        {
            public string code;
            public DateTime EstimatedStartTime;
            public DateTime EstimatedEndTime;
            public DateTime ActualStartTime;
            public DateTime ActualEndTime;
            public DateTime ScheduledStartTime;
            public DateTime ScheduledEndTime;
            public DateTime MostConfidentStartTime;
            public DateTime MostConfidentEndTime;
            public DateTime CalculatedStartTime;
            public DateTime CalculatedEndTime;
        }

        public struct structEvent
        {
            public string code;
            public DateTime ActualTime;
            public DateTime EstimatedTime;
            public DateTime ScheduledTime;
            public DateTime CalculatedTime;
            public DateTime MostConfidentTime;
        }

    }

}

