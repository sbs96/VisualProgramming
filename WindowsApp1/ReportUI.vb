Public Class ReportUI
    Public startDate As Date
    Public endDate As Date

    Private Sub ReportUI_Load(sender As Object, e As EventArgs) Handles Me.Load
        DateTimePicker1.MaxDate = DateTime.Now
        DateTimePicker2.MaxDate = DateTime.Now
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        startDate = DateTimePicker1.Value.ToShortDateString
        endDate = DateTimePicker2.Value.ToShortDateString
        Me.Hide()
        OrderReport.Show()
    End Sub
End Class