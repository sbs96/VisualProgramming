Imports System.Data.SqlClient

Public Class TestBlob
    'SQL Server Database Connection String
    Dim strSQLConn As String = My.Resources.ConnectionString
    'Object To Use As SQL Query	
    Dim strQuery As String
    'SQL Server Database Connection
    Dim sqlSQLCon As SqlConnection

    'Store Image Path
    Dim strImagePath As String

    'Image To Store PictureBox Image
    Dim imgTemp As Image

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        'Search SQL Query, Including a Parameter Called Name
        'This Query Simply Retrieves All Information That Matches Our Criteria
        strQuery = "Select * From Test where itemName=@name"

        'Instantiate Connection Object
        sqlSQLCon = New SqlConnection(strSQLConn)

        'Using Structure Simplifies Garbage Collection And Ensures That The Object Will Be Disposed Of Correctly Afterwards
        Using sqlSQLCon

            'Create Command Object, To Make Use Of SQL Query
            Dim sqlSQLCommand As New SqlCommand(strQuery, sqlSQLCon)

            'Create Parameter Instead Of Hardcoding Values
            'Name = Whatever txtSearch Contains
            sqlSQLCommand.Parameters.AddWithValue("name", TextBox1.Text)

            'Open Connection To The Database
            sqlSQLCon.Open()

            'Reader Object To Traverse Through Found Records
            Dim sqlSQLReader As SqlDataReader = sqlSQLCommand.ExecuteReader()

            'If The Reader Finds Rows
            If sqlSQLReader.HasRows Then

                'Retrieve The Content, For Each Match
                While sqlSQLReader.Read()

                    'GetString(0) Represents Column 1 Of StudentsInfo table
                    TextBox1.Text = sqlSQLReader.GetString(0)

                    Dim bImage As Byte() = CType(sqlSQLReader("itemimage"), Byte())

                    Using ms As New IO.MemoryStream(bImage)

                        PictureBox1.Image = Image.FromStream(ms)
                        PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage

                    End Using

                End While

            Else

                'No Match Was Found
                MessageBox.Show("No Rows Found.")

            End If

            'Close Reader Object
            sqlSQLReader.Close()

        End Using
    End Sub

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        Dim result As DialogResult = OpenFileDialog1.ShowDialog()
        If result = DialogResult.OK Then
            strImagePath = OpenFileDialog1.FileName

            imgTemp = Image.FromFile(OpenFileDialog1.FileName)

            PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage

            PictureBox1.Image = imgTemp
        End If
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If strImagePath <> "" Then

            Dim strImageFinal As String

            strImageFinal = strImagePath

            While (strImageFinal.Contains("\"))

                strImageFinal = strImageFinal.Remove(0, strImageFinal.IndexOf("\") + 1)

            End While

            Dim msImage As New IO.MemoryStream

            If strImagePath.Contains("jpeg") Or strImagePath.Contains("jpg") Then

                imgTemp.Save(msImage, System.Drawing.Imaging.ImageFormat.Jpeg)

            End If

            If strImagePath.Contains("png") Then

                imgTemp.Save(msImage, System.Drawing.Imaging.ImageFormat.Png)

            End If

            If strImagePath.Contains("gif") Then

                imgTemp.Save(msImage, System.Drawing.Imaging.ImageFormat.Gif)

            End If

            If strImagePath.Contains("bmp") Then

                imgTemp.Save(msImage, System.Drawing.Imaging.ImageFormat.Bmp)

            End If

            Dim bImage() As Byte = msImage.ToArray()

            'INSERT SQL Query
            'This Query Inserts Our Input Data Into The Table
            strQuery = "Insert Into Test (itemName, itemImage) Values (@name, @image)"

            'Instantiate Connection Object
            sqlSQLCon = New SqlConnection(strSQLConn)

            'Using Structure Simplifies Garbage Collection And Ensures That The Object Will Be Disposed Of Correctly Afterwards
            Using sqlSQLCon

                'Create Command Object, To Make Use Of SQL Query
                Dim sqlSQLCommand As New SqlCommand(strQuery, sqlSQLCon)

                'Create Parameters Instead Of Hardcoding Values
                'Name = Whatever txtName Contains
                'Surname = Whatever txtSurname Contains
                'StudentNo = txtStudentNumber Text
                sqlSQLCommand.Parameters.AddWithValue("name", TextBox1.Text)
                sqlSQLCommand.Parameters.Add("image", SqlDbType.Image, bImage.Length).Value = bImage

                'Open Connection To The Database
                sqlSQLCon.Open()

                'Execute Command As NonQuery As It Doesn't Return Info
                sqlSQLCommand.ExecuteNonQuery()

                'Inform User That Row Has Been Added
                MessageBox.Show("Added")

            End Using

        End If
    End Sub
End Class