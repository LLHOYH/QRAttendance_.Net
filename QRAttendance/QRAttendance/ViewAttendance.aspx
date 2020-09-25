<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ViewAttendance.aspx.cs" Inherits="QRAttendance.ViewAttendance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="Assets/Jquery/jquery-ui.min.js"></script>
    <script src="Assets/Jquery/jquery.mask.min.js"></script>
    <link href="Assets/Jquery/jquery-ui.min.css" rel="stylesheet" />


    <style>
        .customTable {
            width: auto !important;
            margin-top: 30px;
        }

            .customTable th, .customTable td {
                padding: 5px 10px;
                vertical-align: middle;
                text-align: center;
                border: none;
            }

            .customTable td {
                cursor: pointer;
            }

            .customTable tr {
                border-bottom: 1px solid black;
            }

                .customTable tr:hover {
                    background-color: lightgray;
                }

        .gv {
            width: 100%;
        }

        .linkBtnDivs {
            justify-content: flex-end;
            align-items: flex-end;
            text-align: end;
            width: 100%;
            margin-right: 2%;
        }

        .gvLnkBtns {
            display: inline;
        }
    </style>

    <script>

        $(document).ready(function () {
            CheckFormValidity();

            //on page load, set date picker for lesson date
            $("#tb_LessonDate").datepicker({
                dateFormat: 'dd-M-y',
                showAnim: "slide",
                maxDate: '0'
            });

            $(".ElementValidator").on("change keyup", function () {
                CheckFormValidity();
            });


            //set hrs absent and readonly attr based on the checkbox click
            $("#<%=gv_ViewAttendance.ClientID%> input[id*=cb_AttendanceStatus]").on("change", function () {

                var cbChecked = $(this).prop("checked");
                if (cbChecked) {
                    $(this).parents("tr").find("input[type=text]").attr("readonly", false);
                }
                else {
                    $(this).parents("tr").find("input[type=text]").attr("readonly", true);
                }
            })

            //$(".time").mask("00:00:00");

        });

        //javascript function ready to be called
        function CheckFormValidity() {
            if ($("#ddl_Lessons").val() !== null && $("#ddl_Lessons").val() !== "" && $("#ddl_Lessons").val() !== "0") {
                document.getElementById("<%=btn_ViewAttendance.ClientID%>").disabled = false;
            }
            else {
                document.getElementById("<%=btn_ViewAttendance.ClientID%>").disabled = true;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="text-center alert alert-warning">View Attendance</h2>

    <br />

    <asp:Panel ID="panel_Error" CssClass="alert alert-danger" runat="server" Visible="False">
        <asp:Label ID="lbl_Error" runat="server" Text=""></asp:Label>
    </asp:Panel>

    <asp:Panel ID="panel_Success" CssClass="alert alert-success" runat="server" Visible="False">
        <asp:Label ID="lbl_Success" runat="server" Text=""></asp:Label>
    </asp:Panel>


    <div class="text-center container">
        <h4>Choose Your Lesson</h4>
        <div class="form-group row">
            <label class="col-sm-2 col-form-label">Lesson Date:</label>
            <div class="col-sm-9">
                <asp:TextBox runat="server" ID="tb_LessonDate" ClientIDMode="Static"
                    AutoPostBack="true" CssClass="form-control ElementValidator"
                    OnTextChanged="tb_LessonDate_TextChanged" autocomplete="off"></asp:TextBox>
            </div>
        </div>
        <div class="form-group row">
            <label class="col-sm-2 col-form-label">Module:</label>
            <div class="col-sm-4">
                <asp:DropDownList runat="server" ID="ddl_Modules" CssClass="form-control ElementValidator"
                    AutoPostBack="true" OnSelectedIndexChanged="ddl_Modules_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
            <label class="col-sm-2 col-form-label">Module Group:</label>
            <div class="col-sm-3">
                <asp:DropDownList runat="server" ID="ddl_ModuleGroups" CssClass="form-control ElementValidator"
                    AutoPostBack="true" OnSelectedIndexChanged="ddl_ModuleGroups_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
        </div>
        <div class="form-group row">
            <label class="col-sm-2 col-form-label">Lesson:</label>
            <div class="col-sm-9">
                <asp:DropDownList runat="server" ID="ddl_Lessons" CssClass="form-control ElementValidator"
                    AutoPostBack="true" ClientIDMode="Static">
                </asp:DropDownList>
            </div>
        </div>
        <div class="text-center form-group row">
            <div class="col-sm-2"></div>
            <div class="col-sm-4">
                <asp:Button runat="server" ID="btn_ViewAttendance" CssClass="btn btn-info btn-block"
                    OnClick="btn_ViewAttendance_Click" ClientIDMode="Static" Text="View Attendance" />
            </div>
        </div>
    </div>

    <div id="viewAttDiv" class="container">
        <div class="row linkBtnDivs">
            <asp:LinkButton runat="server" ID="lnkBtn_Edit" CssClass="gvLnkBtns" OnClick="lnkBtn_Edit_Click" Visible="false">Edit</asp:LinkButton>
        </div>
        <asp:GridView runat="server" ID="gv_ViewAttendance" AutoGenerateColumns="False" CssClass="table table-hover gv"
            BackColor="White" BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" CellPadding="4" GridLines="Horizontal"
            DataKeyNames="ScheduleID">
            <Columns>
                <asp:TemplateField HeaderText="Admin Number">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lbl_AdminNumber" Text='<%# Bind("AdminNumber") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Name">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lbl_StudentName" Text='<%# Bind("StudentName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Attendance Status">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lbl_AttendanceStatus" Text='<%# (bool)Eval("AttendanceStatus") ? "Present" : "Absent" %>'></asp:Label>
                        <asp:CheckBox runat="server" ID="cb_AttendanceStatus" Visible="false"></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Clock In Time">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lbl_ClockInTime"
                            Text='<%# (bool)Eval("AttendanceStatus") ? Eval("ClockInTime").ToString() : "" %>'></asp:Label>

                        <asp:TextBox runat="server" ID="tb_ClockInTime" Visible="false" CssClass="form-control time" type="time" step="1"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Clock Out Time">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lbl_ClockOutTime"
                            Text='<%# (bool)Eval("AttendanceStatus") ? Eval("ClockOutTime").ToString() : "" %>'></asp:Label>
                        <asp:TextBox runat="server" ID="tb_ClockOutTime" Visible="false" CssClass="form-control time" type="time" step="1"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mark Attendance Through: ">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lbl_Method" Text='<%# (bool)Eval("AttendanceStatus") ? !string.IsNullOrEmpty(Eval("ClockInTime").ToString()) ? "QR Code": "Staff" : "" %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle CssClass="thead-dark" />
        </asp:GridView>

        <asp:Panel runat="server" ID="panel_LnkBtn" CssClass="row linkBtnDivs" Visible="false">
            <asp:LinkButton runat="server" ID="lnkBtn_Cancel" CssClass="gvLnkBtns" OnClick="lnkBtn_Cancel_Click">Cancel</asp:LinkButton>
            &nbsp;|&nbsp;
            <asp:LinkButton runat="server" ID="lnkBtn_Update" CssClass="gvLnkBtns" OnClick="lnkBtn_Update_Click" OnClientClick="showLoader();">Update</asp:LinkButton>
        </asp:Panel>

    </div>


    <asp:HiddenField runat="server" ID="hf_LessonType" Visible="false" />
</asp:Content>
