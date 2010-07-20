using System;
using System.Web;
using System.Web.Mvc;

namespace PartialViewsPrototype.Controllers
{
	public class TestController : Controller
	{
	    private readonly HttpRequestBase _requestBase;
	    private readonly IPreviousModelStateProvider _previousModelStateProvider;

	    public TestController() : this(new HttpRequestWrapper(System.Web.HttpContext.Current.Request), new PreviousModelStateProvider())
	    {
	        //Of course, this would be IoC
	    }

	    public TestController(HttpRequestBase requestBase, IPreviousModelStateProvider previousModelStateProvider)
	    {
	        _requestBase = requestBase;
	        _previousModelStateProvider = previousModelStateProvider;
	    }

	    public ActionResult Index() {
	        var v = _previousModelStateProvider.Get<int>(this);
	        return PartialView(GetModel(v));
		}

		public ActionResult Submit(int test)
		{
            _previousModelStateProvider.Set(this, test);

            if (JsonRequest())
                return PartialView("Index", GetModel(test));
            if(IsValid(_requestBase.UrlReferrer))
		        return Redirect(_requestBase.UrlReferrer.ToString());
            else
                return new BadRequestResult();
		}

	    private NewsletterViewModel GetModel(int value)
	    {
	        return new NewsletterViewModel{Value = value, Message = "Default"};
	    }

	    private bool JsonRequest()
	    {
	        return false;
	    }

	    private bool IsValid(Uri urlReferrer)
	    {
	        return true;
	    }
	}

    public class NewsletterViewModel
    {
        public string Message { get; set; }
        public int Value { get; set; }
    }

    public interface IPreviousModelStateProvider
    {
        void Set<T>(Controller controller, T value);
        T Get<T>(Controller controller);
    }

    class PreviousModelStateProvider : IPreviousModelStateProvider
    {
        public void Set<T>(Controller controller, T value)
        {
            controller.TempData.Add(controller.GetType().Name, value);
        }

        public T Get<T>(Controller controller)
        {
            object v = controller.TempData[controller.GetType().Name];
            object vAsT = null;

            if (v != null)
                vAsT = Convert.ChangeType(v, typeof (T));

            if (vAsT == null)
                return default(T);

            return (T)vAsT;
        }
    }

    public class BadRequestResult : ViewResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            base.ExecuteResult(context);
            context.HttpContext.Response.StatusCode = 400; //something like that...
        }
    }
}