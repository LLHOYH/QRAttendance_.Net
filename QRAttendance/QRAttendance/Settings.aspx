<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Settings.aspx.cs" Inherits="QRAttendance.Settings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #panel_LocSetting {
            margin-left: 30px;
            padding-left: 10px;
            border-left: 1px solid gray;
        }

        #panel_OverwriteSetting{
            margin-left: 30px;
            padding-left: 10px;
            border-left: 1px solid gray;
        }

        #enable_label {
            border-right: 1px white solid;
        }

        #disable_label {
            border-left: 1px white solid;
        }

        #include_field {
            border-right: 1px white solid;
        }

        #disclude_field {
            border-left: 1px white solid;
        }

        img {
            width: 230px;
            height: auto;
        }

        .inlineDiv {
            display: inline;
        }

        .page_wrapper {
            margin-left: 5%;
        }
    </style>


    <script>

        //set lightbox (onclick image to enlarge) configurations
        lightbox.option({
            'resizeDuration': 200,
            'wrapArount': true,
            'fadeDuration': 200,
            'imageFadeDuration': 200
        });

        $(document).ready(function () {

            ResetAlert();
            ResetPanelToggle();
            GetLocationSettings();
            GetIncludeFieldStatus();
            GetOverwriteStatus();

            $('input[type=radio][name=locStatus]').change(function () {
                var locStatus = this.value;
                UpdateLocationStatus(locStatus);
            });

            $('input[type=radio][name=includeField]').change(function () {
                var includeField = this.value;
                UpdateIncludeFieldStatus(includeField);
            });

            $('input[type=radio][name=overwriteDevice]').change(function () {
                var overwriteStatus = this.value;
                UpdateOverwriteSetting(overwriteStatus);
            });

            $('#a_UpdateChances').click(function () {
                var overwriteStatus = $('input[type=radio][name=overwriteDevice]').val();
                console.log(overwriteStatus);
                UpdateOverwriteSetting(overwriteStatus);
            })
        })


        //Location Function Setting

        function GetLocationSettings() {
            $.ajax({
                type: "GET",
                url: "Settings.aspx/GetLocationSettings",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    SetLocationStatusChecked(result.d);
                    Show_Hide_LocSetting_Panel(result.d);
                },
                error: function (error) {
                    OnError("Failed to Fetech Information!");
                    Show_Hide_LocSetting_Panel(false);
                }
            })
        }

        function GetIncludeFieldStatus() {
            $.ajax({
                type: "GET",
                url: "Settings.aspx/GetIncludeFieldStatus",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    SetIncludeFieldChecked(result.d);
                    Show_Hide_Field_Panel(result.d);
                },
                error: function (error) {
                    OnError("Failed to Fetech Information!");
                    Show_Hide_Field_Panel(false);
                }
            })
        }

        function SetLocationStatusChecked(locationStatus) {
            if (locationStatus) {
                $("#enable_lable").addClass("active");
                $("#disable_lable").removeClass("active");
            }
            else {
                $("#disable_lable").addClass("active");
                $("#enable_lable").removeClass("active");
            }
        }

        function SetIncludeFieldChecked(includeField) {
            if (includeField) {
                $("#include_field").addClass("active");
                $("#disclude_field").removeClass("active");
            }
            else {
                $("#disclude_field").addClass("active");
                $("#include_field").removeClass("active");
            }
        }

        function UpdateLocationStatus(locStatus) {
            $.ajax({
                type: "POST",
                url: "Settings.aspx/UpdateLocationEnable",
                data: "{'locationStatus':'" + locStatus + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    // OnSuccess("Update Done!");
                    if (response.d > 0) {
                        var show = false;

                        if (locStatus == "enable")
                            show = true;

                        Show_Hide_LocSetting_Panel(show);
                    }
                    else {
                        OnError("Update Failed!");
                    }
                },
                error: function (xhr, status, error) {
                    OnError("Update Failed!");
                }
            })
        }

        function UpdateIncludeFieldStatus(incFieldStatus) {
            $.ajax({
                type: "POST",
                url: "Settings.aspx/UpdateIncludeFieldStatus",
                data: "{'incFieldStatus':'" + incFieldStatus + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    // OnSuccess("Update Done!");
                    console.log(response.d);
                    if (response.d > 0) {
                        var show = false;

                        if (incFieldStatus == "include")
                            show = true;

                        Show_Hide_Field_Panel(show);
                    }
                    else {
                        OnError("Update Failed!");
                    }
                },
                error: function (xhr, status, error) {
                    OnError("Update Failed!");
                }
            })
        }




        //Overwrite Device Setting

        function GetOverwriteStatus() {
            $.ajax({
                type: "GET",
                url: "Settings.aspx/GetOverwriteStatus",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    SetOverwriteStatusChecked(result.d);
                    Show_Hide_OverwriteSetting_Panel(result.d);
                },
                error: function (error) {
                    OnError("Failed to Fetch Information!");
                    Show_Hide_OverwriteSetting_Panel(false);
                }
            })
        }

        function SetOverwriteStatusChecked(overwriteStatus) {
            if (overwriteStatus) {
                $("#overwrite_enable").addClass("active");
                $("#overwrite_disable").removeClass("active");
            }
            else {
                $("#overwrite_disable").addClass("active");
                $("#overwrite_enable").removeClass("active");
            }
        }

        function UpdateOverwriteSetting(overwriteStatus) {

            var totalChancesText = $('#<%=tb_Chances.ClientID%>').val();

            if (!isNaN(totalChancesText)) {
                var totalChances = parseInt(totalChancesText);

                $.ajax({
                    type: "POST",
                    url: "Settings.aspx/UpdateOverwriteSetting",
                    data: "{'overwriteStatus':'" + overwriteStatus + "','totalChances':'" + totalChances + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        // OnSuccess("Update Done!");
                        console.log(response.d);
                        if (response.d > 0) {
                            var show = false;

                            if (overwriteStatus == "enable")
                                show = true;

                            Show_Hide_OverwriteSetting_Panel(show);
                        }
                        else {
                            OnError("Update Failed!");
                        }
                    },
                    error: function (xhr, status, error) {
                        OnError("Update Failed!");
                    }
                })
            }
            else {
                OnError("Please Ensure Chances Field is Filled and a Valid Number!");
            }
        }


        //panel toggle (shows or hides)
        function Show_Hide_LocSetting_Panel(show) {
            if (show)
                $('#<%= panel_LocSetting.ClientID %>').toggle(true);
            else
                $('#<%= panel_LocSetting.ClientID %>').toggle(false);
        }

        function Show_Hide_Field_Panel(show) {
            if (show)
                $('#<%= panel_IncludeField.ClientID %>').toggle(true);
            else
                $('#<%= panel_IncludeField.ClientID %>').toggle(false);
        }

        function Show_Hide_OverwriteSetting_Panel(show) {
            if (show)
                $('#<%= panel_OverwriteSetting.ClientID %>').toggle(true);
            else
                $('#<%= panel_OverwriteSetting.ClientID %>').toggle(false);
        }

        function ResetPanelToggle() {
            $('#<%= panel_IncludeField.ClientID %>').toggle(false);
            $('#<%= panel_LocSetting.ClientID %>').toggle(false);
        }

        function OnSuccess(msg) {
            $('#<%= panel_Success.ClientID %>').toggle(true);
            $('#<%= panel_Error.ClientID %>').toggle(false);
            $('#<%= lbl_Success.ClientID %>').text(msg);
        }

        function OnError(msg) {
            $('#<%= panel_Error.ClientID %>').toggle(true);
            $('#<%= panel_Success.ClientID %>').toggle(false);
            $('#<%= lbl_Error.ClientID %>').text(msg);
        }

        function ResetAlert() {
            $('#<%= panel_Success.ClientID %>').toggle(false);
            $('#<%= panel_Error.ClientID %>').toggle(false);
        }

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h2 class="text-center alert alert-warning">Settings</h2>

    <br />
    <asp:Panel ID="panel_Error" CssClass="alert alert-danger" runat="server" ClientIDMode="Static">
        <asp:Label ID="lbl_Error" runat="server" Text=""></asp:Label>
    </asp:Panel>

    <asp:Panel ID="panel_Success" CssClass="alert alert-success" runat="server" ClientIDMode="Static">
        <asp:Label ID="lbl_Success" runat="server" Text=""></asp:Label>
    </asp:Panel>


    <div class="page_wrapper">
        <div class="form-group row">
            <label class="col-sm-4">
                Enable Location Feature In Attendance Taking?
            </label>
            <div class="btn-group btn-group-toggle" data-toggle="buttons">
                <label id="enable_lable" class="btn btn-secondary">
                    <input type="radio" name="locStatus" id="radio_enable" value="enable" autocomplete="off">
                    Enable
                </label>
                <label id="disable_lable" class="btn btn-secondary">
                    <input type="radio" name="locStatus" id="radio_disable" value="disable" autocomplete="off">
                    Disable
                </label>
            </div>
        </div>



        <br />
        <br />
        <br />
        <asp:Panel runat="server" ID="panel_LocSetting" ClientIDMode="Static">

            <!--school center-->

            <div class="row">

                <div class="inlineDiv col-md-8">
                    <div class="form-group row">
                        <label class="col-sm-4 col-form-label">School Centered Latitude:</label>
                        <div class="col-sm-8">
                            <asp:Label runat="server" ID="lbl_SchoolLat" CssClass="form-control"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-4 col-form-label">School Centered Longtitude:</label>
                        <div class="col-sm-8">
                            <asp:Label runat="server" ID="lbl_SchoolLong" CssClass="form-control"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-4 col-form-label">School Radius (In Metres): </label>
                        <div class="col-sm-8">
                            <asp:Label runat="server" ID="lbl_SchoolRadius" CssClass="form-control"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-4">
                            Include School Field Area?
                        </label>
                        <div class="col-sm-4 btn-group btn-group-toggle" data-toggle="buttons">
                            <label id="include_field" class="btn btn-secondary">
                                <input type="radio" name="includeField" id="radio_include" value="include" autocomplete="off">
                                Include
                            </label>
                            <label id="disclude_field" class="btn btn-secondary">
                                <input type="radio" name="includeField" id="radio_disclude" value="disclude" autocomplete="off">
                                Disclude
                            </label>
                        </div>
                    </div>

                </div>
                <div class="inlineDiv col-md-4">
                    <a href="Assets/Images/NYP_Schools.jpeg" class="lightboxImage" data-lightbox="schools">
                        <img src="Assets/Images/NYP_Schools.jpeg" />
                    </a>
                </div>


            </div>



            <!--field center-->

            <asp:Panel runat="server" ID="panel_IncludeField" ClientIDMode="Static">


                <div class="row">
                    <div class="inlineDiv col-md-8">

                        <div class="form-group row">
                            <label class="col-sm-4 col-form-label">Field Centered Latitude:</label>
                            <div class="col-sm-8">
                                <asp:Label runat="server" ID="lbl_FieldLat" CssClass="form-control"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-4 col-form-label">Field Centered Longtitude:</label>
                            <div class="col-sm-8">
                                <asp:Label runat="server" ID="lbl_FieldLong" CssClass="form-control"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-4 col-form-label">Field Radius (In Metres): </label>
                            <div class="col-sm-8">
                                <asp:Label runat="server" ID="lbl_FieldRadius" CssClass="form-control"></asp:Label>
                            </div>
                        </div>

                    </div>
                    <div class="inlineDiv col-md-4">
                        <a href="Assets/Images/NYP_Field.jpeg" class="lightboxImage" data-lightbox="field">
                            <img src="Assets/Images/NYP_Field.jpeg" />
                        </a>
                    </div>
                </div>
            </asp:Panel>

        </asp:Panel>
    </div>




    <br />
    <br />
    <br />


    <!--Overwrite Device Setting-->

    <div class="page_wrapper">
        <div class="form-group row">
            <label class="col-sm-4">
                Enable Overwrite Device Setting?
            </label>
            <div class="btn-group btn-group-toggle" data-toggle="buttons">
                <label id="overwrite_enable" class="btn btn-secondary">
                    <input type="radio" name="overwriteDevice" value="enable" autocomplete="off">
                    Enable
                </label>
                <label id="overwrite_disable" class="btn btn-secondary">
                    <input type="radio" name="overwriteDevice" value="disable" autocomplete="off">
                    Disable
                </label>
            </div>
        </div>



        <br />
        <br />
        <br />
        <asp:Panel runat="server" ID="panel_OverwriteSetting" ClientIDMode="Static">

            <div class="row">

                <div class="inlineDiv col-md-8">
                    <div class="form-group row">
                        <label class="col-sm-4 col-form-label">Total Chances Per Sem:</label>
                        <div class="col-sm-7">
                            <asp:TextBox runat="server" ID="tb_Chances" ClientIDMode="Static" CssClass="form-control"
                                onkeypress="return (event.charCode == 8 || event.charCode == 0 || event.charCode == 13) ? null : event.charCode >= 48 && event.charCode <= 57">
                                <!--onkeypress here allows only positive whole numbers-->
                            </asp:TextBox>
                        </div>
                        <div class="col-sm-1">
                            <a id="a_UpdateChances" href="#">Update</a>
                        </div>
                    </div>
                </div>

            </div>

        </asp:Panel>
    </div>


</asp:Content>
