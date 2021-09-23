Imports System.Data.SqlClient
Imports System.Reflection
Imports System.Text
Imports System.Web.Configuration

Namespace DAL

    Public MustInherit Class BaseDataObject

        Private Shared _connectionstring As String

        Public Shared AppName As String = "EasyKeysWeb2014"

        Protected Shared Function UseCache() As Boolean
            Return False
        End Function


        Protected Shared Function GetSQLConnection() As SqlConnection

            Return New SqlConnection(ConnectionString)

        End Function

        Public Shared Property ConnectionString() As String
            Get
                If (String.IsNullOrEmpty(_connectionstring)) Then
                    Return WebConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
                Else
                    Return _connectionstring

                End If

            End Get
            Set(cs As String)
                _connectionstring = cs
            End Set

        End Property

        Public MustOverride Sub Add()

        Public MustOverride Sub Update()

        Public MustOverride Sub Delete()

        Protected MustOverride Sub SetParameters(ByRef command As SqlCommand)

        Public Overrides Function ToString() As String
            Dim sb As New StringBuilder

            Try
                Dim theType As Type

                theType = Me.GetType

                Dim myProperties() As PropertyInfo = theType.GetProperties(BindingFlags.Public Or BindingFlags.Instance Or BindingFlags.NonPublic)

                For Each propertyItem As PropertyInfo In myProperties
                    Dim val As Object = propertyItem.GetValue(Me, Nothing)

                    sb.Append(propertyItem.Name & ": ")
                    If val Is Nothing Then
                        sb.Append("<Nothing>")
                    Else
                        sb.Append(val.ToString)
                    End If
                    sb.Append(vbCrLf)
                Next
            Catch ex As Exception
                Throw ex
            End Try

            Return sb.ToString
        End Function

        Protected Sub AddException(ex As Exception, methodName As String, sproc As String)
            Dim exceptionAudit As New DAL.Audit(
                AuditEvent.EXCEPTION_EVENT,
                "MethodName:" & methodName & "Sproc:" & sproc & Environment.NewLine & "-------" & Me.ToString() & "-------" & ex.ToString())
            exceptionAudit.Add()
        End Sub

        Protected Shared Sub AddSharedException(ex As Exception, methodName As String, sproc As String)
            Dim exceptionAudit As New DAL.Audit(
                AuditEvent.EXCEPTION_EVENT,
                "MethodName:" & methodName & "Sproc:" & sproc & Environment.NewLine & "-------" & ex.ToString())
            exceptionAudit.Add()
        End Sub

    End Class
End Namespace
