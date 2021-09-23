Imports System.Data.SqlClient

Namespace DAL
    Partial Public Class Audit
        Inherits BaseDataObject

        Private Shared _objectCache As EK.Data.Caching.ObjectCache = New EK.Data.Caching.ObjectCache()

        Public Property AuditID() As Integer?
        Public Property AuditEventID() As Integer

        Public Property DetailDescription() As String

        Public Property LogTime() As Date

        Public Property Code() As String

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal auditEventID As Integer, ByVal detailDescription As String)
            MyBase.New()

            Me.AuditEventID = auditEventID
            Me.DetailDescription = detailDescription
            LogTime = Now
        End Sub

        Public Overrides Sub Add()
            Dim sproc As String = "usp_Audit_insert"
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
                    keyParam = command.Parameters(command.Parameters.IndexOf("@AuditID"))
                    If Not keyParam Is Nothing Then Me.AuditID = keyParam.Value

                End Using
            Catch
            End Try
        End Sub

        Public Overrides Sub Update()
            If UseCache() AndAlso _objectCache.Exists(Me.AuditID) Then
                _objectCache.Delete(Me.AuditID, Me.Code)
            End If

            Dim sproc As String = "usp_Audit_update"
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
            If UseCache() AndAlso _objectCache.Exists(Me.AuditID) Then
                _objectCache.Delete(Me.AuditID, Me.Code)
            End If
            Dim sproc As String = "usp_Audit_delete"
            Try

                Using myConnection As SqlConnection = GetSQLConnection()
                    myConnection.Open()
                    Dim command As New SqlCommand With {
                        .Connection = myConnection,
                        .CommandType = CommandType.StoredProcedure,
                        .CommandText = sproc
                    }

                    command.Parameters.Add("@AuditID", SqlDbType.Int).Value = Me.AuditID

                    command.ExecuteNonQuery()

                End Using

            Catch ex As Exception
                AddException(ex, NameOf(Delete), sproc)
            End Try
        End Sub

        Protected Overrides Sub SetParameters(ByRef command As SqlCommand)
            If Not Me.AuditID.HasValue Then
                command.Parameters.Add("@AuditID", SqlDbType.Int)
                command.Parameters(command.Parameters.IndexOf("@AuditID")).Direction = ParameterDirection.Output
            Else
                command.Parameters.Add("@AuditID", SqlDbType.Int).Value = Me.AuditID
            End If

            command.Parameters.Add("@AuditEventID", SqlDbType.Int).Value = Me.AuditEventID
            command.Parameters.Add("@DetailDescription", SqlDbType.Text).Value = Me.DetailDescription
            command.Parameters.Add("@LogTime", SqlDbType.DateTime).Value = Me.LogTime
            command.Parameters.Add("@Code", SqlDbType.VarChar).Value = Me.Code
        End Sub

        Private Shared Function PopulateObject(dr As SqlDataReader) As Audit
            Dim obj As New Audit

            With obj
                If Not IsDBNull(dr(0)) Then .AuditID = dr.GetInt32(0)
                If Not IsDBNull(dr(1)) Then .AuditEventID = dr.GetInt32(1)
                If Not IsDBNull(dr(2)) Then .DetailDescription = dr.GetString(2)
                If Not IsDBNull(dr(3)) Then .LogTime = dr.GetDateTime(3)
                If Not IsDBNull(dr(4)) Then .Code = dr.GetString(4)
            End With

            Return obj
        End Function


    End Class
End Namespace
