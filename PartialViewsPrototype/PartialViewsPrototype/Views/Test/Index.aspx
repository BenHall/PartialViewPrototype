<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<PartialViewsPrototype.Controllers.NewsletterViewModel>" %>

<script type="text/javascript">

$(function() {  
  $(".submit").click(function() {
      alert('submitting...')
      $.ajax({
          type: "POST",
          url: "/test/Submit",
          data: "test=2",
          success: function (msg) {
              alert('done')
          }
      });  
    return false;
  });  
});  

</script>

<h3>Partial View</h3>
Message: <%= Model.Message %><br />
Value returned: <%= Model.Value %>

<form action="/test/Submit" method="post">
    <input type="text" value="2" name="test">
    <input type="submit" value="submit" class="submit" >
</form>