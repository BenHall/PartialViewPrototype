using System;
using System.Linq;
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
            if (JsonResult())
            {
                var newsletterViewModel = GetModel(test);
                return Json(newsletterViewModel);
            }

		    if (IsValid(_requestBase.UrlReferrer))
		    {
		        _previousModelStateProvider.Set(this, test);
		        return Redirect(_requestBase.UrlReferrer.ToString());
		    }

		    return new BadRequestResult();
		}

	    private bool JsonResult()
	    {
	        return _requestBase.AcceptTypes.Any() && _requestBase.AcceptTypes.Contains("application/json");
	    }

	    private NewsletterViewModel GetModel(int value)
	    {
            if(value == 0)
	            return new DefaultNewsletterViewModel{Value = value};
            if (value == 1)
                return new SuccessNewsletterViewModel { Value = value };
            
            return new ErrorNewsletterViewModel { Value = value };
	    }
        
	    private bool IsValid(Uri urlReferrer)
	    {
	        return true;
	    }
	}

    public abstract class NewsletterViewModel
    {
        public abstract string Message { get; }
        public int Value { get; set; }
    }

    public class DefaultNewsletterViewModel : NewsletterViewModel
    {
        public override string Message { get { return "Default"; } }        
    }

    public class SuccessNewsletterViewModel : NewsletterViewModel
    {
        public override string Message { get { return "Success"; } }
    }

    public class ErrorNewsletterViewModel : NewsletterViewModel
    {
        public override string Message { get { return "Error"; } }
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
            if (controller.TempData.ContainsKey(controller.GetType().Name))
                controller.TempData[controller.GetType().Name] = value;
            else
                controller.TempData.Add(controller.GetType().Name, value);
        }

        public T Get<T>(Controller controller)
        {
            object v = controller.TempData[controller.GetType().Name];
            object vAsT = null;

            if (v != null)
            {
                controller.TempData.Remove(controller.GetType().Name); //Getting some odd results due to the way TempData works in MVC2
                vAsT = Convert.ChangeType(v, typeof (T));
            }

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