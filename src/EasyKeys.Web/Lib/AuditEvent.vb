Imports System.Data.SqlClient

Namespace DAL
	Partial Public Class AuditEvent
		Inherits BaseDataObject

		Private Shared _objectCache As EK.Data.Caching.ObjectCache = New EK.Data.Caching.ObjectCache()
		Private Shared _count As Integer = 0
		Private Shared _lockObject As Object = New Object

		Public Shared ReadOnly ADD_EVENT As Byte = 1
		Public Shared ReadOnly UPDATE_EVENT As Byte = 2
		Public Shared ReadOnly DELETE_EVENT As Byte = 3
		Public Shared ReadOnly EXCEPTION_EVENT As Byte = 4

		Enum EnumValues
			ADD
			UPDATE
			DELETE
			EXCEPTION_OCCURRED
			Unknown
		End Enum

		Public Shared Sub ClearCache()
			SyncLock _lockObject
				_objectCache.Clear()
				_count = 0
			End SyncLock
		End Sub

		Protected Shared Shadows Function UseCache() As Boolean
			Return True
		End Function
		Public Property AuditEventID() As Integer?
		Public Property Description() As String

		Public Property Code() As String

		Public Sub New()
			MyBase.New()
		End Sub

		Public Overrides Sub Add()

			Dim sproc As String = "usp_AuditEvent_insert"
			Try
				Using myConnection As SqlConnection = GetSQLConnection()
					myConnection.Open()
					Dim command As New SqlCommand With {
						.Connection = myConnection,
						.CommandType = CommandType.StoredProcedure,
						.CommandText = sproc
					}

					command.Parameters.Add("@InsertApp", SqlDbType.VarChar).Value = AppName

					SetParameters(command)

					command.ExecuteNonQuery()

					' get the primary key

					Dim keyParam As SqlParameter
					keyParam = command.Parameters(command.Parameters.IndexOf("@AuditEventID"))
					If Not keyParam Is Nothing Then Me.AuditEventID = keyParam.Value

				End Using
			Catch ex As Exception
				AddException(ex, NameOf(Add), sproc)
			End Try
		End Sub

		Public Overrides Sub Update()
			If UseCache() AndAlso _objectCache.Exists(Me.AuditEventID) Then
				_objectCache.Delete(Me.AuditEventID, Me.Code)
			End If

			Dim sproc As String = "usp_AuditEvent_update"
			Try
				Using myConnection As SqlConnection = GetSQLConnection()
					myConnection.Open()
					Dim command As New SqlCommand With {
						.Connection = myConnection,
						.CommandType = CommandType.StoredProcedure,
						.CommandText = sproc
					}

					command.Parameters.Add("@LastUpdateApp", SqlDbType.VarChar).Value = AppName

					SetParameters(command)

					command.ExecuteNonQuery()

				End Using

			Catch ex As Exception
				AddException(ex, NameOf(Update), sproc)
			End Try
		End Sub

		Public Overrides Sub Delete()
			If UseCache() AndAlso _objectCache.Exists(Me.AuditEventID) Then
				_objectCache.Delete(Me.AuditEventID, Me.Code)
			End If

			Dim sproc As String = "usp_AuditEvent_delete"
			Try

				Using myConnection As SqlConnection = GetSQLConnection()
					myConnection.Open()
					Dim command As New SqlCommand With {
						.Connection = myConnection,
						.CommandType = CommandType.StoredProcedure,
						.CommandText = sproc
					}

					command.Parameters.Add("@AuditEventID", SqlDbType.Int).Value = Me.AuditEventID

					command.ExecuteNonQuery()

				End Using

			Catch ex As Exception
				AddException(ex, NameOf(Delete), sproc)
			End Try
		End Sub

		Protected Overrides Sub SetParameters(ByRef command As SqlCommand)
			If Not Me.AuditEventID.HasValue Then
				command.Parameters.Add("@AuditEventID", SqlDbType.Int)
				command.Parameters(command.Parameters.IndexOf("@AuditEventID")).Direction = ParameterDirection.Output
			Else
				command.Parameters.Add("@AuditEventID", SqlDbType.Int).Value = Me.AuditEventID
			End If

			command.Parameters.Add("@Description", SqlDbType.VarChar).Value = Me.Description
			command.Parameters.Add("@Code", SqlDbType.VarChar).Value = Me.Code
		End Sub

		Private Shared Function PopulateObject(dr As SqlDataReader) As AuditEvent
			Dim obj As New AuditEvent

			With obj
				If Not IsDBNull(dr(0)) Then .AuditEventID = dr.GetInt32(0)
				If Not IsDBNull(dr(1)) Then .Description = dr.GetString(1)
				If Not IsDBNull(dr(2)) Then .Code = dr.GetString(2)
			End With

			Return obj
		End Function

	End Class
End Namespace
