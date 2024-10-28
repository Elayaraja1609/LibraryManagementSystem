using System.Data;
using System.Data.Common;

namespace LMS.Data
{
	public static class DB
	{
		private static readonly string dataProvider = "System.Data.SqlClient";
		private static readonly DbProviderFactory factory = DbProviderFactories.GetFactory(dataProvider);
		/// <summary>
		/// Populates a DataTable according to a given stored procedure
		/// </summary>
		/// <param name="storedProcedureName">Stored procedure name.</param>
		/// <param name="parameters">List of parameters of type DataParameter.</param>
		/// <returns>Populated DataTable.</returns>
		public static DataTable GetDataTable(string storedProcedureName, IList<DataParameter> parameters)
		{
			return GetDataSet(storedProcedureName, parameters).Tables[0];
		}
		/// <summary>
		/// Populates a DataSet according to a stored procedure.
		/// </summary>
		/// <param name="storedProcedureName">Stored procedure name.</param>
		/// <param name="parameters">List of parameters of type DataParameter.</param>
		/// <returns>Populated DataSet.</returns>
		public static DataSet GetDataSet(string storedProcedureName, IList<DataParameter> parameters)
		{
			using (DbConnection connection = factory.CreateConnection())
			{
				connection.ConnectionString = DB.GetConnectionString();

				using (DbCommand command = factory.CreateCommand())
				{
					command.Connection = connection;
					command.CommandType = CommandType.StoredProcedure;
					command.CommandText = storedProcedureName;
					foreach (DataParameter parameter in parameters)
					{
						DbParameter dbparameter = command.CreateParameter();
						dbparameter.ParameterName = parameter.ParameterName;
						dbparameter.Value = parameter.Value;

						command.Parameters.Add(dbparameter);
					}

					using (DbDataAdapter adapter = factory.CreateDataAdapter())
					{
						adapter.SelectCommand = command;

						DataSet ds = new DataSet();
						adapter.Fill(ds);

						return ds;
					}
				}
			}
		}
		private static string GetConnectionString()
		{
			return "Server=Raja;Database=LibraryPrj;Trusted_Connection=True;TrustServerCertificate=True;";
		}
	}
}
