using Microsoft.AspNetCore.Mvc;

namespace MvcMovie.Controllers; // 이름 막 적어도 동작은 하지만, 다른곳에서 사용하기 위해 명확한 네임스페이스 지정하는게 좋음

public class HelloWorldController : Controller
{
    //GET: /HelloWorld/
    public string Index()
    {
        return "This is my default action...";
    }

    //GET: /HelloWorld/Welcome/
    public string Welcome()
    {
        return "This is the Welcome action method...";
    }
}
