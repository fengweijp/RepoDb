﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Data.SqlClient;
using RepoDb.Extensions;
using RepoDb.Reflection;
using RepoDb.SqlServer.IntegrationTests.Models;
using RepoDb.SqlServer.IntegrationTests.Setup;
using System.Data.Common;
using System.Linq;

namespace RepoDb.SqlServer.IntegrationTests.Operations
{
    [TestClass]
    public class ExecuteReaderTest
    {
        [TestInitialize]
        public void Initialize()
        {
            Database.Initialize();
            Cleanup();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Database.Cleanup();
        }

        #region Sync

        [TestMethod]
        public void TestPostgreSqlConnectionExecuteReader()
        {
            // Setup
            var tables = Database.CreateCompleteTables(10);

            using (var connection = new SqlConnection(Database.ConnectionString))
            {
                // Act
                using (var reader = connection.ExecuteReader("SELECT \"Id\", \"ColumnInt\", \"ColumnDate\" FROM \"CompleteTable\";"))
                {
                    while (reader.Read())
                    {
                        // Act
                        var id = reader.GetInt64(0);
                        var columnInt = reader.GetInt32(1);
                        var columnDateTime = reader.GetDateTime(2);
                        var table = tables.FirstOrDefault(e => e.Id == id);

                        // Assert
                        Assert.IsNotNull(table);
                        Assert.AreEqual(columnInt, table.ColumnInt);
                        Assert.AreEqual(columnDateTime, table.ColumnDate);
                    }
                }
            }
        }

        [TestMethod]
        public void TestPostgreSqlConnectionExecuteReaderWithMultipleStatements()
        {
            // Setup
            var tables = Database.CreateCompleteTables(10);

            using (var connection = new SqlConnection(Database.ConnectionString))
            {
                // Act
                using (var reader = connection.ExecuteReader("SELECT \"Id\", \"ColumnInt\", \"ColumnDate\" FROM \"CompleteTable\"; SELECT \"Id\", \"ColumnInt\", \"ColumnDate\" FROM \"CompleteTable\";"))
                {
                    do
                    {
                        while (reader.Read())
                        {
                            // Act
                            var id = reader.GetInt64(0);
                            var columnInt = reader.GetInt32(1);
                            var columnDateTime = reader.GetDateTime(2);
                            var table = tables.FirstOrDefault(e => e.Id == id);

                            // Assert
                            Assert.IsNotNull(table);
                            Assert.AreEqual(columnInt, table.ColumnInt);
                            Assert.AreEqual(columnDateTime, table.ColumnDate);
                        }
                    } while (reader.NextResult());
                }
            }
        }

        [TestMethod]
        public void TestPostgreSqlConnectionExecuteReaderAsExtractedEntity()
        {
            // Setup
            var tables = Database.CreateCompleteTables(10);

            using (var connection = new SqlConnection(Database.ConnectionString))
            {
                // Act
                using (var reader = connection.ExecuteReader("SELECT * FROM \"CompleteTable\";"))
                {
                    // Act
                    var result = DataReader.ToEnumerable<CompleteTable>((DbDataReader)reader, connection).AsList();

                    // Assert
                    tables.AsList().ForEach(table => Helper.AssertPropertiesEquality(table, result.First(e => e.Id == table.Id)));
                }
            }
        }

        [TestMethod]
        public void TestPostgreSqlConnectionExecuteReaderAsExtractedDynamic()
        {
            // Setup
            var tables = Database.CreateCompleteTables(10);

            using (var connection = new SqlConnection(Database.ConnectionString))
            {
                // Act
                using (var reader = connection.ExecuteReader("SELECT * FROM \"CompleteTable\";"))
                {
                    // Act
                    var result = DataReader.ToEnumerable((DbDataReader)reader, connection).AsList();

                    // Assert
                    tables.AsList().ForEach(table => Helper.AssertMembersEquality(table, result.First(e => e.Id == table.Id)));
                }
            }
        }

        #endregion

        #region Async

        [TestMethod]
        public void TestPostgreSqlConnectionExecuteReaderAsync()
        {
            // Setup
            var tables = Database.CreateCompleteTables(10);

            using (var connection = new SqlConnection(Database.ConnectionString))
            {
                // Act
                using (var reader = connection.ExecuteReaderAsync("SELECT \"Id\", \"ColumnInt\", \"ColumnDate\" FROM \"CompleteTable\";").Result)
                {
                    while (reader.Read())
                    {
                        // Act
                        var id = reader.GetInt64(0);
                        var columnInt = reader.GetInt32(1);
                        var columnDateTime = reader.GetDateTime(2);
                        var table = tables.FirstOrDefault(e => e.Id == id);

                        // Assert
                        Assert.IsNotNull(table);
                        Assert.AreEqual(columnInt, table.ColumnInt);
                        Assert.AreEqual(columnDateTime, table.ColumnDate);
                    }
                }
            }
        }

        [TestMethod]
        public void TestPostgreSqlConnectionExecuteReaderAsyncWithMultipleStatements()
        {
            // Setup
            var tables = Database.CreateCompleteTables(10);

            using (var connection = new SqlConnection(Database.ConnectionString))
            {
                // Act
                using (var reader = connection.ExecuteReaderAsync("SELECT \"Id\", \"ColumnInt\", \"ColumnDate\" FROM \"CompleteTable\"; SELECT \"Id\", \"ColumnInt\", \"ColumnDate\" FROM \"CompleteTable\";").Result)
                {
                    do
                    {
                        while (reader.Read())
                        {
                            // Act
                            var id = reader.GetInt64(0);
                            var columnInt = reader.GetInt32(1);
                            var columnDateTime = reader.GetDateTime(2);
                            var table = tables.FirstOrDefault(e => e.Id == id);

                            // Assert
                            Assert.IsNotNull(table);
                            Assert.AreEqual(columnInt, table.ColumnInt);
                            Assert.AreEqual(columnDateTime, table.ColumnDate);
                        }
                    } while (reader.NextResult());
                }
            }
        }

        [TestMethod]
        public void TestPostgreSqlConnectionExecuteReaderAsyncAsExtractedEntity()
        {
            // Setup
            var tables = Database.CreateCompleteTables(10);

            using (var connection = new SqlConnection(Database.ConnectionString))
            {
                // Act
                using (var reader = connection.ExecuteReaderAsync("SELECT * FROM \"CompleteTable\";").Result)
                {
                    // Act
                    var result = DataReader.ToEnumerable<CompleteTable>((DbDataReader)reader, connection).AsList();

                    // Assert
                    tables.AsList().ForEach(table => Helper.AssertPropertiesEquality(table, result.First(e => e.Id == table.Id)));
                }
            }
        }

        [TestMethod]
        public void TestPostgreSqlConnectionExecuteReaderAsyncAsExtractedDynamic()
        {
            // Setup
            var tables = Database.CreateCompleteTables(10);

            using (var connection = new SqlConnection(Database.ConnectionString))
            {
                // Act
                using (var reader = connection.ExecuteReaderAsync("SELECT * FROM \"CompleteTable\";").Result)
                {
                    // Act
                    var result = DataReader.ToEnumerable((DbDataReader)reader, connection).AsList();

                    // Assert
                    tables.AsList().ForEach(table => Helper.AssertMembersEquality(table, result.First(e => e.Id == table.Id)));
                }
            }
        }

        #endregion
    }
}
