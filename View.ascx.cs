/*
' Copyright (c) 2014  Plugghest.com
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using System;
using System.Web.UI.WebControls;
using Plugghest.Modules.CreateCourse.Components;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Utilities;
using Plugghest.Base2;
using System.Collections.Generic;

namespace Plugghest.Modules.CreateCourse
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The View class displays the content
    /// 
    /// Typically your view control would be used to display content or functionality in your module.
    /// 
    /// View may be the only control you have in your project depending on the complexity of your module
    /// 
    /// Because the control inherits from CreateCourseModuleBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// 
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class View : CreateCourseModuleBase, IActionable
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                LoadCultureDropDownList();
                SetLocalizationText();
         
                if (!IsPostBack)
                {
                    rdbtnWhoCanEdit.DataSource = Enum.GetNames(typeof(EWhoCanEdit));
                    rdbtnWhoCanEdit.DataBind();
                    rdbtnWhoCanEdit.Items[1].Selected = true;
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
        private void LoadCultureDropDownList()
        {
            try
            {
                Localization myLoc = new Localization();
                Localization.LoadCultureDropDownList(DDLanguage, CultureDropDownTypes.NativeName, myLoc.CurrentCulture);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
            }
        }
        private void SetLocalizationText()
        {
            string curlan = (Page as DotNetNuke.Framework.PageBase).PageCulture.Name;
            btnCancel.Text = Localization.GetString("Cancel", this.LocalResourceFile + ".ascx." + curlan + ".resx");
            btnCheck.Text = Localization.GetString("Check", this.LocalResourceFile + ".ascx." + curlan + ".resx");
            lblCreateCourse.Text = Localization.GetString("CreateCourse", this.LocalResourceFile + ".ascx." + curlan + ".resx");
            lblCreateCourse1.Text = Localization.GetString("CreateCourse", this.LocalResourceFile + ".ascx." + curlan + ".resx");
            //  lblDescription.Text = Localization.GetString("Description", this.LocalResourceFile + ".ascx." + curlan + ".resx");
            lblPluggs.Text = Localization.GetString("Plug", this.LocalResourceFile + ".ascx." + curlan + ".resx");
            lblPluggs.HelpKey = "lblPluggs";
            lblPluggs.HelpText = Localization.GetString("PlugDes", this.LocalResourceFile + ".ascx." + curlan + ".resx");


            btnSubmit.Text = Localization.GetString("Submit", this.LocalResourceFile + ".ascx." + curlan + ".resx");
            lblTitle.Text = Localization.GetString("Title", this.LocalResourceFile + ".ascx." + curlan + ".resx");
            lblTitle.HelpKey = "lblTitle";
            lblTitle.HelpText = Localization.GetString("TitleDes", this.LocalResourceFile + ".ascx." + curlan + ".resx");


        }
        protected void btnCheck_Click(object sender, EventArgs e)
        {
            bool ischeck = CheckPlugg();
        }

        protected void btnSubmit_Click(object  sender, EventArgs e)
        {
            Boolean ischeck = CheckPlugg();

            if (!ischeck) //check validation
                return;

            CourseContainer cc;
            cc = SaveCourse();
            if (cc.TheCourse.CourseId != 0) //0 means Error in Update/Save
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Success", "alert('New Course is created successfully')", true);
             // Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(cc.TheCourse.TabId, "", ""));
            }           
        }

        protected CourseContainer SaveCourse()
        {
            BaseHandler bh = new BaseHandler();
            CourseContainer cc = new CourseContainer(new Localization().CurrentCulture);
            List<CourseItemEntity> cis = new List<CourseItemEntity>();
            ReadFromControls(cc, cis);
            try
            {
                cc.TheCourse.CourseId = 0;
                bh.CreateCourse(cc);  //Create CoursePage, Course and CourseItems (only Pluggs)
            }
            catch (Exception ex)
            {
                lblError.Text = "Failed to create a Course: " + ex.Message;
                Exceptions.LogException(ex);
                cc.TheCourse.CourseId = 0;
            }

            return cc;
        }               

        protected void ReadFromControls(CourseContainer c, List<CourseItemEntity> cis)
        {

            string subjectStr = Page.Request.QueryString["s"];
            if (subjectStr != null)
            {
                int subjectId;
                bool isNum = int.TryParse(subjectStr, out subjectId);
                if (isNum)
                    c.TheCourse.SubjectId = subjectId;
            }

            c.SetTitle(txtTitle.Text);
            c.CultureCode = DDLanguage.SelectedValue;           
            c.TheCourse.WhoCanEdit = (EWhoCanEdit)Enum.Parse(typeof(EWhoCanEdit), rdbtnWhoCanEdit.SelectedValue);  
            c.TheCourse.CreatedOnDate = DateTime.Now;
            c.TheCourse.CreatedByUserId = this.UserId;
            c.TheCourse.ModifiedOnDate = DateTime.Now; ;
            c.TheCourse.ModifiedByUserId = this.UserId;
            c.SetDescription(txtHtmlText.Text); 

            // string is in form "44,45,48,52" holding PluggIDs
            string pluggtext = txtPluggs.Text.Trim();

            // Todo: Check that pluggtext is in the correct format before creating the Course

            CourseItemEntity ci;
            if (!string.IsNullOrEmpty(pluggtext))
            {
                string[] itempluggs = pluggtext.Split(',');

                for (int i = 0; i < itempluggs.Length; i++)
                {
                    ci = new CourseItemEntity();
                    ci.ItemId = Convert.ToInt32(itempluggs[i]);
                    ci.CIOrder = i + 1;
                    ci.ItemType = ECourseItemType.Plugg;
                    ci.MotherId = 0;
                    cis.Add(ci);
                }
            }
        }
        
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL());
        }

        protected Boolean CheckPlugg()
        {
            bool ischecked = true;

            CIInfo.Text = "";
            BaseHandler ph = new BaseHandler();

            //string pluggtext = "12,56,34,45k,56";
            string pluggtext = txtPluggs.Text.Trim();
            if (!string.IsNullOrEmpty(pluggtext))
            {
                string[] itempluggs = pluggtext.Split(',');

                for (int i = 0; i < itempluggs.Length; i++)
                {
                    int num;
                    bool isNumeric = int.TryParse(itempluggs[i], out num);//check number.....
                    if (isNumeric)
                    {
                        PluggContainer p = new PluggContainer(new Localization().CurrentCulture, num);
                       
                       // p.ThePlugg = ph.GetPlugg(num);
                        if (p.ThePlugg != null)
                        {
                            p.CultureCode = (Page as DotNetNuke.Framework.PageBase).PageCulture.Name;
                            p.LoadTitle();
                            CIInfo.Text += num + ": " + p.TheTitle.Text + "<br />";
                        }
                        else
                        {
                            CIInfo.Text += num + ": Error – Plugg " + num + " does not exist" + "<br />";
                            ischecked = false;
                        }
                    }
                    else
                    {
                        CIInfo.Text = "Pluggs in wrong format.";
                        ischecked = false;
                    }
                }
            }

            return ischecked;
        }

        public ModuleActionCollection ModuleActions
        {
            get
            {
                var actions = new ModuleActionCollection
                    {
                        {
                            GetNextActionID(), Localization.GetString("EditModule", LocalResourceFile), "", "", "",
                            EditUrl(), false, SecurityAccessLevel.Edit, true, false
                        }
                    };
                return actions;
            }
        }

     
    }
}