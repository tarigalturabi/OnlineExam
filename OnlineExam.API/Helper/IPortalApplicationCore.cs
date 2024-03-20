namespace Portal.Ui.Arabic
{
    public interface IPortalApplicationCore
    {
        string GetPageView(string folder, string file);
		int IsActive();
		int InternalService();
	}
}
