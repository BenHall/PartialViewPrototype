<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<System.Int32>" %>

<h3>Partial View</h3>

Value returned: <%= Model %>

<form action="/test/Submit" method="post">
<input type="hidden" value="2" name="test">
<input type="submit" value="submit">
</form>