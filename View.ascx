<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="Plugghest.Modules.CreateCourse.View" %>

<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<script src="http://dnndev.me/Script/bPopup.js"></script>
<script>
    $(document).ready(function () {
        $('#element_to_pop').hide();
    });
    $(function ($) {

        //  DOM Ready
        $(function () {

            // Binding a click event
            // From jQuery v.1.7.0 use .on() instead of .bind()
            $('#my-button').bind('click', function (e) {


                e.preventDefault();
                $('#element_to_pop').bPopup({
                    appendTo: 'form'
                                       , closeClass: 'ui-dialog-titlebar-close'
                                       , speed: 650
                       , transition: 'slideIn'
                               , zIndex: 2
                               , modalClose: true
                });

            });

        });

    })(jQuery);
</script>
<style>
    .dnnLabel
    {
        width: 128px;
    }

    #element_to_pop
    {       
        display: block;
        height: auto;
        left: 340px;
        position: absolute;
        top: 120px !important;
        width: 650px;
        background: none repeat scroll 0 0 #FFFFFF;
        box-shadow: 0 0 25px 0 rgba(0, 0, 0, 0.75);
        padding: 18px;
        z-index: 100000;
    }

        #element_to_pop .logo
        {
            color: #2B91AF;
            font: bold 325% 'Petrona',sans;
        }

    .button.b-close, .button.bClose
    {
        border-radius: 7px 7px 7px 7px;
        box-shadow: none;
        font: bold 131% sans-serif;
        padding: 0 6px 2px;
        position: absolute;
        right: -7px;
        top: -7px;
    }

    .button
    {
        background-color: #2B91AF;
        border-radius: 10px;
        box-shadow: 0 2px 3px rgba(0, 0, 0, 0.3);
        color: #FFF;
        cursor: pointer;
        display: inline-block;
        padding: 10px 20px;
        text-align: center;
        text-decoration: none;
    }
</style>



<a id="my-button" href="#">
    <asp:Label ID="lblCreateCourse" runat="server"></asp:Label>
</a>
<br />
<asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
<div id="element_to_pop" class="ui-dialog ui-widget ui-widget-content ui-corner-all ui-front dnnFormPopup ui-draggable ui-resizable"
    style="top: 120px !important">
    <div class="ui-dialog-titlebar ui-widget-header ui-corner-all ui-helper-clearfix">
        <span id="ui-id-1" class="ui-dialog-title"> <asp:Label ID="lblCreateCourse1" runat="server"></asp:Label></span>
        <div class="dnnModalCtrl">
            <a class="dnnToggleMax" href="#">
                <span>Max</span>

            </a>
            <button class="ui-dialog-titlebar-close"></button>
        </div>

    </div>
    <%--    <span class="button b-close"><span>X</span></span>--%>
    <br />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <fieldset>
                <div class="dnnFormItem">
                    <dnn:label id="lblLanguage" runat="server" controlname="DDLanguage" />
                    <asp:DropDownList ID="DDLanguage" runat="server"></asp:DropDownList>
                </div>

                <div class="dnnFormItem">
                    <dnn:label id="lblTitle" runat="server" Text="Title" />
                    <asp:TextBox ID="txtTitle" runat="server" />
                    <asp:RequiredFieldValidator ID="val_txtTitle" Display="Dynamic" runat="server" CssClass="valcls" ErrorMessage="Please Enter Title" ControlToValidate="txtTitle"></asp:RequiredFieldValidator>
                </div>

                <div class="dnnFormItem">
                    <dnn:label id="lblPluggs" runat="server" Text="Plugg" />
                    <asp:TextBox ID="txtPluggs" runat="server" />
                </div>




                <div style="margin-left: 413px">
                    <asp:Label ID="CIInfo" runat="server"></asp:Label>
                </div>

                <div style="margin-left: 413px">
                    <asp:LinkButton ID="btnCheck" runat="server" OnClick="btnCheck_Click"
                        resourcekey="btnCheck" CssClass="dnnPrimaryAction" Text="Check" />
                </div>



                <div class="dnnFormItem">
                    <asp:RadioButtonList ID="rdbtnWhoCanEdit" runat="server" RepeatDirection="Horizontal">                       
                    </asp:RadioButtonList>
                </div>

                <br />

                <div class="dnnFormItem">
                    <dnn:TextEditor ID="txtHtmlText" runat="server" Width="100%" Height="200px" />
                </div>



            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="dnnFormItem">
        <ul class="dnnActions dnnClear">
            <li>
                <asp:LinkButton ID="btnSubmit" runat="server"
                    resourcekey="btnSubmit" CssClass="dnnPrimaryAction" Text="Submit" OnClick="btnSubmit_Click" /></li>
            <li>
                <asp:LinkButton ID="btnCancel" runat="server"
                    resourcekey="btnCancel" CssClass="dnnSecondaryAction" Text="Cancel" OnClick="btnCancel_Click" /></li>
        </ul>
    </div>
</div>
