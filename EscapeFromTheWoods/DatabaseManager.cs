﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace EscapeFromTheWoods
{
    public class DatabaseManager
    {
        private readonly string _connectionString;
        public DatabaseManager(string connectionstring)
        {
            this._connectionString = connectionstring;
        }
        private SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            return connection;
        }
        public int GetWoodId()
        {
            SqlConnection connection = GetConnection();
            string query = "SELECT max(woodid) as woodid FROM woodrecords";
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = query;
            connection.Open();
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                int woodId = (object)reader["woodid"] == DBNull.Value ? woodId = 0 : (int)reader["woodId"] + 1;
                reader.Close();
                return woodId;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 0;
            }
            finally
            {
                connection.Close();
            }
        }
        public int GetMonkeyId()
        {
            SqlConnection connection = GetConnection();
            string query = "SELECT max(monkeyid) as monkeyid FROM monkeyrecords";
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = query;
            connection.Open();
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                int monkeyId = (object)reader["monkeyid"] == DBNull.Value ? monkeyId = 0 : (int)reader["monkeyid"] + 1;
                return monkeyId;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 0;
            }
            finally
            {
                connection.Close();
            }
        }
        public async Task AddWoodRecords(Bos bos)
        {
            Console.WriteLine("start adding Wood to DB");
            SqlConnection connection = GetConnection();
            string query = "INSERT INTO woodrecords(woodid, treeid, x, y) VALUES(@woodid, @treeid, @x, @y)";
            foreach (Boom boom in bos.Bomen)
            {
                using SqlCommand command = connection.CreateCommand();
                connection.Open();
                try
                {
                    command.Parameters.Add(new SqlParameter("@woodid", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@treeid", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@x", SqlDbType.Float));
                    command.Parameters.Add(new SqlParameter("@y", SqlDbType.Float));
                    command.CommandText = query;
                    command.Parameters["@woodid"].Value = bos.Id;
                    command.Parameters["@treeid"].Value = boom.Id;
                    command.Parameters["@x"].Value = boom.X;
                    command.Parameters["@y"].Value = boom.Y;
                    command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    connection.Close();
                }
            }
            Console.WriteLine("stop adding Wood to DB");
        }
        public async Task AddMonkeyRecords(int woodId, Log log)
        {
            Console.WriteLine($"start adding {log.Aap.Naam} to DB.monkeyrecords seqnr: {log.SeqNr}");
            SqlConnection connection = GetConnection();
            string query = "INSERT INTO monkeyrecords(monkeyid, monkeyname, woodid, seqnr, treeid, x, y) VALUES(@monkeyid, @monkeyname, @woodid, @seqnr, @treeid, @x, @y)";

            using SqlCommand command = connection.CreateCommand();
            connection.Open();
            try
            {
                command.Parameters.Add(new SqlParameter("@monkeyid", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@monkeyname", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@woodid", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@seqnr", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@treeid", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@x", SqlDbType.Float));
                command.Parameters.Add(new SqlParameter("@y", SqlDbType.Float));
                command.CommandText = query;
                command.Parameters["@monkeyid"].Value = log.Aap.Id;
                command.Parameters["@monkeyname"].Value = log.Aap.Naam;
                command.Parameters["@woodid"].Value = woodId;
                command.Parameters["@seqnr"].Value = log.SeqNr;
                command.Parameters["@treeid"].Value = log.Boom.Id;
                command.Parameters["@x"].Value = log.Boom.X;
                command.Parameters["@y"].Value = log.Boom.Y;
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                connection.Close();
            }
            Console.WriteLine($"stop adding {log.Aap.Naam} to DB.monkeyrecords seqnr: {log.SeqNr}");
        }
        public async Task AddLogs(int woodId, Log log)
        {
            Console.WriteLine($"start adding {log.Aap.Naam} to DB.logs message: {log.ToString()}");
            SqlConnection connection = GetConnection();
            string query = "INSERT INTO logs(woodid, monkeyid, message) VALUES(@woodid, @monkeyid, @message)";
            using SqlCommand command = connection.CreateCommand();
            connection.Open();
            try
            {
                command.Parameters.Add(new SqlParameter("@woodid", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@monkeyid", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@message", SqlDbType.NVarChar));
                command.CommandText = query;
                command.Parameters["@woodid"].Value = woodId;
                command.Parameters["@monkeyid"].Value = log.Aap.Id;
                command.Parameters["@message"].Value = log.ToString();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                connection.Close();
            }
            Console.WriteLine($"stop adding {log.Aap.Naam} to DB.logs message: {log.ToString()}");
        }
    }
}
