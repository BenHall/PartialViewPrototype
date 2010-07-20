<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<PartialViewsPrototype.Controllers.NewsletterViewModel>" %>

<h3>Partial View</h3>
Message: <%= Model.Message %><br />
Value returned: <%= Model.Value %>

<form action="/test/Submit" method="post">
    <input type="text" value="2" name="test">
    <input type="submit" value="submit">
</form>