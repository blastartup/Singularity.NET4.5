
namespace Singularity.Web
{
    public class Action
    {
        public static PageAction GetPageAction()
        {
            PageAction doing = PageAction.Manage;
            string action = QueryString.GetQueryString("action");
            switch (action)
            {
                case "Manage":
                    doing = PageAction.Manage;
                    break;
                case "Create":
                    doing = PageAction.Create;
                    break;
                case "Edit":
                    doing = PageAction.Edit;
                    break;
                case "EdiBulk":
                    doing = PageAction.EditBulk;
                    break;
                case "Search":
                    doing = PageAction.Search;
                    break;
                case "Delete":
                    doing = PageAction.Delete;
                    break;
                case "View":
                    doing = PageAction.View;
                    break;
                case "Print":
                    doing = PageAction.Print;
                    break;
                default:
                    doing = PageAction.Manage;
                    break;

            }

            return doing;
        
        }
        
    }
}
