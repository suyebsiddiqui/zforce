﻿using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows;

namespace cafe_manager
{
    class DbConnector
    {
        private static MySqlConnection sqlConnection;
        private static string server;
        private static string database;
        private static string Userid;
        private static string password;


        //Constructor to create CONNECTION STRING 
        public DbConnector()
        {
            //server = "192.168.0.2";
            server = "localhost";
            database = "cafe_manager";
            Userid = "root";
            password = "root";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + Userid + ";" + "PASSWORD=" + password + ";";

            sqlConnection = new MySqlConnection(connectionString);
        }


        // To open the connection
        private bool OpenConnection()
        {
            try
            {
                sqlConnection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }


        //To close the connection
        private bool CloseConnection()
        {
            try
            {
                sqlConnection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }

        //To Register User
        public bool registerUser(User user)
        {

            string Getusercount = "select count(UserId) from user";
            string Getcafename = "select CafeName from cafe";
            //Create a list to store the result
            int usercount = 0;
            string cafename = null;
            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(Getusercount, sqlConnection);
                usercount = Convert.ToInt32(cmd.ExecuteScalar());

                //close Connection
                MySqlCommand cmd1 = new MySqlCommand(Getcafename, sqlConnection);

                MySqlDataReader dataReader = cmd1.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {

                    cafename = dataReader["CafeName"].ToString();

                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

            }
            usercount++;


            String userId = cafename + "u" + usercount;

            String query = "INSERT INTO user(UserId,UserName,Pass,FirstName, LastName, Email1, Mobile1, AddressLine1, AddressLine2, City, State, Country, Pincode,IsUserLoggedIn,WalletId) VALUES(" + "'" + userId + "'" + "," + "'" + user.Name + "'" + "," + "'" + user.Password + "'" + "," + "'" + user.Email + "'" + "," + "'" + user.Mobile + "'" + "," + "'" + user.City + "'" + "," + "'" + user.State + "'" + "," + "'" + user.Country + "'" + "," + "'" + user.Pincode + "'" + ")";
            String query1 = "Insert into wallet (WalletId,WalletOfflieAmount) Values (" + "'" + userId + "'" + "," + "'" + 0 + "'" + ")";
            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query1, sqlConnection);

                //Execute command
                cmd.ExecuteNonQuery();


                //close connection
                this.CloseConnection();
                this.OpenConnection();
                MySqlCommand cmd1 = new MySqlCommand(query, sqlConnection);
                cmd1.ExecuteNonQuery();
                this.CloseConnection();
                return true;
            }

            return false;
        }

        // To get the user Details by Id
        public User getUserDetailsById(User user)
        {
            // code to get the user details from the user id
            string query = "SELECT * from user where userId=" + "'" + user.UserId + "'" + "";
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, sqlConnection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {

                    user.UserId = dataReader["userId"].ToString();
                    user.Name = dataReader["name"].ToString();
                    user.Username = dataReader["userName"].ToString();
                    user.Email = dataReader["email1"].ToString();
                    user.Mobile = dataReader["mobile"].ToString();
                    user.City = dataReader["City"].ToString();
                    user.State = dataReader["State"].ToString();
                    user.Country = dataReader["Country"].ToString();
                    user.Pincode = dataReader["Pincode"].ToString();
                    user.Dob = Convert.ToDateTime(dataReader["Dob"].ToString());

                }

                query = "Select * from wallet where WalletId =  " + "'" + user.UserId + "'" + "";
                cmd = new MySqlCommand(query, sqlConnection);
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    user.WalletAmount = Convert.ToDecimal(dataReader["WalletOfflieAmount"].ToString());
                }
                //close connection
                dataReader.Close();
                this.CloseConnection();
            }
            return user;
        }

        // To get the user Details by Id
        public User getUserDetailsByName(User user)
        {
            // code to get the user details from the user id
            string query = "SELECT * from user where userId=" + "'" + user.Username + "'" + "";
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, sqlConnection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {

                    user.UserId = dataReader["userId"].ToString();
                    user.Name = dataReader["name"].ToString();
                    user.Username = dataReader["userName"].ToString();
                    user.Email = dataReader["email1"].ToString();
                    user.Mobile = dataReader["mobile"].ToString();
                    user.City = dataReader["City"].ToString();
                    user.State = dataReader["State"].ToString();
                    user.Country = dataReader["Country"].ToString();
                    user.Pincode = dataReader["Pincode"].ToString();
                    user.Dob = Convert.ToDateTime(dataReader["Dob"].ToString());

                }

                query = "Select * from wallet where WalletId =  " + "'" + user.UserId + "'" + "";
                cmd = new MySqlCommand(query, sqlConnection);
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    user.WalletAmount = Convert.ToDecimal(dataReader["WalletOfflieAmount"].ToString());
                }
                //close connection
                dataReader.Close();
                this.CloseConnection();
            }
            return user;
        }

        // To authenticate user
        public bool authenticateUser(String username, String password)
        {
            /*Query if username exists, If exists then compare the password, If matches return true otherwise false*/

            string query = "SELECT * from user where UserName=" + "'" + username + "'" + "And" + " Pass=" + "'" + password + "'" + "";


            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, sqlConnection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                if (dataReader.HasRows)
                {
                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    this.CloseConnection();

                    return true;
                }
                else
                {
                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    this.CloseConnection();
                    return false;
                }
            }
            return false;
        }

        //To check if the username is unique or not
        public bool checkUsername(String username)
        {
            //query database if the username is unique or not
            string query = "SELECT * from user where UserName=" + "'" + username + "'" + "";
            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, sqlConnection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                if (dataReader.HasRows)
                {

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    this.CloseConnection();

                    return true;
                }
                else
                {
                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    this.CloseConnection();
                    return false;
                }
            }
            return false;

        }

        //To check if the mobile is unique or not
        public bool checkMobile(int mobile)
        {
            //query if the mobile number is unique or not
            string query = "SELECT * from user where Mobile1=" + "'" + mobile + "'" + "";
            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, sqlConnection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                if (dataReader.HasRows)
                {
                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    this.CloseConnection();

                    return true;
                }
                else
                {
                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    this.CloseConnection();
                    return false;
                }
            }
            return false;
        }

        //To check if the Email is unique or not
        public bool checkEmail(String email)
        {
            //Query database for the email uniqueness check

            string query = "SELECT * from user where Email1=" + "'" + email + "'" + "";
            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, sqlConnection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                if (dataReader.HasRows)
                {
                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    this.CloseConnection();

                    return true;
                }
                else
                {
                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    this.CloseConnection();
                    return false;
                }
            }
            return false;
        }
    
        
        
////end braces
  }
}
