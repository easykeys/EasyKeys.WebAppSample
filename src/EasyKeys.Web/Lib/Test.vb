Module decisions
   Sub Main()
      'local variable definition
      Dim grade As Char
      
      Dim isPositive As Predicate(Of Integer) = Function(x) x >= 0
      Dim isLength As Predicate(Of String) = Function(x) x.Length => 10
    If isPositive.Invoke(10) Then
        Console.WriteLine("NUMBER IS POSITIVE")
    End If

      grade = "B"
      Select grade
          Case "A"
              Console.WriteLine("Excellent!")
          Case "B", "C"
              Console.WriteLine("Well done")
          Case "D"
              Console.WriteLine("You passed")
          Case "F"
              Console.WriteLine("Better try again")
          Case Else
              Console.WriteLine("Invalid grade")
      End Select
      Console.WriteLine("Your grade is  {0}", grade)
      Console.ReadLine()
   End Sub
End Module

Public Interface IAsset
    Event ComittedChange(ByVal Success As Boolean)
    Property Division() As String
    Function GetID() As Integer
End Interface