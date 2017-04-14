using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VERPI.Account
{
    public partial class RegisterDone : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoginHyperLink.NavigateUrl = "Login";
        }
    }
}