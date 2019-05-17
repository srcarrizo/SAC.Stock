namespace SAC.Stock.Front.Infrastructure
{
    using System.Net.Http;
    using System.Web.Mvc;

    internal static class ControllerExtension
    {
        public static T ResolveDependency<T>(this System.Web.Http.ApiController apiController)
        {
            return (T)apiController.Request.GetDependencyScope().GetService(typeof(T));
        }

        //public static T ResolveDependency<T>(this Controller controller)
        //{
        //    //return (T)controller.ResolveDependency
                
        //        ControllerFactory.Singleton().ResolveDependency<T>(controller);
        //}
    }
}