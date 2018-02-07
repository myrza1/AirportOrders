using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportOrders.Models
{
    public static class ServicesCheklist
    {
        public static ObservableCollection<CLSet> listsets;
    }

    public class CLSet
    {
        
        private string name;
        private string fieldname;
        private string fieldvalue;
        private string tablename;
        private string needfname;
        private string flightKind;
        private short type;
        private ObservableCollection<typeService> listTypeService;
        private string activityPrepareTime;
        private string activityStartTime;
        private string activityEndTime;
        private string eventPrepareTime;
        private string eventStartTime;
        private string eventEndTime;
        private string category;
        private string addpossible;
        private string typenumeric;
       



        #region Incapsulation
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public string Fieldname
        {
            get
            {
                return fieldname;
            }

            set
            {
                fieldname = value;
            }
        }


        public short Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }

        public string Needfname
        {
            get
            {
                return needfname;
            }

            set
            {
                needfname = value;
            }
        }

        public string Tablename
        {
            get
            {
                return tablename;
            }

            set
            {
                tablename = value;
            }
        }

        public ObservableCollection<typeService> ListTypeService
        {
            get
            {
                return listTypeService;
            }

            set
            {
                listTypeService = value;
            }
        }

        public string FlightKind
        {
            get
            {
                return flightKind;
            }

            set
            {
                flightKind = value;
            }
        }

        public string ActivityPrepareTime
        {
            get
            {
                return activityPrepareTime;
            }

            set
            {
                activityPrepareTime = value;
            }
        }

        internal string ActivityStartTime
        {
            get
            {
                return activityStartTime;
            }

            set
            {
                activityStartTime = value;
            }
        }

        internal string ActivityEndTime
        {
            get
            {
                return activityEndTime;
            }

            set
            {
                activityEndTime = value;
            }
        }

        public string EventPrepareTime
        {
            get
            {
                return eventPrepareTime;
            }

            set
            {
                eventPrepareTime = value;
            }
        }

        public string EventStartTime
        {
            get
            {
                return eventStartTime;
            }

            set
            {
                eventStartTime = value;
            }
        }

        public string EventEndTime
        {
            get
            {
                return eventEndTime;
            }

            set
            {
                eventEndTime = value;
            }
        }

        public string Category
        {
            get
            {
                return category;
            }

            set
            {
                category = value;

            }
        }

        public string Fieldvalue
        {
            get
            {
                return fieldvalue;
            }

            set
            {
                fieldvalue = value;
            }
        }
        public string Addpossible
        {
            get
            {
                return addpossible;
            }

            set
            {
                addpossible = value;
            }
        }

        public string typeNumeric
        {
            get
            {
                return typenumeric;
            }

            set
            {
                typenumeric = value;
            }
        }
        #endregion



        public struct typeService
        {
            public string name;
        }

        public void addType(string name)
        {
            typeService item;
            item.name = name;
            listTypeService.Add(item);
        }
    }
}
