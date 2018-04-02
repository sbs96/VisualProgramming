Imports System.Data.SqlClient
Imports Microsoft.Reporting.WinForms

Public Class OrderReport
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ReportViewer1.ProcessingMode = ProcessingMode.Local
        ReportViewer1.LocalReport.ReportPath = "../../Report1.rdlc"
        Dim dsCustomers As DataSet1 = GetData("select Item.itemID,Item.itemName,sum(quality)as sum
from ItemReservation,Item,Reservation
where Item.itemID=ItemReservation.itemID
and Reservation.reservationID=ItemReservation.reservationID
and convert(date,reservation.reservationDateTime) between @startDate and @endDate
group by item.itemName,item.itemID")
        Dim datasource As New ReportDataSource("DataSet1", dsCustomers.Tables(0))
        ReportViewer1.LocalReport.DataSources.Clear()
        ReportViewer1.LocalReport.DataSources.Add(datasource)
        ReportViewer1.RefreshReport()
        Console.WriteLine(ReportUI.startDate + "  " + ReportUI.endDate)
    End Sub

    Private Function GetData(query As String) As DataSet1
        Dim conString As String = My.Resources.ConnectionString
        Dim cmd As New SqlCommand(query)
        Using con As New SqlConnection(conString)
            Using sda As New SqlDataAdapter()
                cmd.Connection = con
                cmd.Parameters.AddWithValue("startDate", ReportUI.startDate)
                cmd.Parameters.AddWithValue("endDate", ReportUI.endDate)
                sda.SelectCommand = cmd
                Using dsCustomers As New DataSet1()
                    sda.Fill(dsCustomers, "DataTable1")
                    Return dsCustomers
                End Using
            End Using
        End Using
    End Function
End Class