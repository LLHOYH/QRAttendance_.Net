<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="GenerateQR.aspx.cs" Inherits="QRAttendance.GenerateQR" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #divUP {
            margin-top: 20px;
        }

        .tdPadding {
            padding: 15px 15px;
        }

        #alignLeft {
            text-align: left !important;
        }
        #panel_lvContainer{
            height:700px;
            overflow-x:scroll;
            display:inline-flex;
        }
    </style>
    <script src="Assets/Jquery/jquery-3.3.1.min.js"></script>


    <script>



        $(document).ready(function () {
            CheckFormValidity();

            $(".ElementValidator").on("change keyup", function () {
                CheckFormValidity();
            });

        });

        //javascript function ready to be called
        function CheckFormValidity() {
            if ($("#btn_GenerateQR").val() === "Stop Attendance Taking") {
                document.getElementById("<%=btn_GenerateQR.ClientID%>").disabled = false;
            }
            else if ($("#ddl_Lessons").val() !== null && $("#ddl_Lessons").val() !== "" && $("#ddl_Lessons").val() !== "0") {
                document.getElementById("<%=btn_GenerateQR.ClientID%>").disabled = false;
            }
            else {
                document.getElementById("<%=btn_GenerateQR.ClientID%>").disabled = true;
            }


        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="text-center alert alert-warning">Attendance Taking</h2>

    <br />
    <asp:Panel ID="panel_Error" CssClass="alert alert-danger" runat="server" Visible="False">
        <asp:Label ID="lbl_Error" runat="server" Text=""></asp:Label>
    </asp:Panel>

    <asp:Panel ID="panel_Success" CssClass="alert alert-success" runat="server" Visible="False">
        <asp:Label ID="lbl_Success" runat="server" Text=""></asp:Label>
    </asp:Panel>

    <div class="container-fluid">
        <div class="row">
            <%--            <div class="col-md-3">
            </div>--%>
            <div class="col-md-6">
                <div class="text-center container">
                    <h4>Choose Your Lesson</h4>
                    <div class="form-group row">
                        <label class="col-sm-2 col-form-label">Module:</label>
                        <div class="col-sm-4">
                            <asp:DropDownList runat="server" ID="ddl_Modules" CssClass="form-control ElementValidator ToDisableBeforeUnload"
                                AutoPostBack="true" OnSelectedIndexChanged="ddl_Modules_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                        <label class="col-sm-2 col-form-label">Module Group:</label>
                        <div class="col-sm-3">
                            <asp:DropDownList runat="server" ID="ddl_ModuleGroups" CssClass="form-control ElementValidator ToDisableBeforeUnload"
                                AutoPostBack="true" OnSelectedIndexChanged="ddl_ModuleGroups_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-2 col-form-label">Lesson:</label>
                        <div class="col-sm-9">
                            <asp:DropDownList runat="server" ID="ddl_Lessons" CssClass="form-control ElementValidator ToDisableBeforeUnload"
                                AutoPostBack="true" ClientIDMode="Static">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group row" runat="server" id="divFYPJ">
                        <label class="col-sm-2"></label>
                        <div class="col-sm-6" id="alignLeft">
                            <%--<asp:CheckBox runat="server" ID="cb_FYPJ" ClientIDMode="static" CssClass="col-form-label" />--%>
                            <asp:Label runat="server" ID="lbl_FYPJ" ClientIDMode="Static" Visible="false" CssClass="col-form-label">This is a FYPJ session</asp:Label>
                        </div>
                    </div>
                    <div class="text-center form-group row">
                        <div class="col-sm-2"></div>
                        <div class="col-sm-6">
                            <asp:Button runat="server" ID="btn_GenerateQR" CssClass="btn btn-info btn-block ToDisableBeforeUnload" ForeColor="White"
                                OnClick="btn_GenerateQR_Click" ClientIDMode="Static" Text="Generate QR" />
                        </div>
                    </div>

                <asp:HiddenField runat="server" ID="hf_QRInProgress" Visible="false" />
                <asp:HiddenField runat="server" ID="hf_LessonID" Visible="false" />
                <asp:HiddenField runat="server" ID="hf_IsFYPJ" Visible="false" />
                <asp:ScriptManager runat="server" ID="scriptManager1" EnablePageMethods="true"></asp:ScriptManager>

                <asp:Timer runat="server" ID="timer_CountDown" Interval="1000" OnTick="timer_CountDown_Tick" Enabled="false"></asp:Timer>

                <div class="text-center container" id="divUP">
                    <asp:UpdatePanel runat="server" ID="up_QR" Visible="false" class="text-center container" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="timer_CountDown" />
                        </Triggers>
                        <ContentTemplate>

                            <div class="text-center container">
                                <asp:Label runat="server" ID="lbl_CountDown"></asp:Label>
                            </div>

                            <div class="text-center container">
                                <asp:Image runat="server" ID="img_QRCode" Width="500px" Height="500px" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:Label runat="server" ID="lbl_LessonID" Style="opacity: 0;" ClientIDMode="Static"></asp:Label>

                </div>
                </div>
            </div>



            

   <script type="text/javascript">  //this js section allows the student attendance list scroll to remain on that position after each refresh/postback

      // It is important to place this JavaScript code after ScriptManager1
      var xPos, yPos;
      var prm = Sys.WebForms.PageRequestManager.getInstance();

      function BeginRequestHandler(sender, args) {
        if ($get('<%=panel_lvContainer.ClientID%>') != null) {
          // Get X and Y positions of scrollbar before the partial postback
          xPos = $get('<%=panel_lvContainer.ClientID%>').scrollLeft;
          yPos = $get('<%=panel_lvContainer.ClientID%>').scrollTop;
        }
     }

     function EndRequestHandler(sender, args) {
         if ($get('<%=panel_lvContainer.ClientID%>') != null) {
           // Set X and Y positions back to the scrollbar
           // after partial postback
           $get('<%=panel_lvContainer.ClientID%>').scrollLeft = xPos;
           $get('<%=panel_lvContainer.ClientID%>').scrollTop = yPos;
         }
     }

     prm.add_beginRequest(BeginRequestHandler);
     prm.add_endRequest(EndRequestHandler);
 </script>


            <div class="col-md-6">
                <div class="text-center container">
                    <asp:UpdatePanel runat="server" ID="up_Attendance" Visible="false">     <%-- update panel for student attendance--%>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="timer_CountDown" />
                        </Triggers>
                        <ContentTemplate>
                            <asp:Panel runat="server" ID="panel_lvContainer" ClientIDMode="Static" CssClass="lvContainer">
                                <asp:ListView runat="server" ID="lv_StudentAttendance" DataKeyNames="ScheduleID">
                                <LayoutTemplate>
                                    <table>
                                        <tr>
                                            <th runat="server">Admin No.</th>
                                            <th runat="server">Name </th>
                                            <th runat="server">Status</th>
                                        </tr>
                                        <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                                    </table>
                                </LayoutTemplate>

                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_AdminNumber" runat="server"
                                                CssClass='<%# (bool)Eval("AttendanceStatus") ? "form-control alert-success" : "form-control alert-secondary" %>'
                                                Text='<%# Eval("AdminNumber").ToString().ToUpper()%>' Width="90px" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_StudentName" runat="server" CssClass="form-control" Text='<%# Eval("StudentName")%>' Width="350px" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_AttendanceStatus" runat="server" AutoPostBack="true" CssClass="form-control" Checked='<%# Eval("AttendanceStatus") %>' Width="40px"
                                                OnCheckedChanged="cb_AttendanceStatus_CheckedChanged" />
                                        </td>
                                    </tr>

                                </ItemTemplate>
                            </asp:ListView>
                            </asp:Panel>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
