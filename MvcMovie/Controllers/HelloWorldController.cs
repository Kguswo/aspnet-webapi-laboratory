using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace MvcMovie.Controllers; // 이름 막 적어도 동작은 하지만, 다른곳에서 사용하기 위해 명확한 네임스페이스 지정하는게 좋음

public class HelloWorldController : Controller
{
    // GET: /HelloWorld/
    public string Index()
    {
        return "This is my default action...";
    }

    ////GET: /HelloWorld/Welcome/
    public string Welcome()
    {
        return "This is the Welcome action method...";
    }

    // GET: /HelloWorld/Welcome/
    // Requires using System.Text.Encodings.Web;
    public string Welcome(string name, int numTimes = 1)
    {
        return HtmlEncoder.Default.Encode($"Hello {name}, NumTimes is: {numTimes}"); //  HtmlEncoder.Default.Encode로 악의적인 입력으로부터 앱을 보호함
    }

    public string Welcome2(string name, int ID = 1)
    {
        return HtmlEncoder.Default.Encode($"Hello {name}, ID: {ID}");
    }
}
