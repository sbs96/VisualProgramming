Imports System.Data.SqlClient
Imports System.Text.RegularExpressions

Public Class Form3
    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles Me.Load
        ReloadUI()
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        Button1.Enabled = True
        Button2.Enabled = True
        Button3.Enabled = True

        Dim sqlConnection As New SqlConnection(My.Resources.ConnectionString)
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader

        cmd.CommandText = "select item.itemName, item.itemPrice, itemReservation.quality 
        from item, reservation, itemReservation 
        where item.itemID=itemReservation.itemID 
        and reservation.reservationID=itemReservation.reservationID 
        and reservation.reservationID=@id 
        and itemReservation.itemID=(select itemID from Item where itemName=@name)"
        cmd.CommandType = CommandType.Text
        cmd.Connection = sqlConnection

        sqlConnection.Open()
        cmd.Parameters.AddWithValue("id", 5)
        Dim selectedItem As String = ListBox1.SelectedItem
        Dim name As String = selectedItem.Substring(0, selectedItem.IndexOf("   "))
        cmd.Parameters.AddWithValue("name", name)
        reader = cmd.ExecuteReader()

        If (reader.HasRows) Then
            While (reader.Read())
                Dim productName As String = reader.GetString(0)
                Dim unitPrice As Double = reader.GetDecimal(1)
                Dim quantity As Integer = reader.GetInt32(2)
                TextBox2.Text = productName
                TextBox3.Text = unitPrice
                TextBox4.Text = quantity
            End While
        Else
            Console.WriteLine("No rows found.")
        End If
        reader.Close()
        sqlConnection.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim quantity As Integer = TextBox4.Text
        quantity += 1
        TextBox4.Text = quantity
        UpdateDB(quantity)
        ReloadUI()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim quantity As Integer = TextBox4.Text
        quantity = Math.Max(quantity - 1, 1)
        TextBox4.Text = quantity
        UpdateDB(quantity)
        ReloadUI()
    End Sub

    Private Sub UpdateDB(quantity As Integer)
        Dim sqlConnection As New SqlConnection(My.Resources.ConnectionString)
        Dim cmd As New SqlCommand

        cmd.CommandText = "update ItemReservation
        set quality=@quantity
        where reservationID=@id
        and	itemID=(select itemID from Item where itemName=@name)"
        cmd.CommandType = CommandType.Text
        cmd.Connection = sqlConnection

        sqlConnection.Open()
        cmd.Parameters.AddWithValue("quantity", quantity)
        cmd.Parameters.AddWithValue("id", 5)
        cmd.Parameters.AddWithValue("name", TextBox2.Text)
        cmd.ExecuteNonQuery()
        sqlConnection.Close()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim result As DialogResult = MessageBox.Show("Are you sure to remove this item?", "Remove Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If result = DialogResult.Yes Then
            Dim sqlConnection As New SqlConnection(My.Resources.ConnectionString)
            Dim cmd As New SqlCommand

            cmd.CommandText = "delete from ItemReservation
            where reservationID=@id
            and	itemID=(select itemID from Item where itemName=@name)"
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection

            sqlConnection.Open()
            cmd.Parameters.AddWithValue("id", 5)
            cmd.Parameters.AddWithValue("name", TextBox2.Text)
            cmd.ExecuteNonQuery()
            sqlConnection.Close()
            ReloadUI()
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim result As DialogResult = MessageBox.Show("Are you sure to proceed to payment?", "Order Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If result = DialogResult.Yes Then
            Dim sqlConnection As New SqlConnection(My.Resources.ConnectionString)
            Dim sqlConnection2 As New SqlConnection(My.Resources.ConnectionString)
            Dim cmd As New SqlCommand
            Dim cmd2 As New SqlCommand
            Dim reader As SqlDataReader

            cmd.CommandText = "select itemID, quality
            from ItemReservation
            where reservationID=@id"
            cmd.CommandType = CommandType.Text
            cmd.Connection = sqlConnection

            cmd2.CommandText = "update item
            set itemQuality=itemQuality-@quantity
            where itemID=@id"
            cmd2.Connection = sqlConnection2

            sqlConnection.Open()
            sqlConnection2.Open()
            cmd.Parameters.AddWithValue("id", 5)
            reader = cmd.ExecuteReader

            If (reader.HasRows) Then
                While (reader.Read())
                    Dim itemID As Integer = reader.GetInt64(0)
                    Dim quantity As Integer = reader.GetInt32(1)
                    cmd2.Parameters.AddWithValue("quantity", quantity)
                    cmd2.Parameters.AddWithValue("id", itemID)
                    cmd2.ExecuteNonQuery()
                    cmd2.Parameters.Clear()
                End While
            Else
                Console.WriteLine("No rows found.")
            End If
            reader.Close()
            sqlConnection.Close()
            sqlConnection2.Close()
        End If
    End Sub

    Private Sub ReloadUI()
        Dim sqlConnection As New SqlConnection(My.Resources.ConnectionString)
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        Dim grandTotal As Double = 0

        ListBox1.Items.Clear()

        cmd.CommandText = "select item.itemName, item.itemPrice, itemReservation.quality 
        from item, reservation, itemReservation 
        where item.itemID=itemReservation.itemID 
        and reservation.reservationID=itemReservation.reservationID 
        and reservation.reservationID=@id"
        cmd.CommandType = CommandType.Text
        cmd.Connection = sqlConnection

        sqlConnection.Open()
        cmd.Parameters.AddWithValue("id", 5)
        reader = cmd.ExecuteReader()

        If (reader.HasRows) Then
            While (reader.Read())
                Dim productName As String = reader.GetString(0)
                Dim unitPrice As Double = reader.GetDecimal(1)
                Dim quantity As Integer = reader.GetInt32(2)
                Dim totalPrice As Double = unitPrice * quantity
                grandTotal += totalPrice
                ListBox1.Items.Add(String.Format("{0,-20}  {1,8:.00}  {2,3}  {3,8:.00}", productName, unitPrice, quantity, totalPrice))
            End While
        Else
            Console.WriteLine("No rows found.")
        End If
        TextBox1.Text = grandTotal.ToString(".00")
        reader.Close()
        sqlConnection.Close()
    End Sub
End Class