Imports System.Data.SqlClient

Namespace DAL
	Partial Public Class EmailLog
		Inherits BaseDataObject

		Private Shared _objectCache As EK.Data.Caching.ObjectCache = New EK.Data.Caching.ObjectCache()

		Public Property EmailLogID() As Integer?
		Public Property ToEmailList() As String

		Public Property FromEmail() As String

		Public Property BCCEmailList() As String

		Public Property Subject() As String

		Public Property BodyIsHTML() As Boolean

		Public Property Body() As String

		Public Property CreatedDateTime() As Date

		Public Property Sent() As Boolean

		Public Property SentDateTime() As Date

		Public Property Code() As String

		Public Property EmailRequestID() As Integer

		Public Sub New()
			MyBase.New()
		End Sub

		Public Overrides Sub Add()
			Try
				Using myConnection As SqlConnection = GetSQLConnection()
					myConnection.Open()
					Dim command As New SqlCommand With {
						.Connection = myConnection,
						.CommandType = CommandType.StoredProcedure,
						.CommandText = "usp_EmailLog_insert"
					}

					command.Parameters.Add("@InsertApp", SqlDbType.VarChar).Value = ReadApplicationSetting("AppName", "EasyKeysWeb2014")

					SetParameters(command)

					command.ExecuteNonQuery()

					' get the primary key

					Dim keyParam As SqlParameter
					keyParam = command.Parameters(command.Parameters.IndexOf("@EmailLogID"))
					If Not keyParam Is Nothing Then Me.EmailLogID = keyParam.Value

				End Using
			Catch ex As Exception
				Dim exceptionAudit As New Audit(AuditEvent.EXCEPTION_EVENT,
						"add()" & vbCrLf & "-------" & Me.ToString() & "-------" & ex.ToString())
				exceptionAudit.Add()

			End Try
		End Sub

		Public Overrides Sub Update()
			If UseCache() AndAlso _objectCache.Exists(Me.EmailLogID) Then
				_objectCache.Delete(Me.EmailLogID, Me.Code)
			End If
			Try
				Using myConnection As SqlConnection = GetSQLConnection()
					myConnection.Open()
					Dim command As New SqlCommand With {
						.Connection = myConnection,
						.CommandType = CommandType.StoredProcedure,
						.CommandText = "usp_EmailLog_update"
					}

					command.Parameters.Add("@LastUpdateApp", SqlDbType.VarChar).Value = ReadApplicationSetting("AppName", "EasyKeysWeb2014")

					SetParameters(command)

					command.ExecuteNonQuery()

				End Using

			Catch ex As Exception
				Dim exceptionAudit As New Audit(AuditEvent.EXCEPTION_EVENT,
							"update()" & vbCrLf & "-------" & Me.ToString() & "-------" & ex.ToString())
				exceptionAudit.Add()
			End Try
		End Sub

		Public Overrides Sub Delete()
			If UseCache() AndAlso _objectCache.Exists(Me.EmailLogID) Then
				_objectCache.Delete(Me.EmailLogID, Me.Code)
			End If
			Try

				Using myConnection As SqlConnection = GetSQLConnection()
					myConnection.Open()
					Dim command As New SqlCommand With {
						.Connection = myConnection,
						.CommandType = CommandType.StoredProcedure,
						.CommandText = "usp_EmailLog_delete"
					}

					command.Parameters.Add("@EmailLogID", SqlDbType.Int).Value = Me.EmailLogID

					command.ExecuteNonQuery()

				End Using

			Catch ex As Exception
				Dim exceptionAudit As New Audit(AuditEvent.EXCEPTION_EVENT,
						"delete()" & vbCrLf & "-------" & Me.ToString() & "-------" & ex.ToString())
				exceptionAudit.Add()

			End Try
		End Sub

		Private Function ReadApplicationSetting(
				name As String,
				defaultValue As String) As String

			Dim result As String = defaultValue
			Dim var As Object = System.Configuration.ConfigurationManager.AppSettings(name)
			If Not var Is Nothing Then
				result = var.ToString()
			End If
			Return result

		End Function

		Protected Overrides Sub SetParameters(ByRef command As SqlCommand)
			If Not Me.EmailLogID.HasValue Then
				command.Parameters.Add("@EmailLogID", SqlDbType.Int)
				command.Parameters(command.Parameters.IndexOf("@EmailLogID")).Direction = ParameterDirection.Output
			Else
				command.Parameters.Add("@EmailLogID", SqlDbType.Int).Value = Me.EmailLogID
			End If

			command.Parameters.Add("@ToEmailList", SqlDbType.VarChar).Value = Me.ToEmailList
			command.Parameters.Add("@FromEmail", SqlDbType.VarChar).Value = Me.FromEmail
			command.Parameters.Add("@BCCEmailList", SqlDbType.VarChar).Value = Me.BCCEmailList
			command.Parameters.Add("@Subject", SqlDbType.VarChar).Value = Me.Subject
			command.Parameters.Add("@BodyIsHTML", SqlDbType.Bit).Value = Me.BodyIsHTML
			command.Parameters.Add("@Body", SqlDbType.Text).Value = Me.Body
			command.Parameters.Add("@CreatedDateTime", SqlDbType.DateTime).Value = Me.CreatedDateTime
			command.Parameters.Add("@Sent", SqlDbType.Bit).Value = Me.Sent
			command.Parameters.Add("@SentDateTime", SqlDbType.DateTime).Value = Me.SentDateTime
			command.Parameters.Add("@Code", SqlDbType.VarChar).Value = Me.Code
			command.Parameters.Add("@EmailRequestID", SqlDbType.Int).Value = Me.EmailRequestID
		End Sub

		Private Shared Function PopulateObject(dr As SqlDataReader) As EmailLog
			Dim obj As New EmailLog

			With obj
				If Not IsDBNull(dr(0)) Then .EmailLogID = dr.GetInt32(0)
				If Not IsDBNull(dr(1)) Then .ToEmailList = dr.GetString(1)
				If Not IsDBNull(dr(2)) Then .FromEmail = dr.GetString(2)
				If Not IsDBNull(dr(3)) Then .BCCEmailList = dr.GetString(3)
				If Not IsDBNull(dr(4)) Then .Subject = dr.GetString(4)
				If Not IsDBNull(dr(5)) Then .BodyIsHTML = dr.GetBoolean(5)
				If Not IsDBNull(dr(6)) Then .Body = dr.GetString(6)
				If Not IsDBNull(dr(7)) Then .CreatedDateTime = dr.GetDateTime(7)
				If Not IsDBNull(dr(8)) Then .Sent = dr.GetBoolean(8)
				If Not IsDBNull(dr(9)) Then .SentDateTime = dr.GetDateTime(9)
				If Not IsDBNull(dr(10)) Then .Code = dr.GetString(10)
				If Not IsDBNull(dr(11)) Then .EmailRequestID = dr.GetInt32(11)
			End With

			Return obj
		End Function

		Public Shared Function GetEmailLog(emailLogID As Integer) As EmailLog
			Dim obj As EmailLog = Nothing
			Dim dr As SqlDataReader = Nothing

			If UseCache() AndAlso _objectCache.Exists(emailLogID) Then
				Return _objectCache.Read(emailLogID)
			End If
			Try

				Using myConnection As SqlConnection = GetSQLConnection()
					myConnection.Open()
					Dim command As New SqlCommand With {
						.Connection = myConnection,
						.CommandType = CommandType.StoredProcedure,
						.CommandText = "usp_EmailLog_select"
					}

					command.Parameters.Add("@EmailLogID", SqlDbType.Int).Value = emailLogID

					dr = command.ExecuteReader()

					If dr.Read Then
						obj = PopulateObject(dr)
						If UseCache() AndAlso Not _objectCache.Exists(emailLogID) Then
							_objectCache.Save(obj.EmailLogID, obj.Code, obj)
						End If
					End If

				End Using
			Catch ex As Exception


				Dim exceptionAudit As New Audit(AuditEvent.EXCEPTION_EVENT,
						"GetEmailLog(EmailLogID As Integer)" & vbCrLf & "-------" & ex.ToString())
				exceptionAudit.Add()
			Finally
				dr = Nothing
			End Try

			Return obj

		End Function


		Public Shared Function GetEmailLogByCode(code As String) As EmailLog
			Dim obj As EmailLog = Nothing
			Dim dr As SqlDataReader = Nothing

			If UseCache() AndAlso _objectCache.Exists(code) Then
				Return _objectCache.Read(code)
			End If
			Try
				Using myConnection As SqlConnection = GetSQLConnection()
					myConnection.Open()
					Dim command As New SqlCommand With {
						.Connection = myConnection,
						.CommandType = CommandType.StoredProcedure,
						.CommandText = "usp_EmailLog_selectByCode"
					}

					command.Parameters.Add("@Code", SqlDbType.VarChar).Value = code

					dr = command.ExecuteReader()

					If dr.Read Then
						obj = PopulateObject(dr)
						If UseCache() AndAlso Not _objectCache.Exists(obj.EmailLogID) Then
							_objectCache.Save(obj.EmailLogID, obj.Code, obj)
						End If
					End If
				End Using

			Catch ex As Exception


				Dim exceptionAudit As New Audit(AuditEvent.EXCEPTION_EVENT,
						"GetEmailLogByCode(code as String)" & vbCrLf & "-------" & ex.ToString())
				exceptionAudit.Add()
			Finally
				dr = Nothing
			End Try

			Return obj

		End Function

		Public Shared Function GetAllEmailLogs() As List(Of EmailLog)
			Dim l As New List(Of EmailLog)
			Dim obj As New EmailLog
			Dim dr As SqlDataReader = Nothing

			Try

				Using myConnection As SqlConnection = GetSQLConnection()
					myConnection.Open()
					Dim command As New SqlCommand With {
						.Connection = myConnection,
						.CommandType = CommandType.StoredProcedure,
						.CommandText = "usp_EmailLog_selectAll"
					}

					dr = command.ExecuteReader()

					While dr.Read()
						obj = PopulateObject(dr)
						l.Add(obj)
						If UseCache() AndAlso Not _objectCache.Exists(obj.EmailLogID) Then
							_objectCache.Save(obj.EmailLogID, obj.Code, obj)
						End If
					End While
				End Using
			Catch ex As Exception

				Dim exceptionAudit As New Audit(AuditEvent.EXCEPTION_EVENT,
						"GetAllEmailLogs" & vbCrLf & "-------" & ex.ToString())
				exceptionAudit.Add()
			Finally
				dr = Nothing
			End Try

			Return l

		End Function
End Class
End Namespace
