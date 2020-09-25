<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ImportData.aspx.cs" Inherits="QRAttendance.ImportData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .panels{
            border:1px solid #e68a00;
            border-radius:10px;
            padding:12px;
        }
    </style>

    <script>

        $(document).ready(function () {

            CheckFormValidity();

            $(".ElementValidator").on("change keyup", function () {
                CheckFormValidity();
            });
        });


        function CheckFormValidity() {
            if ($("#tb_ImportForAcadYr").val() !== null && $("#ddl_ImportForSemester").val() !== null &&
                $("#tb_ImportForAcadYr").val() !== "" && $("#ddl_ImportForSemester").val() !== "" &&
                document.getElementById("<%=fu_ExcelUpload.ClientID%>").files.length !== 0) {

                document.getElementById("<%=btn_ImportData.ClientID%>").disabled = false;
            }
            else {
                $("<%=fu_ExcelUpload.ClientID%>").text($("<%=fu_ExcelUpload.ClientID%>").val());
                document.getElementById("<%=btn_ImportData.ClientID%>").disabled = true;
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Import Data For New Semester</h2>

    <p>Using the file upload function will erase all data from the current database and replace with the data in the excel file.</p>
    
    <p>Please make sure you have a copy of current data by using the
        <asp:LinkButton runat="server" ID="lknBtn_Export" OnClick="lknBtn_Export_Click" OnClientClick="showLoaderWithTimeOut();">
            Export To Excel
        </asp:LinkButton>
    function!</p>

    <asp:Panel ID="panel_Error" CssClass="alert alert-danger" runat="server" Visible="False">
        <asp:Label ID="lbl_Error" runat="server" Text=""></asp:Label>
    </asp:Panel>

    <asp:Panel ID="panel_Success" CssClass="alert alert-success" runat="server" Visible="False">
        <asp:Label ID="lbl_Success" runat="server" Text=""></asp:Label>
    </asp:Panel>

    

    <asp:Panel runat="server" ID="panel_LastImportInfo" CssClass="panels">
        <h4>Last Import</h4>
        <div class="form-group row">
            <label class="col-sm-2 col-form-label">Imported Date: </label>
            <div class="col-sm-10">
                <asp:Label runat="server" ID="lbl_LastImportDate" CssClass="form-control"></asp:Label>
            </div>
        </div>
        <div class="form-group row">
            <label class="col-sm-2 col-form-label">Imported For Acad Year: </label>
            <div class="col-sm-10">
                <asp:Label runat="server" ID="lbl_ImportForAcadYr" CssClass="form-control"></asp:Label>
            </div>
        </div>
        <div class="form-group row">
            <label class="col-sm-2 col-form-label">Imported For Semester: </label>
            <div class="col-sm-10">
                <asp:Label runat="server" ID="lbl_ImportForSemester" CssClass="form-control"></asp:Label>
            </div>
        </div>
        <div class="form-group row">
            <label class="col-sm-2 col-form-label">Imported By: </label>
            <div class="col-sm-10">
                <asp:Label runat="server" ID="lbl_ImportedBy" CssClass="form-control"></asp:Label>
            </div>
        </div>
        
    </asp:Panel>
    <br />
    <br />
    <asp:Panel runat="server" ID="panel_UploadForm" CssClass="panels">
        <h4>New Import</h4>

        <div class="form-group">
            <label>Import For The Academic Year: </label>
            <asp:TextBox runat="server" ID="tb_ImportForAcadYr" placeholder="2019/2020"
                ClientIDMode="Static" CssClass="ElementValidator form-control"></asp:TextBox>
        </div>

        <div class="form-group">
            <label>Import For Semester: </label>
            <asp:DropDownList runat="server" ID="ddl_ImportForSemester"
                ClientIDMode="Static" CssClass="ElementValidator form-control">
                <asp:ListItem Value="S1" Text="S1"></asp:ListItem>
                <asp:ListItem Value="S2" Text="S2"></asp:ListItem>
            </asp:DropDownList>
        </div>


        <div class="form-group">
            <label style="color:red;font-size:13px;">*Accept Only .xls and .xlsx File Type</label>
                    <div class="form-group row">
            <asp:FileUpload runat="server" ID="fu_ExcelUpload" CssClass="ElementValidator col-sm-2" />
            <div class="col-sm-10">
                <asp:Button runat="server" ID="btn_ImportData" CssClass="btn btn-info mb-2"
                    Text="Upload" OnClick="btn_ImportData_Click" OnClientClick="showLoader();" />
            </div>
        </div>
        </div>


    </asp:Panel>

</asp:Content>
