Imports System.Data.SqlClient
Imports Microsoft.Reporting.WinForms

Public Class OrderReport
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ReportViewer1.ProcessingMode = ProcessingMode.Local
        ReportViewer1.LocalReport.ReportPath = "../../Report1.rdlc"
        'Dim dsCustomers As DataSet1 = GetData("select Item.itemID,Item.itemName,sum(quality)as sum
        'from ItemReservation,Item,Reservation
        'where Item.itemID=ItemReservation.itemID
        'and Reservation.reservationID=ItemReservation.reservationID
        'and convert(date,reservation.reservationDateTime) between @startDate and @endDate
        'group by Item.itemName,Item.itemID
        'order by sum(quality) desc")
        Dim query As String = "select Item.itemID,Item.itemName,sum(quality) as sum
from ItemReservation,Item,Reservation
where Item.itemID=ItemReservation.itemID
and Reservation.reservationID=ItemReservation.reservationID
and convert(date,reservation.reservationDateTime) between @startDate and @endDate
group by Item.itemName,Item.itemID
order by sum(quality) desc"
        Dim conString As String = My.Resources.ConnectionString
        Dim cmd As New SqlCommand(query)
        Dim dataSet As New DataSet1

        Using con As New SqlConnection(conString)
            Using sda As New SqlDataAdapter()
                cmd.Connection = con
                cmd.Parameters.AddWithValue("startDate", ReportUI.startDate)
                cmd.Parameters.AddWithValue("endDate", ReportUI.endDate)
                sda.SelectCommand = cmd
                sda.Fill(dataSet, "DataTable1")
            End Using
        End Using

        Dim datasource As New ReportDataSource("DataSet1", CType(dataSet.DataTable1, DataTable))
        Dim param() As ReportParameter = {New ReportParameter("ReportParameter1", "Heng Ong Huat Electronic Shop"),
        New ReportParameter("ReportParameter2", "Top Selling Item" + vbNewLine + "from" + vbNewLine + ReportUI.startDate + vbNewLine + "to" + vbNewLine + ReportUI.endDate)}
        ReportViewer1.LocalReport.SetParameters(param)
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