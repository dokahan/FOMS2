using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;

namespace FOMSSubmarine
{
	class AccessDB
	{
		private string pathDB;
		private OleDbConnection connection;
		private bool isConnected;

		public AccessDB(string path)
		{
			pathDB = path;
		}

		public bool Connect()
		{
			try
			{
				string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathDB;
				connection = new OleDbConnection(connectionString);
				connection.Open();
				isConnected = true;
			}
			catch (Exception ex)
			{
				DebuggingHelper.Trace(ex);
				return false;
			}

			return true;
		}

		public void Close()
		{
			if (isConnected)
			{
				connection.Close();
				isConnected = false;
			}
		}

		public AccessDataTable GetAccessDataTable(string tableName, string filter)
		{
			if (isConnected == false)
				return null;

			AccessDataTable adt = new AccessDataTable(tableName, filter, connection);
			return adt;
		}
	}

	class AccessDataTable
	{
		private readonly string tableName;
		private readonly OleDbDataAdapter adapter;
		private readonly DataTable table;
		private int selectedIndex;
		private DataRow selectedRow;

		public AccessDataTable(string tableName, string filter, OleDbConnection connection)
		{
			this.tableName = tableName;
			table = new DataTable();
			string sql = "SELECT * FROM " + tableName;
			if (string.IsNullOrEmpty(filter) == false)
			{
				sql += " WHERE " + filter;
			}
			adapter = new OleDbDataAdapter(sql, connection);
			adapter.Fill(table);
			if (table.Rows.Count > 0)
			{
				selectedIndex = 0;
				selectedRow = table.Rows[0];
			}
			else
			{
				selectedIndex = -1;
				selectedRow = null;
			}
			adapter.InsertCommand = CreateInsertCommand(connection);
		}

		public int RecordCount
		{
			get
			{
				return (table != null) ? table.Rows.Count : 0;
			}
		}

		public object this[string columnName]
		{
			get
			{
				return selectedRow?[columnName];
			}
			set
			{
				if (selectedRow != null)
				{
					selectedRow[columnName] = value;
				}
			}
		}

		public void SelectRow(string columnName, string value)
		{
			for (int i = 0; i < table.Rows.Count; ++i)
			{
				if (table.Rows[i][columnName] as string == value)
				{
					selectedIndex = i;
					selectedRow = table.Rows[i];
					return;
				}
			}
		}

		public void MoveNext()
		{
			if (selectedIndex >= 0 && selectedIndex < table.Rows.Count)
			{
				if (++selectedIndex == table.Rows.Count)
				{
					selectedIndex = -1;
					selectedRow = null;
				}
				else
				{
					selectedRow = table.Rows[selectedIndex];
				}
			}
		}

		public void Add()
		{
			DataRow row = table.NewRow();
			int index = table.Rows.Count;
			table.Rows.Add(row);
			selectedRow = table.Rows[index];
		}

		public void Delete(int index)
		{
			table.Rows[index].Delete();
		}

		public void Update()
		{
			adapter.Update(table);
		}

		private OleDbCommand CreateInsertCommand(OleDbConnection connection)
		{
			OleDbCommand command = null;

			try
			{
				List<OleDbParameter> parameters = new List<OleDbParameter>();
				string fields = "";
				string values = "";
				foreach (DataColumn column in table.Columns)
				{
					if (column.ReadOnly == false)
					{
						if (fields.Length > 0)
						{
							fields += ", ";
							values += ", ";
						}

						fields += "[" + column.ColumnName + "]";
						values += "?";
						parameters.Add(new OleDbParameter(column.ColumnName, GetDBType(column.DataType), column.MaxLength, column.ColumnName));
					}
				}
				string sql = "INSERT INTO " + "[" + tableName + "]" + " (" + fields + ") VALUES (" + values + ")";
				command = new OleDbCommand(sql, connection);
				parameters.ForEach(parameter => command.Parameters.Add(parameter));
			}
			catch
			{
			}

			return command;
		}

		private OleDbType GetDBType(Type type)
		{
			OleDbType dbType;

			switch (type.Name)
			{
				case "Byte":
					dbType = OleDbType.UnsignedTinyInt;
					break;
				case "Byte[]":
					dbType = OleDbType.Binary;
					break;
				case "Boolean":
					dbType = OleDbType.Boolean;
					break;
				case "DateTime":
					dbType = OleDbType.Date;
					break;
				case "Decimal":
					dbType = OleDbType.Decimal;
					break;
				case "Double":
					dbType = OleDbType.Double;
					break;
				case "Guid":
					dbType = OleDbType.Guid;
					break;
				case "Int16":
					dbType = OleDbType.SmallInt;
					break;
				case "Int32":
					dbType = OleDbType.Integer;
					break;
				case "Int64":
					dbType = OleDbType.BigInt;
					break;
				case "Object":
					dbType = OleDbType.Variant;
					break;
				case "SByte":
					dbType = OleDbType.TinyInt;
					break;
				case "Single":
					dbType = OleDbType.Single;
					break;
				case "String":
					dbType = OleDbType.BSTR;
					break;
				case "UInt16":
					dbType = OleDbType.UnsignedSmallInt;
					break;
				case "UInt32":
					dbType = OleDbType.UnsignedInt;
					break;
				case "UInt64":
					dbType = OleDbType.UnsignedBigInt;
					break;
				default:
					dbType = OleDbType.BSTR;
					break;
			}

			return dbType;
		}

	}
}
