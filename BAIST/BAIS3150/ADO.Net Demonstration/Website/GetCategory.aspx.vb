
Imports System.Data
Imports System.Data.SqlClient


Partial Class GetCategory
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Using connection = New SqlConnection("Data Source=(LocalDB)\v11.0; Database=Northwind; Integrated Security=SSPI")
            connection.Open()

            Dim northwindProductsByCategory = New DataSet("NorthwindProductsByCategory")

            'Read in category
            Dim readCategoryCommand = New SqlCommand("ababcock1GetCategory", connection)
            readCategoryCommand.CommandType = CommandType.StoredProcedure

            Dim categoryParam = New SqlParameter()
            categoryParam.Direction = ParameterDirection.Input
            categoryParam.ParameterName = "@CategoryID"
            categoryParam.SqlValue = 4
            categoryParam.SqlDbType = SqlDbType.Int
            readCategoryCommand.Parameters.Add(categoryParam)

            Dim dataCategoryAdapter = New SqlDataAdapter(readCategoryCommand)
            Dim northwindCategory = New DataTable("NorthwindCategory")
            dataCategoryAdapter.Fill(northwindCategory)
            northwindProductsByCategory.Tables.Add(northwindCategory)

            'Read in products for that category
            Dim readProductsCommand = New SqlCommand("ababcock1GetProductsByCategory", connection)
            readProductsCommand.CommandType = CommandType.StoredProcedure

            Dim productsParam = New SqlParameter()
            productsParam.Direction = ParameterDirection.Input
            productsParam.ParameterName = "@CategoryID"
            productsParam.SqlValue = 4
            productsParam.SqlDbType = SqlDbType.Int
            readProductsCommand.Parameters.Add(productsParam)

            Dim productsDataAdapter = New SqlDataAdapter(readProductsCommand)
            Dim northwindProducts = New DataTable("NorthwindProducts")
            productsDataAdapter.Fill(northwindProducts)
            northwindProductsByCategory.Tables.Add(northwindProducts)

            connection.Close()

            'Display category data
            Response.Write("NorthwindCategory Columns:<br />")
            For i As Integer = 0 To northwindCategory.Columns.Count - 1
                Response.Write(northwindCategory.Columns(i).ColumnName & "; ")
            Next
            Response.Write("<br /><br />")

            Response.Write("NorthwindCategory Values:<br />")
            For i As Integer = 0 To northwindCategory.Rows.Count - 1
                For j As Integer = 0 To northwindCategory.Columns.Count - 1
                    Response.Write(northwindCategory.Rows(i).Item(j) & "; ")
                Next
                Response.Write("<br />")
            Next
            Response.Write("<br /><br />")

            'Display products data
            Response.Write("NorthwindProducts Columns:<br />")
            For i As Integer = 0 To northwindProducts.Columns.Count - 1
                Response.Write(northwindProducts.Columns(i).ColumnName & "; ")
            Next
            Response.Write("<br /><br />")

            Response.Write("NorthwindProducts Values:<br />")
            For i As Integer = 0 To northwindProducts.Rows.Count - 1
                For j As Integer = 0 To northwindProducts.Columns.Count - 1
                    Response.Write(northwindProducts.Rows(i).Item(j) & "; ")
                Next
                Response.Write("<br />")
            Next
            Response.Write("<br /><br />")

        End Using
    End Sub

End Class
