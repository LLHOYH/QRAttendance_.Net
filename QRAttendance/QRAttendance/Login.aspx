<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="QRAttendance.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <meta name="description" content="" />
    <meta name="author" content="" />
    <title>Attendance System - Login</title>

    <link href="Assets/Font-Awesome/css/all.css" rel="stylesheet" />
    <link href="Assets/Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="Assets/Sb-admin/css/sb-admin.min.css" rel="stylesheet" />

    <script src="Assets/Jquery/jquery-3.3.1.min.js"></script>
        <!-- Bootstrap core JavaScript-->
    <script src="Assets/Bootstrap/js/bootstrap.bundle.min.js"></script>
    <!-- Core plugin JavaScript-->
    <script src="Assets/jquery-easing/jquery.easing.min.js"></script>

    <style>
        #nyp-logo{
            width: 100%;
        }

        .card {
            display: block;
        }
    </style>

    <script>
        $(document).ready(function () {
            $("#<%=tb_StaffID.ClientID%>").focus();
        });

        function ValidateFields() {
            if ($("#<%=tb_StaffID.ClientID%>").val() == "" || $("<%=tb_Password.ClientID%>").val()) {
                alert("Please enter your user ID and password.");
                $("#<%=tb_StaffID.ClientID%>").focus();
                return false;
            }
        }

        function encodeMyHtml() {
            // User ID
            var str_StaffID = $("#<%=tb_StaffID.ClientID%>").val()
            var encodedStaffID = escape(str_StaffID);
            // Password
            var str_Password = $("<%=tb_Password.ClientID%>").val();
            var encodedPassword = escape(str_Password);

            
            $('#<%=hf_EncodedStaffID%>').val(encodedStaffID);
            $('#<%=hf_EncodedPassword.ClientID%>').val(encodedPassword);

            $("#<%=tb_StaffID.ClientID%>").val("");
            $("#<%=tb_Password.ClientID%>").val("");
        }
    </script>
</head>
<body class="bg-dark">
    <div class="container">
        <div class="card card-login mx-auto mt-5">
            <div class="card-header">
                <img src="Assets/Images/Marketing%20NYP%20Logo_Horizontal_CMYK.png" id="nyp-logo" />
                <br />
                <h5>SASCET2.0 - Login</h5></div>
            <div class="card-body">
                <form id="form1" runat="server">
                    <div class="form-group">
                        <asp:Label ID="Label1" runat="server" Text="Staff ID" AssociatedControlID="tb_StaffID"></asp:Label>
                        <asp:TextBox ID="tb_StaffID" runat="server" CssClass="form-control text-uppercase" autocomplete="off"></asp:TextBox>
                        <asp:HiddenField ID="hf_EncodedStaffID" runat="server" />
                        <%--<label for="exampleInputEmail1">Email address</label>
                        <input class="form-control" id="exampleInputEmail1" type="email" aria-describedby="emailHelp" placeholder="Enter email">--%>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label2" runat="server" Text="Password" AssociatedControlID="tb_Password"></asp:Label>
                        <asp:TextBox ID="tb_Password" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                        <asp:HiddenField ID="hf_EncodedPassword" runat="server" />
                        <%--<label for="exampleInputPassword1">Password</label>
                        <input class="form-control" id="exampleInputPassword1" type="password" placeholder="Password">--%>
                    </div>
                    <asp:Button ID="btnLogin" runat="server" CssClass="btn btn-primary btn-block" Text="Login" 
                        OnClientClick="return ValidateFields()" OnClick="btnLogin_Click" />

                    <div class="form-group alert-danger">
                        <asp:Label ID="lb_Error" runat="server" Text="" CssClass="alert-danger" Visible="False"></asp:Label>
                    </div>

                    <div class="form-group">
                        <asp:Label ID="Label3" runat="server" Text="" CssClass="alert-danger" Visible="False"></asp:Label>
                    </div>
                </form>
                <div class="text-center">
                </div>
            </div>
        </div>
    </div>

</body>
</html>
