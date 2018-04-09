Public Class ReportUI
    Public startDate As Date
    Public endDate As Date

    Private Sub ReportUI_Load(sender As Object, e As EventArgs) Handles Me.Load
        DateTimePicker1.MaxDate = DateTime.Now
        DateTimePicker2.MaxDate = DateTime.Now
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If DateTimePicker1.Value <= DateTimePicker2.Value Then
            startDate = DateTimePicker1.Value.ToShortDateString
            endDate = DateTimePicker2.Value.ToShortDateString
            Me.Hide()
            OrderReport.Show()
        Else
            MessageBox.Show("End date must after start date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub
End Class