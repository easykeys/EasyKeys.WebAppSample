Imports System.Data.SqlClient

Namespace DAL
    Partial Public Class EmailLog
        Inherits BaseDataObject

        Public Shared ReadOnly MAILCHIMP_API_KEY As String = "e817b61ccadcc791a25e29448865e26f-us10"
        Public Shared ReadOnly MAILCHIMP_LIST_ID As String = "1702fb2fc8"


        Public Shared Function GetAllPendingEmails(ByVal diffMinutes As Integer) As List(Of EmailLog)
            Dim l As New List(Of EmailLog)
            Dim obj As New EmailLog
            Dim dr As SqlDataReader = Nothing

            Try
                Using myConnection As SqlConnection = GetSQLConnection()
                    myConnection.Open()
                    Dim command As New SqlCommand With {
                        .Connection = myConnection,
                        .CommandType = CommandType.StoredProcedure,
                        .CommandText = "usp_EmailLog_GetAllPendingEmails"
                    }
                    command.Parameters.Add("@diffMinutes", SqlDbType.Int).Value = diffMinutes

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
                ' Close out the data reader before going to the audit

                dr = Nothing

                Dim exceptionAudit As New Audit(AuditEvent.EXCEPTION_EVENT,
                   "GetAllPendingEmails" & vbCrLf & "-------" & ex.ToString())
                exceptionAudit.Add()
            Finally
                If Not dr Is Nothing Then dr.Close()
            End Try

            Return l

        End Function

        Public Shared Function SearchEmailLogs(
                ByVal searchstring As String,
                ByVal dtStartDate As DateTime,
                ByVal dtEndDate As DateTime) As List(Of EmailLog)
            Dim l As New List(Of EmailLog)
            Dim obj As New EmailLog
            Dim dr As SqlDataReader = Nothing

            Try

                Using myConnection As SqlConnection = GetSQLConnection()
                    myConnection.Open()
                    Dim command As New SqlCommand With {
                        .Connection = myConnection,
                        .CommandType = CommandType.StoredProcedure,
                        .CommandText = "usp_EmailLog_Search"
                    }
                    command.Parameters.Add("@SearchString", SqlDbType.VarChar).Value = searchstring
                    command.Parameters.Add("@StartDate", SqlDbType.Date).Value = dtStartDate
                    command.Parameters.Add("@EndDate", SqlDbType.Date).Value = dtEndDate

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
                   "SearchEmailLogs" & vbCrLf & "-------" & ex.ToString())
                exceptionAudit.Add()
            Finally
                dr = Nothing
            End Try

            Return l

        End Function

        Public Shared Async Function SearchEmailLogsAsync(
                ByVal searchstring As String,
                ByVal dtStartDate As DateTime,
                ByVal dtEndDate As DateTime) As Threading.Tasks.Task(Of List(Of EmailLog))
            Dim l As New List(Of EmailLog)
            Dim obj As New EmailLog
            Dim dr As SqlDataReader = Nothing

            Try

                Using myConnection As SqlConnection = GetSQLConnection()
                    myConnection.Open()
                    Dim command As New SqlCommand With {
                        .Connection = myConnection,
                        .CommandType = CommandType.StoredProcedure,
                        .CommandText = "usp_EmailLog_Search"
                    }
                    command.Parameters.Add("@SearchString", SqlDbType.VarChar).Value = searchstring
                    command.Parameters.Add("@StartDate", SqlDbType.Date).Value = dtStartDate
                    command.Parameters.Add("@EndDate", SqlDbType.Date).Value = dtEndDate

                    dr = Await command.ExecuteReaderAsync()

                    While Await dr.ReadAsync()
                        obj = PopulateObject(dr)
                        l.Add(obj)
                        If UseCache() AndAlso Not _objectCache.Exists(obj.EmailLogID) Then
                            _objectCache.Save(obj.EmailLogID, obj.Code, obj)
                        End If
                    End While
                End Using
            Catch ex As Exception

                Dim exceptionAudit As New Audit(AuditEvent.EXCEPTION_EVENT,
                   "SearchEmailLogs" & vbCrLf & "-------" & ex.ToString())
                exceptionAudit.Add()
            Finally
                dr = Nothing
            End Try

            Return l

        End Function
    End Class
End Namespace
