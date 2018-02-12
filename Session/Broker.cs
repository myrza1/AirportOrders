
using Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session
{
    public class Broker
    {
        SqlConnection connection, connection2;
        SqlCommand command, command2;
        string conStr = ConfigurationManager.ConnectionStrings["AirportOrders.Properties.Settings.eAirlinesConnectionString"].ToString();
        string conStr2 = ConfigurationManager.ConnectionStrings["AirportOrders.Properties.Settings.eAirlinesConnectionString"].ToString();

        private void ConnectTo(String Tasil)
        {
            //connection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\database\Database1.mdb;Persist Security Info=False");
            //connection = new OleDbConnection(@"ovider=SQLOLEDB.1;Password=Comnet8273;Persist Security Info=False;User ID=sa;Initial Catalog=ECLIPSXDB;Data Source=10.10.10.2");

            connection = new SqlConnection(conStr);
            connection2 = new SqlConnection(conStr2);

            command = connection.CreateCommand();
            command2 = connection2.CreateCommand();

        }

        public Broker(String Tasil = null)
        {
            ConnectTo(Tasil);
        }

        // чтение из файла
        public string LoadOccupation(string TypeMessage)
        {
            string input;
            try
            {

                // создание объекта StreamReader
                StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "occupation" + TypeMessage + ".txt");
                input = sr.ReadLine(); // построчное чтение    

                sr.Close();
            }
            catch (Exception e)
            {
                WriteMyErrorLOG("Load ocypation " + TypeMessage + " " + e);
                input = "";
            }
            return input;
        }

        public string LoadFFMBlank(string TypeMessage)
        {
            string input;
            try
            {


                using (StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + TypeMessage + ".txt"))
                {
                    input = sr.ReadToEnd();
                }

            }
            catch (Exception e)
            {
                WriteMyErrorLOG("LoadFFMBlank " + TypeMessage + " " + e);
                input = "";
            }
            return input;
        }
        // запись в файл





     
        public void WriteMyLOG(String NewTexxt)
        {
            DateTime bugin = DateTime.Now;
            String yourText = bugin.ToString() + " " + NewTexxt + Environment.NewLine;
            File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "Session.log", yourText);
        }
        public void WriteMonitoring(String NewTexxt)
        {
            DateTime bugin = DateTime.Now;
            String yourText = bugin.ToString() + " " + NewTexxt + Environment.NewLine;
            File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "Monitoring.log", yourText);
        }

        public static void WriteMyErrorLOG(String NewTexxt)
        {
            DateTime bugin = DateTime.Now;
            String yourText = bugin.ToString() + " " + NewTexxt + Environment.NewLine;
            File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "Session.Error.log", yourText);
        }

        public DateTime ConvertLocal(string dayStr, string timeStr)
        {
            DateTime dateTOday = DateTime.Now;
            DateTime dateNeed;
            string convertedDate;
            if (dayStr == "")
            {
                dayStr = Convert.ToString(dateTOday.Day);
            }
            convertedDate = dateTOday.Year + "/" + dateTOday.Month + "/" + dayStr + " " + timeStr.Substring(0, 2)
                + ":" + timeStr.Substring(2, 2);
            dateNeed = Convert.ToDateTime(convertedDate);
            dateTOday = dateNeed.AddHours(6); //UTC ғып жасаймыз
            return dateTOday;
        }

     

        public DateTime LDMDayOfDepartureDate(string dayStr, string FlightNumber)
        {
            //DateTime dateTOday = DateTime.Now;
            //DateTime dateNeed;
            //string timeStr = dateTOday.ToString("HHmm");
            //string convertedDate;
            //if (dateTOday.Day > Convert.ToInt32(dayStr))
            //{
            //    dayStr = Convert.ToString(dateTOday.Day);
            //}
            //convertedDate = dateTOday.Year + "/" + dateTOday.Month + "/" + dayStr + " " + timeStr.Substring(0, 2)
            //    + ":" + timeStr.Substring(2, 2);
            //dateNeed = Convert.ToDateTime(convertedDate);
            //dateTOday = dateNeed; //
            //return dateTOday;

            DateTime dateTOday = DateTime.Now;
            DateTime dateKErek = new DateTime(dateTOday.Year, dateTOday.Month, 01); //Convert.ToInt32(dayStr)
            dateKErek = dateKErek.AddDays(Convert.ToInt32(dayStr) - 1);

            if (dateKErek > dateTOday)
            {
                dateKErek = new DateTime(dateTOday.Year, dateTOday.Month, 01);
                dateKErek = dateKErek.AddMonths(-1);
                dateKErek = dateKErek.AddDays(Convert.ToInt32(dayStr) - 1);
            }
            dateKErek = dateKErek.AddHours(5);
            ///Деректер қорынан рейст UTC +06 диапазон арқылы табамыз
            /// 
            return FlightRadar(dateKErek, 2, FlightNumber);


        }
     

        public DateTime FlightRadar(DateTime dateKerek, int IsDepOrArr, string FlightNumber)
        {

            //  List<DateTime> MessageList = new List<DateTime>();
            String Sdhed = "";
            DateTime dataUshipKelu, dataKeluSchedule;
            try
            {
                if (IsDepOrArr == 1) //Бұл ұшып келулерге 
                {
                    DateTime endDate = dateKerek.AddHours(36);
                    command.CommandText = "SELECT aodb.ArrivalFlight.arrivalFlightId, aodb.ArrivalFlight.flightNumber, aodb.ArrivalFlight.airlineId, aodb.ArrivalFlight.ATTR_7 as estimated, aodb.Airline.iataCode, " +
                      "aodb.ArrivalFlight.scheduledTime, aodb.ArrivalFlight.ATTR_380 AS R1STD, aodb.ArrivalFlight.ATTR_371 AS ACTUAL, aodb.ArrivalFlight.ATTR_381 AS R1ADT, " +
                      " ABS( DATEDIFF(MI,CONVERT (time, getdate()), CONVERT (time,  aodb.ArrivalFlight.scheduledTime))) as times " +
                      "FROM aodb.ArrivalFlight INNER JOIN aodb.Airline ON aodb.ArrivalFlight.airlineId = aodb.Airline.airlineId"
                        + " WHERE (aodb.ArrivalFlight.flightNumber = N'" + FlightNumber
                        + "') AND ((aodb.ArrivalFlight.scheduledTime BETWEEN CONVERT(DATETIME, '" + dateKerek.ToString("yyyy-MM-dd hh:mm:ss") + "', 102) " +
                        "AND CONVERT(DATETIME, '" + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "', 102) AND aodb.ArrivalFlight.ATTR_371 IS NULL) " +
                        " OR(aodb.ArrivalFlight.ATTR_371 BETWEEN CONVERT(DATETIME, '" + dateKerek.ToString("yyyy-MM-dd hh:mm:ss") + "', 102) " +
                        " AND CONVERT(DATETIME, '" + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "', 102))) " +
                        " and aodb.ArrivalFlight.isDeleted = 0 order by times";
                }
                //" AND (aodb.ArrivalFlight.scheduledTime BETWEEN CONVERT(DATETIME, '" + startDate
                //+ "', 102) AND CONVERT(DATETIME, '" + endDate
                //+ "', 102))";

                else if (IsDepOrArr == 2) //Бұл ұшулаларға
                {
                    DateTime endDate = dateKerek.AddHours(24);
                    command.CommandText = "SELECT aodb.DepartureFlight.DepartureFlightId, aodb.DepartureFlight.flightNumber," +
                                          " aodb.DepartureFlight.airlineId, aodb.Airline.iataCode, aodb.DepartureFlight.scheduledTime" +
                                          " FROM aodb.DepartureFlight INNER JOIN aodb.Airline ON aodb.DepartureFlight.airlineId = aodb.Airline.airlineId" +
                                            " WHERE (aodb.DepartureFlight.flightNumber IN (N'0" + FlightNumber + "', N'" + FlightNumber + "')) AND" +
                                            " aodb.DepartureFlight.scheduledTime BETWEEN CONVERT(DATETIME,'" + dateKerek.ToString("yyyy-MM-dd hh:mm:ss") + "', 102)" +
                                            " AND CONVERT(DATETIME,'" + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "', 102)" +
                                            " and aodb.DepartureFlight.isDeleted = 0 ORDER BY scheduledTime";
                }
                //command.CommandType = CommandType.Text;

                connection.Open();

                using (SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        DateTime buferdata;
                        if (IsDepOrArr == 1)
                        {
                            Sdhed = dr["ACTUAL"].ToString().Equals("") ? dr["scheduledTime"].ToString() : dr["ACTUAL"].ToString();
                            dataUshipKelu = Convert.ToDateTime(Sdhed);
                            dataKeluSchedule = Convert.ToDateTime(dr["scheduledTime"].ToString());
                            //Sdhed = dataUshipKelu.Hour*100 + dataUshipKelu.Minute; //hhmm
                            int Ushu = dr["R1ADT"].ToString().Equals("") ? Convert.ToInt32(dr["R1STD"]) : Convert.ToInt32(dr["R1ADT"]);

                            //1 - жағдай
                            if (Ushu < 600)
                            {
                                buferdata = dateKerek.AddDays(1);
                            }
                            //2 - жағдай
                            else if (dataUshipKelu.Hour * 100 + dataUshipKelu.Minute > Ushu)
                            {
                                buferdata = dateKerek;
                            }
                            //3 - жағдай
                            else
                            {
                                buferdata = dateKerek.AddDays(1);
                            }

                            if (buferdata.Day == dataUshipKelu.Day)
                            {
                                //+
                                return dataKeluSchedule;
                            }

                        }
                        else if (IsDepOrArr == 2)
                        {
                            dataKeluSchedule = Convert.ToDateTime(dr["scheduledTime"].ToString());
                            return dataKeluSchedule;
                        }

                    }
                }
                if (command.CommandText.StartsWith("UPDATE"))
                {
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    //messageList.Add(command.CommandText);
                }

            }
            catch (Exception)
            {
                //           throw;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
            return dateKerek; //Если не нашел или не законектился

        }


        public List<Flight> FillListBoxFFMfromAMS(ObservableCollection<Flight> selF)
        {
            List<Flight> messageList = new List<Flight>();
            try
            {
                //command2.CommandText = "SELECT *  FROM [AMSDatawarehouse].[amsdwView].[factmovement_Departure_TableAttribute_Departures AWB]  where Id = " + selF.FlightNumber;

                connection2.Open();

                using (SqlDataReader dr = command2.ExecuteReader(CommandBehavior.CloseConnection))
                {

                    while (dr.Read())
                    {
                        Flight selFFM = new Flight();
                        //selFlight.FlightATO = dr["ATO"].ToString().Equals("") ? dr["scheduledTime"].ToString() : dr["ACTUAL"].ToString();
                        //selFFM.FlightNumber = dr["Id"].ToString();
                        //selFFM.Pieces = dr["Oryn"].ToString();
                        //selFFM.Weight = dr["Weight"].ToString();
                        //selFFM.Goods = dr["Remarks"].ToString();
                        //selFFM.AwbSerialNumber = dr["AWB Number"].ToString();
                        //selFFM.VolumeCode = dr["Measument type"].ToString();
                        //selFFM.Volume = dr["Measument Value"].ToString();
                        //selFFM.Origin = dr["Origin IATA"].ToString();
                        selFFM.AirlineDesignatorIATA = dr["Destination IATA"].ToString();
                        //selFFM.ShipmentDescriptionCode = dr["AWB Types"].ToString();

                        //selFlight.Departure_FFM_NTS1 = Convert.ToBoolean(dr["Departure_UPDATE FFM"]);
                        //Sdhed = dataUshipKelu.Hour*100 + dataUshipKelu.Minute; //hhmm
                        //int Ushu = dr["R1ADT"].ToString().Equals("") ? Convert.ToInt32(dr["R1STD"]) : Convert.ToInt32(dr["R1ADT"]);
                        messageList.Add(selFFM);
                    }
                }

                if (command2.CommandText.StartsWith("UPDATE"))
                {
                    command2.Connection.Open();
                    command2.ExecuteNonQuery();
                    //messageList.Add(command.CommandText);
                }

                return messageList;
            }
            catch (Exception e)
            {
              //  WriteMyErrorLOG("Мен қате жібердім (" + selF.FlightNumber + ") " + e.ToString());
                return messageList;
            }
            finally
            {
                if (connection2 != null)
                {
                    connection2.Close();
                }

            }
        }

        
    }

}
